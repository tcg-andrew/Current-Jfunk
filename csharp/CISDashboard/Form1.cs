#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Epicor.Mfg.Core;
using ModuleBase;

#endregion

namespace CISDashboard
{
    public partial class Form1 : Form
    {
        #region Values

        Session session;
        string server;
        string port;
        string database;
        delegate void controlMethod();
        Dictionary<string, ModuleBase.Module> modules;
        Dictionary<string, string> moduleNames;

        #endregion

        #region Constructors

        public Form1()
        {
            modules = new Dictionary<string, ModuleBase.Module>();
            moduleNames = new Dictionary<string, string>();
            InitializeComponent();
            DrawDashboard();
        }

        public Form1(string s, string p, string d)
            : this()
        {
            server = s;
            port = p;
            database = d;
        }

        #endregion

        #region Private Methods

        #region Helpers

        private string GetKeyValue(string key)
        {
            IsolatedStorageFileStream isoStream = null;
            IsolatedStorageFile isoStore = null;
            try
            {
                isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming, null, null);
                if (isoStore.FileExists("CISDashboard" + key + ".txt"))
                {
                    isoStream = new IsolatedStorageFileStream("CISDashboard" + key + ".txt", FileMode.Open, isoStore);
                    StreamReader reader = new StreamReader(isoStream);
                    string value = reader.ReadLine();
                    reader.Close();
                    return value;
                }
            }
            catch (System.Exception)
            {
                MessageBox.Show("Problem loading your stored connection info.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return "";
        }

        private void SetKeyValue(string key, string value)
        {
            IsolatedStorageFileStream isoStream = null;
            IsolatedStorageFile isoStore = null;
            try
            {
                isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming, null, null);
                if (isoStore.FileExists("CISDashboard" + key + ".txt"))
                    isoStream = new IsolatedStorageFileStream("CISDashboard" + key + ".txt", FileMode.Open, isoStore);
                else
                    isoStream = new IsolatedStorageFileStream("CISDashboard" + key + ".txt", FileMode.Create, isoStore);
                StreamWriter writer = new StreamWriter(isoStream);
                isoStream.Position = 0;
                writer.WriteLine(value);
                writer.Close();
            }
            catch (System.Exception)
            {
                MessageBox.Show("Problem saving your connection info.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ShowPageDataStatusMessage(TabPage page, string message)
        {
            ThreadSafeModify(page, delegate
            {
                Label lbl = page.Controls.Find("lbl_NoData", false)[0] as Label;
                lbl.Text = message;
                lbl.Visible = true;

            });
        }

        private void ThreadSafeModify(Control control, controlMethod method)
        {
            if (control.InvokeRequired)
                control.Invoke(new MethodInvoker(method));
            else
                method();
        }

        private void CreateNewTabPage(string guid)
        {
            TabPage taskPage = new TabPage();
            taskPage.Text = moduleNames[guid];
            taskPage.Name = guid;
            taskPage.UseVisualStyleBackColor = true;
            tabControl1.TabPages.Add(taskPage);

            Label lbl = new Label();
            lbl.Name = "lbl_NoData";
            lbl.Font = new System.Drawing.Font("Arial", 14.0f);
            lbl.Padding = new System.Windows.Forms.Padding(20, 20, 0, 0);
            lbl.Dock = DockStyle.Fill;
            lbl.Visible = false;
            taskPage.Controls.Add(lbl);

            DataGridView dgv = new DataGridView();
            dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dgv.ContextMenuStrip = contextMenuStrip1;
            dgv.Name = "dgv_Data";
            dgv.Dock = DockStyle.Fill;
            dgv.AllowUserToResizeColumns = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToOrderColumns = false;
            dgv.ScrollBars = ScrollBars.Both;
            dgv.Visible = false;
            dgv.EditMode = DataGridViewEditMode.EditOnEnter;
            taskPage.Controls.Add(dgv);
            tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
        }

        private void ToggleToolStripButtons(string guid, bool turnon)
        {
            ThreadSafeModify(toolStrip1, delegate { refreshButton.Enabled = false; });
            ThreadSafeModify(toolStrip1, delegate { saveButton.Enabled = false; });
            ThreadSafeModify(toolStrip1, delegate { closeButton.Enabled = false; });
            ThreadSafeModify(toolStrip1, delegate { printButton.Enabled = false; });

            if (turnon)
            {
                if (modules[guid].Actions.Contains(ModuleBase.ModuleAction.Refresh))
                    ThreadSafeModify(toolStrip1, delegate { refreshButton.Enabled = true; });
                if (modules[guid].Actions.Contains(ModuleBase.ModuleAction.Save))
                    ThreadSafeModify(toolStrip1, delegate { saveButton.Enabled = true; });
                if (modules[guid].Actions.Contains(ModuleBase.ModuleAction.Print))
                    ThreadSafeModify(toolStrip1, delegate { printButton.Enabled = true; });
                ThreadSafeModify(toolStrip1, delegate { closeButton.Enabled = true; });
            }
        }

        private void FormatCells(DataGridView dgv, List<GridColumn> columns)
        {
            foreach (GridColumn column in columns)
            {
                if (!String.IsNullOrEmpty(column.Format))
                    dgv.Columns[column.Name].DefaultCellStyle.Format = column.Format;
                if (column.Frozen)
                    dgv.Columns[column.Name].Frozen = true;
                if (!column.Visible)
                    dgv.Columns[column.Name].Visible = false;
                /*                if (column.ValidValues.Count > 0)
                                {
                                    dgv.Columns.Remove(column.Name);
                                    DataGridViewComboBoxColumn cbc = new DataGridViewComboBoxColumn();
                                    cbc.Name = column.Name;
                                    cbc.DataPropertyName = column.Name;
                                    foreach (string key in column.ValidValues)
                                        cbc.Items.Add(key);
                                    dgv.Columns.Add(cbc);
                                }
                                if (column.ActionColumn)
                                {
                                    dgv.Columns.Remove(column.Name);
                                    DataGridViewLinkColumn c = new DataGridViewLinkColumn();
                                    c.UseColumnTextForLinkValue = false;
                                    c.HeaderText = column.Name;
                                    c.Name = column.Name;
                                    c.DataPropertyName = column.Name;
                                    int index = columns.IndexOf(column);
                                    if (dgv.Columns[0].Name == "")
                                        index++;
                                    dgv.Columns.Insert(index, c);
                                }
                  */
            }
        }

        #endregion

        #region Dialogs

        private void ShowConnectionWindow()
        {
            ServiceConnection sc = new ServiceConnection();
            string lastUser = GetKeyValue("LastUser");
            if (!String.IsNullOrEmpty(lastUser))
                sc.Username = lastUser;

            if (sc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string connString = "AppServerDC://" + server + ":" + port;
                try
                {
                    session = new Session(sc.Username, sc.Password, connString, Session.LicenseType.Default);
                    SetKeyValue("LastUser", sc.Username);
                    serverStatusLabel.Text = server + ":" + port;
                    plantStatusLabel.Text = session.PlantName;
                    companyStatusLabel.Text = session.CompanyName;
                    userStatusLabel.Text = session.UserID;
                    serverStatusLabel.Visible = true;
                    plantStatusLabel.Visible = true;
                    companyStatusLabel.Visible = true;
                    userStatusLabel.Visible = true;
                    tabControl1.Enabled = true;
                    tasksToolStripMenuItem.Enabled = true;
                    connectToolStripMenuItem.Enabled = false;
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region Filters

        #region Drop Down Filters

        private void BuildDropDownFilters(object param)
        {
            string guid = param as string;

            DestroyDropDownFilters();

            if (modules[guid].GetType().GetInterface(typeof(IDateFilter).FullName) != null)
            {
                IDropDownFilter m = modules[guid] as IDropDownFilter;
                Dictionary<string, Dictionary<string, string>> filters = m.Filters;

                foreach (string filter in filters.Keys)
                {
                    ToolStripLabel lbl = new ToolStripLabel();
                    lbl.Name = "lbl_Filter" + filter;
                    lbl.Text = filter;
                    ThreadSafeModify(toolStrip2, delegate { toolStrip2.Items.Add(lbl); });
                    ToolStripComboBox cbl = new ToolStripComboBox();
                    cbl.Name = "cbl_Filter" + filter;
                    cbl.DropDownStyle = ComboBoxStyle.DropDownList;
                    cbl.SelectedIndexChanged += new EventHandler(filterCB_SelectedIndexChanged);
                    foreach (string key in filters[filter].Keys)
                        cbl.Items.Add(key);
                    ThreadSafeModify(toolStrip2, delegate { toolStrip2.Items.Add(cbl); });
                    if (m.SelectedFilter.ContainsKey(filter))
                        ThreadSafeModify(toolStrip2, delegate { ((ToolStripComboBox)toolStrip2.Items[cbl.Name]).SelectedItem = m.SelectedFilter[filter]; });
                }
                ThreadSafeModify(toolStrip2, delegate { toolStrip2.Visible = true; });
            }
        }

        private void DestroyDropDownFilters()
        {
            toolStrip2.Items.Clear();
            ThreadSafeModify(toolStrip2, delegate { toolStrip2.Visible = false; });
        }

        #endregion

        #region Date Filters

        private void BuildDateFilter(object param)
        {
            string guid = param as string;
            DestroyDateFilter();

            if (modules[guid].GetType().GetInterface(typeof(IDropDownFilter).FullName) != null)
            {
                ToolStripButton backWholeMonth = new ToolStripButton();
                backWholeMonth.Name = "backWholeMonth";
                backWholeMonth.Image = global::CISDashboard.Properties.Resources._1206569967984704715pitr_green_double_arrows_set_4_svg_hi;
                backWholeMonth.Click += new EventHandler(backWholeMonth_Click);
                backWholeMonth.ToolTipText = "Previous whole month";

                ToolStripButton backMonth = new ToolStripButton();
                backMonth.Name = "backMonth";
                backMonth.Image = global::CISDashboard.Properties.Resources._12065699261710976909pitr_green_single_arrows_set_4_svg_hi;
                backMonth.Click += new EventHandler(backMonth_Click);
                backMonth.ToolTipText = "Previous 30 days";

                ToolStripButton nextMonth = new ToolStripButton();
                nextMonth.Name = "nextMonth";
                nextMonth.Image = global::CISDashboard.Properties.Resources._1206569902228245216pitr_green_single_arrows_set_1_svg_hi;
                nextMonth.Click += new EventHandler(nextMonth_Click);
                nextMonth.ToolTipText = "Next 30 days";

                ToolStripButton nextWholeMonth = new ToolStripButton();
                nextWholeMonth.Name = "nextWholeMonth";
                nextWholeMonth.Image = global::CISDashboard.Properties.Resources._1206569942771180767pitr_green_double_arrows_set_1_svg_hi;
                nextWholeMonth.Click += new EventHandler(nextWholeMonth_Click);
                nextWholeMonth.ToolTipText = "Next whole month";

                ToolStripLabel fromLbl = new ToolStripLabel();
                fromLbl.Name = "fromLbl";
                fromLbl.Text = "From";

                ToolStripLabel toLbl = new ToolStripLabel();
                toLbl.Name = "toLbl";
                toLbl.Text = "To";

                DateTimePicker fromMonth = new DateTimePicker();
                fromMonth.Name = "fromMonth";
                fromMonth.Format = DateTimePickerFormat.Short;
                fromMonth.Width = 100;
                fromMonth.Text = (modules[guid] as IDateFilter).From.ToShortDateString();
                fromMonth.TextChanged += new EventHandler(fromMonth_TextChanged);
                ToolStripControlHost fromtch = new ToolStripControlHost(fromMonth);
                fromtch.Name = "fromMonthTCH";

                DateTimePicker toMonth = new DateTimePicker();
                toMonth.Name = "toMonth";
                toMonth.Format = DateTimePickerFormat.Short;
                toMonth.Width = 100;
                toMonth.Text = (modules[guid] as IDateFilter).To.ToShortDateString();
                toMonth.TextChanged += new EventHandler(toMonth_TextChanged);
                ToolStripControlHost totch = new ToolStripControlHost(toMonth);
                totch.Name = "toMonthTCH";

                ThreadSafeModify(toolStrip1, delegate { toolStrip1.Items.AddRange(new ToolStripItem[] { backWholeMonth, backMonth, fromLbl, fromtch, toLbl, totch, nextMonth, nextWholeMonth }); });
            }
        }

        private void DestroyDateFilter()
        {
            toolStrip1.Items.RemoveByKey("backWholeMonth");
            toolStrip1.Items.RemoveByKey("backMonth");
            toolStrip1.Items.RemoveByKey("fromLbl");
            toolStrip1.Items.RemoveByKey("fromMonthTCH");
            toolStrip1.Items.RemoveByKey("toLbl");
            toolStrip1.Items.RemoveByKey("toMonthTCH");
            toolStrip1.Items.RemoveByKey("nextMonth");
            toolStrip1.Items.RemoveByKey("nextWholeMonth");
        }

        #endregion

        #endregion

        private void DrawDashboard()
        {
            string taskskey = GetKeyValue("FavoriteTasks");
            string[] tasks = taskskey.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (tasks.Length == 0)
            {
                ThreadSafeModify(label2, delegate { label2.Visible = true; });
                ThreadSafeModify(panel2, delegate { panel2.Visible = false; });
            }
            else
            {
                ThreadSafeModify(label2, delegate { label2.Visible = false; });
                ThreadSafeModify(panel2, delegate { panel2.Visible = true; });
                ThreadSafeModify(flowLayoutPanel1, delegate { flowLayoutPanel1.Controls.Clear(); });
                ThreadSafeModify(flowLayoutPanel2, delegate { flowLayoutPanel2.Controls.Clear(); });
                ThreadSafeModify(flowLayoutPanel3, delegate { flowLayoutPanel3.Controls.Clear(); });
                foreach (string str in tasks)
                {
                    /*                    if (Modules.ContainsKey(str))
                                        {
                                            lock (Modules[str])
                                            {
                                                LinkLabel lnk = new LinkLabel();
                                                lnk.Width = 200;
                                                lnk.Text = str;
                                                lnk.Click += new EventHandler(ActivateTaskFromControl);
                                                ThreadSafeModify(flowLayoutPanel1, delegate { flowLayoutPanel1.Controls.Add(lnk); });

                                                Label update = new Label();
                                                update.Width = 200;
                                                update.Text = "";
                                                if (Modules[str].State != Module.ModuleState.Unloaded)
                                                    update.Text = (Modules[str].LastUpdate.Date == DateTime.Now.Date ? "Today" : Modules[str].LastUpdate.ToShortDateString()) + " " + Modules[str].LastUpdate.ToShortTimeString();
                                                ThreadSafeModify(flowLayoutPanel2, delegate { flowLayoutPanel2.Controls.Add(update); });

                                                Label count = new Label();
                                                count.Width = 200;
                                                count.Text = "";
                                                switch (Modules[str].State)
                                                {
                                                    case Module.ModuleState.Unloaded:
                                                        count.Text = "Not loaded";
                                                        break;
                                                    case Module.ModuleState.Loaded:
                                                    case Module.ModuleState.Saving:
                                                    case Module.ModuleState.Printing:
                                                        count.Text = Modules[str].Data.Rows.Count.ToString();
                                                        break;
                                                    case Module.ModuleState.Loading:
                                                        count.Text = "Loading...";
                                                        break;
                                                }
                                                ThreadSafeModify(flowLayoutPanel3, delegate { flowLayoutPanel3.Controls.Add(count); });
                                            }
                                        }
                                        else
                                        {
                                            SetKeyValue("FavoriteTasks", taskskey.Replace(str, ""));
                                        }
                    */
                }
            }
        }

        private void ActivateTask(string guid)
        {
            bool found = false;

            if (tabControl1.SelectedTab.Name == guid)
                found = true;
            else
                foreach (TabPage page in tabControl1.TabPages)
                    if (page.Name == guid)
                    {
                        found = true;
                        tabControl1.SelectedTab = page;
                    }

            if (!found)
                CreateNewTabPage(guid);
        }

        private void DrawTaskPage(string guid)
        {
            foreach (TabPage page in tabControl1.TabPages)
            {
                if (page.Name == guid)
                {
                    switch (modules[guid].State)
                    {
                        case ModuleBase.ModuleState.Loaded:
                            DataToPage(guid, page);
                            break;
                        case ModuleBase.ModuleState.Unloaded:
                        case ModuleBase.ModuleState.Loading:
                        case ModuleBase.ModuleState.Saved:
                            ShowPageDataStatusMessage(page, "Loading data...");
                            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadData), guid);
                            break;
                        case ModuleBase.ModuleState.Saving:
                            ShowPageDataStatusMessage(page, "Saving data...");
                            ThreadPool.QueueUserWorkItem(new WaitCallback(SaveData), guid);
                            break;
                        case ModuleBase.ModuleState.Printing:
                            ShowPageDataStatusMessage(page, "Printing data...");
                            break;
                    }
                }
            }
        }

        private void DataToPage(string guid, TabPage page)
        {
            DataGridView dgv = new DataGridView();

            ThreadSafeModify(page, delegate { dgv = page.Controls.Find("dgv_Data", false)[0] as DataGridView; });
            ThreadSafeModify(page, delegate { ((Label)page.Controls.Find("lbl_NoData", false)[0]).Visible = false; });
            ThreadSafeModify(dgv, delegate { dgv.Visible = true; });

            lock (modules[guid].Data)
            {
                ThreadSafeModify(dgv, delegate { dgv.DataSource = modules[guid].Data; });
            }

            ThreadSafeModify(dgv, delegate { FormatCells(dgv, modules[guid].Columns); });
            ThreadSafeModify(dgv, delegate { dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells); });

            bool currPage = false;
            ThreadSafeModify(tabControl1, delegate { currPage = tabControl1.SelectedTab == page; });
            if (currPage)
            {
                ToggleToolStripButtons(guid, true);
            }
        }

        private void RefreshTab(string guid)
        {
            foreach (TabPage page in tabControl1.TabPages)
            {
                if (page.Name == guid)
                {
                    ToggleToolStripButtons(guid, false);

                    lock (modules[guid])
                    {
                        if (modules[guid].State != ModuleBase.ModuleState.Saved)
                            modules[guid].Unload();
                    }
                    DrawTaskPage(guid);
                }
            }
        }

        private void LoadData(object param)
        {
            string guid = param as string;
            ModuleBase.Module m = modules[guid];

            lock (m)
            {
                m.LoadData(new Dictionary<string, object>());
            }
            DrawTaskPage(guid);
            DrawDashboard();
        }

        private void SaveData(object param)
        {
            string guid = param as string;
            ModuleBase.Module m = modules[guid];

            lock (m)
            {
                m.SaveData();
            }
            DrawTaskPage(guid);
            DrawDashboard();
        }

        #region Load Module Threading

        private void BeginLoadModules(object args)
        {
           ThreadSafeModify(statusStrip2, delegate { activityLabel.Text = "Loading tasks...."; activityLabel.Visible = true; });
           ThreadPool.QueueUserWorkItem(new WaitCallback(PrepareLoadModules));
        }

        private void PrepareLoadModules(object args)
        {
            DirectoryInfo di = new DirectoryInfo(".\\Modules");
            FileInfo[] files = di.GetFiles("*.task");
            int step = ((int)(100 / files.Length));
            ThreadSafeModify(statusStrip2, delegate { activtyProgress.Step = step; activtyProgress.Visible = true; activtyProgress.Value = 0; activtyProgress.Maximum = step * files.Length; });
            foreach (FileInfo file in files)
                ThreadPool.QueueUserWorkItem(new WaitCallback(LoadModule), new object[] { file, files.Length });
        }

        private void LoadModule(object args)
        {
            object[] argarr = args as object[];
            FileInfo file = (FileInfo)argarr[0];
            int totalTasks = (int)argarr[1];

            XmlTextReader reader = new XmlTextReader(".\\Modules\\" + file.Name);
            object r = new object();
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.Name)
                        {
                            case "assembly":
                                reader.Read();
                                Assembly asm = Assembly.LoadFrom(".\\Modules\\" + reader.Value + ".dll");
                                Type type = asm.GetTypes()[0];
                                r = Activator.CreateInstance(type, new object[] { session, server, database });
                                break;
                            case "menu":
                                reader.Read();
                                string menuValue = reader.Value;
                                string[] menuItems = menuValue.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
                                ToolStripMenuItem currParent = tasksToolStripMenuItem;
                                for (int i = 0; i < menuItems.Length - 1; i++)
                                {
                                    ToolStripItem[] items = currParent.DropDownItems.Find(menuItems[i], true);
                                    if (items.Count() == 0)
                                    {
                                        ToolStripMenuItem newItem = new ToolStripMenuItem(menuItems[i]);
                                        newItem.Name = menuItems[i];
                                        ThreadSafeModify(menuStrip1, delegate { currParent.DropDownItems.Add(newItem); });
                                        currParent = newItem;
                                    }
                                    else
                                    {
                                        currParent = items[0] as ToolStripMenuItem;
                                    }
                                }
                                ToolStripMenuItem newMenuItem = new ToolStripMenuItem(menuItems[menuItems.Count() - 1]);
                                newMenuItem.Click += ActivateTaskFromMenu;
                                newMenuItem.Name = Guid.NewGuid().ToString();
                                modules.Add(newMenuItem.Name, r as ModuleBase.Module);
                                moduleNames.Add(newMenuItem.Name, menuItems[menuItems.Count() - 1]);
                                currParent.DropDownItems.Add(newMenuItem);
                                break;
                        }
                        break;
                }
            }
            ThreadSafeModify(toolStrip2, delegate { activtyProgress.PerformStep(); });
            ThreadPool.QueueUserWorkItem(new WaitCallback(CheckLoadModulesCompletion), totalTasks);
        }

