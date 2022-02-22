#region Usings

using System;
using System.ServiceModel;
using System.ServiceProcess;
using ServiceLibrary;
using System.IO;
using FlatWSDL;

#endregion

namespace CustomerServiceHost
{
    public partial class CustomerService : ServiceBase
    {
        FlatWSDLServiceHost host;

        public CustomerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                host = new FlatWSDLServiceHost(typeof(ServiceLibrary.CustomerService));
                host.Open();
            }
            catch (Exception ex)
            {
                FileStream stream = new FileStream("D:\\Installpoint\\Logs\\CustomerServiceHostLog.txt", FileMode.OpenOrCreate);
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
                FileStream stream = new FileStream("D:\\Installpoint\\Logs\\CustomerServiceHostLog.txt", FileMode.OpenOrCreate);
                stream.Position = stream.Length;
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(DateTime.Now + " - Customer Service - " + ex.Message);
                writer.Close();
            }
        }
    }
}