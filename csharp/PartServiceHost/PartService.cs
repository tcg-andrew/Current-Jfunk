#region Usings

using System;
using System.IO;
using System.ServiceModel;
using System.ServiceProcess;
using FlatWSDL;

#endregion

namespace PartServiceHost
{
    public partial class PartService : ServiceBase
    {
        FlatWSDLServiceHost host;

        public PartService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                host = new FlatWSDLServiceHost(typeof(ServiceLibrary.PartService));
                host.Open();
            }
            catch (Exception ex)
            {
                FileStream stream = new FileStream("D:\\Installpoint\\Logs\\PartServiceHostLog.txt", FileMode.OpenOrCreate);
                stream.Position = stream.Length;
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(DateTime.Now + " - Part Service - " + ex.Message);
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
                FileStream stream = new FileStream("D:\\Installpoint\\Logs\\PartServiceHostLog.txt", FileMode.OpenOrCreate);
                stream.Position = stream.Length;
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(DateTime.Now + " - Part Service - " + ex.Message);
                writer.Close();

            }
        }
    }
}
