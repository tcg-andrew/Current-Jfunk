using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using WindowsConsoleClient.CustomerServiceReference;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using WindowsConsoleClient.PartServiceReference;
using WindowsConsoleClient.ABCCodeServiceReference;
using WindowsConsoleClient.FreightRateServiceReference;
using WindowsConsoleClient.ShipToServiceReference;
using WindowsConsoleClient.QuoteServiceReference;
using MySql.Data.MySqlClient;
using ObjectLibrary;
using System.Configuration;
using System.Data;
using System.Timers;
using System.Threading;
using System.IO;
using WindowsConsoleClient.ProductionService;
using WindowsConsoleClient.LaborService;

namespace WindowsConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //    FreightTest();
            //    ShipToTest();
            //  PartTest();
            //            QuoteTest();
            //PATest();
            LaborTest();
        }

        public static void LaborTest()
        {
            LaborServiceClient client = new LaborServiceClient("basic");
            client.ClientCredentials.UserName.UserName = "RailsAppUserP";
            client.ClientCredentials.UserName.Password = "wA7tA1FaBS1MpLaU";
            SetCertificatePolicy();

            client.Open();

            getstringresult result = client.getempname("CRD", "1689");
            string r = result.epicor.ToString();
        }
        public static void PATest()
        {
            ProductionServiceClient client = new ProductionServiceClient("basic");
            client.ClientCredentials.UserName.UserName = "RailsAppUserP";
            client.ClientCredentials.UserName.Password = "wA7tA1FaBS1MpLaU";
            SetCertificatePolicy();

            client.Open();

            padatagetresult result = client.getpadata("CRD", "0064802-1-1", 2);
            string r = result.epicor.ToString();
        }

        public static void QuoteTest()
        {
            try
            {
                QuoteServiceClient client = new QuoteServiceClient("basic");
                client.ClientCredentials.UserName.UserName = "rails";
                client.ClientCredentials.UserName.Password = "fX@7*n]Ra3";
                SetCertificatePolicy();

                client.Open();

                quotegetresult result = client.getquoteinfo("CRDSerivce", "vantage", 1001, "SYSTEM", "C", "SHELVING", "D", true, true, 77, 6, 64, 17, 4, 36, 32, 55, 45, 42, 55, 45, 42, 28, 10, 0, 0, 0, 0, 0, 0, 55, 45, 42, 21, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        public static void ShipToTest()
        {
            ShipToServiceClient client = new ShipToServiceClient("basic");
            client.ClientCredentials.UserName.UserName = "rails";
            client.ClientCredentials.UserName.Password = "fX@7*n]Ra3";

            client.Open();

            shiptogetresult result = client.getshiptobycustid("1001");

            client.Close();

            foreach (shipto r in result.epicor)
                Console.WriteLine(r.address1);
        }

        public static void PartTest()
        {
            PartServiceClient client = new PartServiceClient("basic");
            client.ClientCredentials.UserName.UserName = "rails";
            client.ClientCredentials.UserName.Password = "fX@7*n]Ra3";
            SetCertificatePolicy();

            client.Open();

            partgetneedingupdateresult result = client.getpartsneedingupdate();

            client.Close();
        }

        public static void FreightTest()
        {
            FreightRateServiceClient client = new FreightRateServiceClient("basic");
            client.ClientCredentials.UserName.UserName = "rails";
            client.ClientCredentials.UserName.Password = "fX@7*n]Ra3";
            SetCertificatePolicy();

            client.Open();

            freightrategetresult result = client.getrate("100", "500", "", "", "", "", "", "", "", "", "34240", "98101", 210869);

            Console.WriteLine(result.epicor[0].carrier);
            Console.WriteLine(result.epicor[0].totalcost);
            Console.WriteLine(result.epicor[0].jointline);
            Console.WriteLine(result.epicor[0].transitdays.ToString() + " days");
        }

        public static void SetCertificatePolicy()
        {
            ServicePointManager.ServerCertificateValidationCallback
                       += RemoteCertificateValidate;
        }

        /// <summary>
        /// Remotes the certificate validate.
        /// </summary>
        private static bool RemoteCertificateValidate(
           object sender, X509Certificate cert,
            X509Chain chain, SslPolicyErrors error)
        {
            // trust any certificate!!!
            System.Console.WriteLine("Warning, trust any certificate");
            return true;
        }
    }
}
