namespace Styleline.WinAnalyzer.DAL.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance = ((Settings) SettingsBase.Synchronized(new Settings()));

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode, SpecialSetting(SpecialSetting.ConnectionString), DefaultSettingValue("Data Source=.;Initial Catalog=PowerAnalyzer;Integrated Security=True")]
        public string PowerAnalyzerConnectionString
        {
            get
            {
                return (string) this["PowerAnalyzerConnectionString"];
            }
        }

        [DefaultSettingValue("Data Source=poweranalyzer.db.it.tcg;Initial Catalog=PowerAnalyzer;Persist Security Info=True;User ID=poweranalyzer;Password=p@nalyzer"), DebuggerNonUserCode, ApplicationScopedSetting, SpecialSetting(SpecialSetting.ConnectionString)]
        public string PowerAnalyzerConnectionString1
        {
            get
            {
                return (string) this["PowerAnalyzerConnectionString1"];
            }
        }
    }
}

