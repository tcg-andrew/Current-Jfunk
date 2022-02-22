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

namespace PowerAnalyzerServiceHost
{
    public partial class PowerAnalyzerService : ServiceBase
    {
        FlatWSDLServiceHost host;

        public PowerAnalyzerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                host = new FlatWSDLServiceHost(typeof(ServiceLibrary.PowerAnalyzerService));
                host.Open();
            }
            catch (Exception ex)
            {
                FileStream stream = new FileStream("C:\\TCGEpicorLog.txt", FileMode.OpenOrCreate);
                stream.Position = stream.Length;
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(DateTime.Now + " - Power Analyzer Service - " + ex.Message);
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
                writer.WriteLine(DateTime.Now + " - Power Analyzer Service - " + ex.Message);
                writer.Close();

            }
        }    }
}
