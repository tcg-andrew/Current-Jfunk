using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MihlfeldConsoleTester.OpenSourceServiceReference;
using System.Data;

namespace MihlfeldConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            OpenSourceSoapClient client = new OpenSourceSoapClient();
/*            DataTable dt = client.Get_Requestor_ID_List("CRDC", "T4H58TK7D6W3");
            foreach(DataRow row in dt.Rows)
            {
                Console.WriteLine(String.Format("Location CD : {0}", row[0]));
                Console.WriteLine(String.Format("Address : {0}", row[1]));
                Console.WriteLine(String.Format("ID : {0}", row[2]));
                Console.WriteLine();
            }
            */
            DataTable dt = client.Get_Rates("CRDC", "T4H58TK7D6W3", "100", "70", "", "", "", "", "", "", "", "", "34240", "49525", "O", DateTime.Now.ToString("MM/dd/yyyy"), "P", 210869);
            foreach (DataRow row in dt.Rows)
                Console.WriteLine(row[0] + " " + row[1] + " " + row[2] + " " + row[3] + " " + row[4] + " " + row[5] + " " + row[6] + " " +row[7] + " " + row[8]);
             
            Console.ReadLine();
        }
    }
}
