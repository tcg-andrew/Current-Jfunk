#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using TCGEpicorForExcel2007.ABCCodeServiceReference;
using Office = Microsoft.Office.Core;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using TCGEpicorForExcel2007.PartServiceReference;
using ObjectLibrary;
using System.Drawing;
using TCGEpicorForExcel2007.PowerAnalyzerServiceReference;
using System.Threading;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using TCGEpicorForExcel2007.JobServiceReference;

#endregion

namespace TCGEpicorForExcel2007
{
    [ComVisible(true)]
    public class Epicor : Office.IRibbonExtensibility
    {
        #region Values

        private enum Operation { ABCCode, Part, Revisions, StandardPowerTable, SpecialPowerTable, TempModel, Job }
        private Office.IRibbonUI ribbon;
        private ServiceConnection info;
        private PlantSelection partfilters;
        private string storedUsername = "";
        private Dictionary<Operation, Guid> operationKeys;
        private Dictionary<string, abccode> codes;
        private Dictionary<string, part1> parts;
        private List<PowerAnalyzerSetting> poweranalyzer;
        private List<Revision> revisions;
        private Dictionary<string, List<string>> tempmodel;
        private Dictionary<string, Job> jobs;

        #endregion

        #region Constructors

        public Epicor()
        {
            info = new ServiceConnection();
            operationKeys = new Dictionary<Operation, Guid>();
        }

        #endregion

        #region Private Methods

        #region Isolated Storage

