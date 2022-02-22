#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Reflection;
using System.Data.SqlClient;

#endregion

namespace AgilentDataProcessor
{
    public partial class Form1 : Form
    {
        FileInfo[] toProcess;
        PropertyInfo[] propertyInfos;

        string server = "SARV-SQLPROD01";

        public Form1()
        {
            InitializeComponent();

            propertyInfos = typeof(ScanBatch).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            ScanForFiles();
        }

        private void ScanForFiles()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
                toProcess = di.GetFiles("Data * * *_*_* *_*_*.csv");

                if (toProcess.Length > 0)
                {
                    lbl_Summary.Text = toProcess.Length.ToString() + " files found to process";
                    btn_Process.Enabled = true;
                }
                else
                {
                    lbl_Summary.Text = "No files found to process";
                    btn_Process.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                DialogResult result = MessageBox.Show(ex.Message, "Error scanning for files", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == System.Windows.Forms.DialogResult.Retry)
                    ScanForFiles();
            }
        }

        private void btn_Process_Click(object sender, EventArgs e)
        {
            int processed = 0;
            foreach (FileInfo file in toProcess)
            {
                if (ProcessFile(file))
                    processed++;
            }

            MessageBox.Show("Processed " + processed.ToString() + " file(s)");
            ScanForFiles();
        }

