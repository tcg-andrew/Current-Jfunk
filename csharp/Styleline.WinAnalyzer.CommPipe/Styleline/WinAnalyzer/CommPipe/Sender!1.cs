namespace Styleline.WinAnalyzer.CommPipe
{
    using System;
    using System.Runtime.CompilerServices;
    using System.ServiceModel;
    using System.Timers;

    public class Sender<TPipe> : IDisposable where TPipe: class
    {
        private bool isStarted;
        private string pipeName;
        private IPipeService<TPipe> proxy;
        public object syncRoot;
        private Timer timer;

        public event ResponseEventHandler OnResponse;

        public Sender() : this(typeof(TPipe).Name)
        {
        }

        public Sender(string pipeName)
        {
            this.syncRoot = new object();
            this.isStarted = true;
            this.pipeName = pipeName;
        }

        private void callback_OnResponse(AnalyzerResponse response)
        {
            if (this.OnResponse != null)
            {
                this.OnResponse(response);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            this.Stop();
        }

        ~Sender()
        {
            this.Dispose(false);
        }

        public void SendMessage(TPipe data)
        {
            try
            {
                this.proxy.PipeIn(data);
            }
            catch
            {
                this.Stop();
            }
        }

        public bool Start()
        {
            try
            {
                EndpointAddress epa = new EndpointAddress(string.Format("{0}/{1}", PipeService<TPipe>.URI, this.pipeName));
                CallBackObject callback = new CallBackObject();
                this.proxy = DuplexChannelFactory<IPipeService<TPipe>>.CreateChannel(callback, new NetNamedPipeBinding(), epa);
                this.proxy.Subscribe();
                this.IsStarted = true;
                callback.OnResponse += new ResponseEventHandler(this.callback_OnResponse);
            }
            catch
            {
                if (this.proxy != null)
                {
                    this.proxy = null;
                }
                return false;
            }
            this.timer = new Timer(1000.0);
            this.timer.Elapsed += new ElapsedEventHandler(this.timer_Elapsed);
            this.timer.AutoReset = true;
            this.timer.Start();
            return true;
        }

        public void Stop()
        {
            lock (this.syncRoot)
            {
                if (this.timer != null)
                {
                    this.timer.Stop();
                    this.timer = null;
                }
                if (this.proxy != null)
                {
                    this.proxy = null;
                }
                this.isStarted = false;
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                this.proxy.KeepAlive();
            }
            catch
            {
                this.Stop();
            }
        }

        public bool IsDisposed { get; set; }

        public bool IsStarted
        {
            get
            {
                lock (this.syncRoot)
                {
                    return this.isStarted;
                }
            }
            private set
            {
                lock (this.syncRoot)
                {
                    this.isStarted = value;
                }
            }
        }

        private class CallBackObject : ICallback
        {
            public event ResponseEventHandler OnResponse;

            public void Response(AnalyzerResponse response)
            {
                if (this.OnResponse != null)
                {
                    this.OnResponse(response);
                }
            }
        }
    }
}

