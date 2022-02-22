using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace ShopCapacity
{
    public partial class ShopCapacity : Form
    {
        private CapacityOneDay.CapacityOneDay[] aryDays = new CapacityOneDay.CapacityOneDay[43];
        private int intMaxDoors;
        private int intMaxFrames;
        private int intGreenToYellowDoors;
        private int intGreenToYellowFrames;
        private int[] aryCapCalMaxDoors = new int[46];
        private int[] aryCapCalMaxFrames = new int[46];
        private TextBox[] txtTotalDoors = new TextBox[6];
        private TextBox[] txtTotalFrames = new TextBox[6];
        private string company = ConfigurationManager.AppSettings["company"];
        string server = "SARV-SQLDEV01";
        string database = "EpicorDB_PG";

        public ShopCapacity()
        {
            InitializeComponent();
        }

        private void ShopCapacity_Load(object sender, EventArgs e)
        {
            SqlConnection conn;
            SqlCommand command;
            SqlDataReader reader;
   
            
            conn = new SqlConnection("Data Source=" + server + ";Database=" + database + ";Integrated Security=SSPI");
            command = new SqlCommand("[dbo].sp_CapacityInfo", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 15).Value = company;
            command.Parameters.Add("@Function", SqlDbType.VarChar, 15).Value = "Set";
            conn.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                intMaxDoors = System.Convert.ToInt32(System.Convert.ToDecimal((reader["maxdoors"].ToString())).ToString("N0"));
                intMaxFrames = System.Convert.ToInt32(System.Convert.ToDecimal((reader["maxframes"].ToString())).ToString("N0"));
                intGreenToYellowDoors = System.Convert.ToInt32(System.Convert.ToDecimal((reader["gtoydoors"].ToString())).ToString("N0"));
                intGreenToYellowFrames = System.Convert.ToInt32(System.Convert.ToDecimal((reader["gtoyframes"].ToString())).ToString("N0"));
            }
            reader.Dispose();
            command.Dispose();
            conn.Close();

            command = new SqlCommand("[dbo].sp_CapacityInfo", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 15).Value = company;
            command.Parameters.Add("@Function", SqlDbType.VarChar, 15).Value = "Cal";
            conn.Open();
            reader = command.ExecuteReader();
            int count = 1;
            while (reader.Read())
            {
                aryCapCalMaxDoors[count] = System.Convert.ToInt32(System.Convert.ToDecimal((reader["maxdoors"].ToString())).ToString("N0"));
                aryCapCalMaxFrames[count] = System.Convert.ToInt32(System.Convert.ToDecimal((reader["maxframes"].ToString())).ToString("N0"));
                count++;
            }
            reader.Dispose();
            command.Dispose();
            conn.Close();


            int intIndex = 1;
            for (int intY = 0; intY <= 5; intY++)
            {
                for (int intX = 0; intX <= 6; intX++)
                {
                    aryDays[intIndex] = new CapacityOneDay.CapacityOneDay();
                    if (intX == 5 || intX == 6)
                    {
                        aryDays[intIndex].MaxDoors = 0;
                        aryDays[intIndex].MaxFrames = 0;
                    }
                    else
                    {
                        aryDays[intIndex].MaxDoors = intMaxDoors;
                        aryDays[intIndex].MaxFrames = intMaxFrames;
                    }
                    aryDays[intIndex].GreenToYellowDoors = intGreenToYellowDoors;
                    aryDays[intIndex].GreenToYellowFrames = intGreenToYellowFrames;
                    aryDays[intIndex].Location = new Point(37+71*intX, 27+76*intY);
                    intIndex++;
                }
                txtTotalDoors[intY] = new TextBox();
                txtTotalFrames[intY] = new TextBox();
                txtTotalDoors[intY].ReadOnly = true;
                txtTotalFrames[intY].ReadOnly = true;
                txtTotalDoors[intY].Location = new Point(47 + 71 * 7, 53 + 76 * intY);
                txtTotalFrames[intY].Location = new Point(47 + 71 * 7, 77 + 76 * intY);
                txtTotalDoors[intY].Width = 55;
                txtTotalFrames[intY].Width = 55;
                txtTotalDoors[intY].TabStop = false;
                txtTotalFrames[intY].TabStop = false;
                this.Controls.Add(txtTotalDoors[intY]);
                this.Controls.Add(txtTotalFrames[intY]);
            }
            //
            RefreshDays();
            //
            for (int intCount = 1; intCount <= 42; intCount++)
            {
                this.Controls.Add(aryDays[intCount]);
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDays();
        }

        private void RefreshDays()
        {
            SqlConnection conn;
            SqlCommand command;
            SqlDataReader reader;
            conn = new SqlConnection("Data Source=" + server + ";Database=" + database + ";Integrated Security=SSPI");
            command = new SqlCommand("[dbo].sp_JobAsmbl_FrameandDoor_Counts", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 15).Value = company;
            conn.Open();
            reader = command.ExecuteReader();
            lblLastUpdate.Text = "Last Refresh:" + DateTime.Now.ToString("M/d/yyyy HH:mm:ss");
            int intSuper7 = 1;
            byte isFirst = 1;
            int intIndex = 0;
            int intCapCalIndex = 1;
            DateTime dteDay;
            while (reader.Read())
            {
                string[] strQtys = new string[4];
                strQtys[0] = reader["DayDate"].ToString();
                strQtys[1] = System.Convert.ToDecimal((reader["doorqty"].ToString())).ToString(/*"N0"*/);
                strQtys[2] = System.Convert.ToDecimal((reader["frameqty"].ToString())).ToString(/*"N0"*/);
                strQtys[3] = System.Convert.ToDecimal((reader["frmdoorqty"].ToString())).ToString(/*"N0"*/);
                dteDay = DateTime.Parse(strQtys[0]);
                int intDayOfWeek = (int)dteDay.DayOfWeek;
                if (intIndex == 0)
                {
                    for (intIndex = 1; intIndex < intDayOfWeek; intIndex++)
                    {
                        aryDays[intIndex].IsHidden = 1;
                    }
                }

                if (aryCapCalMaxDoors[intCapCalIndex] != -1)
                {
                    aryDays[intIndex].MaxDoors = aryCapCalMaxDoors[intCapCalIndex];
                }
                if (aryCapCalMaxFrames[intCapCalIndex] != -1)
                {
                    aryDays[intIndex].MaxFrames = aryCapCalMaxFrames[intCapCalIndex];
                }

                aryDays[intIndex].DisplayDate = dteDay;
                aryDays[intIndex].Doors = Convert.ToInt32(Decimal.Parse(strQtys[1]));
                aryDays[intIndex].Frames = Convert.ToInt32(Decimal.Parse(strQtys[2]));
                aryDays[intIndex].FrameDoorOpens = Convert.ToInt32(Decimal.Parse(strQtys[3]));

                if (isFirst != 1 && intSuper7 <= 7 && intDayOfWeek != 6 && intDayOfWeek != 0 && aryCapCalMaxDoors[intCapCalIndex] != 0 && aryCapCalMaxFrames[intCapCalIndex] != 0)
                {
                    aryDays[intIndex].InSuper7 = 1;
                    intSuper7++;
                }

                intIndex++;
                intCapCalIndex++;
                isFirst = 0;
                if (intIndex > 42)
                {
                    break;
                }
            }
            reader.Dispose();
            command.Dispose();
            conn.Close();

            //update totals
            int intWeekDoorTotal;
            int intWeekFrameTotal;
            for (int intY = 0; intY <= 5; intY++)
            {
                intWeekDoorTotal = 0;
                intWeekFrameTotal = 0;
                for (int intX = 1; intX <= 7; intX++)
                {
                    intWeekDoorTotal += aryDays[intX + intY * 7].Doors;
                    intWeekFrameTotal += aryDays[intX + intY * 7].Frames;
                }
                txtTotalDoors[intY].Text = intWeekDoorTotal.ToString("N0");
                txtTotalFrames[intY].Text = intWeekFrameTotal.ToString("N0");
            }

        }
    }



}
