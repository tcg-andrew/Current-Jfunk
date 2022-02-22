using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Timers;
using System.Data;
using System.Configuration;
using System.Threading;
using MySql.Data.MySqlClient;

namespace ObjectConsoleTester
{
    class Program
    {

        static void Main(string[] args)
        {
            SQLAccess.SQLAccess.SendMail("SARV-SQLPROD01", "EpicorDB", "RailsAppUserP", "wA7tA1FaBS1MpLaU", "jfsoftwarellc@gmail.com", "DB Mail Test", "This is a test");
                
        }


    }
}
