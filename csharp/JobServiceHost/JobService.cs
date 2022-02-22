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

namespace JobServiceHost
{
    public partial class JobService : ServiceBase
    {
        FlatWSDLServiceHost host;

        public JobService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                host = new FlatWSDLServiceHost(typeof(ServiceLibrary.JobService));
                host.Open();
            }
            catch (Exception ex)
            {
                FileStream stream = new FileStream("C:\\TCGEpicorLog.txt", FileMode.OpenOrCreate);
                stream.Position = stream.Length;
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(DateTime.Now + " - Job Service - " + ex.Message);
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
                writer.WriteLine(DateTime.Now + " - Job Service - " + ex.Message);
                writer.Close();

            }
        }
    }
}
