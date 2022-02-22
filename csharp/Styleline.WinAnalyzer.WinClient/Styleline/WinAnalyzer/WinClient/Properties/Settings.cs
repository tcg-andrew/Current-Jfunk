namespace Styleline.WinAnalyzer.WinClient.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0"), CompilerGenerated]
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance = ((Settings) SettingsBase.Synchronized(new Settings()));

        [DebuggerNonUserCode, DefaultSettingValue(""), ApplicationScopedSetting]
        public string ComPort
        {
            get
            {
                return (string) this["ComPort"];
            }
        }

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [ApplicationScopedSetting, DefaultSettingValue("False"), DebuggerNonUserCode]
        public bool EnableDoorlineRecord
        {
            get
            {
                return (bool) this["EnableDoorlineRecord"];
            }
        }

        [DebuggerNonUserCode, ApplicationScopedSetting, DefaultSettingValue("False")]
        public bool EnableFramelineRecord
        {
            get
            {
                return (bool) this["EnableFramelineRecord"];
            }
        }
    }
}

