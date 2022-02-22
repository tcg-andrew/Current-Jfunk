namespace Styleline.WinAnalyzer.CommPipe
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.ServiceModel;

    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class PipeService<TPipe> : IPipeService<TPipe> where TPipe: class
    {
        public delegate void DataIsReady(TPipe data);
        public DataIsReady DataReady;
        private List<ICallback> subscribers;
        private object syncRoot;

        public PipeService()
        {
            this.syncRoot = new object();
            this.subscribers = new List<ICallback>();
        }

        public bool KeepAlive()
        {
            return true;
        }

        public void PipeIn(TPipe data)
        {
            if (this.DataReady != null)
            {
                this.DataReady(data);
            }
        }

        protected void RemoveDeadConnections()
        {
            lock (this.syncRoot)
            {
                for (int i = this.subscribers.Count - 1; i >= 0; i--)
                {
                    this.RemoveIfDead(this.subscribers[i]);
                }
            }
        }

        protected bool RemoveIfDead(ICallback callback)
        {
            ICommunicationObject commObject = (ICommunicationObject) callback;
            if (commObject.State == CommunicationState.Opened)
            {
                return true;
            }
            commObject.Abort();
            if (this.subscribers.Contains(callback))
            {
                this.subscribers.Remove(callback);
            }
            return false;
        }

        public void SendResponse(AnalyzerResponse response)
        {
            lock (this.syncRoot)
            {
                for (int i = this.subscribers.Count - 1; i >= 0; i--)
                {
                    if (this.RemoveIfDead(this.subscribers[i]))
                    {
                        this.subscribers[i].Response(response);
                    }
                }
            }
        }

        public void Subscribe()
        {
            lock (this.syncRoot)
            {
                ICallback callback = OperationContext.Current.GetCallbackChannel<ICallback>();
                if (!this.subscribers.Contains(callback))
                {
                    this.subscribers.Add(callback);
                }
            }
            this.RemoveDeadConnections();
        }

        public void Unsubscribe()
        {
            lock (this.syncRoot)
            {
                ICallback callback = OperationContext.Current.GetCallbackChannel<ICallback>();
                if (this.subscribers.Contains(callback))
                {
                    this.subscribers.Remove(callback);
                }
            }
            this.RemoveDeadConnections();
        }

        public static string URI
        {
            get
            {
                return ("net.pipe://localhost/" + typeof(TPipe).Name);
            }
        }

    }
}

