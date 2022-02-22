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

namespace LaborServiceHost
{
    public partial class LaborService : ServiceBase
    {
        FlatWSDLServiceHost host;

        public LaborService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                host = new FlatWSDLServiceHost(typeof(ServiceLibrary.LaborService));
                host.Open();
            }
            catch (Exception ex)
            {
                FileStream stream = new FileStream("D:\\Installpoint\\Logs\\LaborServiceHostLog.txt", FileMode.OpenOrCreate);
                stream.Position = stream.Length;
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(DateTime.Now + " - Labor Service - " + ex.Message);
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
                FileStream stream = new FileStream("D:\\Installpoint\\Logs\\LaborServiceHostLog.txt", FileMode.OpenOrCreate);
                stream.Position = stream.Length;
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(DateTime.Now + " - Labor Service - " + ex.Message);
                writer.Close();

            }
        }
    }
}
