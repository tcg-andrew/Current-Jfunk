namespace Styleline.WinAnalyzer.DAL
{
    using System;
    using System.Configuration;

    public class Db
    {
        public static PowerAnalyzerDataContext GetContext()
        {
            return new PowerAnalyzerDataContext(ConfigurationManager.ConnectionStrings["PowerAnalyzerConnectionString"].ConnectionString);
        }
    }
}