        private void PopulateConnectionInfoFromIsolatedStorage()
        {
            IsolatedStorageFileStream isoStream = null;
            IsolatedStorageFile isoStore = null;
            try
            {
                isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming, null, null);
                if (isoStore.FileExists("EpicorConnection.txt"))
                {
                    isoStream = new IsolatedStorageFileStream("EpicorConnection.txt", FileMode.Open, isoStore);
                    StreamReader reader = new StreamReader(isoStream);
                    info.Username = storedUsername = reader.ReadLine();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Problem loading your stored connection info.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SaveConnectionInfoToIsolatedStorage()
        {
            IsolatedStorageFileStream isoStream = null;
            IsolatedStorageFile isoStore = null;
            try
            {
                isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming, null, null);
                if (storedUsername != info.Username)
                {
                    if (isoStore.FileExists("EpicorConnection.txt"))
                        isoStream = new IsolatedStorageFileStream("EpicorConnection.txt", FileMode.Open, isoStore);
                    else
                        isoStream = new IsolatedStorageFileStream("EpicorConnection.txt", FileMode.Create, isoStore);
                    StreamWriter writer = new StreamWriter(isoStream);
                    isoStream.Position = 0;
                    writer.WriteLine(info.Username);
                    writer.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Problem saving your connection info.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Helpers

        #region General Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        private void ProcessExceptions(string exception)
        {
            if (exception.Contains("Invalid user ID or password"))
                MessageBox.Show("Invalid user ID or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (exception.Contains("Unable to connect to the AppServer"))
                MessageBox.Show("Could not connect to Epicor Server.  Please notify IT", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (exception.Contains("There was no endpoint listening"))
                MessageBox.Show("Unable to connect to TCGEpicor Service.  Please notify IT", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (exception.Contains("The HTTP request is unauthorized with client authentication scheme 'Negotiate'"))
                MessageBox.Show("You are not authorized to access the TCGEpicor Service.  If you believe this is in error, please notify IT" + Environment.NewLine + exception, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("General error in process." + " - " + exception, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        #region Windows Security Helpers

        private bool IsInRole(string role)
        {
            WindowsIdentity current = WindowsIdentity.GetCurrent();

            WindowsPrincipal princ = new WindowsPrincipal(current);
            return princ.IsInRole(role);
        }

        #endregion

        #region Service Security Helpers

        private static void SetCertificatePolicy()
        {
            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;
        }

        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            // trust any certificate!!!
            System.Console.WriteLine("Warning, trust any certificate");
            return true;
        }

        #endregion

        #region Worksheet Proptection Helpers

        private void ClearProtection(Worksheet activeWorksheet)
        {
            foreach (Operation op in operationKeys.Keys)
                foreach (CustomProperty prop in activeWorksheet.CustomProperties)
                    if (prop.Name == "guid" + op.ToString() && prop.Value == operationKeys[op].ToString())
                        try
                        {
                            activeWorksheet.Unprotect(operationKeys[op].ToString());
                        }
                        catch (Exception)
                        {
                        }
        }

        private void SetProtection(Worksheet activeWorksheet, Operation op)
        {
            if (!operationKeys.Keys.Contains(op))
                operationKeys.Add(op, Guid.NewGuid());
            else
                operationKeys[op] = Guid.NewGuid();
            bool found = false;
            foreach (CustomProperty prop in activeWorksheet.CustomProperties)
                if (prop.Name == "guid" + op.ToString())
                {
                    prop.Value = operationKeys[op].ToString();
                    found = true;
                }
                else
                    prop.Value = "cleared";

            if (!found)
                activeWorksheet.CustomProperties.Add("guid" + op.ToString(), operationKeys[op].ToString());
            activeWorksheet.Protect(operationKeys[op].ToString());
        }

        private void SetLimitedProtection(Worksheet activeWorksheet, Operation op)
        {
            if (!operationKeys.Keys.Contains(op))
                operationKeys.Add(op, Guid.NewGuid());
            else
                operationKeys[op] = Guid.NewGuid();
            bool found = false;
            foreach (CustomProperty prop in activeWorksheet.CustomProperties)
                if (prop.Name == "guid" + op.ToString())
                {
                    prop.Value = operationKeys[op].ToString();
                    found = true;
                }
                else
                    prop.Value = "cleared";

            if (!found)
                activeWorksheet.CustomProperties.Add("guid" + op.ToString(), operationKeys[op].ToString());
            activeWorksheet.Protect(operationKeys[op].ToString(), true, true, true, false, false, true, false, false, true, false, false, true, false, false, false);
        }

        #endregion

        #region Worksheet Writing Helpers

        private Worksheet GetOrCreateActiveWorksheet()
        {
            Worksheet activeWorksheet = ((Worksheet)Globals.ThisAddIn.Application.ActiveSheet);
            if (activeWorksheet == null)
            {
                Globals.ThisAddIn.Application.Workbooks.Add(Missing.Value);
                activeWorksheet = ((Worksheet)Globals.ThisAddIn.Application.ActiveSheet);
            }
            return activeWorksheet;
        }

        private Worksheet GetValidActiveWorksheetForSaveOperation(Operation op)
        {
            Worksheet activeWorksheet = null;
            if (op == Operation.StandardPowerTable)
            {
                foreach (Worksheet sheet in Globals.ThisAddIn.Application.Worksheets)
                    if (sheet.Name == "Standard Products")
                        activeWorksheet = sheet;
            }
            else if (op == Operation.SpecialPowerTable)
            {
                foreach (Worksheet sheet in Globals.ThisAddIn.Application.Worksheets)
                    if (sheet.Name == "Special Products")
                        activeWorksheet = sheet;
            }
            else
            {
                activeWorksheet = ((Worksheet)Globals.ThisAddIn.Application.ActiveSheet);
            }

            string opMessage = "";
            if (op == Operation.ABCCode)
                opMessage = "ABC Code";
            else if (op == Operation.Part)
                opMessage = "Part";
            else if (op == Operation.StandardPowerTable)
                opMessage = "Standard Products";
            else if (op == Operation.SpecialPowerTable)
                opMessage = "Special Products";
            else if (op == Operation.Job)
                opMessage = "Job";
            if (activeWorksheet == null)
                MessageBox.Show("Must have an open worksheet with " + opMessage + " data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                if (op != Operation.StandardPowerTable && op != Operation.SpecialPowerTable)
                {
                    bool secure = false;
                    foreach (CustomProperty prop in activeWorksheet.CustomProperties)
                        if (prop.Name == "guid" + op.ToString() && prop.Value == operationKeys[op].ToString())
                            secure = true;
                    if (!secure)
                    {
                        MessageBox.Show("Active worksheet does not appear to hold " + opMessage + " data that was recently loaded.  Please reload data or use the worksheet that was last loaded from the server.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        activeWorksheet = null;
                    }
                }
            }
            return activeWorksheet;
        }

        private void WriteWorksheetHeader(Worksheet worksheet, params string[] headers)
        {
            WriteWorksheetLine(worksheet, 1, headers);
            worksheet.get_Range("A1").EntireRow.Font.Bold = true;
            worksheet.get_Range("A1", "G1").EntireRow.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
        }

        private void WriteWorksheetLine(Worksheet worksheet, int rowNum, params string[] contents)
        {
            for (int i = 0; i < contents.Length; i++)
                worksheet.Cells[rowNum, i + 1].Value2 = contents[i];
        }

        #endregion

        #endregion

        #region Power Analyzer Methods

        private bool PopulatePowerAnalyzer()
        {
            PowerAnalyzerInterface powerAnalyzerInterface = new PowerAnalyzerInterface();
            poweranalyzer = powerAnalyzerInterface.GetPowerTable("SQL", "PowerAnalyzer", "rails", "hJ*G6_!pZ2");
            revisions = powerAnalyzerInterface.GetRevisions("SQL", "PowerAnalyzer", "rails", "hJ*G6_!pZ2");
            tempmodel = powerAnalyzerInterface.GetTemperatureModel("SQL", "PowerAnalyzer", "rails", "hJ*G6_!pZ2");

            return true;
        }

        private void WritePowerAnalyzer()
        {
            #region Write Revision Sheet

            Worksheet activeWorksheet = GetOrCreateActiveWorksheet();
            ClearProtection(activeWorksheet);

            if (activeWorksheet.ProtectContents)
                MessageBox.Show("The current worksheet appears to be protected.  This may be due to protection from a previous load of Epicor data.  Please switch to a new worksheet or the last worksheet that was used to load Epicor data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                int lineCursor = 1;
                foreach (Revision r in revisions)
                {
                    WriteWorksheetLine(activeWorksheet, lineCursor, r.Name, r.Comment);
                    lineCursor++;
                }

                activeWorksheet.Name = "Revisions";
                SetProtection(activeWorksheet, Operation.Revisions);
            }

            #endregion

            #region Write Power Table

            if (Globals.ThisAddIn.Application.Worksheets.Count < 2)
                Globals.ThisAddIn.Application.Worksheets.Add(Type.Missing, Globals.ThisAddIn.Application.Worksheets[1], Type.Missing, Type.Missing);
            else
                Globals.ThisAddIn.Application.Worksheets[2].Activate();
            activeWorksheet = GetOrCreateActiveWorksheet();
            ClearProtection(activeWorksheet);

            if (activeWorksheet.ProtectContents)
                MessageBox.Show("The current worksheet appears to be protected.  This may be due to protection from a previous load of Epicor data.  Please switch to a new worksheet or the last worksheet that was used to load Epicor data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                activeWorksheet.UsedRange.Clear();

                #region Write Headers

                Range r = activeWorksheet.get_Range("G2", "O2");
                r.Merge();
                r.Value2 = "Each Door";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.Orange;

                r = activeWorksheet.get_Range("P2", "AK2");
                r.Merge();
                r.Value = "Frame";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.Yellow;

                r = activeWorksheet.get_Range("AL2", "AM2");
                r.Merge();
                r.Value = "Lights";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.LawnGreen;

                r = activeWorksheet.get_Range("AN2", "AX2");
                r.Merge();
                r.Value = "Summary";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.Cyan;

                r = activeWorksheet.get_Range("G3", "I3");
                r.Merge();
                r.Value = "Frame";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("J3", "K3");
                r.Merge();
                r.Value = "Glass";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("L3", "O3");
                r.Merge();
                r.Value = "Door";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("G3", "I3");
                r.Merge();
                r.Value = "Frame";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("P3", "R3");
                r.Merge();
                r.Value = "Wrap #1";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("S3", "U3");
                r.Merge();
                r.Value = "Wrap #2";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("X3", "Z3");
                r.Merge();
                r.Value = "Each Mullion Heat";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AA3", "AC3");
                r.Merge();
                r.Value = "Each Stainless Heat";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("E4", "F4");
                r.Merge();
                r.Value = "Line Voltage";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("G4", "G5");
                r.Merge();
                r.Value = "Wire";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("H4", "I4");
                r.Merge();
                r.Value = "Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("J4", "K4");
                r.Merge();
                r.Value = "Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("L4", "M4");
                r.Merge();
                r.Value = "Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("N4", "O4");
                r.Merge();
                r.Value = "Amps";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("P4", "P5");
                r.Merge();
                r.Value = "Wire";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("Q4", "R4");
                r.Merge();
                r.Value = "Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("S4", "S5");
                r.Merge();
                r.Value = "Wire";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("T4", "U4");
                r.Merge();
                r.Value = "Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("V3", "W4");
                r.Merge();
                r.Value = "Frame Wrap Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("E4", "F4");
                r.Merge();
                r.Value = "Line Voltage";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("X4", "X5");
                r.Merge();
                r.Value = "Wire";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("Y4", "Z4");
                r.Merge();
                r.Value = "Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AA4", "AA5");
                r.Merge();
                r.Value = "Wire";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AB4", "AC4");
                r.Merge();
                r.Value = "Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AD3", "AE4");
                r.Merge();
                r.Value = "Total Mullion Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AF3", "AG4");
                r.Merge();
                r.Value = "Total Stainless Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AH3", "AI4");
                r.Merge();
                r.Value = "Total Frame Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AJ3", "AK4");
                r.Merge();
                r.Value = "Total Frame Amps";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AL3", "AM4");
                r.Merge();
                r.Value = "Amps";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AN3", "AO4");
                r.Merge();
                r.Value = "Frame and Light Amps";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AP3", "AQ4");
                r.Merge();
                r.Value = "Door and Frame Amps";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AR3", "AS4");
                r.Merge();
                r.Value = "Door and Frame and Light Amps";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AT3", "AV4");
                r.Merge();
                r.Value = "Maximum Amps";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AW3", "AX4");
                r.Merge();
                r.Value = "Rated Amps";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("E5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("F5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("H5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("I5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("J5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("K5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("L5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("M5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("N5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("O5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("Q5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("R5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("T5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("U5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("V5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("W5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("Y5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("Z5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AB5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AC5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AD5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AE5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AF5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AG5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AH5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AI5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AJ5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AK5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AL5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AM5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AN5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AO5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AP5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AQ5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AR5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AS5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AT5");
                r.Value = "Heater";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AU5");
                r.Value = "Lights";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AV5");
                r.Value = "Total";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AW5");
                r.Value = "Heater";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AX5");
                r.Value = "Lights";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                activeWorksheet.get_Range("A5").Value2 = "Style";
                activeWorksheet.get_Range("A6").Value2 = "style";
                activeWorksheet.get_Range("A7").Value2 = "0";
                activeWorksheet.get_Range("B5").Value2 = "Size";
                activeWorksheet.get_Range("B6").Value2 = "doorsize";
                activeWorksheet.get_Range("B7").Value2 = "0";
                activeWorksheet.get_Range("C5").Value2 = "Qty";
                activeWorksheet.get_Range("C6").Value2 = "qty";
                activeWorksheet.get_Range("C7").Value2 = "2";
                activeWorksheet.get_Range("D5").Value2 = "SF/DF/F";
                activeWorksheet.get_Range("D6").Value2 = "endltype";
                activeWorksheet.get_Range("D7").Value2 = "0";
                activeWorksheet.get_Range("E6").Value2 = "lowvolt";
                activeWorksheet.get_Range("E7").Value2 = "0";
                activeWorksheet.get_Range("F6").Value2 = "highvolt";
                activeWorksheet.get_Range("F7").Value2 = "0";
                activeWorksheet.get_Range("G6").Value2 = "drfrwire";
                activeWorksheet.get_Range("G7").Value2 = "1";
                activeWorksheet.get_Range("H6").Value2 = "drfrohlo";
                activeWorksheet.get_Range("H7").Value2 = "1";
                activeWorksheet.get_Range("I6").Value2 = "drfrohhi";
                activeWorksheet.get_Range("I7").Value2 = "1";
                activeWorksheet.get_Range("J6").Value2 = "drglohlo";
                activeWorksheet.get_Range("J7").Value2 = "1";
                activeWorksheet.get_Range("K6").Value2 = "drglohhi";
                activeWorksheet.get_Range("K7").Value2 = "1";
                activeWorksheet.get_Range("L6").Value2 = "drdrohlo";
                activeWorksheet.get_Range("L7").Value2 = "1";
                activeWorksheet.get_Range("M6").Value2 = "drdrohhi";
                activeWorksheet.get_Range("M7").Value2 = "1";
                activeWorksheet.get_Range("N6").Value2 = "drdraml";
                activeWorksheet.get_Range("N7").Value2 = "2";
                activeWorksheet.get_Range("O6").Value2 = "drdramh";
                activeWorksheet.get_Range("O7").Value2 = "2";
                activeWorksheet.get_Range("P6").Value2 = "frw1wire";
                activeWorksheet.get_Range("P7").Value2 = "1";
                activeWorksheet.get_Range("Q6").Value2 = "frw1ohlo";
                activeWorksheet.get_Range("Q7").Value2 = "1";
                activeWorksheet.get_Range("R6").Value2 = "frw1ohhi";
                activeWorksheet.get_Range("R7").Value2 = "1";
                activeWorksheet.get_Range("S6").Value2 = "frw2wire";
                activeWorksheet.get_Range("S7").Value2 = "1";
                activeWorksheet.get_Range("T6").Value2 = "frw2ohlo";
                activeWorksheet.get_Range("T7").Value2 = "1";
                activeWorksheet.get_Range("U6").Value2 = "frw2ohhi";
                activeWorksheet.get_Range("U7").Value2 = "1";
                activeWorksheet.get_Range("V6").Value2 = "frfwohlo";
                activeWorksheet.get_Range("V7").Value2 = "1";
                activeWorksheet.get_Range("W6").Value2 = "frfwohhi";
                activeWorksheet.get_Range("W7").Value2 = "1";
                activeWorksheet.get_Range("X6").Value2 = "frmuwire";
                activeWorksheet.get_Range("X7").Value2 = "1";
                activeWorksheet.get_Range("Y6").Value2 = "frmuohl";
                activeWorksheet.get_Range("Y7").Value2 = "1";
                activeWorksheet.get_Range("Z6").Value2 = "frmuohh";
                activeWorksheet.get_Range("Z7").Value2 = "1";
                activeWorksheet.get_Range("AA6").Value2 = "frstwire";
                activeWorksheet.get_Range("AA7").Value2 = "1";
                activeWorksheet.get_Range("AB6").Value2 = "frstohlo";
                activeWorksheet.get_Range("AB7").Value2 = "1";
                activeWorksheet.get_Range("AC6").Value2 = "frstohhi";
                activeWorksheet.get_Range("AC7").Value2 = "1";
                activeWorksheet.get_Range("AD6").Value2 = "frtmohlo";
                activeWorksheet.get_Range("AD7").Value2 = "1";
                activeWorksheet.get_Range("AE6").Value2 = "frtmohhi";
                activeWorksheet.get_Range("AE7").Value2 = "1";
                activeWorksheet.get_Range("AF6").Value2 = "frtsohlo";
                activeWorksheet.get_Range("AF7").Value2 = "1";
                activeWorksheet.get_Range("AG6").Value2 = "frtsohhi";
                activeWorksheet.get_Range("AG7").Value2 = "1";
                activeWorksheet.get_Range("AH6").Value2 = "frtfohlo";
                activeWorksheet.get_Range("AH7").Value2 = "1";
                activeWorksheet.get_Range("AI6").Value2 = "frtfohhi";
                activeWorksheet.get_Range("AI7").Value2 = "1";
                activeWorksheet.get_Range("AJ6").Value2 = "frtfamlo";
                activeWorksheet.get_Range("AJ7").Value2 = "2";
                activeWorksheet.get_Range("AK6").Value2 = "frtfamhi";
                activeWorksheet.get_Range("AK7").Value2 = "2";
                activeWorksheet.get_Range("AL6").Value2 = "ltampsl";
                activeWorksheet.get_Range("AL7").Value2 = "2";
                activeWorksheet.get_Range("AM6").Value2 = "ltampshi";
                activeWorksheet.get_Range("AM7").Value2 = "2";
                activeWorksheet.get_Range("AN6").Value2 = "suflaml";
                activeWorksheet.get_Range("AN7").Value2 = "2";
                activeWorksheet.get_Range("AO6").Value2 = "suflamh";
                activeWorksheet.get_Range("AO7").Value2 = "2";
                activeWorksheet.get_Range("AP6").Value2 = "sudfaml";
                activeWorksheet.get_Range("AP7").Value2 = "2";
                activeWorksheet.get_Range("AQ6").Value2 = "sudfam";
                activeWorksheet.get_Range("AQ7").Value2 = "2";
                activeWorksheet.get_Range("AR6").Value2 = "sudlaml";
                activeWorksheet.get_Range("AR7").Value2 = "2";
                activeWorksheet.get_Range("AS6").Value2 = "sudlamh";
                activeWorksheet.get_Range("AS7").Value2 = "2";
                activeWorksheet.get_Range("AT6").Value2 = "sumxamhe";
                activeWorksheet.get_Range("AT7").Value2 = "2";
                activeWorksheet.get_Range("AU6").Value2 = "sumxamlt";
                activeWorksheet.get_Range("AU7").Value2 = "2";
                activeWorksheet.get_Range("AV6").Value2 = "sumxamto";
                activeWorksheet.get_Range("AV7").Value2 = "2";
                activeWorksheet.get_Range("AW6").Value2 = "surtamhe";
                activeWorksheet.get_Range("AW7").Value2 = "2";
                activeWorksheet.get_Range("AX6").Value2 = "surtamlt";
                activeWorksheet.get_Range("AX7").Value2 = "2";

                activeWorksheet.get_Range("G8").Select();
                Globals.ThisAddIn.Application.ActiveWindow.FreezePanes = true;

                r = activeWorksheet.get_Range("A2", "A7").EntireRow;
                r.Font.Bold = true;

                #endregion

                int lineCursor = 8;
                List<PowerAnalyzerSetting> toWrite = poweranalyzer.Where(i => i.TemperatureCode.Length > 0).ToList();
                foreach (PowerAnalyzerSetting pas in toWrite)
                {
                    Globals.ThisAddIn.Application.StatusBar = String.Format("Writing power analyzer row {0} of {1}", lineCursor - 7, toWrite.Count);
                    WriteWorksheetLine(activeWorksheet, lineCursor, pas.TemperatureCode, pas.ModelCode, pas.NumberDoors.ToString(), pas.FrameTypeCode, pas.LowVoltage.ToString(), pas.HighVoltage.ToString(), pas.DoorFrameWire, pas.DoorFrameOhms.Low, pas.DoorFrameOhms.High, pas.DoorGlassOhms.Low, pas.DoorGlassOhms.High, pas.DoorDoorOhms.Low, pas.DoorDoorOhms.High, pas.DoorDoorAmps.Low, pas.DoorDoorAmps.High, pas.FrameWrap1Wire, pas.FrameWrap1Ohms.Low, pas.FrameWrap1Ohms.High, pas.FrameWrap2Wire, pas.FrameWrap2Ohms.Low, pas.FrameWrap2Ohms.High, pas.FrameWrapOhms.Low, pas.FrameWrapOhms.High, pas.FrameMullionWire, pas.FrameMullionOhms.Low, pas.FrameMullionOhms.High, pas.FrameStainlessWire, pas.FrameStainlessOhms.Low, pas.FrameStainlessOhms.High, pas.FrameTotalMullionOhms.Low, pas.FrameTotalMullionOhms.High, pas.FrameTotalStainlessOhms.Low, pas.FrameTotalStainlessOhms.High, pas.FrameOhms.Low, pas.FrameOhms.High, pas.FrameAmps.Low, pas.FrameAmps.High, pas.LightAmps.Low, pas.LightAmps.High, pas.SummaryFrameAndLightAmps.Low, pas.SummaryFrameAndLightAmps.High, pas.SummaryDoorAndFrameAmps.Low, pas.SummaryDoorAndFrameAmps.High, pas.SummaryDoorAndFrameAndLampAmps.Low, pas.SummaryDoorAndFrameAndLampAmps.High, pas.SummaryMaxAmpsHeater, pas.SummaryMaxAmpsLights, pas.SummaryMaxAmpsTotal, pas.SummaryRatedAmpsHeater, pas.SummaryRatedAmpsLights);
                    lineCursor++;
                }
                activeWorksheet.UsedRange.RowHeight = "12.75";
                activeWorksheet.UsedRange.HorizontalAlignment = 3;
                activeWorksheet.UsedRange.VerticalAlignment = 2;
                activeWorksheet.UsedRange.Font.Name = "Arial";
                activeWorksheet.UsedRange.Font.Size = 10;
                activeWorksheet.UsedRange.WrapText = true;

                activeWorksheet.Name = "Standard Products";
                Globals.ThisAddIn.Application.ActiveWindow.Zoom = "75";

                activeWorksheet.UsedRange.EntireRow.EntireColumn.Locked = false;
                activeWorksheet.get_Range("A1", "A7").EntireRow.Locked = true;

                SetLimitedProtection(activeWorksheet, Operation.StandardPowerTable);

            }

            #endregion

            #region Write Temperature Model

            if (Globals.ThisAddIn.Application.Worksheets.Count < 3)
                Globals.ThisAddIn.Application.Worksheets.Add(Type.Missing, Globals.ThisAddIn.Application.Worksheets[2], Type.Missing, Type.Missing);
            else
                Globals.ThisAddIn.Application.Worksheets[3].Activate();
            activeWorksheet = GetOrCreateActiveWorksheet();
            ClearProtection(activeWorksheet);

            if (activeWorksheet.ProtectContents)
                MessageBox.Show("The current worksheet appears to be protected.  This may be due to protection from a previous load of Epicor data.  Please switch to a new worksheet or the last worksheet that was used to load Epicor data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                activeWorksheet.UsedRange.Clear();
                activeWorksheet.Cells[1, 1].Value2 = "Temp";
                activeWorksheet.Cells[1, 2].Value2 = "Models";
                activeWorksheet.get_Range("A1", "M1").Borders[XlBordersIndex.xlEdgeBottom].Color = Color.Black;
                activeWorksheet.get_Range("A1", "M1").Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                activeWorksheet.get_Range("A1", "A14").Borders[XlBordersIndex.xlEdgeRight].Color = Color.Black;
                activeWorksheet.get_Range("A1", "A14").Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;

                int lineCursor = 2;
                foreach (string key in tempmodel.Keys)
                {
                    List<string> row = new List<string>();
                    row.Add(key);
                    row.AddRange(tempmodel[key]);
                    WriteWorksheetLine(activeWorksheet, lineCursor, row.ToArray());
                    lineCursor++;
                }

                activeWorksheet.Name = "Temperature Model";
                activeWorksheet.get_Range("A1").EntireColumn.Font.Bold = true;
                activeWorksheet.get_Range("A1").EntireRow.Font.Bold = true;

                activeWorksheet.UsedRange.EntireRow.EntireColumn.Locked = false;
                activeWorksheet.get_Range("A1").EntireRow.Locked = true;
                activeWorksheet.get_Range("A1").EntireColumn.Locked = true;

                SetProtection(activeWorksheet, Operation.TempModel);
            }

            #endregion

            #region Write Special Products Table

            Globals.ThisAddIn.Application.Worksheets.Add(Type.Missing, Globals.ThisAddIn.Application.Worksheets[3], Type.Missing, Type.Missing);
            activeWorksheet = GetOrCreateActiveWorksheet();
            ClearProtection(activeWorksheet);

            if (activeWorksheet.ProtectContents)
                MessageBox.Show("The current worksheet appears to be protected.  This may be due to protection from a previous load of Epicor data.  Please switch to a new worksheet or the last worksheet that was used to load Epicor data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                #region Write Headers

                Range r = activeWorksheet.get_Range("G2", "O2");
                r.Merge();
                r.Value2 = "Each Door";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.Orange;

                r = activeWorksheet.get_Range("P2", "AK2");
                r.Merge();
                r.Value = "Frame";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.Yellow;

                r = activeWorksheet.get_Range("AL2", "AM2");
                r.Merge();
                r.Value = "Lights";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.LawnGreen;

                r = activeWorksheet.get_Range("AN2", "AX2");
                r.Merge();
                r.Value = "Summary";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.Cyan;

                r = activeWorksheet.get_Range("G3", "I3");
                r.Merge();
                r.Value = "Frame";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("J3", "K3");
                r.Merge();
                r.Value = "Glass";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("L3", "O3");
                r.Merge();
                r.Value = "Door";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("G3", "I3");
                r.Merge();
                r.Value = "Frame";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("P3", "R3");
                r.Merge();
                r.Value = "Wrap #1";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("S3", "U3");
                r.Merge();
                r.Value = "Wrap #2";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("X3", "Z3");
                r.Merge();
                r.Value = "Each Mullion Heat";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AA3", "AC3");
                r.Merge();
                r.Value = "Each Stainless Heat";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("E4", "F4");
                r.Merge();
                r.Value = "Line Voltage";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("G4", "G5");
                r.Merge();
                r.Value = "Wire";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("H4", "I4");
                r.Merge();
                r.Value = "Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("J4", "K4");
                r.Merge();
                r.Value = "Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("L4", "M4");
                r.Merge();
                r.Value = "Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("N4", "O4");
                r.Merge();
                r.Value = "Amps";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("P4", "P5");
                r.Merge();
                r.Value = "Wire";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("Q4", "R4");
                r.Merge();
                r.Value = "Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("S4", "S5");
                r.Merge();
                r.Value = "Wire";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("T4", "U4");
                r.Merge();
                r.Value = "Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("V3", "W4");
                r.Merge();
                r.Value = "Frame Wrap Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("E4", "F4");
                r.Merge();
                r.Value = "Line Voltage";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("X4", "X5");
                r.Merge();
                r.Value = "Wire";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("Y4", "Z4");
                r.Merge();
                r.Value = "Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AA4", "AA5");
                r.Merge();
                r.Value = "Wire";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AB4", "AC4");
                r.Merge();
                r.Value = "Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AD3", "AE4");
                r.Merge();
                r.Value = "Total Mullion Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AF3", "AG4");
                r.Merge();
                r.Value = "Total Stainless Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AH3", "AI4");
                r.Merge();
                r.Value = "Total Frame Ohms";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AJ3", "AK4");
                r.Merge();
                r.Value = "Total Frame Amps";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AL3", "AM4");
                r.Merge();
                r.Value = "Amps";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AN3", "AO4");
                r.Merge();
                r.Value = "Frame and Light Amps";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AP3", "AQ4");
                r.Merge();
                r.Value = "Door and Frame Amps";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AR3", "AS4");
                r.Merge();
                r.Value = "Door and Frame and Light Amps";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AT3", "AV4");
                r.Merge();
                r.Value = "Maximum Amps";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AW3", "AX4");
                r.Merge();
                r.Value = "Rated Amps";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("E5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("F5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("H5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("I5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("J5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("K5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("L5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("M5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("N5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("O5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("Q5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("R5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("T5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("U5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("V5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("W5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("Y5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("Z5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AB5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AC5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AD5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AE5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AF5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AG5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AH5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AI5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AJ5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AK5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AL5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AM5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);

                r = activeWorksheet.get_Range("AN5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AO5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AP5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AQ5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AR5");
                r.Value = "Low";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AS5");
                r.Value = "High";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AT5");
                r.Value = "Heater";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AU5");
                r.Value = "Lights";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AV5");
                r.Value = "Total";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AW5");
                r.Value = "Heater";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                r = activeWorksheet.get_Range("AX5");
                r.Value = "Lights";
                r.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Color.Black);
                r.Interior.Color = Color.HotPink;

                activeWorksheet.get_Range("A5").Value2 = "Style";
                activeWorksheet.get_Range("A6").Value2 = "style";
                activeWorksheet.get_Range("A7").Value2 = "0";
                activeWorksheet.get_Range("B5").Value2 = "Item Number";
                activeWorksheet.get_Range("B6").Value2 = "item";
                activeWorksheet.get_Range("B7").Value2 = "0";
                activeWorksheet.get_Range("C5").Value2 = "Qty";
                activeWorksheet.get_Range("C6").Value2 = "qty";
                activeWorksheet.get_Range("C7").Value2 = "2";
                activeWorksheet.get_Range("D5").Value2 = "SF/DF/F";
                activeWorksheet.get_Range("D6").Value2 = "endltype";
                activeWorksheet.get_Range("D7").Value2 = "0";
                activeWorksheet.get_Range("E6").Value2 = "lowvolt";
                activeWorksheet.get_Range("E7").Value2 = "0";
                activeWorksheet.get_Range("F6").Value2 = "highvolt";
                activeWorksheet.get_Range("F7").Value2 = "0";
                activeWorksheet.get_Range("G6").Value2 = "drfrwire";
                activeWorksheet.get_Range("G7").Value2 = "1";
                activeWorksheet.get_Range("H6").Value2 = "drfrohlo";
                activeWorksheet.get_Range("H7").Value2 = "1";
                activeWorksheet.get_Range("I6").Value2 = "drfrohhi";
                activeWorksheet.get_Range("I7").Value2 = "1";
                activeWorksheet.get_Range("J6").Value2 = "drglohlo";
                activeWorksheet.get_Range("J7").Value2 = "1";
                activeWorksheet.get_Range("K6").Value2 = "drglohhi";
                activeWorksheet.get_Range("K7").Value2 = "1";
                activeWorksheet.get_Range("L6").Value2 = "drdrohlo";
                activeWorksheet.get_Range("L7").Value2 = "1";
                activeWorksheet.get_Range("M6").Value2 = "drdrohhi";
                activeWorksheet.get_Range("M7").Value2 = "1";
                activeWorksheet.get_Range("N6").Value2 = "drdraml";
                activeWorksheet.get_Range("N7").Value2 = "2";
                activeWorksheet.get_Range("O6").Value2 = "drdramh";
                activeWorksheet.get_Range("O7").Value2 = "2";
                activeWorksheet.get_Range("P6").Value2 = "frw1wire";
                activeWorksheet.get_Range("P7").Value2 = "1";
                activeWorksheet.get_Range("Q6").Value2 = "frw1ohlo";
                activeWorksheet.get_Range("Q7").Value2 = "1";
                activeWorksheet.get_Range("R6").Value2 = "frw1ohhi";
                activeWorksheet.get_Range("R7").Value2 = "1";
                activeWorksheet.get_Range("S6").Value2 = "frw2wire";
                activeWorksheet.get_Range("S7").Value2 = "1";
                activeWorksheet.get_Range("T6").Value2 = "frw2ohlo";
                activeWorksheet.get_Range("T7").Value2 = "1";
                activeWorksheet.get_Range("U6").Value2 = "frw2ohhi";
                activeWorksheet.get_Range("U7").Value2 = "1";
                activeWorksheet.get_Range("V6").Value2 = "frfwohlo";
                activeWorksheet.get_Range("V7").Value2 = "1";
                activeWorksheet.get_Range("W6").Value2 = "frfwohhi";
                activeWorksheet.get_Range("W7").Value2 = "1";
                activeWorksheet.get_Range("X6").Value2 = "frmuwire";
                activeWorksheet.get_Range("X7").Value2 = "1";
                activeWorksheet.get_Range("Y6").Value2 = "frmuohl";
                activeWorksheet.get_Range("Y7").Value2 = "1";
                activeWorksheet.get_Range("Z6").Value2 = "frmuohh";
                activeWorksheet.get_Range("Z7").Value2 = "1";
                activeWorksheet.get_Range("AA6").Value2 = "frstwire";
                activeWorksheet.get_Range("AA7").Value2 = "1";
                activeWorksheet.get_Range("AB6").Value2 = "frstohlo";
                activeWorksheet.get_Range("AB7").Value2 = "1";
                activeWorksheet.get_Range("AC6").Value2 = "frstohhi";
                activeWorksheet.get_Range("AC7").Value2 = "1";
                activeWorksheet.get_Range("AD6").Value2 = "frtmohlo";
                activeWorksheet.get_Range("AD7").Value2 = "1";
                activeWorksheet.get_Range("AE6").Value2 = "frtmohhi";
                activeWorksheet.get_Range("AE7").Value2 = "1";
                activeWorksheet.get_Range("AF6").Value2 = "frtsohlo";
                activeWorksheet.get_Range("AF7").Value2 = "1";
                activeWorksheet.get_Range("AG6").Value2 = "frtsohhi";
                activeWorksheet.get_Range("AG7").Value2 = "1";
                activeWorksheet.get_Range("AH6").Value2 = "frtfohlo";
                activeWorksheet.get_Range("AH7").Value2 = "1";
                activeWorksheet.get_Range("AI6").Value2 = "frtfohhi";
                activeWorksheet.get_Range("AI7").Value2 = "1";
                activeWorksheet.get_Range("AJ6").Value2 = "frtfamlo";
                activeWorksheet.get_Range("AJ7").Value2 = "2";
                activeWorksheet.get_Range("AK6").Value2 = "frtfamhi";
                activeWorksheet.get_Range("AK7").Value2 = "2";
                activeWorksheet.get_Range("AL6").Value2 = "ltampsl";
                activeWorksheet.get_Range("AL7").Value2 = "2";
                activeWorksheet.get_Range("AM6").Value2 = "ltampshi";
                activeWorksheet.get_Range("AM7").Value2 = "2";
                activeWorksheet.get_Range("AN6").Value2 = "suflaml";
                activeWorksheet.get_Range("AN7").Value2 = "2";
                activeWorksheet.get_Range("AO6").Value2 = "suflamh";
                activeWorksheet.get_Range("AO7").Value2 = "2";
                activeWorksheet.get_Range("AP6").Value2 = "sudfaml";
                activeWorksheet.get_Range("AP7").Value2 = "2";
                activeWorksheet.get_Range("AQ6").Value2 = "sudfam";
                activeWorksheet.get_Range("AQ7").Value2 = "2";
                activeWorksheet.get_Range("AR6").Value2 = "sudlaml";
                activeWorksheet.get_Range("AR7").Value2 = "2";
                activeWorksheet.get_Range("AS6").Value2 = "sudlamh";
                activeWorksheet.get_Range("AS7").Value2 = "2";
                activeWorksheet.get_Range("AT6").Value2 = "sumxamhe";
                activeWorksheet.get_Range("AT7").Value2 = "2";
                activeWorksheet.get_Range("AU6").Value2 = "sumxamlt";
                activeWorksheet.get_Range("AU7").Value2 = "2";
                activeWorksheet.get_Range("AV6").Value2 = "sumxamto";
                activeWorksheet.get_Range("AV7").Value2 = "2";
                activeWorksheet.get_Range("AW6").Value2 = "surtamhe";
                activeWorksheet.get_Range("AW7").Value2 = "2";
                activeWorksheet.get_Range("AX6").Value2 = "surtamlt";
                activeWorksheet.get_Range("AX7").Value2 = "2";

                activeWorksheet.get_Range("G8").Select();
                Globals.ThisAddIn.Application.ActiveWindow.FreezePanes = true;

                r = activeWorksheet.get_Range("A2", "A7").EntireRow;
                r.Font.Bold = true;

                #endregion

                int lineCursor = 8;
                List<PowerAnalyzerSetting> toWrite = poweranalyzer.Where(i => i.TemperatureCode.Length == 0).ToList();
                foreach (PowerAnalyzerSetting pas in toWrite)
                {
                    Globals.ThisAddIn.Application.StatusBar = String.Format("Writing power analyzer row {0} of {1}", lineCursor - 7, toWrite.Count);
                    WriteWorksheetLine(activeWorksheet, lineCursor, (pas.IsDoor ? pas.IsFrame ? "BOTH" : "DOOR" : "FRAM"), pas.Item, pas.NumberDoors.ToString(), pas.FrameTypeCode, pas.LowVoltage.ToString(), pas.HighVoltage.ToString(), pas.DoorFrameWire, pas.DoorFrameOhms.Low, pas.DoorFrameOhms.High, pas.DoorGlassOhms.Low, pas.DoorGlassOhms.High, pas.DoorDoorOhms.Low, pas.DoorDoorOhms.High, pas.DoorDoorAmps.Low, pas.DoorDoorAmps.High, pas.FrameWrap1Wire, pas.FrameWrap1Ohms.Low, pas.FrameWrap1Ohms.High, pas.FrameWrap2Wire, pas.FrameWrap2Ohms.Low, pas.FrameWrap2Ohms.High, pas.FrameWrapOhms.Low, pas.FrameWrapOhms.High, pas.FrameMullionWire, pas.FrameMullionOhms.Low, pas.FrameMullionOhms.High, pas.FrameStainlessWire, pas.FrameStainlessOhms.Low, pas.FrameStainlessOhms.High, pas.FrameTotalMullionOhms.Low, pas.FrameTotalMullionOhms.High, pas.FrameTotalStainlessOhms.Low, pas.FrameTotalStainlessOhms.High, pas.FrameOhms.Low, pas.FrameOhms.High, pas.FrameAmps.Low, pas.FrameAmps.High, pas.LightAmps.Low, pas.LightAmps.High, pas.SummaryFrameAndLightAmps.Low, pas.SummaryFrameAndLightAmps.High, pas.SummaryDoorAndFrameAmps.Low, pas.SummaryDoorAndFrameAmps.High, pas.SummaryDoorAndFrameAndLampAmps.Low, pas.SummaryDoorAndFrameAndLampAmps.High, pas.SummaryMaxAmpsHeater, pas.SummaryMaxAmpsLights, pas.SummaryMaxAmpsTotal, pas.SummaryRatedAmpsHeater, pas.SummaryRatedAmpsLights);
                    lineCursor++;
                }
                activeWorksheet.UsedRange.RowHeight = "12.75";
                activeWorksheet.UsedRange.HorizontalAlignment = 3;
                activeWorksheet.UsedRange.VerticalAlignment = 2;
                activeWorksheet.UsedRange.Font.Name = "Arial";
                activeWorksheet.UsedRange.Font.Size = 10;
                activeWorksheet.UsedRange.WrapText = true;

                activeWorksheet.get_Range("B1").EntireColumn.AutoFit();
                activeWorksheet.Name = "Special Products";

                activeWorksheet.UsedRange.EntireRow.EntireColumn.Locked = false;
                activeWorksheet.get_Range("A1", "A7").EntireRow.Locked = true;

                SetLimitedProtection(activeWorksheet, Operation.StandardPowerTable);
            }

            #endregion
        }

        private List<PowerAnalyzerSetting> PrepareStandardPowerTable(Worksheet activeWorksheet)
        {
            List<PowerAnalyzerSetting> powertable = new List<PowerAnalyzerSetting>();

            bool reading = false;

            foreach (Range row in activeWorksheet.UsedRange.Rows)
            {
                if (reading)
                {
                    if (row.Cells[1, 1].Value2 != null && !String.IsNullOrEmpty(row.Cells[1, 1].Value2.ToString()))
                    {
                        PowerAnalyzerSetting newSetting = new PowerAnalyzerSetting(row.Cells[1, 1].Value2.ToString(), row.Cells[1, 2].Value2.ToString(), Int32.Parse(row.Cells[1, 3].Value2.ToString()), row.Cells[1, 4].Value2.ToString(), Int32.Parse(row.Cells[1, 5].Value2.ToString()), Int32.Parse(row.Cells[1, 6].Value2.ToString()), false, false, "", row.Cells[1, 7].Value2.ToString(), row.Cells[1, 8].Value2.ToString(), row.Cells[1, 9].Value2.ToString(), row.Cells[1, 10].Value2.ToString(), row.Cells[1, 11].Value2.ToString(), row.Cells[1, 12].Value2.ToString(), row.Cells[1, 13].Value2.ToString(), row.Cells[1, 14].Value2.ToString(), row.Cells[1, 15].Value2.ToString(), row.Cells[1, 16].Value2.ToString(), row.Cells[1, 17].Value2.ToString(), row.Cells[1, 18].Value2.ToString(), row.Cells[1, 19].Value2.ToString(), row.Cells[1, 20].Value2.ToString(), row.Cells[1, 21].Value2.ToString(), row.Cells[1, 22].Value2.ToString(), row.Cells[1, 23].Value2.ToString(), row.Cells[1, 24].Value2.ToString(), row.Cells[1, 25].Value2.ToString(), row.Cells[1, 26].Value2.ToString(), row.Cells[1, 27].Value2.ToString(), row.Cells[1, 28].Value2.ToString(), row.Cells[1, 29].Value2.ToString(), row.Cells[1, 30].Value2.ToString(), row.Cells[1, 31].Value2.ToString(), row.Cells[1, 32].Value2.ToString(), row.Cells[1, 33].Value2.ToString(), row.Cells[1, 34].Value2.ToString(), row.Cells[1, 35].Value2.ToString(), row.Cells[1, 36].Value2.ToString(), row.Cells[1, 37].Value2.ToString(), row.Cells[1, 38].Value2.ToString(), row.Cells[1, 39].Value2.ToString(), row.Cells[1, 40].Value2.ToString(), row.Cells[1, 41].Value2.ToString(), row.Cells[1, 42].Value2.ToString(), row.Cells[1, 43].Value2.ToString(), row.Cells[1, 44].Value2.ToString(), row.Cells[1, 45].Value2.ToString(), row.Cells[1, 46].Value2.ToString(), row.Cells[1, 47].Value2.ToString(), row.Cells[1, 48].Value2.ToString(), row.Cells[1, 49].Value2.ToString(), row.Cells[1, 50].Value2.ToString());
                        powertable.Add(newSetting);

                        
                    }
                }

                if (row.Cells[1, 1].Value2 != null && row.Cells[1, 1].Value2.ToString() == "0")
                    reading = true;
            }

            return powertable;
        }

        private List<PowerAnalyzerSetting> PrepareSpecialPowerTable(Worksheet activeWorksheet)
        {
            List<PowerAnalyzerSetting> powertable = new List<PowerAnalyzerSetting>();

            bool reading = false;

            foreach (Range row in activeWorksheet.UsedRange.Rows)
            {
                if (reading)
                {
                    if (row.Cells[1, 1].Value2 != null && !String.IsNullOrEmpty(row.Cells[1, 1].Value2.ToString()))
                    {
                        string type = row.Cells[1, 1].Value2.ToString();
                        bool isDoor = (type == "DOOR" || type == "BOTH");
                        bool isFrame = (type == "FRAM" || type == "BOTH");

                        PowerAnalyzerSetting newSetting = new PowerAnalyzerSetting("", "", Int32.Parse(row.Cells[1, 3].Value2.ToString()), row.Cells[1, 4].Value2.ToString(), Int32.Parse(row.Cells[1, 5].Value2.ToString()), Int32.Parse(row.Cells[1, 6].Value2.ToString()), isDoor, isFrame, row.Cells[1, 2].Value2.ToString(), row.Cells[1, 7].Value2.ToString(), row.Cells[1, 8].Value2.ToString(), row.Cells[1, 9].Value2.ToString(), row.Cells[1, 10].Value2.ToString(), row.Cells[1, 11].Value2.ToString(), row.Cells[1, 12].Value2.ToString(), row.Cells[1, 13].Value2.ToString(), row.Cells[1, 14].Value2.ToString(), row.Cells[1, 15].Value2.ToString(), row.Cells[1, 16].Value2.ToString(), row.Cells[1, 17].Value2.ToString(), row.Cells[1, 18].Value2.ToString(), row.Cells[1, 19].Value2.ToString(), row.Cells[1, 20].Value2.ToString(), row.Cells[1, 21].Value2.ToString(), row.Cells[1, 22].Value2.ToString(), row.Cells[1, 23].Value2.ToString(), row.Cells[1, 24].Value2.ToString(), row.Cells[1, 25].Value2.ToString(), row.Cells[1, 26].Value2.ToString(), row.Cells[1, 27].Value2.ToString(), row.Cells[1, 28].Value2.ToString(), row.Cells[1, 29].Value2.ToString(), row.Cells[1, 30].Value2.ToString(), row.Cells[1, 31].Value2.ToString(), row.Cells[1, 32].Value2.ToString(), row.Cells[1, 33].Value2.ToString(), row.Cells[1, 34].Value2.ToString(), row.Cells[1, 35].Value2.ToString(), row.Cells[1, 36].Value2.ToString(), row.Cells[1, 37].Value2.ToString(), row.Cells[1, 38].Value2.ToString(), row.Cells[1, 39].Value2.ToString(), row.Cells[1, 40].Value2.ToString(), row.Cells[1, 41].Value2.ToString(), row.Cells[1, 42].Value2.ToString(), row.Cells[1, 43].Value2.ToString(), row.Cells[1, 44].Value2.ToString(), row.Cells[1, 45].Value2.ToString(), row.Cells[1, 46].Value2.ToString(), row.Cells[1, 47].Value2.ToString(), row.Cells[1, 48].Value2.ToString(), row.Cells[1, 49].Value2.ToString(), row.Cells[1, 50].Value2.ToString());
                        powertable.Add(newSetting);
                    }
                }

                if (row.Cells[1, 1].Value2 != null && row.Cells[1, 1].Value2.ToString() == "0")
                    reading = true;
            }

            return powertable;
        }

        #endregion

        #region Job Methods

        private bool PopulateJobs()
        {
            JobServiceClient client = new JobServiceClient("windows");
            SetCertificatePolicy();

            jobgetmismatchresult jobInterface = client.GetJobsWithMismatchedDates();
            string exception = jobInterface.exception;

            jobs = new Dictionary<string, Job>();

            foreach (Job j in jobInterface.epicor)
                jobs.Add(j.JobNum.ToUpper(), j);

            if (!String.IsNullOrEmpty(exception))
            {
                ProcessExceptions(exception);
                return false;
            }
            else
                return true;
        }

        private void WriteJobs()
        {
            Worksheet activeWorksheet = GetOrCreateActiveWorksheet();
            ClearProtection(activeWorksheet);

            if (activeWorksheet.ProtectContents)
                MessageBox.Show("The current worksheet appears to be protected.  This may be due to protection from a previous load of Epicor data.  Please switch to a new worksheet or the last worksheet that was used to load Epicor data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                activeWorksheet.UsedRange.Clear();
                WriteWorksheetHeader(activeWorksheet, "Job Number", "Order #", "Order Line #", "Order Rel #", "Order Date", "Job Date", "Process");
                int lineCursor = 2;

                activeWorksheet.Cells[1, 8].Value2 = "TRUE";
                activeWorksheet.Cells[2, 8].Value2 = "FALSE";
                activeWorksheet.get_Range("H1").EntireColumn.Hidden = true;
                foreach (Job j in jobs.Values)
                {
                    activeWorksheet.get_Range("A" + lineCursor.ToString(), "D" + lineCursor.ToString()).NumberFormat = "@";
                    WriteWorksheetLine(activeWorksheet, lineCursor, j.JobNum, j.OrderNum, j.OrderLine.ToString(), j.OrderRel.ToString(), j.OrderDate.ToString("MM/dd/yyyy"), j.ReqDate.ToString("MM/dd/yyyy"));
                    Range c = activeWorksheet.get_Range("E" + lineCursor.ToString(), "F" + lineCursor.ToString());
                    c.Interior.Color = System.Drawing.Color.Yellow;

                    Range r = activeWorksheet.get_Range("G" + lineCursor.ToString());
                    r.Validation.Add(XlDVType.xlValidateList, Missing.Value, Missing.Value, "=H1:H2", Missing.Value);
                    r.Validation.InCellDropdown = true;
                    r.Validation.IgnoreBlank = false;
                    r.Value2 = "FALSE";
                    activeWorksheet.get_Range("E" + lineCursor.ToString(), "G" + lineCursor.ToString()).Locked = false;
                    lineCursor++;
                }
                activeWorksheet.get_Range("A1", "G1").EntireColumn.AutoFit();

                activeWorksheet.get_Range("A1").EntireColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                SetProtection(activeWorksheet, Operation.Job);
            }
        }

        private Dictionary<string, Job> PrepareJobs(Worksheet activeWorksheet)
        {
            Dictionary<string, Job> result = new Dictionary<string, Job>();

            bool reading = true;
            int lineCursor = 2;
            string errors = "";
            while (reading)
            {
                if (!String.IsNullOrEmpty(activeWorksheet.Cells[lineCursor, 1].Text))
                {
                    if (activeWorksheet.Cells[lineCursor, 7].Text == "TRUE")
                    {
                        #region Data Validation and Population

                        Job newJob = new Job();
                        newJob.JobNum = activeWorksheet.Cells[lineCursor, 1].Value.ToString();
                        newJob.OrderNum = activeWorksheet.Cells[lineCursor, 2].Value.ToString();
                        newJob.OrderLine = Int32.Parse(activeWorksheet.Cells[lineCursor, 3].Value.ToString());
                        newJob.OrderRel = Int32.Parse(activeWorksheet.Cells[lineCursor, 4].Value.ToString());
                        try
                        {
                            DateTime orderDate = DateTime.Parse(activeWorksheet.Cells[lineCursor, 5].Value.ToString());
                            newJob.OrderDate = orderDate;
                        }
                        catch (Exception)
                        {
                            errors += "Invalid Order Date for job " + newJob.JobNum + ". Value must be a valid date." + Environment.NewLine;
                        }
                        try
                        {
                            DateTime jobDate = DateTime.Parse(activeWorksheet.Cells[lineCursor, 5].Value.ToString());
                            newJob.ReqDate = jobDate;
                        }
                        catch (Exception)
                        {
                            errors += "Invalid Job Date for job " + newJob.JobNum + ". Value must be a valid date." + Environment.NewLine;
                        }

                        result.Add(newJob.JobNum, newJob);

                        #endregion
                    }
                    lineCursor++;
                }
                else
                    reading = false;
            }

            if (!String.IsNullOrEmpty(errors))
            {
                MessageBox.Show("The following data validation errors were detected.  Please fix before saving" + Environment.NewLine + Environment.NewLine + errors, "Validation failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            else
                return result;

        }

        private void SaveJobs(string username, string password, Dictionary<string, Job> newJobs)
        {
            Dictionary<string, Job> verifyJobs = jobs;

            if (PopulateJobs())
            {
                bool valid = true;
                foreach (string job in jobs.Keys)
                {
                    if (verifyJobs.Keys.Contains(job))
                    {
                        if (verifyJobs[job].OrderDate != jobs[job].OrderDate || verifyJobs[job].ReqDate != verifyJobs[job].ReqDate)
                            valid = false;
                    }
                    else
                        valid = false;
                }
                if (valid)
                {
                    foreach (string job in verifyJobs.Keys)
                        if (!jobs.Keys.Contains(job))
                            valid = false;
                }

                if (!valid)
                {
                    MessageBox.Show("The Job data has changed since it was loaded.  Please reload and perform edits again", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    JobServiceClient client = new JobServiceClient("windows");
                    SetCertificatePolicy();

                    string exception = client.UpdateJobDates(username, password, newJobs.Values.ToArray()).exception;
                    if (!String.IsNullOrEmpty(exception))
                        ProcessExceptions(exception);
                }
            }
        }

        #endregion

        #region Part Methods

        private bool FilterParts()
        {
            PartServiceClient client = new PartServiceClient("windows");
            SetCertificatePolicy();

            partgetstringresult plants = client.getplants();
            partgetclassresult classes = client.getpartclasses();

            if (!String.IsNullOrEmpty(plants.exception))
            {
                ProcessExceptions(plants.exception);
                return false;
            }
            else if (!String.IsNullOrEmpty(classes.exception))
            {
                ProcessExceptions(classes.exception);
                return false;
            }
            else
            {
                string plant = "";
                if (partfilters != null)
                    plant = partfilters.SelectedPlant;
                partfilters = new PlantSelection();
                partfilters.Plants = plants.epicor.ToList();
                partfilters.Classes = classes.epicor.ToList();
                if (!String.IsNullOrEmpty(plant))
                    partfilters.SelectedPlant = plant;

                if (partfilters.ShowDialog() == DialogResult.OK)
                    return true;
                else
                    return false;
            }
        }

        private bool PopulateParts()
        {
            PartServiceClient client = new PartServiceClient("windows");
            SetCertificatePolicy();

            partgetneedingupdateresult partInterface = client.getpartsneedingupdate(partfilters.SelectedPlant, partfilters.SelectedClass);
            string exception = partInterface.exception;

            parts = new Dictionary<string, part1>();

            foreach (part1 p in partInterface.epicor)
                parts.Add(p.partnum.ToUpper(), p);

            if (!String.IsNullOrEmpty(exception))
            {
                ProcessExceptions(exception);
                return false;
            }
            else
                return true;
        }

        private void WriteParts()
        {
            Worksheet activeWorksheet = GetOrCreateActiveWorksheet();
            ClearProtection(activeWorksheet);

            if (activeWorksheet.ProtectContents)
                MessageBox.Show("The current worksheet appears to be protected.  This may be due to protection from a previous load of Epicor data.  Please switch to a new worksheet or the last worksheet that was used to load Epicor data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                activeWorksheet.UsedRange.Clear();
                WriteWorksheetHeader(activeWorksheet, "Part", "Description", "Plant", "Minimum Quantity", "Percent Difference", "Monthly Usage", "Months to Stock", "New Minimum", "Cost", "Process Update");
                int lineCursor = 2;

                activeWorksheet.Cells[1, 11].Value2 = "TRUE";
                activeWorksheet.Cells[2, 11].Value2 = "FALSE";
                activeWorksheet.get_Range("K1").EntireColumn.Hidden = true;
                foreach (part1 p in parts.Values)
                {
                    activeWorksheet.get_Range("A" + lineCursor.ToString()).NumberFormat = "@";
                    WriteWorksheetLine(activeWorksheet, lineCursor, p.partnum, p.desc, p.plant, p.minimumqty.ToString(), p.percentdiff.ToString(), p.monthlyusage.ToString(), "1", "=F" + lineCursor.ToString() + "*G" + lineCursor.ToString(), "=H" + lineCursor.ToString() + "*" + p.avgcost.ToString());
                    Range c = activeWorksheet.get_Range("H" + lineCursor.ToString());
                    c.Interior.Color = System.Drawing.Color.Yellow;

                    Range r = activeWorksheet.get_Range("J" + lineCursor.ToString());
                    r.Validation.Add(XlDVType.xlValidateList, Missing.Value, Missing.Value, "=K1:K2", Missing.Value);
                    r.Validation.InCellDropdown = true;
                    r.Validation.IgnoreBlank = false;
                    r.Value2 = "FALSE";
                    activeWorksheet.get_Range("G" + lineCursor.ToString(), "H" + lineCursor.ToString()).Locked = false;
                    activeWorksheet.get_Range("J" + lineCursor.ToString()).Locked = false;
                    lineCursor++;
                }
                activeWorksheet.get_Range("A1", "J1").EntireColumn.AutoFit();

                activeWorksheet.get_Range("A1").EntireColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                activeWorksheet.get_Range("B2").Select();
                Globals.ThisAddIn.Application.ActiveWindow.FreezePanes = true;

                SetProtection(activeWorksheet, Operation.Part);
            }
        }

        private Dictionary<string, part1> PrepareParts(Worksheet activeWorksheet)
        {
            Dictionary<string, part1> result = new Dictionary<string, part1>();

            bool reading = true;
            int lineCursor = 2;
            string errors = "";
            while (reading)
            {
                if (!String.IsNullOrEmpty(activeWorksheet.Cells[lineCursor, 1].Text))
                {
                    //                    Microsoft.Office.Tools.Excel.Worksheet worksheet = Globals.Factory.GetVstoObject(activeWorksheet);
                    //                    Microsoft.Office.Tools.Excel.Controls.CheckBox process = worksheet.Controls["cb_" + lineCursor.ToString()] as Microsoft.Office.Tools.Excel.Controls.CheckBox;

                    if (/*process.Checked*/activeWorksheet.Cells[lineCursor, 10].Text == "TRUE")
                    {
                        #region Data Validation and Population

                        part1 newPart = new part1();
                        newPart.partnum = activeWorksheet.Cells[lineCursor, 1].Value.ToString();
                        newPart.desc = activeWorksheet.Cells[lineCursor, 2].Value.ToString();
                        newPart.plant = activeWorksheet.Cells[lineCursor, 3].Value.ToString();
                        newPart.percentdiff = System.Convert.ToDecimal(activeWorksheet.Cells[lineCursor, 5].Value.ToString());
                        try
                        {
                            decimal monthlyusage = System.Convert.ToDecimal(activeWorksheet.Cells[lineCursor, 8].Value.ToString());
                            if (monthlyusage < 0)
                                errors += "Invalid Monthly Usage for Part " + newPart.partnum + ". Value must be positive." + Environment.NewLine;
                            // the updated part is going to use the value from the monthly usage field as the new minimum qty
                            newPart.minimumqty = monthlyusage;
                            newPart.monthlyusage = monthlyusage;
                        }
                        catch (Exception)
                        {
                            errors += "Invalid Monthly Usage for Part " + newPart.partnum + ". Value must be a decimal." + Environment.NewLine;
                        }

                        result.Add(newPart.partnum, newPart);

                        #endregion
                    }
                    lineCursor++;
                }
                else
                    reading = false;
            }

            if (!String.IsNullOrEmpty(errors))
            {
                MessageBox.Show("The following data validation errors were detected.  Please fix before saving" + Environment.NewLine + Environment.NewLine + errors, "Validation failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            else
                return result;
        }

        private void SaveParts(string username, string password, Dictionary<string, part1> newParts)
        {
            Dictionary<string, part1> verifyParts = parts;

            if (PopulateParts())
            {
                bool valid = true;
                foreach (string part in parts.Keys)
                {
                    if (verifyParts.Keys.Contains(part))
                    {
                        if (verifyParts[part].minimumqty != parts[part].minimumqty || verifyParts[part].monthlyusage != parts[part].monthlyusage || verifyParts[part].percentdiff != parts[part].percentdiff)
                            valid = false;
                    }
                    else
                        valid = false;
                }
                if (valid)
                {
                    foreach (string part in verifyParts.Keys)
                        if (!parts.Keys.Contains(part))
                            valid = false;
                }

                if (!valid)
                {
                    MessageBox.Show("The Part data has changed since it was loaded.  Please reload and perform edits again", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    partsminqtyupdaterequest request = new partsminqtyupdaterequest();
                    request.username = username;
                    request.password = password;
                    request.epicor = newParts.Values.ToArray();

                    PartServiceClient client = new PartServiceClient("windows");
                    SetCertificatePolicy();

                    string exception = client.updatepartsminqty(request).exception;
                    if (!String.IsNullOrEmpty(exception))
                        ProcessExceptions(exception);
                }
            }
        }

        #endregion

        #region ABC Code Methods

        private bool PopulateABCCodes(string username, string password)
        {
            ABCCodeServiceClient client = new ABCCodeServiceClient("windows");
            SetCertificatePolicy();

            abccodegetresult codeInterface = client.getallabccodes();
            string exception = codeInterface.exception;

            codes = new Dictionary<string, abccode>();

            foreach (abccode code in codeInterface.epicor)
                codes.Add(code.code, code);

            if (!String.IsNullOrEmpty(exception))
            {
                ProcessExceptions(exception);
                return false;
            }
            else
                return true;
        }

        private void WriteABCCodes()
        {
            Worksheet activeWorksheet = GetOrCreateActiveWorksheet();
            ClearProtection(activeWorksheet);
            if (activeWorksheet.ProtectContents)
                MessageBox.Show("The current worksheet appears to be protected.  This may be due to protection from a previous load of ABC Code data.  Please switch to a new worksheet or the last worksheet that was used to load ABC Code data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                activeWorksheet.UsedRange.Clear();
                WriteWorksheetHeader(activeWorksheet, "Code", "Minimum Volume", "Minimum Unit Cost", "Count Frequency");
                int lineCursor = 2;
                foreach (abccode code in codes.Values)
                {
                    WriteWorksheetLine(activeWorksheet, lineCursor, code.code, code.minvol.ToString(), code.mincost.ToString(), code.freq.ToString());
                    activeWorksheet.get_Range("B" + lineCursor.ToString(), "D" + lineCursor.ToString()).Locked = false;
                    lineCursor++;
                }
                activeWorksheet.get_Range("A1", "D1").EntireColumn.AutoFit();
                SetProtection(activeWorksheet, Operation.ABCCode);
            }

        }

        private Dictionary<string, abccode> PrepareABCCodes()
        {
            Worksheet activeWorksheet = ((Worksheet)Globals.ThisAddIn.Application.ActiveSheet);

            Dictionary<string, abccode> newCodes = new Dictionary<string, abccode>();

            bool reading = true;
            int lineCursor = 2;
            string errors = "";
            while (reading)
            {
                if (!String.IsNullOrEmpty(activeWorksheet.Cells[lineCursor, 1].Value))
                {
                    #region Data Validation and Population

                    abccode newCode = new abccode();
                    newCode.code = activeWorksheet.Cells[lineCursor, 1].Value.ToString();
                    try
                    {
                        decimal minvol = System.Convert.ToDecimal(activeWorksheet.Cells[lineCursor, 2].Value.ToString());
                        if (minvol < 0)
                            errors += "Invalid Minimum Volume for ABC Code " + newCode.code + ". Value must be positive." + Environment.NewLine;
                        newCode.minvol = minvol;
                    }
                    catch (Exception)
                    {
                        errors += "Invalid Minimum Volume for ABC Code " + newCode.code + ". Value must be a decimal." + Environment.NewLine;
                    }
                    try
                    {
                        decimal mincost = System.Convert.ToDecimal(activeWorksheet.Cells[lineCursor, 3].Value.ToString());
                        if (mincost < 0)
                            errors += "Invalid Minimum Cost for ABC Code " + newCode.code + ". Value must be positive." + Environment.NewLine;
                        newCode.mincost = mincost;
                    }
                    catch (Exception)
                    {
                        errors += "Invalid Minimum Unit Cost for ABC Code " + newCode.code + ". Value must be a decimal." + Environment.NewLine;
                    }
                    try
                    {
                        int freq = System.Convert.ToInt32(activeWorksheet.Cells[lineCursor, 4].Value.ToString());
                        if (freq < 0)
                            errors += "Invalid Count Frequency for ABC Code " + newCode.code + ". Value must be positive." + Environment.NewLine;

                        newCode.freq = freq;
                    }
                    catch (Exception)
                    {
                        errors += "Invalid Count Frequency for ABC Code " + newCode.code + ". Value must be an whole number." + Environment.NewLine;
                    }

                    newCodes.Add(newCode.code, newCode);

                    #endregion

                    lineCursor++;
                }
                else
                    reading = false;
            }

            if (!String.IsNullOrEmpty(errors))
            {
                MessageBox.Show("The following data validation errors were detected.  Please fix before saving" + Environment.NewLine + Environment.NewLine + errors, "Validation failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            else
                return newCodes;
        }

        private void SaveABCCodes(string username, string password, Dictionary<string, abccode> newCodes)
        {
            Dictionary<string, abccode> verifyCodes = codes;

            if (PopulateABCCodes(username, password))
            {
                bool valid = true;

                foreach (string code in codes.Keys)
                {
                    if (verifyCodes.Keys.Contains(code))
                    {
                        if (verifyCodes[code].mincost != codes[code].mincost || verifyCodes[code].minvol != codes[code].minvol || verifyCodes[code].freq != codes[code].freq)
                            valid = false;
                    }
                    else
                        valid = false;
                }

                if (valid)
                {
                    foreach (string code in verifyCodes.Keys)
                        if (!codes.Keys.Contains(code))
                            valid = false;
                }

                if (!valid)
                {
                    MessageBox.Show("The ABC Code data has changed since it was loaded.  Please reload and perform edits again", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    abccodeupdaterequest request = new abccodeupdaterequest();
                    request.username = username;
                    request.password = password;
                    request.epicor = newCodes.Values.ToArray();

                    ABCCodeServiceClient client = new ABCCodeServiceClient("windows");
                    SetCertificatePolicy();

                    string exception = client.updateabccodes(request).exception;
                    if (!String.IsNullOrEmpty(exception))
                        ProcessExceptions(exception);
                }
            }
        }

        #endregion

        #endregion

        #region Public Methods

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("TCGEpicorForExcel2007.Epicor.xml");
        }

        #endregion

        #region Ribbon Callbacks

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        #region Job Methods

        public void but_LoadJobs_Click(IRibbonControl control)
        {
            try
            {
                Globals.ThisAddIn.Application.StatusBar = "Loading Job Data...";
                Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlWait;
                if (PopulateJobs())
                    WriteJobs();
            }
            catch (Exception ex)
            {
                ProcessExceptions(ex.Message);
            }
            finally
            {
                Globals.ThisAddIn.Application.StatusBar = false;
                Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlDefault;
            }
        }

        public void but_SaveJobs_Click(IRibbonControl control)
        {
            try
            {
                Worksheet activeWorksheet = GetValidActiveWorksheetForSaveOperation(Operation.Job);
                if (activeWorksheet != null)
                {
                    Globals.ThisAddIn.Application.StatusBar = "Saving Job Data...";
                    Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlWait;
                    Dictionary<string, Job> newJobs = PrepareJobs(activeWorksheet);

                    if (newJobs != null)
                    {
                        if (newJobs.Count > 0)
                        {
                            PopulateConnectionInfoFromIsolatedStorage();
                            Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlDefault;
                            if (info.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlWait;
                                SaveConnectionInfoToIsolatedStorage();
                                SaveJobs(info.Username, info.Password, newJobs);
                            }
                        }
                        else
                            MessageBox.Show("No jobs selected to update", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessExceptions(ex.Message);
            }
            finally
            {
                Globals.ThisAddIn.Application.StatusBar = false;
                Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlDefault;
            }
        }

        #endregion

        #region Power Analyzer Methods

        public void but_LoadPowerAnalyzer_Click(IRibbonControl control)
        {
            /*            try
                        {
                            Globals.ThisAddIn.Application.StatusBar = "Loading Part Data...";
                            Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlWait;
                            if (PopulatePowerAnalyzer())
                                WritePowerAnalyzer();
                        }
                        catch (Exception ex)
                        {
                            ProcessExceptions(ex.Message);
                        }
                        finally
                        {
                            Globals.ThisAddIn.Application.StatusBar = false;
                            Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlDefault;
                        }
            */
        }

        public void but_SavePowerAnalyzer_Click(IRibbonControl control)
        {
            try
            {
                string t = "";
                if (MessageBox.Show("Caution: Saving this data to the database will make it the active revision.  Do you want to proceed?", "Make active revision?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                {
                    Worksheet standardWorksheet = GetValidActiveWorksheetForSaveOperation(Operation.StandardPowerTable);
                    Worksheet specialWorksheet = GetValidActiveWorksheetForSaveOperation(Operation.SpecialPowerTable);

                    if (standardWorksheet != null && specialWorksheet != null)
                    {
                        Globals.ThisAddIn.Application.StatusBar = "Reading Power Analyzer Data...";
                        Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlWait;
                        List<PowerAnalyzerSetting> standardPowerTable = PrepareStandardPowerTable(standardWorksheet);
                        List<PowerAnalyzerSetting> specialPowerTable = PrepareSpecialPowerTable(specialWorksheet);

                        standardPowerTable.AddRange(specialPowerTable);

                        Globals.ThisAddIn.Application.StatusBar = "Saving Power Analyzer Data...";
                        PowerAnalyzerServiceClient client = new PowerAnalyzerServiceClient("basic");
                        SetCertificatePolicy(); 
                        powertableupdateresult result = client.UpdatePowerTable(standardPowerTable.ToArray());

                        if (!String.IsNullOrEmpty(result.exception))
                            ProcessExceptions(result.exception);
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessExceptions(ex.Message);
            }
            finally
            {
                Globals.ThisAddIn.Application.StatusBar = false;
                Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlDefault;
            }

        }

        #endregion

        #region Part Methods

        public void but_LoadPart_Click(IRibbonControl control)
        {
            try
            {
                Globals.ThisAddIn.Application.StatusBar = "Loading Part Data...";
                Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlWait;
                if (FilterParts())
                    if (PopulateParts())
                        WriteParts();
            }
            catch (Exception ex)
            {
                ProcessExceptions(ex.Message);
            }
            finally
            {
                Globals.ThisAddIn.Application.StatusBar = false;
                Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlDefault;
            }
        }

        public void but_SavePart_Click(IRibbonControl control)
        {
            try
            {
                Worksheet activeWorksheet = GetValidActiveWorksheetForSaveOperation(Operation.Part);
                if (activeWorksheet != null)
                {
                    Globals.ThisAddIn.Application.StatusBar = "Saving Part Data...";
                    Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlWait;
                    Dictionary<string, part1> newParts = PrepareParts(activeWorksheet);

                    if (newParts != null)
                    {
                        if (newParts.Count > 0)
                        {
                            PopulateConnectionInfoFromIsolatedStorage();
                            Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlDefault;
                            if (info.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlWait;
                                SaveConnectionInfoToIsolatedStorage();
                                SaveParts(info.Username, info.Password, newParts);
                            }
                        }
                        else
                            MessageBox.Show("No parts selected to update", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessExceptions(ex.Message);
            }
            finally
            {
                Globals.ThisAddIn.Application.StatusBar = false;
                Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlDefault;
            }
        }

        #endregion

        #region ABC Code Methods

        public void but_LoadABCCode_Click(IRibbonControl control)
        {
            try
            {
                Globals.ThisAddIn.Application.StatusBar = "Loading ABC Code Data...";
                Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlWait;
                if (PopulateABCCodes(info.Username, info.Password))
                    WriteABCCodes();
            }
            catch (Exception ex)
            {
                ProcessExceptions(ex.Message);
            }
            finally
            {
                Globals.ThisAddIn.Application.StatusBar = false;
                Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlDefault;
            }
        }

        public void but_SaveABCCode_Click(IRibbonControl control)
        {
            try
            {
                Worksheet activeWorksheet = GetValidActiveWorksheetForSaveOperation(Operation.ABCCode);
                if (activeWorksheet != null)
                {
                    Globals.ThisAddIn.Application.StatusBar = "Saving ABC Code Data...";
                    Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlWait;
                    Dictionary<string, abccode> newCodes = PrepareABCCodes();
                    if (newCodes != null)
                    {
                        PopulateConnectionInfoFromIsolatedStorage();
                        Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlDefault;
                        if (info.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlWait;
                            SaveConnectionInfoToIsolatedStorage();
                            SaveABCCodes(info.Username, info.Password, newCodes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessExceptions(ex.Message);
            }
            finally
            {
                Globals.ThisAddIn.Application.StatusBar = false;
                Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlDefault;
            }
        }

        #endregion

        #region Visibility Methods

        public bool grp_PowerAnalyzer_IsVisible(Office.IRibbonControl control)
        {
            return IsInRole(@"IT\Vantage Service Power Analyzer");
        }

        public bool grp_Parts_IsVisible(Office.IRibbonControl control)
        {
            return IsInRole(@"IT\Vantage Service Part Min Qty");
        }

        public bool grp_ABCCode_IsVisible(Office.IRibbonControl control)
        {
            return IsInRole(@"IT\Vantage Service ABC Codes");
        }

        public bool grp_Jobs_IsVisible(Office.IRibbonControl control)
        {
            return IsInRole(@"IT\Vantage Service Job Dates");
        }

        #endregion

        #endregion

        #endregion
    }
}

// TODO:4 Abstract equality logic
