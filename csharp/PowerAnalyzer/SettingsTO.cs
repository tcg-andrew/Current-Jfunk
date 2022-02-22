namespace PowerAnalyzer
{
    using Microsoft.Win32;
    using System;
    using System.Runtime.CompilerServices;

    public class SettingsTO
    {
        private RegistryKey analyzer = Registry.CurrentUser.CreateSubKey(@"Software\WindowsAnalyzer");
        private static SettingsTO instance = new SettingsTO();

        private SettingsTO()
        {
            this.LoadSettings();
        }

        public void LoadSettings()
        {
            this.ComPort = this.analyzer.GetValue("ComPort", "COM1").ToString();
            this.EnableFramelineRecord = bool.Parse(this.analyzer.GetValue("EnableFramelineRecord", false).ToString());
            this.EnableDoorlineRecord = bool.Parse(this.analyzer.GetValue("EnableDoorlineRecord", false).ToString());
            this.EnablePrintULLabel = bool.Parse(this.analyzer.GetValue("EnablePrintULLabel", false).ToString());
            this.PrinterName = this.analyzer.GetValue("UlPrinterName", "").ToString();
            this.Plant = this.analyzer.GetValue("Plant", "FL").ToString();
        }

        public void SaveSettings()
        {
            this.analyzer.SetValue("ComPort", this.ComPort);
            this.analyzer.SetValue("EnableFramelineRecord", this.EnableFramelineRecord);
            this.analyzer.SetValue("EnableDoorlineRecord", this.EnableDoorlineRecord);
            this.analyzer.SetValue("EnablePrintULLabel", this.EnablePrintULLabel);
            this.analyzer.SetValue("UlPrinterName", this.PrinterName);
            this.analyzer.SetValue("Plant", this.Plant);
            this.LoadSettings();
        }

        public string ComPort { get; set; }

        public static SettingsTO Default
        {
            get
            {
                return instance;
            }
        }

        public bool EnableDoorlineRecord { get; set; }

        public bool EnableFramelineRecord { get; set; }

        public bool EnablePrintULLabel { get; set; }

        public string PrinterName { get; set; }

        public string Plant { get; set; }
    }
}

