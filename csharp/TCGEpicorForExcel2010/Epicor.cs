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
using TCGEpicorForExcel2010.ABCCodeServiceReference;
using Office = Microsoft.Office.Core;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using TCGEpicorForExcel2010.PartServiceReference;

#endregion

namespace TCGEpicorForExcel2010
{
    [ComVisible(true)]
    public class Epicor : Office.IRibbonExtensibility
    {
        #region Values

        private enum Operation { ABCCode, Part }
        private Office.IRibbonUI ribbon;
        private ServiceConnection info;
        private string storedUsername = "";
        private Dictionary<Operation, Guid> operationKeys;
        private Dictionary<string, abccode> codes;
        private Dictionary<string, part1> parts;

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
            Worksheet activeWorksheet = ((Worksheet)Globals.ThisAddIn.Application.ActiveSheet);
            string opMessage = "";
            if (op == Operation.ABCCode)
                opMessage = "ABC Code";
            else if (op == Operation.Part)
                opMessage = "Part";
            if (activeWorksheet == null)
                MessageBox.Show("Must have an open worksheet with " + opMessage + " data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
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

        #region Part Methods

        private bool PopulateParts()
        {
            PartServiceClient client = new PartServiceClient("windows");
            SetCertificatePolicy();

            partgetneedingupdateresult partInterface = client.getpartsneedingupdate();
            string exception = partInterface.exception;

            parts = new Dictionary<string, part1>();

            foreach (part1 p in partInterface.epicor)
                parts.Add(p.partnum, p);

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
                WriteWorksheetHeader(activeWorksheet, "Part", "Description", "Plant", "Minimum Quantity", "Percent Difference", "Monthly Usage", "Process Update");
                int lineCursor = 2;
                foreach (part1 p in parts.Values)
                {
                    WriteWorksheetLine(activeWorksheet, lineCursor, p.partnum, p.desc, p.plant, p.minimumqty.ToString(), p.percentdiff.ToString(), p.monthlyusage.ToString());
                    Range c = activeWorksheet.get_Range("F" + lineCursor.ToString());
                    c.Interior.Color = System.Drawing.Color.Yellow;
                    Microsoft.Office.Tools.Excel.Worksheet worksheetHost = Globals.Factory.GetVstoObject(activeWorksheet);
                    if (worksheetHost.Controls.Contains("cb_" + lineCursor.ToString()))
                        ((Microsoft.Office.Tools.Excel.Controls.CheckBox)worksheetHost.Controls["cb_" + lineCursor.ToString()]).Checked = true;
                    else
                    {
                        Microsoft.Office.Tools.Excel.Controls.CheckBox cb = new Microsoft.Office.Tools.Excel.Controls.CheckBox();
                        cb.Checked = true;
                        worksheetHost.Controls.AddControl(cb, activeWorksheet.get_Range("G" + lineCursor.ToString()), "cb_" + lineCursor.ToString());
                    }
                    activeWorksheet.get_Range("F" + lineCursor.ToString(), "G" + lineCursor.ToString()).Locked = false;
                    lineCursor++;
                }
                activeWorksheet.get_Range("A1", "G1").EntireColumn.AutoFit();
                activeWorksheet.get_Range("A1").EntireColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
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
                    Microsoft.Office.Tools.Excel.Worksheet worksheet = Globals.Factory.GetVstoObject(activeWorksheet);
                    Microsoft.Office.Tools.Excel.Controls.CheckBox process = worksheet.Controls["cb_" + lineCursor.ToString()] as Microsoft.Office.Tools.Excel.Controls.CheckBox;

                    if (process.Checked)
                    {
                        #region Data Validation and Population

                        part1 newPart = new part1();
                        newPart.partnum = activeWorksheet.Cells[lineCursor, 1].Value.ToString();
                        newPart.desc = activeWorksheet.Cells[lineCursor, 2].Value.ToString();
                        newPart.plant = activeWorksheet.Cells[lineCursor, 3].Value.ToString();
                        newPart.percentdiff = System.Convert.ToDecimal(activeWorksheet.Cells[lineCursor, 5].Value.ToString());
                        try
                        {
                            decimal monthlyusage = System.Convert.ToDecimal(activeWorksheet.Cells[lineCursor, 6].Value.ToString());
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
            return GetResourceText("TCGEpicorForExcel2010.Epicor.xml");
        }

        #endregion

        #region Ribbon Callbacks

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        #region Part Methods

        public void but_LoadPart_Click(IRibbonControl control)
        {
            try
            {
                Globals.ThisAddIn.Application.StatusBar = "Loading Part Data...";
                Globals.ThisAddIn.Application.Cursor = XlMousePointer.xlWait;
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

        #endregion

        #endregion
    }
}

// TODO:4 Abstract equality logic
// TODO:5 Test business logic errors