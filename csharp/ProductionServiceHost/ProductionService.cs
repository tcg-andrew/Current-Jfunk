#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using FlatWSDL;
using System.IO;

#endregion

namespace ProductionServiceHost
{
    public partial class ProductionService : ServiceBase
    {
        FlatWSDLServiceHost host;

        public ProductionService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                host = new FlatWSDLServiceHost(typeof(ServiceLibrary.ProductionService));
                host.Open();
            }
            catch (Exception ex)
            {
                FileStream stream = new FileStream("D:\\Installpoint\\Logs\\ProductionServiceHostLog.txt", FileMode.OpenOrCreate);
                stream.Position = stream.Length;
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(DateTime.Now + " - Production Service - " + ex.Message);
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
                FileStream stream = new FileStream("D:\\Installpoint\\Logs\\ProductionServiceHostLog.txt", FileMode.OpenOrCreate);
                stream.Position = stream.Length;
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(DateTime.Now + " - Production Service - " + ex.Message);
                writer.Close();

            }
        }
    }
}
