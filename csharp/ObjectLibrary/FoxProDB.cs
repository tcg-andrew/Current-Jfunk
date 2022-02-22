using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;

namespace ObjectLibrary
{
    public class FoxProDB
    {
        public DataSet ReadTable(string source, string table)
        {
            try
            {
                DataSet result = new DataSet();
                string connectionString = "Provider=vfpoledb;Data Source=" + source + table + ";Exclusive=OFF;";

                OleDbConnection connection = new OleDbConnection(connectionString);
                OleDbCommand command = connection.CreateCommand();
                command.CommandText = "select * from " + table;
                connection.Open();

                OleDbDataAdapter da = new OleDbDataAdapter(command);
                da.Fill(result);
                connection.Close();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ReadTableWhere(string source, string table, string where)
        {
            try
            {
                DataSet result = new DataSet();
                string connectionString = "Provider=vfpoledb;Data Source=" + source + table + ";Exclusive=OFF;";

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand command = connection.CreateCommand();
                    command.CommandText = "select * from [" + table + "] where " + where;
                    connection.Open();

                    OleDbDataAdapter da = new OleDbDataAdapter(command);
                    da.Fill(result);
                    connection.Close();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertCIGPAReading(string unitnum, string watt, string pf, string volts, string amps, string hertz, string heater)
        {
            try
            {
                string connectionString = "Provider=vfpoledb;Data Source=\\\\SARV-FILE01\\cig\\unitlist\\data\\autonum.DBF;Exclusive=ON;";
                int autonum = 0;
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand command = connection.CreateCommand();
                    command.CommandText = "select top 1 number from [autonum.DBF] order by number";

                    connection.Open();
                    autonum = Int32.Parse(command.ExecuteScalar().ToString());
                    autonum++;
                    command.CommandText = "update [autonum.DBF] set number = " + autonum;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                connectionString = "Provider=vfpoledb;Data Source=\\\\SARV-FILE01\\cig\\unitlist\\data\\unitvals.DBF;Exclusive=OFF;";
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand command = connection.CreateCommand();
                    command.CommandText = String.Format("insert into [unitvals.DBF] (autonum, unitnum, datetime, watt, pf, volts, amps, hz, heater) values ({0}, '{1}', DATETIME(), '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')", autonum, unitnum, watt, pf, volts, amps, hertz, heater);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    return autonum;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
