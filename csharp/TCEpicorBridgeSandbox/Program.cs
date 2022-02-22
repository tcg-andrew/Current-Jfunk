using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ObjectLibrary.TCBridgeInterface;

namespace TCEpicorBridgeSandbox
{
    class Program
    {
        static string username = "CRDService";
        static string password = "gfd723trajsdc97";
        static void Main(string[] args)
        {
            string folder = @"\\STLV-APPTCT02\tc12data\crd_utilities\crd_pas_vas_wu_report\report\";
            EngWorkbenchInterface workBench = new EngWorkbenchInterface();
            PartInterface pi = new PartInterface();
            foreach (string file in Directory.GetFiles(folder))
            {
                string[] lines = System.IO.File.ReadAllLines(file);

                // Display the file contents by using a foreach loop.
                System.Console.WriteLine("Contents of WriteLines2.txt = ");
                foreach (string line in lines)
                {
                    if (!line.StartsWith("Child Part Number"))
                    {
                        // Use a tab to indent each line of the file.
                        Console.WriteLine("\t" + line);
                        string[] split = line.Split('|');
                        string child = split[0].Split('/')[0];
                        bool pull = split[1].ToLower() == "1";
                        bool view = split[2].ToLower() == "1";
                        string parent = split[3].Split('/')[0];
                        string rev = split[3].Split('/')[1];


                        bool approved = pi.GetRevStatus(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, parent, rev);


                        workBench.ReviseMaterial(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, parent, rev, child, pull, view);

                        if (!approved)
                            pi.UnapproveRevision(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, parent, rev);
                    }
                }
            }

            /*            List<PublishEvent> error = new List<PublishEvent>();
                        List<PublishEvent> complete = new List<PublishEvent>();

                        string errors;

                        try
                        {
                            TCBridgeInterface bridge = new TCBridgeInterface();
                            PartInterface partInterface = new PartInterface();
                            EngWorkbenchInterface workBench = new EngWorkbenchInterface();
                            UOMInterface uomInterface = new UOMInterface();
                            UOMClassInterface uomClassInterface = new UOMClassInterface();

                            List<PublishEvent> events = bridge.GetPendingEvents(ConfigurationManager.AppSettings["TCStageServer"].ToString(), ConfigurationManager.AppSettings["TCStageDatabase"].ToString(), ConfigurationManager.AppSettings["TCStageUsername"].ToString(), ConfigurationManager.AppSettings["TCStagePassword"].ToString(), out errors);

                            List<PublishEvent> incomplete = new List<PublishEvent>();


                            foreach (PublishEvent ev in events)
                            {
                                try
                                {
                                    string str_error = "";
                                    ev.SetPart(bridge.GetPartInfo(ConfigurationManager.AppSettings["TCStageServer"].ToString(), ConfigurationManager.AppSettings["TCStageDatabase"].ToString(), ConfigurationManager.AppSettings["TCStageUsername"].ToString(), ConfigurationManager.AppSettings["TCStagePassword"].ToString(), ev.PublishID, out str_error));
                                    if (str_error.Length > 0)
                                        throw new Exception(str_error);

                                    uomInterface.CheckCreate(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ev.Part.InvUOM);
                                    uomClassInterface.Create(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ev.Part.InvUOM);

                                    if (partInterface.PartExists(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ev.Part.PartNum))
                                    {
                                        partInterface.UpdateTCPart(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ev.Part);
                                    }
                                    else
                                    {
                                        partInterface.CreateTCPart(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ev.Part);
                                    }

                                    if (String.IsNullOrEmpty(ev.Part.MakeFrom))
                                        bridge.GetBOMInfo(ConfigurationManager.AppSettings["TCStageServer"].ToString(), ConfigurationManager.AppSettings["TCStageDatabase"].ToString(), ConfigurationManager.AppSettings["TCStageUsername"].ToString(), ConfigurationManager.AppSettings["TCStagePassword"].ToString(), ev.Part);

                                    if (ev.Part.TypeCode == "M" && ev.Part.BOM.Count == 0 && String.IsNullOrEmpty(ev.Part.MakeFrom))
                                        throw new Exception("Manufactured part must have BOMINFO records or a MakeFrom");

                                    if (ev.Part.BOM.Count > 0 || !String.IsNullOrEmpty(ev.Part.MakeFrom))
                                    {
                                        workBench.CreateTCRev(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ev.Part);
                                    }
                                    if (ev.Part.ReleaseStatus == "PRODUCTION")
                                        partInterface.UnapproveOtherRevision(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ev.Part);

                                    try
                                    {
                                        if (ev.Part.TypeCode == "M")
                                        {
                                            partInterface.ActivatePart(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ev.Part.PartNum);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("Error activating manufactured part: " + ex.Message);
                                    }
                                    ev.Complete();
                                    complete.Add(ev);
                                }
                                catch (MissingPartException ex)
                                {
                                    ev.SetError(ex.Message);
                                    incomplete.Add(ev);
                                }
                                catch (Exception ex)
                                {
                                    ev.SetError(ex.Message);
                                    error.Add(ev);
                                }
                            }

                            List<PublishEvent> reprocess = new List<PublishEvent>();
                            do
                            {
                                reprocess.Clear();
                                foreach (PublishEvent ev in incomplete)
                                    reprocess.Add(ev);
                                incomplete.Clear();
                                foreach (PublishEvent ev in reprocess)
                                {
                                    try
                                    {
                                        workBench.CreateTCRev(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ev.Part);
                                        ev.Complete();
                                        complete.Add(ev);
                                    }
                                    catch (MissingPartException ex)
                                    {
                                        ev.SetError(ex.Message);
                                        incomplete.Add(ev);
                                    }
                                    catch (Exception ex)
                                    {
                                        ev.SetError(ex.Message);
                                        error.Add(ev);
                                    }
                                }
                            } while (incomplete.Count < reprocess.Count);

                            foreach (PublishEvent ev in incomplete)
                            {
                                error.Add(ev);
                            }
                        }
                        catch (Exception ex)
                        {
                            //// TODO: Add failure email;
                        }

                        List<string> complete_errors = new List<string>();
                        foreach (PublishEvent ev in complete)
                        {
                            try
                            {
                                ev.WriteComplete(ConfigurationManager.AppSettings["TCStageServer"].ToString(), ConfigurationManager.AppSettings["TCStageDatabase"].ToString(), ConfigurationManager.AppSettings["TCStageUsername"].ToString(), ConfigurationManager.AppSettings["TCStagePassword"].ToString());
                            }
                            catch (Exception ex)
                            {
                                complete_errors.Add("Could not update complete status for PUBLISHEVENT with EVENTID " + ev.EventID.ToString() + ".  " + ex.Message);
                            }
                        }

                        List<string> error_errors = new List<string>();
                        foreach (PublishEvent ev in error)
                        {
                            try
                            {
                                ev.WriteError(ConfigurationManager.AppSettings["TCStageServer"].ToString(), ConfigurationManager.AppSettings["TCStageDatabase"].ToString(), ConfigurationManager.AppSettings["TCStageUsername"].ToString(), ConfigurationManager.AppSettings["TCStagePassword"].ToString());
                            }
                            catch (Exception ex)
                            {
                                error_errors.Add("Could not update error status for PUBLISHEVENT with EVENTID " + ev.EventID.ToString() + ". " + ex.Message);
                            }
                        }

                        //email/log errors
                        //email/log complete_errors
                        //email/log error_errors
                    }*/
        }
    }
}
