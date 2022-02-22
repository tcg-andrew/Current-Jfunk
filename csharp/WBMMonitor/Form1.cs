using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace WBMMonitor
{
    public partial class Form1 : Form
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AuthToken { get; set; }
        public string DepartmentID { get; set; }
        public string UserID { get; set; }
        public List<Message> messagecache { get; set; }
        public int newcount { get; set; }
        public int mycount { get; set; }
        public Timer refresh { get; set; }

        public string web_address = "http://10.78.70.92/wbm/";
        //public string web_address = "http://rails2/";

        public Form1()
        {
            InitializeComponent();
            newcount = 0;
            mycount = 0;
            messagecache = new List<Message>();
            LoginForm f = new LoginForm();
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!f.LoggedIn)
                {
                    MessageBox.Show("Failed To Login");
                    this.Close();
                }
                else
                {
                    FirstName = f.FirstName;
                    LastName = f.LastName;
                    AuthToken = f.AuthToken;
                    DepartmentID = f.DepartmentID;
                    UserID = f.UserID;
                    GetMessages();
                }
            }
            refresh = new Timer();
            refresh.Interval = 900000;
            refresh.Tick += new EventHandler(refresh_Tick);
            refresh.Enabled = true;
            refresh.Start();
            this.SizeChanged +=new EventHandler(Form1_SizeChanged);
        }

        void refresh_Tick(object sender, EventArgs e)
        {
            refresh.Stop();
            GetMessages();
            refresh.Start();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
        }

        private void GetMessages()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(web_address + "/departments/" + DepartmentID + ".json?authentication_token=" + AuthToken);
                request.ContentType = "application/json";
                request.ContentLength = 0;
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Message[] messages = null;
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string content = reader.ReadToEnd();
                    messages = js.Deserialize<Message[]>(content);
                }

                /* Purge old messages */
                List<Message> toremove = new List<Message>();
                foreach (Message cm in messagecache)
                {
                    bool found = false;
                    foreach (Message message in messages)
                    {
                        if (message.id == cm.id)
                            found = true;
                    }
                    if (!found)
                        toremove.Add(cm);
                }
                foreach (Message rm in toremove)
                    messagecache.Remove(rm);

                /* Find new messages */
                bool hasnew = false;
                foreach (Message m in messages)
                {
                    bool found = false;
                    foreach (Message cm in messagecache)
                    {
                        if (m.id == cm.id)
                            found = true;
                    }
                    if (!found)
                    {
                        if (m.department_id == DepartmentID && (String.IsNullOrEmpty(m.assigned_user) || m.assigned_user == UserID))
                        {
                            hasnew = true;
                            newcount++;
                            messagecache.Add(m);
                            lb_Items.Items.Add(m);
                            if (m.assigned_user == UserID)
                                mycount++;
                        }
                    }
                }
                if (hasnew)
                    FlashWindow.Flash(this);
                this.Text = String.Format("({0}) new messages, ({1}) assigned to you", newcount, mycount);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when retrieving messages - " + ex.Message);
                this.Close();
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                FlashWindow.Stop(this);
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                FlashWindow.Stop(this);
                lb_Items.Items.Clear();
                newcount = 0;
                mycount = 0;
                this.Text = "(0) new messages";
            }
        }

        private void lb_Items_SelectedIndexChanged(object sender, EventArgs e)
        {
            Process.Start("C:/Program Files/Google/Chrome/Application/chrome.exe", web_address +"/messages/" + ((Message)lb_Items.SelectedItem).id.ToString());
        }
    }

    public class message_response
    {
        public Message message { get; set; }
    }

    public class Message
    {
        public string id { get; set; }
        public string department { get; set; }
        public string department_id { get; set; }
        public string message_type { get; set; }
        public string workflow_step { get; set; }
        public string assigned_user { get; set; }

        public override string ToString()
        {
            return this.department + " - " + this.message_type + " - " + this.workflow_step + " - " + (String.IsNullOrEmpty(this.assigned_user) ? "unassigned" : "assigned to you");
        }
    }

    public static class FlashWindow
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            /// <summary>
            /// The size of the structure in bytes.
            /// </summary>
            public uint cbSize;
            /// <summary>
            /// A Handle to the Window to be Flashed. The window can be either opened or minimized.
            /// </summary>
            public IntPtr hwnd;
            /// <summary>
            /// The Flash Status.
            /// </summary>
            public uint dwFlags;
            /// <summary>
            /// The number of times to Flash the window.
            /// </summary>
            public uint uCount;
            /// <summary>
            /// The rate at which the Window is to be flashed, in milliseconds. If Zero, the function uses the default cursor blink rate.
            /// </summary>
            public uint dwTimeout;
        }

        /// <summary>
        /// Stop flashing. The system restores the window to its original stae.
        /// </summary>
        public const uint FLASHW_STOP = 0;

        /// <summary>
        /// Flash the window caption.
        /// </summary>
        public const uint FLASHW_CAPTION = 1;

        /// <summary>
        /// Flash the taskbar button.
        /// </summary>
        public const uint FLASHW_TRAY = 2;

        /// <summary>
        /// Flash both the window caption and taskbar button.
        /// This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags.
        /// </summary>
        public const uint FLASHW_ALL = 3;

        /// <summary>
        /// Flash continuously, until the FLASHW_STOP flag is set.
        /// </summary>
        public const uint FLASHW_TIMER = 4;

        /// <summary>
        /// Flash continuously until the window comes to the foreground.
        /// </summary>
        public const uint FLASHW_TIMERNOFG = 12;


        /// <summary>
        /// Flash the spacified Window (Form) until it recieves focus.
        /// </summary>
        /// <param name="form">The Form (Window) to Flash.</param>
        /// <returns></returns>
        public static bool Flash(System.Windows.Forms.Form form)
        {
            // Make sure we're running under Windows 2000 or later
            if (Win2000OrLater)
            {
                FLASHWINFO fi = Create_FLASHWINFO(form.Handle, FLASHW_ALL | FLASHW_TIMERNOFG, uint.MaxValue, 0);
                return FlashWindowEx(ref fi);
            }
            return false;
        }

        private static FLASHWINFO Create_FLASHWINFO(IntPtr handle, uint flags, uint count, uint timeout)
        {
            FLASHWINFO fi = new FLASHWINFO();
            fi.cbSize = Convert.ToUInt32(Marshal.SizeOf(fi));
            fi.hwnd = handle;
            fi.dwFlags = flags;
            fi.uCount = count;
            fi.dwTimeout = timeout;
            return fi;
        }

        /// <summary>
        /// Flash the specified Window (form) for the specified number of times
        /// </summary>
        /// <param name="form">The Form (Window) to Flash.</param>
        /// <param name="count">The number of times to Flash.</param>
        /// <returns></returns>
        public static bool Flash(System.Windows.Forms.Form form, uint count)
        {
            if (Win2000OrLater)
            {
                FLASHWINFO fi = Create_FLASHWINFO(form.Handle, FLASHW_ALL, count, 0);
                return FlashWindowEx(ref fi);
            }
            return false;
        }

        /// <summary>
        /// Start Flashing the specified Window (form)
        /// </summary>
        /// <param name="form">The Form (Window) to Flash.</param>
        /// <returns></returns>
        public static bool Start(System.Windows.Forms.Form form)
        {
            if (Win2000OrLater)
            {
                FLASHWINFO fi = Create_FLASHWINFO(form.Handle, FLASHW_ALL, uint.MaxValue, 0);
                return FlashWindowEx(ref fi);
            }
            return false;
        }

        /// <summary>
        /// Stop Flashing the specified Window (form)
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static bool Stop(System.Windows.Forms.Form form)
        {
            if (Win2000OrLater)
            {
                FLASHWINFO fi = Create_FLASHWINFO(form.Handle, FLASHW_STOP, uint.MaxValue, 0);
                return FlashWindowEx(ref fi);
            }
            return false;
        }

        /// <summary>
        /// A boolean value indicating whether the application is running on Windows 2000 or later.
        /// </summary>
        private static bool Win2000OrLater
        {
            get { return System.Environment.OSVersion.Version.Major >= 5; }
        }
    }
}
