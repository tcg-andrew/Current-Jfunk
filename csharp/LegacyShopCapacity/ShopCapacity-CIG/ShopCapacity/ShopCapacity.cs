using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ShopCapacity
{
    public partial class ShopCapacity : Form
    {
        private CapacityOneDay.CapacityOneDay[] aryDays = new CapacityOneDay.CapacityOneDay[43];
        private int intMaxCut;
        private int intMaxTemper;
        private int intMaxUnit;
        private int intGreenToYellowCut;
        private int intGreenToYellowTemper;
        private int intGreenToYellowUnit;
        private int[] aryCapCalMaxCut = new int[46];
        private int[] aryCapCalMaxTemper = new int[46];
        private int[] aryCapCalMaxUnit = new int[46];
        private TextBox[] txtTotalCut = new TextBox[6];
        private TextBox[] txtTotalTemper = new TextBox[6];
        private TextBox[] txtTotalUnit = new TextBox[6];
        private Label[] lblCut = new Label[6];
        private Label[] lblTemper = new Label[6];
        private Label[] lblUnit = new Label[6];

        //        string server = "SARV-SQLPROD01";
        //        string database = "EpicorDB";
        string server = "SARV-SQLDEV01";
        string database = "EpicorDB_D";


        public ShopCapacity()
        {
            InitializeComponent();
        }

        private void ShopCapacity_Load(object sender, EventArgs e)
        {
            //SqlConnection conn;
            //SqlCommand command;
            //SqlDataReader reader;

            cmbPlant.Text = "TN";
            


            int intIndex = 1;
            for (int intY = 0; intY <= 5; intY++)
            {
                for (int intX = 0; intX <= 6; intX++)
                {
                    aryDays[intIndex] = new CapacityOneDay.CapacityOneDay();
                    if (intX == 5 || intX == 6)
                    {
                        aryDays[intIndex].MaxCut = 0;
                        aryDays[intIndex].MaxTemper = 0;
                        aryDays[intIndex].MaxUnit = 0;
                    }
                    else
                    {
                        aryDays[intIndex].MaxCut = intMaxCut;
                        aryDays[intIndex].MaxTemper = intMaxTemper;
                        aryDays[intIndex].MaxUnit = intMaxUnit;
                    }
                    aryDays[intIndex].GreenToYellowCut = intGreenToYellowCut;
                    aryDays[intIndex].GreenToYellowTemper = intGreenToYellowTemper;
                    aryDays[intIndex].GreenToYellowUnit = intGreenToYellowUnit;
                    aryDays[intIndex].Location = new Point(37 + 71 * intX, 27 + 101 * intY);
                    intIndex++;
                }
                txtTotalCut[intY] = new TextBox();
                txtTotalTemper[intY] = new TextBox();
                txtTotalUnit[intY] = new TextBox();

                lblCut[intY] = new Label();
                lblTemper[intY] = new Label();
                lblUnit[intY] = new Label();
                
                txtTotalCut[intY].ReadOnly = true;
                txtTotalTemper[intY].ReadOnly = true;
                txtTotalUnit[intY].ReadOnly = true;
                txtTotalCut[intY].Location = new Point(47 + 71 * 7, 53 + 101 * intY);
                lblCut[intY].Location = new Point(0, 55 + 101 * intY);
                txtTotalTemper[intY].Location = new Point(47 + 71 * 7, 77 + 101 * intY);
                lblTemper[intY].Location = new Point(0, 79 + 101 * intY);
                txtTotalUnit[intY].Location = new Point(47 + 71 * 7, 101 + 101 * intY);
                lblUnit[intY].Location = new Point(0, 103 + 101 * intY);
                lblCut[intY].Text = "Cut";
                lblTemper[intY].Text = "Temper";
                lblUnit[intY].Text = "Unit";
                txtTotalCut[intY].Width = 55;
                txtTotalTemper[intY].Width = 55;
                txtTotalUnit[intY].Width = 55;
                lblCut[intY].Width=35;
                lblTemper[intY].Width=35;
                lblUnit[intY].Width=35;
                txtTotalCut[intY].TabStop = false;
                txtTotalTemper[intY].TabStop = false;
                txtTotalUnit[intY].TabStop = false;
                this.Controls.Add(txtTotalCut[intY]);
                this.Controls.Add(txtTotalTemper[intY]);
                this.Controls.Add(txtTotalUnit[intY]);
                this.Controls.Add(lblCut[intY]);
                this.Controls.Add(lblTemper[intY]);
                this.Controls.Add(lblUnit[intY]);
            }
            //
            RefreshDays();
            //
            for (int intCount = 1; intCount <= 42; intCount++)
            {
                this.Controls.Add(aryDays[intCount]);
            }

            RefreshDays();

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDays();
            RefreshDays();
        }

        private void RefreshDays()
        {
            SqlConnection conn;
            SqlCommand command;
            SqlDataReader reader;
            int intIndex;



            conn = new SqlConnection("Data Source=" + server + ";Database=" + database + ";Integrated Security=SSPI");
            command = new SqlCommand("[dbo].sp_CapacityInfo_CIG", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 8).Value = "CIG";
            command.Parameters.Add("@Function", SqlDbType.VarChar, 3).Value = "Set";
            command.Parameters.Add("@Plant", SqlDbType.VarChar, 8).Value = cmbPlant.Text;
            conn.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                intMaxCut = System.Convert.ToInt32(reader["maxcut"].ToString());
                intMaxTemper = System.Convert.ToInt32(reader["maxtemper"].ToString());
                intMaxUnit = System.Convert.ToInt32(reader["maxunit"].ToString());
                intGreenToYellowCut = System.Convert.ToInt32(reader["gtoycut"].ToString());
                intGreenToYellowTemper = System.Convert.ToInt32(reader["gtoytemper"].ToString());
                intGreenToYellowUnit = System.Convert.ToInt32(reader["gtoyunit"].ToString());
                //intMaxCut = System.Convert.ToInt32(System.Convert.ToDecimal((reader["maxcut"].ToString())).ToString("N0"));
                //intMaxTemper = System.Convert.ToInt32(System.Convert.ToDecimal((reader["maxtemper"].ToString())).ToString("N0"));
                //intMaxUnit = System.Convert.ToInt32(System.Convert.ToDecimal((reader["maxunit"].ToString())).ToString("N0"));
                //intGreenToYellowCut = System.Convert.ToInt32(System.Convert.ToDecimal((reader["gtoycut"].ToString())).ToString("N0"));
                //intGreenToYellowTemper = System.Convert.ToInt32(System.Convert.ToDecimal((reader["gtoytemper"].ToString())).ToString("N0"));
                //intGreenToYellowUnit = System.Convert.ToInt32(System.Convert.ToDecimal((reader["gtoyunit"].ToString())).ToString("N0"));
            }
            reader.Dispose();
            command.Dispose();
            conn.Close();

            command = new SqlCommand("[dbo].sp_CapacityInfo_CIG", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 15).Value = "CIG";
            command.Parameters.Add("@Function", SqlDbType.VarChar, 15).Value = "Cal";
            command.Parameters.Add("@Plant", SqlDbType.VarChar, 8).Value = cmbPlant.Text;
            conn.Open();
            reader = command.ExecuteReader();
            int count = 1;
            while (reader.Read())
            {
                aryCapCalMaxCut[count] = System.Convert.ToInt32(System.Convert.ToDecimal((reader["maxcut"].ToString())).ToString("N0"));
                aryCapCalMaxTemper[count] = System.Convert.ToInt32(System.Convert.ToDecimal((reader["maxtemper"].ToString())).ToString("N0"));
                aryCapCalMaxUnit[count] = System.Convert.ToInt32(System.Convert.ToDecimal((reader["maxunit"].ToString())).ToString("N0"));
                count++;
            }
            reader.Dispose();
            command.Dispose();
            conn.Close();
            
            
            command = new SqlCommand("[dbo].sp_JobAsmbl_UnitCutTemper_Count", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 15).Value = "CIG";
            command.Parameters.Add("@Revision", SqlDbType.VarChar, 8).Value = cmbPlant.Text;
            conn.Open();
            reader = command.ExecuteReader();
            lblLastUpdate.Text = "Last Refresh:" + DateTime.Now.ToString("M/d/yyyy HH:mm:ss");
            int intSuper7 = 1;
            byte isFirst = 1;
            intIndex = 0;
            int intCapCalIndex = 1;
            DateTime dteDay;
            while (reader.Read())
            {
                string[] strQtys = new string[4];
                strQtys[0] = reader["DayDate"].ToString();
                strQtys[1] = System.Convert.ToInt32(reader["cutqty"]).ToString() ;
                strQtys[2] = System.Convert.ToInt32(reader["temperqty"]).ToString();
                strQtys[3] = System.Convert.ToInt32(reader["unitqty"]).ToString();

                //strQtys[2] = System.Convert.ToDecimal((reader["temperqty"].ToString())).ToString("N0");
                //strQtys[3] = System.Convert.ToDecimal((reader["unitqty"].ToString())).ToString("N0");
                dteDay = DateTime.Parse(strQtys[0]);
                int intDayOfWeek = (int)dteDay.DayOfWeek;
                if (intIndex == 0)
                {
                    for (intIndex = 1; intIndex < intDayOfWeek; intIndex++)
                    {
                        aryDays[intIndex].IsHidden = 1;
                    }
                }

                if (aryCapCalMaxCut[intCapCalIndex] != -1)
                {
                    aryDays[intIndex].MaxCut = aryCapCalMaxCut[intCapCalIndex];
                }
                if (aryCapCalMaxTemper[intCapCalIndex] != -1)
                {
                    aryDays[intIndex].MaxTemper = aryCapCalMaxTemper[intCapCalIndex];
                }
                if (aryCapCalMaxUnit[intCapCalIndex] != -1)
                {
                    aryDays[intIndex].MaxUnit = aryCapCalMaxUnit[intCapCalIndex];
                }

                aryDays[intIndex].DisplayDate = dteDay;
                aryDays[intIndex].Cut = Convert.ToInt32(strQtys[1]);
                aryDays[intIndex].Temper = Convert.ToInt32(strQtys[2]);
                aryDays[intIndex].Unit = Convert.ToInt32(strQtys[3]);

                if (isFirst != 1 && intSuper7 <= 7 && intDayOfWeek != 6 && intDayOfWeek != 0 && aryCapCalMaxCut[intCapCalIndex] != 0 && aryCapCalMaxTemper[intCapCalIndex] != 0 && aryCapCalMaxUnit[intCapCalIndex] != 0)
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

            intIndex = 1;
            for (int intY = 0; intY <= 5; intY++)
            {
                for (int intX = 0; intX <= 6; intX++)
                {
                    if (intX == 5 || intX == 6)
                    {
                        aryDays[intIndex].MaxCut = 0;
                        aryDays[intIndex].MaxTemper = 0;
                        aryDays[intIndex].MaxUnit = 0;
                    }
                    else
                    {
                        aryDays[intIndex].MaxCut = intMaxCut;
                        aryDays[intIndex].MaxTemper = intMaxTemper;
                        aryDays[intIndex].MaxUnit = intMaxUnit;
                    }
                    aryDays[intIndex].GreenToYellowCut = intGreenToYellowCut;
                    aryDays[intIndex].GreenToYellowTemper = intGreenToYellowTemper;
                    aryDays[intIndex].GreenToYellowUnit = intGreenToYellowUnit;
                    //aryDays[intIndex].Refresh();
                    //aryDays[intIndex].Invalidate();
                    intIndex++;
                }
            }


            //update totals
            int intWeekCutTotal;
            int intWeekTemperTotal;
            int intWeekUnitTotal;
            for (int intY = 0; intY <= 5; intY++)
            {
                intWeekCutTotal = 0;
                intWeekTemperTotal = 0;
                intWeekUnitTotal = 0;
                for (int intX = 1; intX <= 7; intX++)
                {
                    intWeekCutTotal += aryDays[intX + intY * 7].Cut;
                    intWeekTemperTotal += aryDays[intX + intY * 7].Temper;
                    intWeekUnitTotal += aryDays[intX + intY * 7].Unit;
                }
                txtTotalCut[intY].Text = intWeekCutTotal.ToString("N0");
                txtTotalTemper[intY].Text = intWeekTemperTotal.ToString("N0");
                txtTotalUnit[intY].Text = intWeekUnitTotal.ToString("N0");
            }

        }
    }



}
