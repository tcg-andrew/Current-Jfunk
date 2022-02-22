/// TODO: Figure out fix or exclude
/*
#region Usings

using Epicor.Mfg.BO;
using Epicor.Mfg.Core;
using System.Collections.Generic;
using System;
using System.IO;

#endregion

namespace ObjectLibrary
{
    public class ConfigurationInterface : EpicorExtension<Configuration>
    {
        #region Public Methods

        #region Retrieve Methods

        public Dictionary<string, string> GetConfigValues(string server, string port, string username, string password, int quotenum, int linenum, string partnum, string revision)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);

                GeneratedConfigurationDataSet configDS = BusinessObject.GetGeneratedConfiguration(partnum, revision, "QUOTE");
                ConfigurationValueDataSet configValueDS = new ConfigurationValueDataSet();
                BusinessObject.GetQuoteValues(quotenum, linenum, 0, partnum, revision, configValueDS, 0, configDS);

                foreach (ConfigurationValueDataSet.ConfigurationValueRow row in configValueDS.ConfigurationValue.Rows)
                    result.Add(row.InputName, row.InputValue);
            }
            catch (Exception ex)
            {
                if (ex.Message != "Configuration does not exist")
                    throw new Exception("GetConfigValues - " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
            return result;
        }

        #endregion

        #region Update Methods

        public void UpdateConfiguration(string server, string port, string username, string password, int quotenum, int linenum, string partnum, string revision, Dictionary<string, string> configvalues)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);

                bool outbool;

                ConfigurationSequenceDataSet configSeqDS = BusinessObject.GetConfigurationSequence(partnum, revision);
                GeneratedConfigurationDataSet configDS = BusinessObject.GetGeneratedConfiguration(partnum, revision, "QUOTE");
                ConfigurationValueDataSet configValueDS = new ConfigurationValueDataSet();
                BusinessObject.GetQuoteValues(quotenum, linenum, 0, partnum, revision, configValueDS, 0, configDS);

                Dictionary<string, ConfigurationValueDataSet.ConfigurationValueRow> pageInputs = new Dictionary<string, ConfigurationValueDataSet.ConfigurationValueRow>();

                // Set initial value and cache config row
                foreach (string key in configvalues.Keys)
                {
                    foreach (ConfigurationValueDataSet.ConfigurationValueRow configrow in configValueDS.ConfigurationValue.Rows)
                    {
                        if (configrow.InputName == key)
                        {
                            //                        configrow.InputValue = configvalues[key];
                            pageInputs.Add(key, configrow);
                        }
                    }
                }

                List<string> keysToWrite = new List<string>();
                // Apply leaves and reset values each pass
                foreach (string key in pageInputs.Keys)
                {
                    keysToWrite.Add(key);
                    if (pageInputs[key].HasLeaveTrigger && !pageInputs[key].DisabledInput && pageInputs[key].InputValue != configvalues[key])
                    {
                        foreach (ConfigurationValueDataSet.ConfigurationValueRow configrow in configValueDS.ConfigurationValue.Rows)
                            configrow.RowMod = "U";

                        foreach (string towrite in keysToWrite)
                            if (!pageInputs[towrite].DisabledInput && pageInputs[towrite].InputValue != configvalues[towrite])
                                pageInputs[towrite].InputValue = configvalues[towrite];

                        BusinessObject.ApplyLeave(partnum, revision, Int32.Parse(key.Substring(1, 2)), key, "QUOTE", configValueDS, 0, configDS);
                    }
                }

                foreach (ConfigurationValueDataSet.ConfigurationValueRow configrow in configValueDS.ConfigurationValue.Rows)
                    configrow.RowMod = "U";
              BusinessObject.SaveQuoteConfiguration(quotenum, linenum, 0, partnum, revision, "", "", "", true, false, 0, configValueDS, out outbool, configSeqDS);
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateConfiguration - " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
        }

        #endregion

        #endregion
    }
}
*/