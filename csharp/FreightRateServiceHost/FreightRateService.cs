#region Usings

using System;
using System.IO;
using System.ServiceProcess;
using FlatWSDL;

#endregion

namespace FreightRateServiceHost
{
    public partial class FreightRateService : ServiceBase
    {
        FlatWSDLServiceHost host;

        public FreightRateService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                host = new FlatWSDLServiceHost(typeof(ServiceLibrary.FreightRateService));
                host.Open();
            }
            catch (Exception ex)
            {
                FileStream stream = new FileStream("C:\\TCGEpicorLog.txt", FileMode.OpenOrCreate);
                stream.Position = stream.Length;
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(DateTime.Now + " - Customer Service - " + ex.Message);
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
                writer.WriteLine(DateTime.Now + " - Customer Service - " + ex.Message);
                writer.Close();
            }

        }
    }
}