        private void CheckLoadModulesCompletion(object args)
        {
            int totalTasks = (int)args;
            if (modules.Count == totalTasks)
            {
                ThreadSafeModify(toolStrip2, delegate { activtyProgress.Visible = false; activityLabel.Visible = false; });
                ThreadSafeModify(menuStrip1, delegate { tasksToolStripMenuItem.Enabled = true; });
            }
        }

        #endregion

        #endregion

        #region Event Handlers

        private void Form1_Shown(object sender, EventArgs e)
        {
            ShowConnectionWindow();
            ThreadPool.QueueUserWorkItem(new WaitCallback(BeginLoadModules));
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowConnectionWindow();
        }

        private void ActivateTaskFromMenu(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name != ((ToolStripMenuItem)sender).Name)
            {
                ActivateTask(((ToolStripMenuItem)sender).Name);
                DrawTaskPage(((ToolStripMenuItem)sender).Name);
            }
        }

        private void filterCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataGridView dgv = new DataGridView();
            ThreadSafeModify(tabControl1, delegate { dgv = tabControl1.SelectedTab.Controls.Find("dgv_Data", false)[0] as DataGridView; });
            if (dgv.DataSource != null)
            {
                string module = "";
                ThreadSafeModify(tabControl1, delegate { module = tabControl1.SelectedTab.Name; });
                IDropDownFilter m = modules[module] as IDropDownFilter;
                m.SelectedFilter[((ToolStripComboBox)sender).Name.Replace("cbl_Filter", "")] = ((ToolStripComboBox)sender).SelectedItem.ToString();

                CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dgv.DataSource];
                currencyManager1.SuspendBinding();

