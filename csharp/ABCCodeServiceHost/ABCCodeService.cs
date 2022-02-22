#region Usings

using System;
using System.ServiceModel;
using System.ServiceProcess;
using System.IO;
using FlatWSDL;

#endregion

namespace ABCCodeServiceHost
{
    public partial class ABCCodeService : ServiceBase
    {
        FlatWSDLServiceHost host;

        public ABCCodeService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                host = new FlatWSDLServiceHost(typeof(ServiceLibrary.ABCCodeService));
                host.Open();
            }
            catch (Exception ex)
            {
                FileStream stream = new FileStream("C:\\TCGEpicorLog.txt", FileMode.OpenOrCreate);
                stream.Position = stream.Length;
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(DateTime.Now + " - ABCCode Service - " + ex.Message);
                writer.Close();
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (host != null)
                    host.Close();
            }
            catch (Exception ex)
            {
                FileStream stream = new FileStream("C:\\TCGEpicorLog.txt", FileMode.OpenOrCreate);
                stream.Position = stream.Length;
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(DateTime.Now + " - ABCCode Service - " + ex.Message);
                writer.Close();
            }
        }
    }
}