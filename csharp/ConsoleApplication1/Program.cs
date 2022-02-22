using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            FileStream stream = new FileStream("C:\\UserDefinedColumns.txt", FileMode.OpenOrCreate);
            stream.Position = stream.Length;
            StreamWriter writer = new StreamWriter(stream);

            SqlCommand sqlCommand = new SqlCommand(@"SELECT TABLE_NAME
                                                    FROM INFORMATION_SCHEMA.TABLES
                                                    WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG='MfgSys803'
                                                    order by TABLE_NAME");

            try
            {
                
                DataSet ds = ObjectLibrary.SQLAccess.GetDataSet("SQL", "MfgSys803", "rails", "hJ*G6_!pZ2", sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                {


                    SqlCommand sqlCommand2 = new SqlCommand(@"SELECT COLUMN_NAME
                        FROM MfgSys803.INFORMATION_SCHEMA.COLUMNS (nolock)
                        WHERE TABLE_NAME = N'" + row[0].ToString() + @"'
						and (COLUMN_NAME like 'character%' or COLUMN_NAME like 'number%' or COLUMN_NAME like 'date%' or COLUMN_NAME like 'checkbox%' or COLUMN_NAME like 'shortchar%')
						ORDER BY COLUMN_NAME");
                    sqlCommand2.CommandTimeout = 300;
                    DataSet ds2 = ObjectLibrary.SQLAccess.GetDataSet("SQL", "MfgSys803", "rails", "hJ*G6_!pZ2", sqlCommand2);

                    bool written = false;

                    foreach (DataRow row2 in ds2.Tables[0].Rows)
                    {
                        SqlCommand sqlCommand3 = new SqlCommand(@"SELECT distinct convert(varchar," + row2[0].ToString() + ") from " + row[0].ToString() + " (nolock) where " + row2[0].ToString() + " is not null");
                        sqlCommand3.CommandTimeout = 300;

                        DataSet ds3 = ObjectLibrary.SQLAccess.GetDataSet("SQL", "MfgSys803", "rails", "hJ*G6_!pZ2", sqlCommand3);

                        if (ds3.Tables[0].Rows.Count > 1 || (ds3.Tables[0].Rows.Count == 1 && !String.IsNullOrEmpty(ds3.Tables[0].Rows[0][0].ToString()) && ds3.Tables[0].Rows[0][0].ToString() != "0" && ds3.Tables[0].Rows[0][0].ToString() != "0.00" && ds3.Tables[0].Rows[0][0].ToString() != "0.000000000" && ds3.Tables[0].Rows[0][0].ToString() != "0.00000"))
                        {
                            if (!written)
                            {
                                writer.WriteLine(row[0].ToString());
                                written = true;
                            }
                            writer.WriteLine(row2[0].ToString());
                        }
                    }
                    if (written)
                        writer.WriteLine("");
                }
                    
            }
            catch (Exception ex)
            {
                writer.WriteLine(ex.Message);
            }
            writer.Close();

        }
     }

}