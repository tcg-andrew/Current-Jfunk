namespace Styleline.WinAnalyzer.CommPipe
{
    using System;
    using System.Runtime.CompilerServices;
    using System.ServiceModel;

    public class Receiver<TRequest> : IDisposable where TRequest: class
    {
        private ServiceHost host;
        private bool operational;
        private string pipeName;
        private PipeService<TRequest> ps;

        public Receiver()
        {
            this.ps = new PipeService<TRequest>();
            this.pipeName = typeof(TRequest).Name;
        }

        public void Dispose()
        {
            this.ServiceOff();
            if (this.ps != null)
            {
                this.ps = null;
            }
        }

        protected bool HostService()
        {
            try
            {
                this.host = new ServiceHost(this.ps, new Uri[] { new Uri(PipeService<TRequest>.URI) });
                NetNamedPipeBinding nnpb = new NetNamedPipeBinding();
                this.host.AddServiceEndpoint(typeof(IPipeService<TRequest>), nnpb, this.pipeName);
                this.host.Open();
                this.operational = true;
            }
            catch (Exception ex)
            {
                this.Error = ex;
                this.operational = false;
            }
            return this.operational;
        }

        public void SendResponse(AnalyzerResponse callback)
        {
            this.ps.SendResponse(callback);
        }

        public void ServiceOff()
        {
            if ((this.host != null) && (this.host.State != CommunicationState.Closed))
            {
                this.host.Close();
            }
            this.host = null;
            this.operational = false;
        }

        public bool ServiceOn()
        {
            return (this.operational = this.HostService());
        }

        public string CurrentPipe
        {
            get
            {
                return this.pipeName;
            }
        }

        public PipeService<TRequest>.DataIsReady Data
        {
            get
            {
                return this.ps.DataReady;
            }
            set
            {
                this.ps.DataReady = (PipeService<TRequest>.DataIsReady) Delegate.Combine(this.ps.DataReady, value);
            }
        }

        public Exception Error { get; private set; }
    }
}

