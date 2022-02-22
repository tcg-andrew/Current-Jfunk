using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using ObjectLibrary;
using System.Data.SqlClient;

namespace RailsPartInfoUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            string server = "172.16.6.60";
            string database = "wbq_production";

//            string server = "172.16.6.51";
  //          string database = "wbq_development";

            MySqlCommand command = new MySqlCommand("SELECT id, name, price, weight, description, freight_class, prodcode FROM parts");
            DataSet ds = MySQLAccess.GetDataSet(server, database, "ror", "SomePW23", command);

            List<string> errors = new List<string>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                SqlCommand sqlcommand = new SqlCommand("SELECT p.partdescription, p.unitprice, p.netweight, isnull(u.key1, 0) as freightclass, p.prodcode FROM part as p LEFT JOIN [dbo].ud09 as u (nolock) on p.ShortChar07 = convert(varchar, u.character01) where p.company = 'CRD' and p.partnum = @partnum");
                sqlcommand.Parameters.AddWithValue("partnum", row["name"]);
                DataSet dsx = SQLAccess.GetDataSet("SQL", "MfgSys803", "rails", "hJ*G6_!pZ2", sqlcommand);

                if (dsx.Tables[0].Rows.Count > 0)
                {
                    DataRow rowx = dsx.Tables[0].Rows[0];
/*                    if (row["price"].ToString() != rowx["unitprice"].ToString())
                    {
                        if (row["price"].ToString() == "" || row["price"].ToString() == "0.00")
                        {
                            Console.WriteLine(row["name"].ToString() + " - Updating Price");
                            MySqlCommand commandx = new MySqlCommand("UPDATE parts SET price = @price WHERE id = @id");
                            commandx.Parameters.AddWithValue("price", rowx["unitprice"].ToString());
                            commandx.Parameters.AddWithValue("id", row["id"].ToString());
                            MySQLAccess.NonQuery(server, database, "ror", "SomePW23", commandx);

                        }
                        else
                            errors.Add(row["name"].ToString() + " - Price mismatch");
                    }
                    if (row["description"].ToString() != rowx["partdescription"].ToString() && row["description"].ToString().Contains("automatic beer cave"))
                    {
                            Console.WriteLine(row["name"].ToString() + " - Updating Description");
                            MySqlCommand commandx = new MySqlCommand("UPDATE parts SET description = @description WHERE id = @id");
                            commandx.Parameters.AddWithValue("description", rowx["partdescription"].ToString());
                            commandx.Parameters.AddWithValue("id", row["id"].ToString());
                            MySQLAccess.NonQuery(server, database, "ror", "SomePW23", commandx);
                    }
*/                      if (row["prodcode"].ToString() != rowx["prodcode"].ToString())
                      {
                          Console.WriteLine(row["name"].ToString() + " - Updating Prod Code");
                          MySqlCommand commandx = new MySqlCommand("UPDATE parts SET prodcode = @prodcode WHERE id = @id");
                          commandx.Parameters.AddWithValue("prodcode", rowx["prodcode"].ToString());
                          commandx.Parameters.AddWithValue("id", row["id"].ToString());
                          MySQLAccess.NonQuery(server, database, "ror", "SomePW23", commandx);

                      }
/*                    if (row["weight"].ToString() != rowx["netweight"].ToString())
                    {
                        Console.WriteLine(row["name"].ToString() + " - Updating weight");
                        MySqlCommand commandx = new MySqlCommand("UPDATE parts SET weight = @weight WHERE id = @id");
                        commandx.Parameters.AddWithValue("weight", rowx["netweight"].ToString());
                        commandx.Parameters.AddWithValue("id", row["id"].ToString());
                        MySQLAccess.NonQuery(server, database, "ror", "SomePW23", commandx);
                    }
                    if (row["freight_class"].ToString() != rowx["freightclass"].ToString())
                    {
                        Console.WriteLine(row["name"].ToString() + " - Updating freight class");
                        MySqlCommand commandx = new MySqlCommand("UPDATE parts SET freight_class = @freightclass WHERE id = @id");
                        commandx.Parameters.AddWithValue("freightclass", rowx["freightclass"].ToString());
                        commandx.Parameters.AddWithValue("id", row["id"].ToString());
                        MySQLAccess.NonQuery(server, database, "ror", "SomePW23", commandx);
                    }
 */               }
            }
            foreach (string str in errors)
                Console.WriteLine(str);
        }
    }
}
