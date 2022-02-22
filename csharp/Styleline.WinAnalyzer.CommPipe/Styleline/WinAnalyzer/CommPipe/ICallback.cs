namespace Styleline.WinAnalyzer.CommPipe
{
    using System;
    using System.Runtime.CompilerServices;
    using System.ServiceModel;

    public interface ICallback
    {
        event ResponseEventHandler OnResponse;

        [OperationContract(IsOneWay=true)]
        void Response(AnalyzerResponse response);
    }
}

