using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace ObjectLibrary
{
    #region Class SettingThreshold

    public class SettingThreshold
    {
        #region Properties

        public string Low { get; set; }
        public string High { get; set; }

        #endregion

        #region Constructors

        public SettingThreshold()
        {
            Low = "";
            High = "";
        }

        public SettingThreshold(string low, string high)
        {
            Low = low;
            High = high;
        }

        #endregion
    }

    #endregion

    #region Class Revision

    public class Revision
    {
        #region Properties

        public int Number { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public bool IsActive { get; set; }

        #endregion

        #region Constructors

        public Revision()
        {
            Number = 0;
            Name = "";
            Comment = "";
            IsActive = false;
        }

        public Revision(int number, string name, string comment, bool isactive)
        {
            Number = number;
            Name = name;
            Comment = comment;
            IsActive = isactive;
        }

        #endregion
    }

    #endregion

    #region Class PowerAnalyzerSetting

    public class PowerAnalyzerSetting
    {
        #region Properties

        public string TemperatureCode { get; set; }
        public string ModelCode { get; set; }
        public int NumberDoors { get; set; }
        public string FrameTypeCode { get; set; }
        public int LowVoltage { get; set; }
        public int HighVoltage { get; set; }
        public bool IsDoor { get; set; }
        public bool IsFrame { get; set; }
        public string Item { get; set; }
        public string DoorFrameWire { get; set; }
        public SettingThreshold DoorFrameOhms { get; set; }
        public SettingThreshold DoorGlassOhms { get; set; }
        public SettingThreshold DoorDoorOhms { get; set; }
        public SettingThreshold DoorDoorAmps { get; set; }
        public string FrameWrap1Wire { get; set; }
        public SettingThreshold FrameWrap1Ohms { get; set; }
        public string FrameWrap2Wire { get; set; }
        public SettingThreshold FrameWrap2Ohms { get; set; }
        public SettingThreshold FrameWrapOhms { get; set; }
        public string FrameMullionWire { get; set; }
        public SettingThreshold FrameMullionOhms { get; set; }
        public string FrameStainlessWire { get; set; }
        public SettingThreshold FrameStainlessOhms { get; set; }
        public SettingThreshold FrameTotalMullionOhms { get; set; }
        public SettingThreshold FrameTotalStainlessOhms { get; set; }
        public SettingThreshold FrameOhms { get; set; }
        public SettingThreshold FrameAmps { get; set; }
        public SettingThreshold LightAmps { get; set; }
        public SettingThreshold SummaryFrameAndLightAmps { get; set; }
        public SettingThreshold SummaryDoorAndFrameAmps { get; set; }
        public SettingThreshold SummaryDoorAndFrameAndLampAmps { get; set; }
        public string SummaryMaxAmpsHeater { get; set; }
        public string SummaryMaxAmpsLights { get; set; }
        public string SummaryMaxAmpsTotal { get; set; }
        public string SummaryRatedAmpsHeater { get; set; }
        public string SummaryRatedAmpsLights { get; set; }

        #endregion

        #region Constructors

        public PowerAnalyzerSetting()
        {
            TemperatureCode = "";
            ModelCode = "";
            NumberDoors = 0;
            FrameTypeCode = "";
            LowVoltage = 0;
            HighVoltage = 0;
            IsDoor = false;
            IsFrame = false;
            Item = "";
            DoorFrameWire = "";
            DoorFrameOhms = new SettingThreshold();
            DoorGlassOhms = new SettingThreshold();
            DoorDoorOhms = new SettingThreshold();
            DoorDoorAmps = new SettingThreshold();
            FrameWrap1Wire = "";
            FrameWrap1Ohms = new SettingThreshold();
            FrameWrap2Wire = "";
            FrameWrap2Ohms = new SettingThreshold();
            FrameWrapOhms = new SettingThreshold();
            FrameMullionWire = "";
            FrameMullionOhms = new SettingThreshold();
            FrameStainlessWire = "";
            FrameStainlessOhms = new SettingThreshold();
            FrameTotalMullionOhms = new SettingThreshold();
            FrameTotalStainlessOhms = new SettingThreshold();
            FrameOhms = new SettingThreshold();
            FrameAmps = new SettingThreshold();
            LightAmps = new SettingThreshold();
            SummaryFrameAndLightAmps = new SettingThreshold();
            SummaryDoorAndFrameAmps = new SettingThreshold();
            SummaryDoorAndFrameAndLampAmps = new SettingThreshold();
            SummaryMaxAmpsHeater = "";
            SummaryMaxAmpsLights = "";
            SummaryMaxAmpsTotal = "";
            SummaryRatedAmpsHeater = "";
            SummaryRatedAmpsLights = "";
        }

        public PowerAnalyzerSetting(string tempcode, string modelcode, int numdoors, string frametypecode, int lowvoltage, int highvoltage, bool isdoor, bool isframe, string item, string doorframewire, string doorframeohmslow, string doorframeohmshigh,
            string doorglassohmslow, string doorglassohmshigh, string doordoorohmslow, string doordoorohmshigh, string doordoorampslow, string doordoorampshigh, string framewrap1wire, string framewrap1ohmslow, string framewrap1ohmshigh,
            string framewrap2wire, string framewrap2ohmslow, string framewrap2ohmshigh, string framewrapohmslow, string framewrapohmshigh, string framemullionwire, string framemullionohmslow, string framemullionohmshigh, string framestainlesswire,
            string framestainlessohmslow, string framestainlessohmshigh, string frametotalmullionohmslow, string frametotalmullionohmshigh, string frametotalstainlessohmslow, string frametotalstainlessohmshigh, string frameohmslow, string frameohmshigh,
            string frameampslow, string frameampshigh, string lightampslow, string lightampshigh, string summaryframeandlightampslow, string summaryframeandlightampshigh, string summarydoorandframeampslow, string summarydoorandframeampshigh,
            string summarydoorandframeandlampampslow, string summarydoorandframeandlampampshigh, string summarymaxampsheater, string summarymaxampslights, string summarymaxampstotal,
            string summaryratedampsheater, string summaryratedampslights)
        {
            TemperatureCode = tempcode;
            ModelCode = modelcode;
            NumberDoors = numdoors;
            FrameTypeCode = frametypecode;
            LowVoltage = lowvoltage;
            HighVoltage = highvoltage;
            IsDoor = isdoor;
            IsFrame = isframe;
            Item = item;
            DoorFrameWire = doorframewire;
            DoorFrameOhms = new SettingThreshold(doorframeohmslow, doorframeohmshigh);
            DoorGlassOhms = new SettingThreshold(doorglassohmslow, doorglassohmshigh);
            DoorDoorOhms = new SettingThreshold(doordoorohmslow, doordoorohmshigh);
            DoorDoorAmps = new SettingThreshold(doordoorampslow, doordoorampshigh);
            FrameWrap1Wire = framewrap1wire;
            FrameWrap1Ohms = new SettingThreshold(framewrap1ohmslow, framewrap1ohmshigh);
            FrameWrap2Wire = framewrap2wire;
            FrameWrap2Ohms = new SettingThreshold(framewrap2ohmslow, framewrap2ohmshigh);
            FrameWrapOhms = new SettingThreshold(framewrapohmslow, framewrapohmshigh);
            FrameMullionWire = framemullionwire;
            FrameMullionOhms = new SettingThreshold(framemullionohmslow, framemullionohmshigh);
            FrameStainlessWire = framestainlesswire;
            FrameStainlessOhms = new SettingThreshold(framestainlessohmslow, framestainlessohmshigh);
            FrameTotalMullionOhms = new SettingThreshold(frametotalmullionohmslow, frametotalmullionohmshigh);
            FrameTotalStainlessOhms = new SettingThreshold(frametotalstainlessohmslow, frametotalstainlessohmshigh);
            FrameOhms = new SettingThreshold(frameohmslow, frameohmshigh);
            FrameAmps = new SettingThreshold(frameampslow, frameampshigh);
            LightAmps = new SettingThreshold(lightampslow, lightampshigh);
            SummaryFrameAndLightAmps = new SettingThreshold(summaryframeandlightampslow, summaryframeandlightampshigh);
            SummaryDoorAndFrameAmps = new SettingThreshold(summarydoorandframeampslow, summarydoorandframeampshigh);
            SummaryDoorAndFrameAndLampAmps = new SettingThreshold(summarydoorandframeandlampampslow, summarydoorandframeandlampampshigh);
            SummaryMaxAmpsHeater = summarymaxampsheater;
            SummaryMaxAmpsLights = summarymaxampslights;
            SummaryMaxAmpsTotal = summarymaxampstotal;
            SummaryRatedAmpsHeater = summaryratedampsheater;
            SummaryRatedAmpsLights = summaryratedampslights;
        }

        #endregion
    }

    #endregion

    public class PowerAnalyzerInterface
    {
        #region Public Methods

        #region Utility Methods

        public static string ParseDoorCountCode(string partnum)
        {
            StringBuilder doorcount = new StringBuilder();

            if (partnum.Substring(0, 2) == "00")
                doorcount.Append("01");
            else
            {
                /*if (partnum[4] == '-')
                    doorcount.Append(partnum.Substring(1, 1));
                else
                {
                    if (partnum[0] == '0')
                        doorcount.Append(partnum.Substring(1, 1));
                    else*/
                    doorcount.Append(partnum.Substring(0, 2));
                    
                //}
            }

              

            return doorcount.ToString();
        }

        public static string ParseModelCode(string partnum)
        {
            StringBuilder model = new StringBuilder();

            model.Append(partnum.Substring(2, 2));
            if (partnum.Substring(4, 1) == "-")
            {
                if (model.ToString()[0] == 'W')
                    model.Append("-");
                else
                    model.Append(partnum.Substring(9, 1));
            }

                return model.ToString();
        }

        public static string ParseLineCode(string partnum)
        {
            StringBuilder line = new StringBuilder();
            if (partnum.Substring(4,1) == "-")
                line.Append(partnum.Substring(5,2));
            else
                line.Append(partnum.Substring(4, 2));
            if (partnum.Substring(2, 1) == "W")
                line.Append(partnum.Substring(9, 1));
            return line.ToString();
        }

        public static string ParseFrameCode(string partnum)
        {
            try
            {
                StringBuilder frame = new StringBuilder();

                if (partnum.Substring(0, 2) == "00")
                {
                    if (partnum[2] == 'W')
                        frame.Append(partnum.Substring(10, 3));
                    else
                        frame.Append("NLX");
                }
                else if (partnum.IndexOf('-') < 0)
                    frame.Append("");
                else if (partnum.IndexOf('-') == partnum.LastIndexOf('-'))
                    frame.Append(partnum.Substring(partnum.IndexOf('-') + 1, 2));
                else if (partnum[4] == 'M' && partnum.Count(f => f == '-') == 3)
                {
                    string[] sections = partnum.Split(new char[] { '-' });
                    frame.Append(sections[1][0]);
                    frame.Append(sections[2][0]);
                    if (sections[3].IndexOf('X') >= 0)
                    {
                        if (sections[1][0] == 'L')
                            frame.Append(sections[3][sections[3].IndexOf('X') - 1]);
                        else if (sections[1][0] == 'C' || sections[1][0] == 'F')
                        {
                            int index = sections[3].IndexOf('X');
                            if ((index + 1) == sections[3].Length || sections[3][index + 1] == 'X')
                                frame.Append(sections[3][index - 1]);
                            else
                                frame.Append(sections[3][index + 1]);
                        }
                        else
                            frame.Append(sections[3][sections[3].LastIndexOf('X') + 1]);
                    }
                    else
                        frame.Append(sections[3][0].ToString());
                }
                else if (partnum[4] == '-')
                    //frame.Append(partnum.Substring(10, 2)); OLD EDT
                frame.Append(partnum.Substring(10, 3)); //NEW EDT
                else
                    frame.Append(partnum.Substring(partnum.IndexOf('-') + 1, partnum.IndexOf('-', partnum.IndexOf('-') + 1) - partnum.IndexOf('-') - 1));

                return frame.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Error - ParseFrameCode(" + (String.IsNullOrEmpty(partnum) ? "null" : partnum) + ") - " + ex.Message + " - " + ex.StackTrace);
            }
        }

        public static string ParseVoltage(string partnum)
        {
            StringBuilder voltage = new StringBuilder();

            if (partnum.EndsWith("230"))
                voltage.Append("230");
            else
                voltage.Append("110");

            return voltage.ToString();
        }


        #endregion

        #region Create Methods

        public void CreatePowerTableRevision(string server, string database, string username, string password, List<PowerAnalyzerSetting> settings, string revisionComment)
        {
            SqlConnection connection = new SqlConnection();
            string lastlow = "";
            string lasthigh = "";
            try
            {
                connection = SQLAccess.SQLAccess.BeginTransaction(server, database, username, password);

                SqlCommand command = new SqlCommand("exec dbo.sp_CreateRevision @Description");
                command.Parameters.AddWithValue("Description", revisionComment);

                int revid = Int32.Parse(SQLAccess.SQLAccess.GetScalar(server, database, username, password, command, connection));

                command = new SqlCommand("exec dbo.sp_CreatePowerTableEntry @Model, @Temperature, @LowVoltage, @HighVoltage, @FrameType, @Revision, @DoorCount, @IsDoor, @IsFrame, @Item, @drfrwire, @drfrohlo, @drfrohhi, @drglohlo, @drglohhi, @drdrohlo, @drdrohhi," +
                "@drdramlo, @drdramhi, @frw1wire, @frw1ohlo, @frw1ohhi, @frw2wire, @frw2ohlo, @frw2ohhi, @frfwohlo, @frfwohhi, @frmuwire, @frmuohlo, @frmuohhi, @frstwire, @frstohlo, @frstohhi, @frtmohlo, @frtmohhi, @frtsohlo, @frtsohhi," +
                "@frtfohlo, @frtfohhi, @frtfamlo, @frtfamhi, @ltampslo, @ltampshi, @suflamlo, @suflamhi, @sudfamlo, @sudfamhi, @sudlamlo, @sudlamhi, @sumxamhe, @sumxamlt, @sumxamto, @surtamhe, @surtamlt");

                foreach (PowerAnalyzerSetting setting in settings)
                {
                    lastlow = setting.LowVoltage.ToString();
                    lasthigh = setting.HighVoltage.ToString();
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("Model", setting.ModelCode.PadLeft(2, '0'));
                    command.Parameters.AddWithValue("Temperature", setting.TemperatureCode);
                    command.Parameters.AddWithValue("LowVoltage", setting.LowVoltage);
                    command.Parameters.AddWithValue("HighVoltage", setting.HighVoltage);
                    command.Parameters.AddWithValue("FrameType", setting.FrameTypeCode);
                    command.Parameters.AddWithValue("Revision", revid);
                    command.Parameters.AddWithValue("DoorCount", String.Format("{0:00}", setting.NumberDoors));
                    command.Parameters.AddWithValue("IsDoor", setting.IsDoor);
                    command.Parameters.AddWithValue("IsFrame", setting.IsFrame);
                    command.Parameters.AddWithValue("Item", setting.Item);

                    decimal d;
                    command.Parameters.AddWithValue("drfrwire", Decimal.TryParse(setting.DoorFrameWire, out d) ? String.Format("{0:0.0}", d) : setting.DoorFrameWire);
                    command.Parameters.AddWithValue("drfrohlo", Decimal.TryParse(setting.DoorFrameOhms.Low, out d) ? String.Format("{0:0.0}", d) : setting.DoorFrameOhms.Low);
                    command.Parameters.AddWithValue("drfrohhi", Decimal.TryParse(setting.DoorFrameOhms.High, out d) ? String.Format("{0:0.0}", d) : setting.DoorFrameOhms.High);
                    command.Parameters.AddWithValue("drglohlo", Decimal.TryParse(setting.DoorGlassOhms.Low, out d) ? String.Format("{0:0.0}", d) : setting.DoorGlassOhms.Low);
                    command.Parameters.AddWithValue("drglohhi", Decimal.TryParse(setting.DoorGlassOhms.High, out d) ? String.Format("{0:0.0}", d) : setting.DoorGlassOhms.High);
                    command.Parameters.AddWithValue("drdrohlo", Decimal.TryParse(setting.DoorDoorOhms.Low, out d) ? String.Format("{0:0.0}", d) : setting.DoorDoorOhms.Low);
                    command.Parameters.AddWithValue("drdrohhi", Decimal.TryParse(setting.DoorDoorOhms.High, out d) ? String.Format("{0:0.0}", d) : setting.DoorDoorOhms.High);
                    command.Parameters.AddWithValue("drdramlo", Decimal.TryParse(setting.DoorDoorAmps.Low, out d) ? String.Format("{0:0.00}", d) : setting.DoorDoorAmps.Low);
                    command.Parameters.AddWithValue("drdramhi", Decimal.TryParse(setting.DoorDoorAmps.High, out d) ? String.Format("{0:0.00}", d) : setting.DoorDoorAmps.High);
                    command.Parameters.AddWithValue("frw1wire", Decimal.TryParse(setting.FrameWrap1Wire, out d) ? String.Format("{0:0.0}", d) : setting.FrameWrap1Wire);
                    command.Parameters.AddWithValue("frw1ohlo", Decimal.TryParse(setting.FrameWrap1Ohms.Low, out d) ? String.Format("{0:0.0}", d) : setting.FrameWrap1Ohms.Low);
                    command.Parameters.AddWithValue("frw1ohhi", Decimal.TryParse(setting.FrameWrap1Ohms.High, out d) ? String.Format("{0:0.0}", d) : setting.FrameWrap1Ohms.High);
                    command.Parameters.AddWithValue("frw2wire", Decimal.TryParse(setting.FrameWrap2Wire, out d) ? String.Format("{0:0.0}", d) : setting.FrameWrap2Wire);
                    command.Parameters.AddWithValue("frw2ohlo", Decimal.TryParse(setting.FrameWrap2Ohms.Low, out d) ? String.Format("{0:0.0}", d) : setting.FrameWrap2Ohms.Low);
                    command.Parameters.AddWithValue("frw2ohhi", Decimal.TryParse(setting.FrameWrap2Ohms.High, out d) ? String.Format("{0:0.0}", d) : setting.FrameWrap2Ohms.High);
                    command.Parameters.AddWithValue("frfwohlo", Decimal.TryParse(setting.FrameWrapOhms.Low, out d) ? String.Format("{0:0.0}", d) : setting.FrameWrapOhms.Low);
                    command.Parameters.AddWithValue("frfwohhi", Decimal.TryParse(setting.FrameWrapOhms.High, out d) ? String.Format("{0:0.0}", d) : setting.FrameWrapOhms.High);
                    command.Parameters.AddWithValue("frmuwire", Decimal.TryParse(setting.FrameMullionWire, out d) ? String.Format("{0:0.0}", d) : setting.FrameMullionWire);
                    command.Parameters.AddWithValue("frmuohlo", Decimal.TryParse(setting.FrameMullionOhms.Low, out d) ? String.Format("{0:0.0}", d) : setting.FrameMullionOhms.Low);
                    command.Parameters.AddWithValue("frmuohhi", Decimal.TryParse(setting.FrameMullionOhms.High, out d) ? String.Format("{0:0.0}", d) : setting.FrameMullionOhms.High);
                    command.Parameters.AddWithValue("frstwire", Decimal.TryParse(setting.FrameStainlessWire, out d) ? String.Format("{0:0.0}", d) : setting.FrameStainlessWire);
                    command.Parameters.AddWithValue("frstohlo", Decimal.TryParse(setting.FrameStainlessOhms.Low, out d) ? String.Format("{0:0.0}", d) : setting.FrameStainlessOhms.Low);
                    command.Parameters.AddWithValue("frstohhi", Decimal.TryParse(setting.FrameStainlessOhms.High, out d) ? String.Format("{0:0.0}", d) : setting.FrameStainlessOhms.High);
                    command.Parameters.AddWithValue("frtmohlo", Decimal.TryParse(setting.FrameTotalMullionOhms.Low, out d) ? String.Format("{0:0.0}", d) : setting.FrameTotalMullionOhms.Low);
                    command.Parameters.AddWithValue("frtmohhi", Decimal.TryParse(setting.FrameTotalMullionOhms.High, out d) ? String.Format("{0:0.0}", d) : setting.FrameTotalMullionOhms.High);
                    command.Parameters.AddWithValue("frtsohlo", Decimal.TryParse(setting.FrameTotalStainlessOhms.Low, out d) ? String.Format("{0:0.0}", d) : setting.FrameTotalStainlessOhms.Low);
                    command.Parameters.AddWithValue("frtsohhi", Decimal.TryParse(setting.FrameTotalStainlessOhms.High, out d) ? String.Format("{0:0.0}", d) : setting.FrameTotalStainlessOhms.High);
                    command.Parameters.AddWithValue("frtfohlo", Decimal.TryParse(setting.FrameOhms.Low, out d) ? String.Format("{0:0.0}", d) : setting.FrameOhms.Low);
                    command.Parameters.AddWithValue("frtfohhi", Decimal.TryParse(setting.FrameOhms.High, out d) ? String.Format("{0:0.0}", d) : setting.FrameOhms.High);
                    command.Parameters.AddWithValue("frtfamlo", Decimal.TryParse(setting.FrameAmps.Low, out d) ? String.Format("{0:0.00}", d) : setting.FrameAmps.Low);
                    command.Parameters.AddWithValue("frtfamhi", Decimal.TryParse(setting.FrameAmps.High, out d) ? String.Format("{0:0.00}", d) : setting.FrameAmps.High);
                    command.Parameters.AddWithValue("ltampslo", Decimal.TryParse(setting.LightAmps.Low, out d) ? String.Format("{0:0.00}", d) : setting.LightAmps.Low);
                    command.Parameters.AddWithValue("ltampshi", Decimal.TryParse(setting.LightAmps.High, out d) ? String.Format("{0:0.00}", d) : setting.LightAmps.High);
                    command.Parameters.AddWithValue("suflamlo", Decimal.TryParse(setting.SummaryFrameAndLightAmps.Low, out d) ? String.Format("{0:0.00}", d) : setting.SummaryFrameAndLightAmps.Low);
                    command.Parameters.AddWithValue("suflamhi", Decimal.TryParse(setting.SummaryFrameAndLightAmps.High, out d) ? String.Format("{0:0.00}", d) : setting.SummaryFrameAndLightAmps.High);
                    command.Parameters.AddWithValue("sudfamlo", Decimal.TryParse(setting.SummaryDoorAndFrameAmps.Low, out d) ? String.Format("{0:0.00}", d) : setting.SummaryDoorAndFrameAmps.Low);
                    command.Parameters.AddWithValue("sudfamhi", Decimal.TryParse(setting.SummaryDoorAndFrameAmps.High, out d) ? String.Format("{0:0.00}", d) : setting.SummaryDoorAndFrameAmps.High);
                    command.Parameters.AddWithValue("sudlamlo", Decimal.TryParse(setting.SummaryDoorAndFrameAndLampAmps.Low, out d) ? String.Format("{0:0.00}", d) : setting.SummaryDoorAndFrameAndLampAmps.Low);
                    command.Parameters.AddWithValue("sudlamhi", Decimal.TryParse(setting.SummaryDoorAndFrameAndLampAmps.High, out d) ? String.Format("{0:0.00}", d) : setting.SummaryDoorAndFrameAndLampAmps.High);
                    command.Parameters.AddWithValue("sumxamhe", Decimal.TryParse(setting.SummaryMaxAmpsHeater, out d) ? String.Format("{0:0.00}", d) : setting.SummaryMaxAmpsHeater);
                    command.Parameters.AddWithValue("sumxamlt", Decimal.TryParse(setting.SummaryMaxAmpsLights, out d) ? String.Format("{0:0.00}", d) : setting.SummaryMaxAmpsLights);
                    command.Parameters.AddWithValue("sumxamto", Decimal.TryParse(setting.SummaryMaxAmpsTotal, out d) ? String.Format("{0:0.00}", d) : setting.SummaryMaxAmpsTotal);
                    command.Parameters.AddWithValue("surtamhe", Decimal.TryParse(setting.SummaryRatedAmpsHeater, out d) ? String.Format("{0:0.00}", d) : setting.SummaryRatedAmpsHeater);
                    command.Parameters.AddWithValue("surtamlt", Decimal.TryParse(setting.SummaryRatedAmpsLights, out d) ? String.Format("{0:0.00}", d) : setting.SummaryRatedAmpsLights);

                    SQLAccess.SQLAccess.NonQuery(command, connection);
                }

                command = new SqlCommand("exec dbo.sp_ActivateRevision @RevisionID");
                command.Parameters.AddWithValue("RevisionID", revid);
                SQLAccess.SQLAccess.NonQuery(command, connection);

                SQLAccess.SQLAccess.CommitTransaction(connection);
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    SQLAccess.SQLAccess.RollbackTransaction(connection);

                throw new Exception(server + " - " + lastlow + ", " + lasthigh + " - " + ex.Message);
            }
        }

        #endregion

        #region Retrieve Methods

        public List<Revision> GetRevisions(string server, string database, string username, string password)
        {
            List<Revision> result = new List<Revision>();
            try
            {
                SqlCommand command = new SqlCommand("exec dbo.sp_GetRevisions");
                DataSet ds = SQLAccess.SQLAccess.GetDataSet(server, database, username, password, command);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(new Revision(Int32.Parse(row["number"].ToString()), row["name"].ToString(), row["comment"].ToString(), Boolean.Parse(row["active"].ToString())));

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PowerAnalyzerSetting> GetPowerTable(string server, string database, string username, string password)
        {
            List<PowerAnalyzerSetting> result = new List<PowerAnalyzerSetting>();
            try
            {
                SqlCommand command = new SqlCommand("exec dbo.sp_GetPowerTable");
                DataSet ds = SQLAccess.SQLAccess.GetDataSet(server, database, username, password, command);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(new PowerAnalyzerSetting(row["tempcode"].ToString(), row["modelcode"].ToString(), Int32.Parse(row["number"].ToString()), row["frametypecode"].ToString(), Int32.Parse(row["lowvoltage"].ToString()), Int32.Parse(row["highvoltage"].ToString()), Boolean.Parse(row["isdoor"].ToString()), Boolean.Parse(row["isframe"].ToString()), row["item"].ToString(), row["drfrwire"].ToString(), row["drfrohlo"].ToString(), row["drfrohhi"].ToString(), row["drglohlo"].ToString(), row["drglohhi"].ToString(), row["drdrohlo"].ToString(), row["drdrohhi"].ToString(), row["drdramlo"].ToString(), row["drdramhi"].ToString(), row["frw1wire"].ToString(), row["frw1ohlo"].ToString(), row["frw1ohhi"].ToString(), row["frw2wire"].ToString(), row["frw2ohlo"].ToString(), row["frw2ohhi"].ToString(), row["frfwohlo"].ToString(), row["frfwohhi"].ToString(), row["frmuwire"].ToString(), row["frmuohlo"].ToString(), row["frmuohhi"].ToString(), row["frstwire"].ToString(), row["frstohlo"].ToString(), row["frstohhi"].ToString(), row["frtmohlo"].ToString(), row["frtmohhi"].ToString(), row["frtsohlo"].ToString(), row["frtsohhi"].ToString(), row["frtfohlo"].ToString(), row["frtfohhi"].ToString(), row["frtfamlo"].ToString(), row["frtfamhi"].ToString(), row["ltampslo"].ToString(), row["ltampshi"].ToString(), row["suflamlo"].ToString(), row["suflamhi"].ToString(), row["sudfamlo"].ToString(), row["sudfamhi"].ToString(), row["sudlamlo"].ToString(), row["sudlamhi"].ToString(), row["sumxamhe"].ToString(), row["sumxamlt"].ToString(), row["sumxamto"].ToString(), row["surtamhe"].ToString(), row["surtamlt"].ToString()));

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<string, List<string>> GetTemperatureModel(string server, string database, string username, string password)
        {
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
            try
            {
                SqlCommand command = new SqlCommand("exec dbo.sp_GetTemperatureModel");
                DataSet ds = SQLAccess.SQLAccess.GetDataSet(server, database, username, password, command);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string temp = row["TempCode"].ToString();
                    string model = row["LineCode"].ToString();
                    if (!result.Keys.Contains(temp))
                        result.Add(temp, new List<string>());
                    result[temp].Add(model);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] GetAssemblyDetails(string server, string database, string username, string password, string company, string jobnum, int assemblyseq)
        {
            string[] result = new string[4];
            try
            {
                SqlCommand command = new SqlCommand("exec [dbo].sp_GetAssemblyDetails @Company, @Jobnum, @AssemblySeq");
                command.Parameters.AddWithValue("Company", company);
                command.Parameters.AddWithValue("Jobnum", jobnum);
                command.Parameters.AddWithValue("AssemblySeq", assemblyseq);
                DataSet ds = SQLAccess.SQLAccess.GetDataSet(server, database, username, password, command);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    result[0] = ds.Tables[0].Rows[0]["partnum"].ToString();
                    result[1] = ds.Tables[0].Rows[0]["description"].ToString();
                    result[2] = ds.Tables[0].Rows[0]["qty"].ToString();
                    result[3] = ds.Tables[0].Rows[0]["IsFrame"].ToString();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet GetRecordedData(string server, string database, string username, string password)
        {
            SqlCommand command = new SqlCommand("exec dbo.sp_GetRecordedData");
            command.CommandTimeout = 300;
            try
            {
                DataSet ds = SQLAccess.SQLAccess.GetDataSet(server, database, username, password, command);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #endregion
    }
}
