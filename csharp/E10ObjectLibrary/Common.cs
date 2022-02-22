#region Usings

using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

#endregion

namespace ObjectLibrary
{
    #region Class Address

    public class Address
    {
        #region Properties

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }

        #endregion

        #region Constructors

        public Address()
        {
            Address1 = "";
            Address2 = "";
            Address3 = "";
            City = "";
            State = "";
            Zip = "";
            Country = "";
        }

        public Address(string address1, string address2, string address3, string city, string state, string zip, string country)
        {
            Address1 = address1;
            Address2 = address2;
            Address3 = address3;
            City = city;
            State = state;
            Zip = zip;
            Country = country;
        }

        #endregion
    }

    #endregion

    #region Class MySQLAccess

    public static class MySQLAccess
    {
        public static DataSet GetDataSet(string server, string database, string username, string password, MySqlCommand command)
        {
            try
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(String.Format("Server = {0}; Database = {1}; uid = {2}; password = {3};", server, database, username, password)))
                {
                    command.Connection = sqlConnection;

                    sqlConnection.Open();

                    MySqlDataAdapter sda = new MySqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);

                    sqlConnection.Close();

                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void NonQuery(string server, string database, string username, string password, MySqlCommand command)
        {
            try
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(String.Format("Server = {0}; Database = {1}; uid = {2}; password = {3};", server, database, username, password)))
                {
                    command.Connection = sqlConnection;

                    sqlConnection.Open();

                    command.ExecuteNonQuery();

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    #endregion

    #region Class SQLAccess
    // TODO:5 Add unit tests
    public static class SQLAccess
    {
        public static DataSet GetDataSet(string server, string database, string username, string password, SqlCommand command)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(String.Format("Data Source = {0}; Initial Catalog = {1}; User ID = {2}; Password = {3};", server, database, username, password)))
                {
                    command.Connection = sqlConnection;

                    sqlConnection.Open();

                    SqlDataAdapter sda = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);

                    sqlConnection.Close();

                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetScalar(string server, string database, string username, string password, SqlCommand command)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(String.Format("Data Source = {0}; Initial Catalog = {1}; User ID = {2}; Password = {3};", server, database, username, password)))
                {
                    command.Connection = sqlConnection;

                    sqlConnection.Open();

                    string result = command.ExecuteScalar().ToString();

                    sqlConnection.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void NonQuery(string server, string database, string username, string password, SqlCommand command)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(String.Format("Data Source = {0}; Initial Catalog = {1}; User ID = {2}; Password = {3};", server, database, username, password)))
                {
                    command.Connection = sqlConnection;

                    sqlConnection.Open();

                    command.ExecuteNonQuery();

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void NonQuery(string server, string database, SqlCommand command)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(String.Format("Data Source = {0}; Initial Catalog = {1}; Integrated Security=SSPI;", server, database)))
                {
                    command.Connection = sqlConnection;

                    sqlConnection.Open();

                    command.ExecuteNonQuery();

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetScalar(string server, string database, string username, string password, SqlCommand command, SqlConnection connection)
        {
            try
            {
                command.Connection = connection;
                string result = command.ExecuteScalar().ToString();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void NonQuery(SqlCommand command, SqlConnection connection)
        {
            try
            {
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SqlConnection BeginTransaction(string server, string database, string username, string password)
        {
            SqlConnection sqlConnection = new SqlConnection(String.Format("Data Source = {0}; Initial Catalog = {1}; User ID = {2}; Password = {3};", server, database, username, password));

            try
            {
                SqlCommand command = new SqlCommand("begin transaction");
                command.Connection = sqlConnection;
                sqlConnection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (sqlConnection.State == ConnectionState.Open)
                    sqlConnection.Close();
                throw ex;
            }

            return sqlConnection;
        }

        public static void CommitTransaction(SqlConnection connection)
        {
            try
            {
                SqlCommand command = new SqlCommand("commit transaction");
                command.Connection = connection;
                command.ExecuteNonQuery();

                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        public static void RollbackTransaction(SqlConnection connection)
        {
            try
            {
                SqlCommand command = new SqlCommand("rollback transaction");
                command.Connection = connection;
                command.ExecuteNonQuery();

                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }            
        }

        public static void SendMail(string server, string database, string username, string password, string email, string subject, string body)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand("EXEC dbo.sp_SendMail @Email, @presubject, @tableHTML");
                sqlCommand.Parameters.AddWithValue("Email", email);
                sqlCommand.Parameters.AddWithValue("presubject", subject);
                sqlCommand.Parameters.AddWithValue("tableHTML", body);

                NonQuery(server, database, username, password, sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    #endregion
}
// TODO:5 Add advanced error handling where applicable to entire solution