#region Usings

using System;
using System.ServiceModel;
using System.ServiceProcess;
using System.IO;
using FlatWSDL;

#endregion

namespace ShipToServiceHost
{
    public partial class ShipToService : ServiceBase
    {
        FlatWSDLServiceHost host;

        public ShipToService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                host = new FlatWSDLServiceHost(typeof(ServiceLibrary.ShipToService));
                host.Open();
            }
            catch (Exception ex)
            {
                FileStream stream = new FileStream("D:\\Installpoint\\Logs\\ShipToServiceHostLog.txt", FileMode.OpenOrCreate);
                stream.Position = stream.Length;
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(DateTime.Now + " - ShipTo Service - " + ex.Message);
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
                FileStream stream = new FileStream("D:\\Installpoint\\Logs\\ShipToServiceHostLog.txt", FileMode.OpenOrCreate);
                stream.Position = stream.Length;
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(DateTime.Now + " - ShipTo Service - " + ex.Message);
                writer.Close();

            }
        }
    }
}