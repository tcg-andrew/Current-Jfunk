using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace ObjectLibrary
{
    class StartsWithCheck
    {
        private int _length;
        private string _matches;

        public StartsWithCheck(int length, string matches)
        {
            _length = length;
            _matches = matches;
        }

        public bool Passes(string partnum)
        {
            return _matches.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Contains(partnum.Substring(0, _length));
        }
    }

    class CharCodeCheck
    {
        private int _position;
        private string _matches;

        public CharCodeCheck(int position, string matches)
        {
            _position = position;
            _matches = matches;
        }

        public bool Passes(string partnum)
        {
            return _matches.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Contains(partnum[_position].ToString());
        }
    }

    class SegmentCheck
    {
        private StartsWithCheck _startswith;
        private CharCodeCheck _charcode;
        private string _segment;

        public SegmentCheck(StartsWithCheck startswith, CharCodeCheck charcode, string segment)
        {
            _startswith = startswith;
            _charcode = charcode;
            _segment = segment;
        }

        public string GetSegment(string partnum)
        {
            if (_startswith.Passes(partnum) && _charcode.Passes(partnum))
                return _segment;
            return "";
        }
    }

    public class OprTimeDetail
    {
        private int _openings;
        private int _doors;
        private string _suffix;

        public int Openings { get { return _openings; } }
        public int Doors { get { return _doors; } }
        public string Suffix { get { return _suffix; } }

        public OprTimeDetail(int openings, int doors, string suffix)
        {
            _openings = openings;
            _doors = doors;
            _suffix = suffix;
        }

    }
    static public class OprTimesInterface
    {
        static private DateTime _driver_modified;
        static private DateTime _times_modified;

        // [prodgroup][# openings][locls][opr code] = time
        static Dictionary<string, int> _times_columns;
        static Dictionary<string, Dictionary<int, Dictionary<bool, Dictionary<string, decimal>>>> times;

        static public OprTimeDetail GetDetail(string partnum)
        {
            SqlCommand sqlCommand = new SqlCommand("select * from dbo.fn_OprTimeDetails(@Partnum)");
            sqlCommand.Parameters.AddWithValue("Partnum", partnum);
            List<int> result = new List<int>();

            try
            {
                DataSet ds = SQLAccess.GetDataSet(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), sqlCommand);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    return new OprTimeDetail((int)Decimal.Parse(row["FrameDoorOpenings"].ToString()), (int)Decimal.Parse(row["DoorCount"].ToString()), row["Suffix"].ToString());
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        static public decimal GetOprTime(string prodgrup, int openings, bool locks, string opcode)
        {
            try
            {
                decimal result = 0;

                LoadData();

                return times[prodgrup][openings][locks][opcode];
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        static private void ReadColumnHeaders(ExcelWorksheet ws)
        {
            int row = 1;
            int col = 7;
            _times_columns = new Dictionary<string, int>();
            while (ws.Cells[row, col].Value != null)
            {
                string name = ws.Cells[row, col].Value.ToString();
                if (Char.IsUpper(name[0]))
                    _times_columns[name] = col;
                col++;
            }
        }

        static private void LoadData()
        {
            FileInfo newFile = new FileInfo(ConfigurationManager.AppSettings["OprTimesLoc"].ToString());
            if (_times_modified != newFile.LastWriteTime)
            {
                times = new Dictionary<string, Dictionary<int, Dictionary<bool, Dictionary<string, decimal>>>>();

                ExcelPackage pck = new ExcelPackage(newFile);

                var ws = pck.Workbook.Worksheets["Cycle Time Matrix"];

                ReadColumnHeaders(ws);
                
                int row = 2;
                while (ws.Cells[row, 1].Value != null)
                {
                    string prodgrup = ws.Cells[row, 2].Value.ToString();
                    if (!times.Keys.Contains(prodgrup))
                        times[prodgrup] = new Dictionary<int, Dictionary<bool, Dictionary<string, decimal>>>();

                    int openings = Int32.Parse(ws.Cells[row, 6].Value.ToString());
                    if (!times[prodgrup].Keys.Contains(openings))
                        times[prodgrup][openings] = new Dictionary<bool, Dictionary<string, decimal>>();

                    bool locks = ws.Cells[row, 5].Value.ToString() == "Y";
                    if (!times[prodgrup][openings].Keys.Contains(locks))
                        times[prodgrup][openings][locks] = new Dictionary<string, decimal>();

                    foreach (string column in _times_columns.Keys)
                    {
                        decimal val;
                        if (ws.Cells[row, _times_columns[column]].Value != null && Decimal.TryParse(ws.Cells[row, _times_columns[column]].Value.ToString(), out val))
                            times[prodgrup][openings][locks][column] = val;
                    }

                    row++;
                }
                _times_modified = newFile.LastWriteTime;
            }

        }
    }
}
