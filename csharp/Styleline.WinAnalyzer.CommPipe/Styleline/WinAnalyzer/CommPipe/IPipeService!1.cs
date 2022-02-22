namespace Styleline.WinAnalyzer.CommPipe
{
    using System;
    using System.ServiceModel;

    [ServiceContract(Namespace="http://www.styleline.com/PipeService", SessionMode=SessionMode.Required, CallbackContract=typeof(ICallback))]
    public interface IPipeService<TPipe> where TPipe: class
    {
        [OperationContract]
        bool KeepAlive();
        [OperationContract(IsOneWay=true)]
        void PipeIn(TPipe data);
        [OperationContract(IsOneWay=true)]
        void Subscribe();
        [OperationContract(IsOneWay=true)]
        void Unsubscribe();
    }
}