        private bool ProcessFile(FileInfo file)
        {
            bool processed = false;
            try
            {
                // Process Data
                StreamReader reader = new StreamReader(file.OpenRead());
                ScanBatch batch = ParseFileData(reader);
                reader.Close();

                SqlConnection connection = SQLAccess.BeginTransaction(server, "AgilentData");
                try
                {
                    if (WriteData(batch, connection))
                    {
                        // Copy File To Backup
                        file.CopyTo(ConfigurationManager.AppSettings["BackupLocation"] + "\\" + file.Name);

                        // Delete File
                        file.Delete();

                        // Commit data
                        SQLAccess.CommitTransaction(connection);
                        processed = true;
                    }
                    else
                        SQLAccess.RollbackTransaction(connection);
                }
                catch (Exception ex)
                {
                    SQLAccess.RollbackTransaction(connection);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                DialogResult result = MessageBox.Show(ex.Message, "Error processing file " + file.Name, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == System.Windows.Forms.DialogResult.Retry)
                    ProcessFile(file);
            }
            return processed;
        }

        private ScanBatch ParseFileData(StreamReader reader)
        {
            ScanBatch sb = new ScanBatch();
            try
            {
                foreach (PropertyInfo propertyInfo in propertyInfos)
                        ExtractField(sb, propertyInfo.Name, reader);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sb;
        }

        private void ExtractField(ScanBatch sb, string field, StreamReader reader)
        {
            string source = "";
            try
            {
                object[] value = null;

                switch (field)
                {
                    case "Name":
                    case "Owner":
                    case "Comments":
                    case "AcquisitionDate":
                        {
                            source = reader.ReadLine();
                            value = new string[1] { source.Split(',')[1] };
                            sb.GetType().InvokeMember(field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty, Type.DefaultBinder, sb, value);
                        }
                        break;
                    case "Instrument":
                        {
                            source = reader.ReadLine();
                            string[] split = source.Split(',');
                            field = "Instrument - Name";
                            sb.Instrument.Name = split[1];
                            field = "Instrument - Address";
                            sb.Instrument.Address = split[3];
                            field = "Instrument - NumModules";
                            sb.Instrument.NumModules = Int32.Parse(split[5]);
                            for (int i = 0; i < sb.Instrument.NumModules; i++)
                            {
                                field = "Instrument - Module " + (i + 1).ToString();
                                string slot = split[6 + (i * 2)];
                                slot = slot.Replace("Slot", "");
                                slot = slot.Remove(slot.Length - 1);
                                sb.Instrument.Modules.Add(Int32.Parse(slot), split[6 + (i * 2) + 1]);
                            }
                        }
                        break;
                    case "Channels":
                        {
                            source = reader.ReadLine();
                            field = "Channels - Num Channels";
                            int numchannels = Int32.Parse(source.Split(',')[1]);
                            source = reader.ReadLine();
                            string[] columns = source.Split(',');
                            for (int i = 0; i < numchannels; i++)
                            {
                                source = reader.ReadLine();
                                string[] values = source.Split(',');

                                ScanChannel sc = new ScanChannel();
                                for (int j = 0; j < columns.Length; j++)
                                {
                                    field = "Channels - Channel " + (i + 1).ToString() + " - Column #" + (j + 1).ToString() + ":" + columns[j];
                                    sc.GetType().InvokeMember(columns[j], BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty, Type.DefaultBinder, sc, new string[1] { values[j] });
                                }
                                sb.Channels.Add(sc);
                            }
                        }
                        break;
                    case "ScanControl":
                        {
                            ScanControl sc = new ScanControl();
                            source = reader.ReadLine();
                            string[] split = source.Split(',');
                            field = "ScanControl - Start Action";
                            sc.StartAction = split[2];
                            field = "ScanControl - End Action";
                            sc.EndAction = split[4];
                            sb.ScanControl = sc;
                        }
                        break;
                    case "Scans":
                        {
                            source = reader.ReadLine();
                            string[] columns = source.Split(',');
                            int scanline = 0;
                            while (!reader.EndOfStream)
                            {
                                scanline++;
                                source = reader.ReadLine();
                                string[] data = source.Split(',');
                                field = "Scans - Scan Line " + scanline.ToString() + " - Scan #";
                                int cycle = Int32.Parse(data[0]);
                                field = "Scans - Scan Line " + scanline.ToString() + " - Scan Time";
                                string time = data[1];
                                for (int i = 2; i < columns.Length; i += 2)
                                {
                                    Scan scan = new Scan();
                                    scan.Cycle = cycle;
                                    scan.ScanTime = time;
                                    field = "Scans - Channel " + ((i / 2) + 1).ToString() + " - Channel";
                                    scan.Channel = columns[i].Split(new string[] { " " }, StringSplitOptions.None)[0];
                                    field = "Scans - Scan Line " + scanline.ToString() + " - Column " + (i + 1).ToString() + " - Result";
                                    scan.Result = data[i];
                                    field = "Scans - Scan Line " + scanline.ToString() + " - Column " + (i + 1).ToString() + " - Alarm";
                                    scan.Alarm = data[i + 1].ToString() == "1";
                                    sb.Scans.Add(scan);
                                }
                            }
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error Extracting Field '" + field + "' from source string '" + source + "' - " + ex.Message);
            }
        }

        private bool WriteData(ScanBatch batch, SqlConnection connection)
        {
            bool success = false;
            try
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "exec dbo.sp_CreateScanBatch @Name, @Owner, @Comments, @AcquisitionDate, @Instrument, @Address, @Slot1Module, @Slot2Module, @Slot3Module";
                command.Parameters.AddWithValue("Name", batch.Name);
                command.Parameters.AddWithValue("Owner", batch.Owner);
                command.Parameters.AddWithValue("Comments", batch.Comments);
                command.Parameters.AddWithValue("AcquisitionDate", batch.AcquisitionDate);
                command.Parameters.AddWithValue("Instrument", batch.Instrument.Name);
                command.Parameters.AddWithValue("Address", batch.Instrument.Address);
                if (batch.Instrument.Modules.ContainsKey(1))
                    command.Parameters.AddWithValue("Slot1Module", batch.Instrument.Modules[1]);
                else
                    command.Parameters.AddWithValue("Slot1Module", DBNull.Value);
                if (batch.Instrument.Modules.ContainsKey(2))
                    command.Parameters.AddWithValue("Slot2Module", batch.Instrument.Modules[2]);
                else
                    command.Parameters.AddWithValue("Slot2Module", DBNull.Value);
                if (batch.Instrument.Modules.ContainsKey(3))
                    command.Parameters.AddWithValue("Slot3Module", batch.Instrument.Modules[3]);
                else
                    command.Parameters.AddWithValue("Slot3Module", DBNull.Value);

                int batchid = Int32.Parse(SQLAccess.GetScalar(command, connection));

                Dictionary<string, int> channelIDs = new Dictionary<string, int>();
                foreach (ScanChannel channel in batch.Channels)
                {
                    command = connection.CreateCommand();
                    command.Parameters.Clear();
                    command.CommandText = "exec dbo.sp_CreateScanChannel @ScanBatchID, @Channel, @Name, @Function, @Range, @Resolution, @AdvSettings, @Scale, @Gain, @Offset, @Label, @Test, @Low, @High, @HWAlarm";
                    command.Parameters.AddWithValue("ScanBatchID", batchid);
                    command.Parameters.AddWithValue("Channel", channel.Channel);
                    command.Parameters.AddWithValue("Name", channel.Name);
                    command.Parameters.AddWithValue("Function", channel.Function);
                    command.Parameters.AddWithValue("Range", channel.Range);
                    command.Parameters.AddWithValue("Resolution", channel.Resolution);
                    command.Parameters.AddWithValue("AdvSettings", channel.AdvSettings);
                    command.Parameters.AddWithValue("Scale", channel.Scale);
                    command.Parameters.AddWithValue("Gain", channel.Gain);
                    command.Parameters.AddWithValue("Offset", channel.Offset);
                    command.Parameters.AddWithValue("Label", channel.Label);
                    command.Parameters.AddWithValue("Test", channel.Test);
                    command.Parameters.AddWithValue("Low", channel.Low);
                    command.Parameters.AddWithValue("High", channel.High);
                    command.Parameters.AddWithValue("HWAlarm", channel.HWAlarm);

                    channelIDs.Add(channel.Channel, Int32.Parse(SQLAccess.GetScalar(command, connection)));
                }

                foreach (Scan scan in batch.Scans)
                {
                    command = connection.CreateCommand();
                    command.Parameters.Clear();
                    command.CommandText = "exec dbo.sp_CreateScan @ScanChannelID, @ScanCycle, @ScanTime, @Result, @Alarm";
                    command.Parameters.AddWithValue("ScanChannelID", channelIDs[scan.Channel]);
                    command.Parameters.AddWithValue("ScanCycle", scan.Cycle);
                    command.Parameters.AddWithValue("ScanTime", scan.ScanTime);
                    command.Parameters.AddWithValue("Result", scan.Result);
                    command.Parameters.AddWithValue("Alarm", scan.Alarm);

                    SQLAccess.NonQuery(command, connection);
                }

                success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return success;
        }
    }

    public class ScanInstrument
    {
        #region Properties

        public string Name { get; set; }
        public string Address { get; set; }
        public int NumModules { get; set; }
        public Dictionary<int, string> Modules;

        #endregion

        #region Constructors

        public ScanInstrument()
        {
            Name = "";
            Address = "";
            NumModules = 0;
            Modules = new Dictionary<int, string>();
        }

        #endregion
    }

    public class ScanChannel
    {
        #region Properties

        public string Channel { get; set; }
        public string Name { get; set; }
        public string Function { get; set; }
        public string Range { get; set; }
        public string Resolution { get; set; }
        public string AdvSettings { get; set; }
        public string Scale { get; set; }
        public string Gain { get; set; }
        public string Offset { get; set; }
        public string Label { get; set; }
        public string Test { get; set; }
        public string Low { get; set; }
        public string High { get; set; }
        public string HWAlarm { get; set; }

        #endregion

        #region Constructors

        public ScanChannel()
        {
            Channel = "";
            Name = "";
            Function = "";
            Range = "";
            Resolution = "";
            AdvSettings = "";
            Scale = "";
            Gain = "";
            Offset = "";
            Label = "";
            Test = "";
            Low = "";
            High = "";
            HWAlarm = "";
        }

        #endregion
    }

    public class ScanControl
    {
        #region Properties

        public string StartAction { get; set; }
        public string EndAction { get; set; }

        #endregion

        #region Constructors

        public ScanControl()
        {
            StartAction = "";
            EndAction = "";
        }

        #endregion
    }

    public class ScanBatch
    {
        #region Properties

        // Reflection is used to determine fields to parse out of the data file, which goes on a line by line basis
        // So the order of properties here must match the order of lines in the data file

        public string Name { get; set; }
        public string Owner { get; set; }
        public string Comments { get; set; }
        public string AcquisitionDate { get; set; }
        public ScanInstrument Instrument { get; set; }
        public List<ScanChannel> Channels { get; set; }
        public ScanControl ScanControl { get; set; }
        public List<Scan> Scans { get; set; }

        #endregion

        #region Constructors

        public ScanBatch()
        {
            Name = "";
            Owner = "";
            Comments = "";
            AcquisitionDate = "";
            Instrument = new ScanInstrument();
            Channels = new List<ScanChannel>();
            ScanControl = new ScanControl();
            Scans = new List<Scan>();
        }

        #endregion
    }

    public class Scan
    {
        #region Properties

        public int Cycle { get; set; }
        public string ScanTime { get; set; }
        public string Channel { get; set; }
        public string Result { get; set; }
        public Boolean Alarm { get; set; }

        #endregion

        #region Constructor

        public Scan()
        {
            Cycle = 0;
            ScanTime = "";
            Channel = "";
            Result = "";
            Alarm = false;
        }

        #endregion
    }

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

        public static string GetScalar(SqlCommand command, SqlConnection connection)
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

        public static SqlConnection BeginTransaction(string server, string database)
        {
            SqlConnection sqlConnection = new SqlConnection(String.Format("Data Source = {0}; Initial Catalog = {1}; Integrated Security = SSPI;", server, database));

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
    }

}
