using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using OfficeOpenXml;
using SolidEdge.RevisionManager.Interop;

namespace FileSearcher
{
 
    public partial class Form1 : Form
    {
        XmlDocument doc;
        Form2 working;
        private List<ListViewItem> files;


        public Form1()
        {
            InitializeComponent();
            working = new Form2();
            files = new List<ListViewItem>();

            doc = new XmlDocument();
            CheckForNeverCached();
        }

        delegate void HideWorkingCallback();

        private void HideWorking()
        {
            if (this.working.InvokeRequired)
            {
                HideWorkingCallback d = new HideWorkingCallback(HideWorking);
                this.Invoke(d);
            }
            else
            {
                working.Hide();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

            foreach (string str in txt_NewLocations.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                string form_str = str.Trim();
                form_str = form_str.TrimEnd(new char[] { '\\' });
                form_str = form_str + "\\\\";
                ListViewItem item = new ListViewItem(form_str);
                item.SubItems.Add("Never");
                lst_Locations.Items.Add(item);
            }
            txt_NewLocations.Text = "";
            CheckForNeverCached();
        }

        private void CheckForNeverCached()
        {
            bool never = false;
            foreach(ListViewItem item in lst_Locations.Items)
            {
                if (item.SubItems[1].Text == "Never")
                    never = true;
            }
            btn_CacheNew.Enabled = never;
        }

        private void btn_CacheNew_Click(object sender, EventArgs e)
        {
            List<string> locs = new List<string>();
            foreach(ListViewItem item in lst_Locations.Items)
            {
                if (item.SubItems[1].Text == "Never")
                    locs.Add(item.SubItems[0].Text);
            }
            working.Show(this);
            ThreadPool.QueueUserWorkItem(BuildCache, new object[] { locs });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<string> locs = new List<string>();
            foreach(ListViewItem item in lst_Locations.Items)
            {
                locs.Add(item.SubItems[0].Text);
            }
            working.Show(this);
            ThreadPool.QueueUserWorkItem(BuildCache, new object[] { locs });
        }

        delegate void AddLocationCallback(ListViewItem item);

        private void AddLocation(ListViewItem item)
        {
            if (lst_Locations.InvokeRequired)
            {
                AddLocationCallback d = new AddLocationCallback(AddLocation);
                this.Invoke(d, item);
            }
            else
                lst_Locations.Items.Add(item);
        }

        delegate void ClearLocationsCallback();

        private void ClearLocations()
        {
            if (lst_Locations.InvokeRequired)
            {
                ClearLocationsCallback d = new ClearLocationsCallback(ClearLocations);
                this.Invoke(d);

            }
            else
                lst_Locations.Items.Clear();
        }

        private void ReadCache()
        {
            try
            {
                FileStream stream = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FileSearcherCache.xml", FileMode.Open);
                doc.Load(stream);
                ClearLocations();
                for (int x = 0; x < doc.ChildNodes[1].ChildNodes.Count; x++)
                {
                    XmlNode node = doc.ChildNodes[1].ChildNodes[x];
                    ListViewItem item = new ListViewItem(((XmlElement)node).GetAttribute("name"));
                    item.SubItems.Add(((XmlElement)node).GetAttribute("cached"));
                    AddLocation(item);
                }
                stream.Close();
            }
            catch (Exception ex)
            {
                FileInfo file = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FileSearcherCache.xml");
                if (file.Exists)
                {
                    MessageBox.Show("Cache error.  Cache deleted, please re-enter locations.");
                    file.Delete();
                }
            }
            HideWorking();
        }

        private void BuildCache(object data)
        {
            List<string> locs = ((object[])data)[0] as List<string>;
            string filename = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FileSearcherCache.xml";
            FileInfo file = new FileInfo(filename);
            XmlNode root;
            if (file.Exists)
            {
                doc.Load(filename);
                root = doc.ChildNodes[1];
            }
            else
            {
                doc = new XmlDocument();
                root = doc.CreateNode(XmlNodeType.Element, "CACHE", "");
                doc.AppendChild(root);
            }
            foreach (string str in locs)
            {

                DirectoryInfo dinfo = new DirectoryInfo(str);
                if (!dinfo.Exists)
                {
                    MessageBox.Show("Directory '" + str + "' does not exist");
                    RemoveLocation(str);

                }
                else
                {
                    XmlNode node = doc.CreateNode(XmlNodeType.Element, "DIR", "");
                    ((XmlElement)node).SetAttribute("name", dinfo.FullName);
                    ((XmlElement)node).SetAttribute("cached", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                    root.AppendChild(node);
                    ParseDir(dinfo, ref node);

                }
            }
            FileStream stream = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FileSearcherCache.xml", FileMode.Create);
            XmlWriter writer = XmlWriter.Create(stream);
            doc.WriteTo(writer);
            writer.Flush();
            writer.Close();
            stream.Close();
            ReadCache();
        }

        private void RemoveFromCache(string loc)
        {
            XmlDocument doc = new XmlDocument();
            string filename = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FileSearcherCache.xml";
            FileInfo file = new FileInfo(filename);
            if (file.Exists)
            {
                doc.Load(filename);
                foreach (XmlNode node in doc.ChildNodes[1])
                {
                    string name = node.Name;
                    if (node.Name == "DIR")
                    {
                        if (node.Attributes["name"].Value.ToString() == loc)
                        {
                            node.ParentNode.RemoveChild(node);
                        }
                    }
                }
                doc.Save(filename);

                ReadCache();
            }

        }

        private void ParseDir(DirectoryInfo dinfo, ref XmlNode node)
        {
            foreach (DirectoryInfo d in dinfo.GetDirectories())
            {
                    try
                    {
                        XmlNode nnode = doc.CreateNode(XmlNodeType.Element, "DIR", "");
                        ((XmlElement)nnode).SetAttribute("name", d.FullName);
                        ParseDir(d, ref nnode);
                        node.AppendChild(nnode);
                    }
                    catch (Exception x)
                    {

                    }
            }
            foreach (FileInfo f in dinfo.GetFiles())
            {
                XmlNode nnode = doc.CreateNode(XmlNodeType.Element, "FILE", "");
                ((XmlElement)nnode).SetAttribute("name", f.Name);

                if ((new List<string> { ".dft", ".par", ".asm", ".psm" }).Contains(f.Extension))
                {
                    SolidEdge.RevisionManager.Interop.Application ac = new SolidEdge.RevisionManager.Interop.Application();
                    

                    Document dc = (Document)ac.Open(f.FullName);
                    ((XmlElement)nnode).SetAttribute("revision", dc.Revision);
                }
                else if (f.Extension == ".dwg")
                {
                    if (f.Name.IndexOf("-R") >= 0)
                    {
                        string rev = f.Name.Substring(f.Name.IndexOf("-R") + 2, 1);
                        if (rev == "0")
                            rev = "-";
                        ((XmlElement)nnode).SetAttribute("revision", rev);
                    }
                }
                else
                {
                    ((XmlElement)nnode).SetAttribute("revision", "");
                }

                node.AppendChild(nnode);
            }
        }


        delegate void RemoveLocationCallback(string loc);



        private void RemoveLocation(string loc)
        {
            if (lst_Locations.InvokeRequired)
            {
                RemoveLocationCallback d = new RemoveLocationCallback(RemoveLocation);
                this.Invoke(d, loc);
            }
            else
                foreach (ListViewItem item in lst_Locations.Items)
                {
                    if (item.Text == loc)
                        lst_Locations.Items.Remove(item);
                }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            working.Show(this);
            ReadCache();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            working.Show(this);
            lst_Files.Items.Clear();
            files = new List<ListViewItem>();
            ThreadPool.QueueUserWorkItem(BuildSearch, new object[] { });
        }

        private void BuildSearch(object data)
        {
            string searchstring = txt_Search.Text;
            string[] split = searchstring.Split(new char[] { '*' }, StringSplitOptions.None);
            searchstring = "";
            for (int x = 0; x < split.Length; x++)
            {
                searchstring += Regex.Escape(split[x]);
                if (x < split.Length - 1)
                    searchstring += ".*";
            }
            searchstring = "^" + searchstring + "$";
            Regex regex = new Regex(searchstring);
            for (int x = 0; x < doc.ChildNodes[1].ChildNodes.Count; x++)
            {
                SearchNode(doc.ChildNodes[1].ChildNodes[x], regex);
            }
            ShowFiles();
            HideWorking();
        }


        delegate void ShowFilesCallback();

        private void ShowFiles()
        {
            if (lst_Files.InvokeRequired)
            {
                ShowFilesCallback d = new ShowFilesCallback(ShowFiles);
                this.Invoke(d);
            }
            else
                foreach (ListViewItem item in files)
                    lst_Files.Items.Add(item);
        }

        private void SearchNode(XmlNode node, Regex search)
        {
            if (node.Name == "DIR")
            {
                foreach (XmlNode cnode in node.ChildNodes)
                {
                    SearchNode(cnode, search);
                }
            }
            else if (node.Name == "FILE")
            {
                string file = ((XmlElement)node).GetAttribute("name");
                if (search.Matches(file).Count > 0)
                {
                    ListViewItem item = new ListViewItem(file);
                    item.SubItems.Add(((XmlElement)node.ParentNode).GetAttribute("name"));
                    item.SubItems.Add(((XmlElement)node).GetAttribute("revision"));
                    files.Add(item);
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lst_Files.Items)
            {
                item.Selected = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lst_Files.Items)
            {
                item.Selected = false;
            }

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            working.Show(this);
            CopyLocal(null);
        }

        private void CopyLocal(object data)
        {
            string localdir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FileSearcherLocalCopy";
            foreach (ListViewItem item in lst_Files.SelectedItems)
            {
                try
                {
                    string file = item.SubItems[0].Text;
                    string loc = item.SubItems[1].Text;
                    string src = loc + "\\" + file;
                    Directory.CreateDirectory(localdir);
                    string dst = localdir + "\\" + file;
                    FileInfo srcinfo = new FileInfo(src);
                    FileInfo dstinfo = new FileInfo(dst);
                    if (!dstinfo.Exists || (dstinfo.Exists && srcinfo.LastWriteTime != dstinfo.LastWriteTime))
                    {
                        File.Copy(src, dst, true);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            HideWorking();
            MessageBox.Show("Files copied to " + localdir);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss");
            ExcelPackage pck = null;
            string filename = dir + " - FileSearchExport.xlsx";
            FileInfo newFile = new FileInfo(filename);

            pck = new ExcelPackage(newFile);

            var ws = pck.Workbook.Worksheets.Add("Results");

            ws.Cells[1, 1].Value = "File";
            ws.Cells[1, 2].Value = "Location";
            ws.Cells[1, 3].Value = "Revision";
            ws.Cells[1, 4].Value = "Report";

            int row = 2;
            foreach (ListViewItem lvi in lst_Files.Items)
            {
                ws.Cells[row, 1].Value = lvi.SubItems[0].Text;
                ws.Cells[row, 2].Value = lvi.SubItems[1].Text;
                ws.Cells[row, 3].Value = lvi.SubItems[2].Text;
                ws.Cells[row, 4].Value = lvi.SubItems[3].Text;
                row++;
            }

            pck.Save();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Engineering Report Output\\";
            foreach (ListViewItem lvi in lst_Files.SelectedItems)
            {
                string filename = lvi.SubItems[0].Text;
                string location = lvi.SubItems[1].Text;

                string dest = dir + filename + ".txt";

                string strCmdText;
                strCmdText = (location+"\\"+filename)+ " /t=ASM_BOM " + "/o="+dest+ " /w=FALSE /l=C:\\ErrorLog.log";
                                System.Diagnostics.Process.Start("C:\\Program Files\\Siemens\\Solid Edge 2019\\Program\\report.exe", strCmdText);
                //System.Diagnostics.Process.Start("C:\\Program Files\\Solid Edge ST10\\Program\\report.exe", strCmdText);

                Thread.Sleep(5000);

                FileInfo confFilr = new FileInfo(dest);
                if (confFilr.Exists)
                {
                    lvi.SubItems.Add("output");
                } else
                {
                    lvi.SubItems.Add("error");
                }

            }
            Thread.Sleep(10000);

            DirectoryInfo di = new DirectoryInfo(dir);

            using (ExcelPackage combPackage = new ExcelPackage())
            {
                ExcelWorksheet combworksheet = combPackage.Workbook.Worksheets.Add("Sheet 1");
                combworksheet.Cells[1, 1].Value = "Parent";
                combworksheet.Cells[1, 2].Value = "Level";
                combworksheet.Cells[1, 3].Value = "Document Number";
                combworksheet.Cells[1, 4].Value = "Revision";
                combworksheet.Cells[1, 5].Value = "Title";
                combworksheet.Cells[1, 6].Value = "Quantity";

                int combrow = 2;
                Dictionary<int, string> parents = new Dictionary<int, string>();

                foreach (FileInfo fi in di.GetFiles("*.txt"))
                {
                    ExcelTextFormat format = new ExcelTextFormat();
                    format.Delimiter = '\t';
                    format.Culture = new CultureInfo(Thread.CurrentThread.CurrentCulture.ToString());
                    format.Culture.DateTimeFormat.ShortDatePattern = "dd-mm-yyyy";
                    format.Encoding = new UTF8Encoding();

                    //create a new Excel package
                    using (ExcelPackage excelPackage = new ExcelPackage())
                    {
                        parents[1] = fi.Name.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0];
                        
                        //create a WorkSheet
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");

                        //load the CSV data into cell A1
                        worksheet.Cells["A1"].LoadFromText(fi, format);

                        bool parse = true;
                        int row = 3;
                        while (parse)
                        {
                            try
                            {
                                if (String.IsNullOrEmpty(worksheet.Cells[row, 1].Value.ToString().Trim()))
                                {
                                    if (String.IsNullOrEmpty(worksheet.Cells[row, 4].Value.ToString()))
                                    {
                                        parse = false;
                                    }
                                    else
                                    {
                                        worksheet.Cells[row - 1, 4].Value = worksheet.Cells[row - 1, 4].Value.ToString().Trim() + " " + worksheet.Cells[row, 4].Value.ToString().Trim();
                                        worksheet.DeleteRow(row);
                                    }

                                }
                                else
                                {
                                    int level = Int32.Parse(worksheet.Cells[row, 1].Value.ToString());
                                    parents[level + 1] = worksheet.Cells[row, 2].Value.ToString();
                                    combworksheet.Cells[combrow, 1].Value = parents[level];
                                    combworksheet.Cells[combrow, 2].Value = worksheet.Cells[row, 1].Value;
                                    combworksheet.Cells[combrow, 3].Value = worksheet.Cells[row, 2].Value;
                                    combworksheet.Cells[combrow, 4].Value = worksheet.Cells[row, 3].Value;
                                    combworksheet.Cells[combrow, 5].Value = worksheet.Cells[row, 4].Value;
                                    combworksheet.Cells[combrow, 6].Value = worksheet.Cells[row, 5].Value;
                                    combrow++;
                                    row++;
                                }
                            }
                            catch (Exception ex)
                            {
                                parse = false;
                            }
                        }

                        string filename = fi.FullName;

                        FileStream stream = new FileStream(fi.FullName + ".xlsx", FileMode.Create);
                        excelPackage.SaveAs(stream);
                        stream.Close();
                    }

                }

                FileStream combstream = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\CombinedBOMSheet" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx", FileMode.Create);
                combPackage.SaveAs(combstream);
                combstream.Close();
            }
            MessageBox.Show("Done parsing reports.  Individual report text and xlsx files created alongside source files.  Combined xlsx file created in My Documents.");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            List<string> remove = new List<string>();
            foreach (ListViewItem lvi in lst_Locations.SelectedItems)
            {
                remove.Add(lvi.SubItems[0].Text);
            }

            foreach (string loc in remove)
            {
                RemoveFromCache(loc);
                RemoveLocation(loc);
            }
        }
    }
}