                foreach (DataGridViewRow row in dgv.Rows)
                    ThreadSafeModify(dgv, delegate { row.Visible = true; });

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    bool visible = true;
                    foreach (string filter in m.Filters.Keys)
                    {
                        object selected = null;
                        if (toolStrip2.Items["cbl_Filter" + filter] != null)
                            ThreadSafeModify(toolStrip2, delegate { selected = ((ToolStripComboBox)toolStrip2.Items["cbl_Filter" + filter]).SelectedItem; });

                        if (selected != null)
                        {
                            string filterValue = m.Filters[filter][selected.ToString()];
                            if (filterValue != "%" && row.Cells[filter].Value.ToString() != filterValue)
                                visible = false;
                        }
                    }
                    if (!visible)
                        ThreadSafeModify(dgv, delegate { row.Visible = false; });
                }

                currencyManager1.ResumeBinding();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((TabControl)sender).SelectedTab.Name != "dashboardPage")
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(BuildDropDownFilters), ((TabControl)sender).SelectedTab.Name);
                ThreadPool.QueueUserWorkItem(new WaitCallback(BuildDateFilter), ((TabControl)sender).SelectedTab.Name);
            }
            else
            {
                DestroyDateFilter();
                DestroyDropDownFilters();
            }
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dgv = tabControl1.SelectedTab.Controls.Find("dgv_Data", false)[0] as DataGridView;
            DataObject obj = dgv.GetClipboardContent();
            Clipboard.SetDataObject(obj, true);
        }

        #region Date Filter

        void backWholeMonth_Click(object sender, EventArgs e)
        {
            DateTimePicker from = toolStrip1.Controls.Find("fromMonth", true)[0] as DateTimePicker;
            DateTimePicker to = toolStrip1.Controls.Find("toMonth", true)[0] as DateTimePicker;

            DateTime originalFrom = DateTime.Parse(from.Text);
            DateTime newTo = new DateTime(originalFrom.Year, originalFrom.Month, 1).AddDays(-1);
            to.Text = newTo.ToShortDateString();
            from.Text = new DateTime(newTo.Year, newTo.Month, 1).ToShortDateString();
        }

        void nextWholeMonth_Click(object sender, EventArgs e)
        {
            DateTimePicker from = toolStrip1.Controls.Find("fromMonth", true)[0] as DateTimePicker;
            DateTimePicker to = toolStrip1.Controls.Find("toMonth", true)[0] as DateTimePicker;

            DateTime originalTo = DateTime.Parse(to.Text);
            DateTime newFrom = new DateTime(originalTo.Year, originalTo.Month, 1).AddMonths(1);
            from.Text = newFrom.ToShortDateString();
            to.Text = newFrom.AddMonths(1).AddDays(-1).ToShortDateString();
        }

        void nextMonth_Click(object sender, EventArgs e)
        {
            DateTimePicker from = toolStrip1.Controls.Find("fromMonth", true)[0] as DateTimePicker;
            DateTimePicker to = toolStrip1.Controls.Find("toMonth", true)[0] as DateTimePicker;

            from.Text = DateTime.Parse(to.Text).AddDays(1).ToShortDateString();
            to.Text = DateTime.Parse(to.Text).AddMonths(1).ToShortDateString();
        }

        void backMonth_Click(object sender, EventArgs e)
        {
            DateTimePicker from = toolStrip1.Controls.Find("fromMonth", true)[0] as DateTimePicker;
            DateTimePicker to = toolStrip1.Controls.Find("toMonth", true)[0] as DateTimePicker;

            to.Text = DateTime.Parse(from.Text).AddDays(-1).ToShortDateString();
            from.Text = DateTime.Parse(from.Text).AddMonths(-1).ToShortDateString();
        }

        void fromMonth_TextChanged(object sender, EventArgs e)
        {
            (modules[tabControl1.SelectedTab.Name] as IDateFilter).From = DateTime.Parse(((DateTimePicker)sender).Text);
        }

        void toMonth_TextChanged(object sender, EventArgs e)
        {
            (modules[tabControl1.SelectedTab.Name] as IDateFilter).To = DateTime.Parse(((DateTimePicker)sender).Text);
        }

        #endregion

        #region Tool Strip Buttons

        private void refreshButton_Click(object sender, EventArgs e)
        {
            RefreshTab(tabControl1.SelectedTab.Name);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            ToggleToolStripButtons(tabControl1.SelectedTab.Name, false);
            modules[tabControl1.SelectedTab.Name].Presave();
            DrawTaskPage(tabControl1.SelectedTab.Name);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            modules[tabControl1.SelectedTab.Name].Unload();
            refreshButton.Enabled = false;
            saveButton.Enabled = false;
            printButton.Enabled = false;
            closeButton.Enabled = false;
            DestroyDateFilter();

            int index = tabControl1.SelectedIndex;
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            tabControl1.SelectTab(index - 1);
        }

        #endregion

        #endregion
    }
}
