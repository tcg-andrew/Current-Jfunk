using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Text.RegularExpressions;
using System.Net;

namespace ClassicLineConversion
{
    static class Helper
    {
        static private string ParseRule(string fullrule, string rule)
        {
            string outrule = fullrule;
            if (outrule.IndexOf('=') >= 0)
            {
//                string[] parts = rule.TrimStart(new char[] { '(' }).TrimEnd(new char[] { ')' }).Split(new string[] { " OR " }, StringSplitOptions.RemoveEmptyEntries);
                string[] parts = rule.Split(new string[] { " OR ", " or " }, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, string> keypairs = new Dictionary<string, string>();
                if (parts.Count() > 1)
                {
                    foreach (String s in parts)
                    {
                        string key = s.Split(new string[] { " = " }, StringSplitOptions.RemoveEmptyEntries)[0].TrimStart(new char[] { ' ' }).TrimEnd(new char[] { ' ' });
                        if (!keypairs.ContainsKey(key))
                            keypairs[key] = "";
                        string[] split = s.Split(new string[] { " = " }, StringSplitOptions.RemoveEmptyEntries);
                        if (split.Count() > 1)
                        {
                            if (keypairs[key].Length > 0)
                                keypairs[key] += ", ";
                            keypairs[key] += s.Split(new string[] { " = " }, StringSplitOptions.RemoveEmptyEntries)[1];
                        }
                    }
                    string newrule = "";
                    foreach (string key in keypairs.Keys)
                    {
                        if (keypairs[key].Length > 0)
                        {
                            if (newrule.Length > 0)
                                newrule += " || ";
                            newrule += "IN(" + key + ", " + keypairs[key] + ")";
                        }
                    }
                    outrule = outrule.Replace(rule, newrule);
                }
            }
            return outrule;
        }

        static public string TranslateRule(string inrule)
        {
            string outrule = TranslateFields(inrule.Replace("\r", " ").Replace("\n", " ").TrimEnd());

            Regex quotereg = new Regex("(\".*?\")");
            List<string> quotes = new List<string>();
            foreach (Match m in quotereg.Matches(outrule))
            {
                quotes.Add(m.Value);
                outrule = outrule.Replace(m.Value, "{QUOTE_" + quotes.Count().ToString() + "}");
            }

            Regex r = new Regex(@"DECIMAL\(([^\)]*)\)");
            foreach (Match m in r.Matches(outrule))
            {
                int result;
                if (Int32.TryParse(m.Groups[1].Value, out result))
                    outrule = outrule.Replace(m.Groups[0].Value, m.Groups[1].Value + ".0");
                else
                    outrule = outrule.Replace(m.Groups[0].Value, m.Groups[1].Value);
            }

            for (int i = 0; i < outrule.Length-1; i++)
            {
                if (outrule[i] == '(')
                {
                    int ps = 1;
                    for (int j = i + 1; j < outrule.Length; j++)
                    {
                        if (outrule[j] == '(')
                        {
                            ps += 1;
                            break;
                        }
                        else if (outrule[j] == ')')
                        {
                            ps -= 1;
                            if (ps == 0)
                            {
                                string newr = TranslateRule(outrule.Substring(i + 1, j - i - 1));
                                newr = ParseRule(newr, newr);
                                //                                if (j + 5 < outrule.Length && outrule.Substring(j + 1, 4) == " OR ")
                                outrule = outrule.Replace(outrule.Substring(i + 1, j - i - 1), newr);
                                //                              else
                                //                                outrule = outrule.Replace(outrule.Substring(i, j - i + 1), newr);
                                i += newr.Length;
                                break;
                            }
                        }
                    }
                }
            }
//            outrule = ParseRule(outrule, outrule);
/*            Regex r = new Regex(@"\([^\)]*\)");
            MatchCollection ms = r.Matches((outrule.StartsWith("((") ? outrule.Substring(1, outrule.Length - 1) : outrule));
            if (ms.Count == 0)
            {
                outrule = ParseRule(outrule, outrule);
            }
            else
            {
                foreach (Match m in r.Matches((outrule.StartsWith("((") ? outrule.Substring(1, outrule.Length - 1) : outrule)))
                {
                    outrule = ParseRule(outrule, m.Value);
                }
            }*/

            r = new Regex(@"\(([^\)]*)\)");
            foreach (Match m in r.Matches(outrule))
            {
                int result;
                if (Int32.TryParse(m.Groups[1].Value, out result))
                    outrule = outrule.Replace(m.Groups[0].Value, m.Groups[1].Value);
            }

            if (outrule.ToUpper().Contains("IF") && outrule.ToUpper().Contains("THEN") && outrule.ToUpper().Contains("ELSE"))
            {
                outrule = outrule.Replace("IF ", "IIF(").Replace(" THEN ", ", ").Replace(" ELSE ", ", ").Replace("if ", "IIF(").Replace(" then ", ", ").Replace(" else ", ", ");
                outrule = outrule + ")";
            }

            for (int i = 0; i < quotes.Count(); i++)
            {
                outrule = outrule.Replace("{QUOTE_" + (i + 1).ToString() + "}", quotes[i]);
            }
            return outrule;
        }

        static public string CleanRule(string inrule)
        {
            string outrule = inrule;
            outrule = outrule.Replace(", \"\"", "");
            outrule = outrule.Replace("=", "==");
            outrule = outrule.Replace(" OR ", " || ");
            outrule = outrule.Replace(" or ", " || ");
            outrule = outrule.Replace(" AND ", " && ");
            outrule = outrule.Replace(" <> 0", " > 0");
            outrule = outrule.Replace("<> ", " != ");
            outrule = outrule.Replace(", 0)", ")");
            outrule = outrule.Replace("{field_led}[result] == 3 || {field_led}[result] == 4", "IN({field_led}[result], 3, 4)");
            outrule = outrule.Replace("{field_led}[result] == 5 || {field_led}[result] == 8", "IN({field_led}[result], 5, 8)");

            for (int i = 0; i < outrule.Length - 1; i++)
            {
                if (outrule[i] == '(')
                {
                    int ps = 1;
                    for (int j = i + 1; j < outrule.Length; j++)
                    {
                        if (outrule[j] == '(')
                        {
                            ps += 1;
                        }
                        else if (outrule[j] == ')')
                        {
                            ps -= 1;
                            if (ps == 0 && (i < 2 || (i >= 2 && outrule.Substring(i-2,2).ToLower() != "in")))
                            {
                                outrule = outrule.Replace(outrule.Substring(i, j - i + 1), outrule.Substring(i + 1, j - i - 1));
                                break;
                            }
                        }
                    }
                }
            }

            return outrule;
        }

        static private string TranslateFields(string inrule)
        {
            string outrule = inrule;
            outrule = outrule.Replace("TRUE", "1");

            outrule = outrule.Replace(">= \"02\"", "> 1");
            outrule = outrule.Replace("= \"01\"", "= 1");
            outrule = outrule.Replace("= \"02\"", "= 2");
            outrule = outrule.Replace("= \"03\"", "= 3");
            outrule = outrule.Replace("= \"04\"", "= 4");
            outrule = outrule.Replace("= \"05\"", "= 5");
            outrule = outrule.Replace("> \"01\"", "> 1");
            outrule = outrule.Replace("<> \"01\"", "!= 1");
            outrule = outrule.Replace("= \"72\"", " = 72");
            outrule = outrule.Replace("= \"80\"", " = 80");
            outrule = outrule.Replace("= \"63\"", " = 63");
            outrule = outrule.Replace("= \"66\"", " = 66");

            outrule = outrule.Replace(" AND (P01_CMB_SYSDOORCONSTRUCT = \"CLASSIC NT (2 pane unheated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"CLASSIC MT (3 pane unheated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"CLASSIC LT (3 pane heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"CLASSIC 20//20 (2 pane heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"CLASSIC PLUS (2 pane E-coat gas-filled non-heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"20//20 PLUS (2 pane E-coat gas-filled heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"CLASSIC PLUS FREEZER (3 pane E-coat heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"S//E NT (2 pane unheated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"S//E LT (3 pane heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"S//E NT (2 pane E-coat unheated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"CAL-20 (3 pane E-coat unheated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"S//E PLUS (2 pane E-coat gas-filled non-heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"HYBRIDOOR (2 pane E-coat gas-filled non-heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"HYBRIDOOR FREEZER (3 pane E-coat heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"F//X NT (2 pane unheated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"F//X LT (3 pane heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"F//X 20//20 (2 pane heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"F//X NT (2 pane E-coat unheated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"F//X PLUS (3 pane E-coat non-heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"F//X PLUS FREEZER (3 pane E-coat heated)\")", "");
            outrule = outrule.Replace(" AND (P01_CMB_SYSDOORCONSTRUCT = \"CLASSIC NT (2 pane unheated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"CLASSIC MT (3 pane unheated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"CLASSIC LT (3 pane heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"CLASSIC 20//20 (2 pane heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"CLASSIC PLUS (2 pane E-coat gas-filled non-heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"20//20 PLUS (2 pane E-coat gas-filled heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"CLASSIC PLUS FREEZER (3 pane E-coat heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"S//E NT (2 pane unheated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"S//E LT (3 pane heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"S//E NT (2 pane E-coat unheated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"CAL-20 (3 pane E-coat unheated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"S//E PLUS (2 pane E-coat gas-filled non-heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"HYBRIDOOR (2 pane E-coat gas-filled non-heated)\" OR P01_CMB_SYSDOORCONSTRUCT = \"HYBRIDOOR FREEZER (3 pane E-coat heated)\")", "");
            outrule = outrule.Replace("(DECIMAL(SUBSTRING(P02_TXT_LOCKS,1,1)) + DECIMAL(SUBSTRING(P02_TXT_LOCKS,2,1)) + DECIMAL(SUBSTRING(P02_TXT_LOCKS,3,1)) + DECIMAL(SUBSTRING(P02_TXT_LOCKS,4,1)) + DECIMAL(SUBSTRING(P02_TXT_LOCKS,5,1)) + DECIMAL(SUBSTRING(P02_TXT_LOCKS,6,1)) + DECIMAL(SUBSTRING(P02_TXT_LOCKS,7,1)) + DECIMAL(SUBSTRING(P02_TXT_LOCKS,8,1)) + DECIMAL(SUBSTRING(P02_TXT_LOCKS,9,1)) + DECIMAL(SUBSTRING(P02_TXT_LOCKS,10,1)) + DECIMAL(SUBSTRING(P02_TXT_LOCKS,11,1)) + DECIMAL(SUBSTRING(P02_TXT_LOCKS,12,1)) + DECIMAL(SUBSTRING(P02_TXT_LOCKS,13,1)) + DECIMAL(SUBSTRING(P02_TXT_LOCKS,14,1)) + DECIMAL(SUBSTRING(P02_TXT_LOCKS,15,1)) + DECIMAL(SUBSTRING(P02_TXT_LOCKS,16,1)) + DECIMAL(SUBSTRING(P02_TXT_LOCKS,17,1)) + DECIMAL(SUBSTRING(P02_TXT_LOCKS,18,1)) + DECIMAL(SUBSTRING(P02_TXT_LOCKS,19,1)) + DECIMAL(SUBSTRING(P02_TXT_LOCKS,20,1)))", "{field_numlocks}[result]");

            outrule = outrule.Replace("P01_CHK_MOTIONSENSOR = 1 AND STRING(P01_TXT_CUSTID) = \"ELECKRA\"", "{field_led}[result] = 4");
            outrule = outrule.Replace("P01_CHK_MOTIONSENSOR = 1 AND STRING(P01_TXT_CUSTID) = \"SLOAN\"", "{field_led}[result] = 8");
            outrule = outrule.Replace("P01_CHK_MOTIONSENSOR = 1", "{field_led}[result] = 4 OR {field_led}[result] = 8");
            outrule = outrule.Replace("P01_CMB_KICKPLATES = \"STANDARD\"", "{field_backkick}[result] = 2");
            outrule = outrule.Replace("P01_CMB_KICKPLATES = \"12 IN\"", "{field_backkick}[result] = 3");
            outrule = outrule.Replace("P01_CMB_KICKPLATES = \"DIAMOND\"", "{field_backkick}[result] = 4");
            outrule = outrule.Replace("P01_TXT_DESCHANDLESELECTION = \"STANDARD\"", "{field_handle}[result] = 2");
            outrule = outrule.Replace("P01_CMB_SYSHANDLETYPE = \"STANDARD\"", "{field_handle}[result] = 2");
            outrule = outrule.Replace("P01_TXT_WRENCHKITAMT", "CEIL({field_numdoors}[result] / 5)");
            outrule = outrule.Replace("P01_DEC_DOORNUM", "CEIL({field_numdoors}[result] / 5)");
            outrule = outrule.Replace("P01_RAD_SYSFLANGE = \"1\"", "{field_flangeless}[result] = 0");
            outrule = outrule.Replace("P01_CMB_BRACKETCOLOR = \"BLACK\"", "{field_brkcolor}[result] = 2");
            outrule = outrule.Replace("STRING(P01_TXT_CUSTID) = \"\"", "{field_led}[result] = 0");
            outrule = outrule.Replace("STRING(P01_TXT_CUSTID) = \"SLOAN\"", "{field_led}[result] = 5 OR {field_led}[result] = 8");
            outrule = outrule.Replace("STRING(P01_TXT_CUSTID) = \"ELECKRA\"", "{field_led}[result] = 3 OR {field_led}[result] = 4");
            outrule = outrule.Replace("STRING(P01_TXT_CUSTID) = \"YES\"", "{field_led}[result] = 1");
            outrule = outrule.Replace("P01_CHK_ELECkRA = 1", "{field_led}[result] = 3 OR {field_led}[result] = 4");
            outrule = outrule.Replace("P01_CHK_ELECKRA = 1", "{field_led}[result] = 3 OR {field_led}[result] = 4");
            outrule = outrule.Replace("STRING(P01_TXT_CUSTID) = \"HIGH\"", "{field_led}[result] = 2");
            outrule = outrule.Replace("STRING(P01_TXT_CUSTID) = \"LED80\"", "{field_led}[result] = 7");
            outrule = outrule.Replace("STRING(P01_TXT_CUSTID) = \"USLED\"", "{field_led}[result] = 6");

            outrule = outrule.Replace("P01_CMB_SYSTEMSELECTION", "{field_system}[result]");
            outrule = outrule.Replace("P01_TXT_COLORALUMCODE", "{field_finish}[result]");
            outrule = outrule.Replace("P01_CMB_SYSDOORCONSTRUCT", "{field_construction}[result]");
            outrule = outrule.Replace("P01_CMB_SYSHANDLETYPE", "{field_handle}[result]");
            outrule = outrule.Replace("P01_CMB_SYSDOORSIZE", "{field_size}[result]");
            outrule = outrule.Replace("P01_CMB_SYSLIGHTTYPE", "{field_light}[result]");
            outrule = outrule.Replace("P01_CHK_FRONTKICKPLATES", "{field_frontkick}[result]");
            outrule = outrule.Replace("P01_CHK_FRONTKICKPLATES", "{field_frontkick}[result]");
            outrule = outrule.Replace("P01_CMB_SYSCOLOR", "{field_finish}[result]");
            outrule = outrule.Replace("P01_CMB_SYSDOORNUM", "{field_numdoors}[result]");
            outrule = outrule.Replace("P01_DEC_BACKBMPGRDS", "{field_backbgqty}[result]");
            outrule = outrule.Replace("P01_DEC_FRONTBMPGRD", "{field_frontbgqty}[result]");
            outrule = outrule.Replace("P01_CMB_BRACKETCOLOR", "{field_brkcolor}[result]");
            outrule = outrule.Replace("P01_RAD_SYSFLANGE = \"2\"", "{field_flangeless}[result] = 1");

            outrule = outrule.Replace("DECIMAL(SUBSTRING(P02_TXT_FDLUPARTNUM1,1,2))", "{field_frmopen}[result]");        //movable
            outrule = outrule.Replace("SUBSTRING(P02_TXT_FDLUPARTNUM1,1,2)", "{field_frmopen}[result]");
            outrule = outrule.Replace("STRING(SUBSTRING(P02_TXT_FDLUPARTNUM1, 1, 4))", "{field_frmcode}[result]");        //movable
            outrule = outrule.Replace("SUBSTRING(P02_TXT_FDLUPARTNUM1,1,4)", "{field_frmcode}[result]");
            outrule = outrule.Replace("DECIMAL(SUBSTRING(P02_TXT_FDLUPARTNUM2,1,2))", "{field_frmopen}[result]");        //movable
            outrule = outrule.Replace("SUBSTRING(P02_TXT_FDLUPARTNUM2,1,2)", "{field_frmopen}[result]");
            outrule = outrule.Replace("STRING(SUBSTRING(P02_TXT_FDLUPARTNUM2, 1, 4))", "{field_frmcode}[result]");        //movable
            outrule = outrule.Replace("SUBSTRING(P02_TXT_FDLUPARTNUM2,1,4)", "{field_frmcode}[result]");
            outrule = outrule.Replace("DECIMAL(SUBSTRING(P02_TXT_FDLUPARTNUM3,1,2))", "{field_frmopen}[result]");        //movable
            outrule = outrule.Replace("SUBSTRING(P02_TXT_FDLUPARTNUM3,1,2)", "{field_frmopen}[result]");
            outrule = outrule.Replace("STRING(SUBSTRING(P02_TXT_FDLUPARTNUM3, 1, 4))", "{field_frmcode}[result]");        //movable
            outrule = outrule.Replace("SUBSTRING(P02_TXT_FDLUPARTNUM3,1,4)", "{field_frmcode}[result]");
            outrule = outrule.Replace("DECIMAL(SUBSTRING(P02_TXT_FDLUPARTNUM4,1,2))", "{field_frmopen}[result]");        //movable
            outrule = outrule.Replace("SUBSTRING(P02_TXT_FDLUPARTNUM4,1,2)", "{field_frmopen}[result]");
            outrule = outrule.Replace("STRING(SUBSTRING(P02_TXT_FDLUPARTNUM4, 1, 4))", "{field_frmcode}[result]");       //movable
            outrule = outrule.Replace("SUBSTRING(P02_TXT_FDLUPARTNUM4,1,4)", "{field_frmcode}[result]");

            outrule = outrule.Replace("STRING(P01_TXT_DOORW)", "{field_size}[result_width]");
            outrule = outrule.Replace("STRING(P01_TXT_DOORH)", "{field_size}[result_height]");
            outrule = outrule.Replace("(Decimal(P01_TXT_DOORW))", "{field_size}[result_width]");
            outrule = outrule.Replace("Decimal(P01_TXT_DOORW)", "{field_size}[result_width]");
            outrule = outrule.Replace("DECIMAL(P01_TXT_DOORW)", "{field_size}[result_width]");
            outrule = outrule.Replace("P01_TXT_DOORW", "{field_size}[result_width]");
            outrule = outrule.Replace("DECIMAL(P01_TXT_DOORH)", "{field_size}[result_height]");
            outrule = outrule.Replace("(Decimal(P01_TXT_DOORH))", "{field_size}[result_height]");
            outrule = outrule.Replace("Decimal(P01_TXT_DOORH)", "{field_size}[result_height]");
            outrule = outrule.Replace("P01_TXT_DOORH", "{field_size}[result_height]");
            outrule = outrule.Replace("STRING(P01_TXT_DOORGLASSPARTNUM)", "{field_glasspart}[result]");

            outrule = outrule.Replace("STRING(P02_TXT_DOORHTRWIRENUM)", "{field_htrwire}[result]");
            outrule = outrule.Replace("STRING(P02_TXT_FRAME1HTRWIREPARTNUM)", "{field_frmhtrwire}[result]");
            outrule = outrule.Replace("STRING(P02_TXT_FR1SECHTRWIREPARTNUM)", "{field_frmsechtrwire}[result]");
            outrule = outrule.Replace("STRING(P02_TXT_FRAME2HTRWIREPARTNUM)", "{field_frmhtrwire}[result]");
            outrule = outrule.Replace("STRING(P02_TXT_FR2SECHTRWIREPARTNUM)", "{field_frmsechtrwire}[result]");
            outrule = outrule.Replace("STRING(P02_TXT_FRAME3HTRWIREPARTNUM)", "{field_frmhtrwire}[result]");
            outrule = outrule.Replace("STRING(P02_TXT_FR3SECHTRWIREPARTNUM)", "{field_frmsechtrwire}[result]");
            outrule = outrule.Replace("STRING(P02_TXT_FRAME4HTRWIREPARTNUM)", "{field_frmhtrwire}[result]");
            outrule = outrule.Replace("STRING(P02_TXT_FR4SECHTRWIREPARTNUM)", "{field_frmsechtrwire}[result]");
            outrule = outrule.Replace("STRING(P02_TXT_MULHTRWIRENUM)", "{field_mulhtrwire}[result]");
            outrule = outrule.Replace("P02_DEC_LOCKFRBRKTAMT", "{field_lockfrmbrkamt}[result]");


            outrule = outrule.Replace("\"FULL-LENGTH\"", "3");

            outrule = outrule.Replace("\"BLACK\"", "2");

            outrule = outrule.Replace("\"CLASSIC PLUS (2 pane E-coat gas-filled non-heated)\"", "3");
            outrule = outrule.Replace("\"20//20 PLUS (2 pane E-coat gas-filled heated)\"", "6");
            outrule = outrule.Replace("\"CLASSIC PLUS FREEZER (3 pane E-coat heated)\"", "4");
            outrule = outrule.Replace("\"CLASSIC NT (2 pane unheated)\"", "1");
            outrule = outrule.Replace("\"CLASSIC 20//20 (2 pane heated)\"", "5");
            outrule = outrule.Replace("\"CLASSIC LT (3 pane heated)\"", "2");
            outrule = outrule.Replace("\"HYBRIDOOR (2 pane E-coat gas-filled non-heated)\"", "7");
            outrule = outrule.Replace("\"HYBRIDOOR FREEZER (3 pane E-coat heated)\"", "8");
            outrule = outrule.Replace("\"S//E PLUS (2 pane E-coat gas-filled non-heated)\"", "12");
            outrule = outrule.Replace("\"S//E NT (2 pane unheated)\"", "9");
            outrule = outrule.Replace("\"S//E LT (3 pane heated)\"", "11");
            outrule = outrule.Replace("\"S//E NT (2 pane E-coat unheated)\"", "10");
            outrule = outrule.Replace("\"CAL-20 (3 pane E-coat unheated)\"", "13");

            outrule = outrule.Replace("'CLASSIC PLUS (2 pane E-coat gas-filled non-heated)'", "3");
            outrule = outrule.Replace("'20//20 PLUS (2 pane E-coat gas-filled heated)'", "6");
            outrule = outrule.Replace("'CLASSIC PLUS FREEZER (3 pane E-coat heated)'", "4");
            outrule = outrule.Replace("'CLASSIC NT (2 pane unheated)'", "1");
            outrule = outrule.Replace("'CLASSIC 20//20 (2 pane heated)'", "5");
            outrule = outrule.Replace("'CLASSIC LT (3 pane heated)'", "2");
            outrule = outrule.Replace("'HYBRIDOOR (2 pane E-coat gas-filled non-heated)'", "7");
            outrule = outrule.Replace("'HYBRIDOOR FREEZER (3 pane E-coat heated)'", "8");
            outrule = outrule.Replace("'S//E PLUS (2 pane E-coat gas-filled non-heated)'", "12");
            outrule = outrule.Replace("'S//E NT (2 pane unheated)'", "9");
            outrule = outrule.Replace("'S//E LT (3 pane heated)'", "11");
            outrule = outrule.Replace("'S//E NT (2 pane E-coat unheated)'", "10");
            outrule = outrule.Replace("'CAL-20 (3 pane E-coat unheated)'", "13");

            outrule = outrule.Replace("CLASSIC MT (3 pane unheated)", "");
            outrule = outrule.Replace("F//X NT (2 pane unheated)", "");
            outrule = outrule.Replace("F//X LT (3 pane heated)", "");
            outrule = outrule.Replace("F//X 20//20 (2 pane heated)", "");
            outrule = outrule.Replace("F//X NT (2 pane E-coat unheated)", "");
            outrule = outrule.Replace("F//X PLUS (3 pane E-coat non-heated)", "");
            outrule = outrule.Replace("F//X PLUS FREEZER (3 pane E-coat heated)", "");

            outrule = outrule.Replace("\"DOOR/FRAME SYSTEM\"", "1");
            outrule = outrule.Replace("\"DOOR ONLY\"", "2");
            outrule = outrule.Replace("\"FRAME ONLY\"", "3");
            outrule = outrule.Replace("\"S//D PLUS\"", "4");
            outrule = outrule.Replace("\"R//M PLUS\"", "5");

            outrule = outrule.Replace("\"K1\"", "1");
            outrule = outrule.Replace("\"K2\"", "2");
            outrule = outrule.Replace("\"K3\"", "3");
            outrule = outrule.Replace("\"G1\"", "4");
            outrule = outrule.Replace("\"G2\"", "5");
            outrule = outrule.Replace("\"G3\"", "6");
            outrule = outrule.Replace("\"S3\"", "7");
            outrule = outrule.Replace("\"S2\"", "8");
            outrule = outrule.Replace("\"S5\"", "9");
            outrule = outrule.Replace("\"D1\"", "10");
            outrule = outrule.Replace("\"D2\"", "11");
            outrule = outrule.Replace("\"D3\"", "12");
            outrule = outrule.Replace("\"D5\"", "13");
            outrule = outrule.Replace("\"R1\"", "14");
            outrule = outrule.Replace("\"R2\"", "15");
            outrule = outrule.Replace("\"R3\"", "16");
            outrule = outrule.Replace("\"R5\"", "17");

            outrule = outrule.Replace("\"SS\"", "2");
            outrule = outrule.Replace("\"BB\"", "6");
            outrule = outrule.Replace("\"SB\"", "3");
            outrule = outrule.Replace("\"BG\"", "5");
            outrule = outrule.Replace("\"BS\"", "4");
            outrule = outrule.Replace("\"SSS\"", "7");
            outrule = outrule.Replace("\"SSB\"", "8");
            outrule = outrule.Replace("\"SBB\"", "10");
            outrule = outrule.Replace("\"SBS\"", "9");

            outrule = outrule.Replace("\"BRIGHT GOLD\"", "5");
            outrule = outrule.Replace("\"BRIGHT SILVER\"", "4");
            outrule = outrule.Replace("\"SATIN SILVER\"", "2");
            outrule = outrule.Replace("\"SMOOTH BRIGHT SILVER\"", "9");
            outrule = outrule.Replace("\"SMOOTH SATIN SILVER\"", "7");
            outrule = outrule.Replace("\"BRIGHT BLACK\"", "6");
            outrule = outrule.Replace("\"SATIN BLACK\"", "3");
            outrule = outrule.Replace("\"SMOOTH BRIGHT BLACK\"", "10");
            outrule = outrule.Replace("\"SMOOTH SATIN BLACK\"", "8");
            outrule = outrule.Replace("\"CHERRY TOMATO\"", "");
            outrule = outrule.Replace("\"FOREST GREEN\"", "");
            outrule = outrule.Replace("\"GRANITE\"", "");
            outrule = outrule.Replace("\"ANTIQUE SILVER\"", "");
            outrule = outrule.Replace("\"WHITE\"", "");

            outrule = outrule.Replace("'BRIGHT GOLD'", "5");
            outrule = outrule.Replace("'BRIGHT SILVER'", "4");
            outrule = outrule.Replace("'SATIN SILVER'", "2");
            outrule = outrule.Replace("'SMOOTH BRIGHT SILVER'", "9");
            outrule = outrule.Replace("'SMOOTH SATIN SILVER'", "7");
            outrule = outrule.Replace("'BRIGHT BLACK'", "6");
            outrule = outrule.Replace("'SATIN BLACK'", "3");
            outrule = outrule.Replace("'SMOOTH BRIGHT BLACK'", "10");
            outrule = outrule.Replace("'SMOOTH SATIN BLACK'", "8");
            outrule = outrule.Replace("'CHERRY TOMATO'", "");
            outrule = outrule.Replace("'FOREST GREEN'", "");
            outrule = outrule.Replace("'GRANITE'", "");
            outrule = outrule.Replace("'ANTIQUE SILVER'", "");
            outrule = outrule.Replace("'WHITE'", "");

            outrule = outrule.Replace("\"NONE\"", "1");
            outrule = outrule.Replace("\"STANDARD T-8\"", "2");
            outrule = outrule.Replace("\"LED\"", "3");
            outrule = outrule.Replace("\"ALS\"", "");
            outrule = outrule.Replace("\"RECESSED ALS\"", "");

//            outrule = outrule.Replace("", "");




            return outrule;
        }

        static public List<string> SortRules(List<string> rules)
        {
            List<string> r = rules;

            for (int x = 0; x < r.Count - 1; x++)
            {
                for (int y = x + 1; y < r.Count; y++)
                {
                    int xpoints = 0;
                    int ypoints = 0;
                    if (r[x].StartsWith("IN({field_construction}[result]"))
                        xpoints = 2;
                    if (r[y].StartsWith("IN({field_construction}[result]"))
                        ypoints = 2;
                    if (r[x].StartsWith("IN({field_system}[result]"))
                        xpoints = 3;
                    if (r[y].StartsWith("IN({field_system}[result]"))
                        ypoints = 3;
                    if (r[x] == "{field_backbgqty}[result] > 0")
                        xpoints = 1;
                    if (r[y] == "{field_backbgqty}[result] > 0")
                        ypoints = 1;
                    if (r[x] == "{field_frontbgqty}[result] > 0")
                        xpoints = 1;
                    if (r[y] == "{field_frontbgqty}[result] > 0")
                        ypoints = 1;
                    if (ypoints > xpoints)
                    {
                        string temp = r[x];
                        r[x] = r[y];
                        r[y] = temp;
                    }
                }
            }

            return r;
        }
    }

    class Part
    {
        public string partnum { get; set; }
        public List<string> keepwhens;
        public string qty;
        public bool as_asm { get; set; }

        public Part(string num)
        {
            partnum = num;
            keepwhens = new List<string>();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Part p = obj as Part;
            if ((System.Object)p == null)
            {
                return false;
            }

            if (partnum != p.partnum)
                return false;

            if (keepwhens.Count != p.keepwhens.Count)
                return false;

            bool match = true;
            for (int i = 0; i < keepwhens.Count; i++)
            {
                if (keepwhens[i] != p.keepwhens[i])
                    match = false;
            }

            return match;
        }

        public void ParseRule(string rule)
        {
            rule = rule.TrimEnd();
            string trule = "";
            if (!String.IsNullOrEmpty(rule))
            {
                if (rule[0] == '{')
                {
                    string path = rule.Substring(1, rule.Length - 2).Replace("E:", @"\\sql");
                    StreamReader sr = new StreamReader(path);
                    keepwhens = TranslateRule(sr.ReadToEnd());
                }
                else
                {
                    keepwhens = TranslateRule(rule);
                }
            }
        }

        public void ParseQty(string rule)
        {
            rule = rule.TrimEnd();
            string trule = "";
            if (String.IsNullOrEmpty(rule))
                qty = "1.0";
            else if (rule[0] == '{')
            {
                string path = rule.Substring(1, rule.Length - 2).Replace("E:", @"\\sql");
                StreamReader sr = new StreamReader(path);
                qty = Helper.CleanRule(Helper.TranslateRule(sr.ReadToEnd()).Replace(" AND ", " && "));
            }
            else
            {
                qty = Helper.CleanRule(Helper.TranslateRule(rule).Replace(" AND ", " && "));
            }
            if (qty[0] == '(' && qty[qty.Length - 1] == ')')
                qty = qty.Substring(1, qty.Length - 2);
        }

        private List<string> TranslateRule(string inrule)
        {
            List<string> rules = new List<string>();
            foreach (string s in Helper.CleanRule(Helper.TranslateRule(inrule)).Split(new string[] { " && " }, StringSplitOptions.RemoveEmptyEntries))
            {
                string b = s;
/*                if (b[0] == '(' && b[b.Length - 1] == ')')
                    b = b.Substring(1, b.Length - 2);*/
                if (b != "IN({field_6}[result], 1, 2, 5, 3, 6, 4, 9, 11, 10, 13, 12, 7, 8)")
                    rules.Add(b.Trim());
            }

            return Helper.SortRules(rules);
        }
    }

    class Program
    {
        static int configid = 57;
        //static string url = "http://172.16.10.21:3000/";
        static string url = "http://172.16.6.160/wbc/";

        static void Main(string[] args)
        {
            SqlCommand command = new SqlCommand(@"select pm.partnum, pm.mtlpartnum, pm.qtyper, pm.mtlseq, pm.relatedoperation, po.opcode, pm.pullasasm, 
case 
when pm.partnum like 'DOOR%' then 
	case
		when mtlpartnum in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-LH-WITHLOCKS')
			 and mtlpartnum in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-RH-WITHLOCKS')
			 and mtlpartnum in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-LH-NOLOCKS')
			 and mtlpartnum in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-RH-NOLOCKS')
			 then 'AD'
		when mtlpartnum in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-LH-WITHLOCKS')
			 and mtlpartnum in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-RH-WITHLOCKS')
			 and mtlpartnum not in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-LH-NOLOCKS')
			 and mtlpartnum not in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-RH-NOLOCKS')
			 then 'D-LOCKS'
		when mtlpartnum not in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-LH-WITHLOCKS')
			 and mtlpartnum not in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-RH-WITHLOCKS')
			 and mtlpartnum in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-LH-NOLOCKS')
			 and mtlpartnum in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-RH-NOLOCKS')
			 then 'D-NOLOCKS'
		when mtlpartnum in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-LH-WITHLOCKS')
			 and mtlpartnum not in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-RH-WITHLOCKS')
			 and mtlpartnum in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-LH-NOLOCKS')
			 and mtlpartnum not in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-RH-NOLOCKS')
			 then 'D-LH'
		when mtlpartnum not in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-LH-WITHLOCKS')
			 and mtlpartnum in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-RH-WITHLOCKS')
			 and mtlpartnum not in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-LH-NOLOCKS')
			 and mtlpartnum in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum = 'DOOR-RH-NOLOCKS')
			 then 'D-RH'
		else 'D?'
	end
when pm.partnum = 'FRAME1' then
	case 
		when mtlpartnum not in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum in ('FRAME2', 'FRAME3', 'FRAME4')) then 'F1'
		else 'AF'
	end
when pm.partnum = 'FRAME2' then
	case 
		when mtlpartnum not in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum in ('FRAME1', 'FRAME3', 'FRAME4')) then 'F2'
		else 'AF'
	end
when pm.partnum = 'FRAME3' then
	case 
		when mtlpartnum not in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum in ('FRAME2', 'FRAME1', 'FRAME4')) then 'F3'
		else 'AF'
	end
when pm.partnum = 'FRAME4' then
	case 
		when mtlpartnum not in (select distinct mtlpartnum from partmtl (nolock) where company = 'CRD' and partnum in ('FRAME2', 'FRAME3', 'FRAME1')) then 'F4'
		else 'AF'
	end
else 'UNK'
end as code,
pc_kw.ruleexpr as keepwhen,
pc_qp.ruleexpr as qtyper,
 * 
from partmtl (nolock) as pm 
inner join partopr (nolock) as po on pm.company = po.company and pm.relatedoperation = po.oprseq and pm.partnum = po.partnum
left join pcrules (nolock) as pc_kw on pm.company = pc_kw.company and pc_kw.partnum = 'SYSTEM' and pm.partnum = pc_kw.asmpartnum and pm.relatedoperation = pc_kw.oprseq and pm.mtlseq = pc_kw.mtlseq and pc_kw.ruletype = 'Keep When'
left join pcrules (nolock) as pc_qp on pm.company = pc_qp.company and pc_qp.partnum = 'SYSTEM' and pm.partnum = pc_qp.asmpartnum and pm.relatedoperation = pc_qp.oprseq and pm.mtlseq = pc_qp.mtlseq and pc_qp.ruletype = 'Set Field'
where 
pm.company = 'CRD' 
and pm.partnum in ('FRAME1', 'FRAME2', 'FRAME3', 'FRAME4', 'DOOR-LH-NOLOCKS', 'DOOR-RH-NOLOCKS', 'DOOR-LH-WITHLOCKS', 'DOOR-RH-WITHLOCKS')
and pm.mtlpartnum not in (select partnum from _wbc_inserted_parts)
order by pm.mtlpartnum, po.opcode, pm.partnum, pm.mtlseq");

                using (SqlConnection sqlConnection = new SqlConnection(String.Format("Data Source = {0}; Initial Catalog = {1}; User ID = {2}; Password = {3};", "SQL", "MfgSys803", "rails", "hJ*G6_!pZ2")))
                {
                    command.Connection = sqlConnection;

                    sqlConnection.Open();

                    SqlDataAdapter sda = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);

                    sqlConnection.Close();

//                    CreateParts(ds);
                    WriteSheet(ds);


                }
        }

        static private string GetBlockCode(string rule)
        {
            if (rule.StartsWith("{field_26}[result] =="))
                return "GLASS";
            switch (rule)
            {
                case "IN({field_construction}[result], 1, 2, 5, 3, 6, 4)":
                    return "CLASSIC";
                case "IN({field_construction}[result], 9, 11, 10, 13, 12, 7, 8)":
                    return "SE";
                case "IN({field_system}[result], 1, 2)":
                    return "SYSDOOR";
                case "IN({field_system}[result], 1, 3)":
                    return "SYSFRAME";
                case "IN({field_system}[result], 4, 5, 2)":
                    return "ENTDOOR";
                case "IN({field_system}[result], 1, 2, 4, 5)":
                    return "ALLDOOR";
                case "IN({field_system}[result], 1, 3, 4, 5)":
                    return "ALLFRAME";
                case "{field_backbgqty}[result] > 0":
                    return "BBMPG";
                case "{field_frontbgqty}[result] > 0":
                    return "FBMPG";
                
                default:
                    return rule;
            }
        }

        static private int WhereToInsert(string code, string op, ref List<string> rules)
        {
                switch (op)
                {
                    case "WIREWAY":
                        {
                            int id = 2075;
                            int index = 0;
                            while (index < rules.Count)
                            {
                                if (GetBlockCode(rules[index]) == "SYSFRAME")
                                {
                                    id = 2063;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "ALLFRAME")
                                {
                                    id = 2065;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "CLASSIC")
                                {
                                    if (id == 2063)
                                    {
                                        id = 2067;
                                        rules.RemoveAt(index);
                                    }
                                    else if (id == 2065)
                                    {
                                        id = 2071;
                                        rules.RemoveAt(index);
                                    }
                                    else
                                        index++;
                                }
                                else if (GetBlockCode(rules[index]) == "SE")
                                {
                                    if (id == 2063)
                                    {
                                        id = 2069;
                                        rules.RemoveAt(index);
                                    }
                                    else if (id == 2065)
                                    {
                                        id = 2073;
                                        rules.RemoveAt(index);
                                    }
                                    else
                                        index++;
                                }
                                else
                                    index++;
                            }
                            return id;
                        }
                    case "FRWELD":
                        {
                            int id = 2045;
                            int index = 0;
                            while (index < rules.Count)
                            {
                                if (GetBlockCode(rules[index]) == "SYSFRAME")
                                {
                                    id = 2033;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "ALLFRAME")
                                {
                                    id = 2035;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "CLASSIC")
                                {
                                    if (id == 2033)
                                    {
                                        id = 2037;
                                        rules.RemoveAt(index);
                                    }
                                    else if (id == 2035)
                                    {
                                        id = 2041;
                                        rules.RemoveAt(index);
                                    }
                                    else
                                        index++;
                                }
                                else if (GetBlockCode(rules[index]) == "SE")
                                {
                                    if (id == 2033)
                                    {
                                        id = 2039;
                                        rules.RemoveAt(index);
                                    }
                                    else if (id == 2035)
                                    {
                                        id = 2043;
                                        rules.RemoveAt(index);
                                    }
                                    else
                                        index++;
                                }
                                else
                                    index++;
                            }
                            return id;
                        }
                    case "FRPUNCH":
                        {
                            int id = 2013;
                            int index = 0;
                            while (index < rules.Count)
                            {
                                if (GetBlockCode(rules[index]) == "SYSFRAME")
                                {
                                    id = 2001;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "ALLFRAME")
                                {
                                    id = 2003;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "CLASSIC")
                                {
                                    if (id == 2001)
                                    {
                                        id = 2005;
                                        rules.RemoveAt(index);
                                    }
                                    else if (id == 2003)
                                    {
                                        id = 2009;
                                        rules.RemoveAt(index);
                                    }
                                    else
                                        index++;
                                }
                                else if (GetBlockCode(rules[index]) == "SE")
                                {
                                    if (id == 2001)
                                    {
                                        id = 2007;
                                        rules.RemoveAt(index);
                                    }
                                    else if (id == 2003)
                                    {
                                        id = 2011;
                                        rules.RemoveAt(index);
                                    }
                                    else
                                        index++;
                                }
                                else
                                    index++;
                            }
                            return id;
                        }
                    case "FRPREP":
                        {
                            int id = 1958;
                            int index = 0;
                            while (index < rules.Count)
                            {
                                if (GetBlockCode(rules[index]) == "SYSFRAME")
                                {
                                    id = 1954;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "ALLFRAME")
                                {
                                    id = 1956;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "CLASSIC")
                                {
                                    if (id == 1954)
                                    {
                                        id = 1960;
                                        rules.RemoveAt(index);
                                    }
                                    else if (id == 1956)
                                    {
                                        id = 1964;
                                        rules.RemoveAt(index);
                                    }
                                    else
                                        index++;
                                }
                                else if (GetBlockCode(rules[index]) == "SE")
                                {
                                    if (id == 1954)
                                    {
                                        id = 1962;
                                        rules.RemoveAt(index);
                                    }
                                    else if (id == 1956)
                                    {
                                        id = 1966;
                                        rules.RemoveAt(index);
                                    }
                                    else
                                        index++;
                                }
                                else
                                    index++;
                            }
                            return id;
                        }
                    case "FRASM":
                        {
                            int id = 1895;
                            int index = 0;
                            while (index < rules.Count)
                            {
                                if (GetBlockCode(rules[index]) == "SYSFRAME")
                                {
                                    id = 1877;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "ALLFRAME")
                                {
                                    id = 1879;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "CLASSIC")
                                {
                                    if (id == 1877)
                                    {
                                        id = 1881;
                                        rules.RemoveAt(index);
                                    }
                                    else if (id == 1879)
                                    {
                                        id = 1885;
                                        rules.RemoveAt(index);
                                    }
                                    else if (id == 1895)
                                    {
                                        id = 1891;
                                        rules.RemoveAt(index);
                                    }
                                    else
                                        index++;
                                }
                                else if (GetBlockCode(rules[index]) == "SE")
                                {
                                    if (id == 1877)
                                    {
                                        id = 1883;
                                        rules.RemoveAt(index);
                                    }
                                    else if (id == 1879)
                                    {
                                        id = 1887;
                                        rules.RemoveAt(index);
                                    }
                                    else if (id == 1895)
                                    {
                                        id = 1893;
                                        rules.RemoveAt(index);
                                    }
                                    else
                                        index++;
                                }
                                else
                                    index++;
                            }
                            return id;
                        }
                    case "UNASM":
                        {
                            return 1488;
                        }
                    case "DRPUNCH":
                        {
                            int id = 1480;
                            int index = 0;
                            while (index < rules.Count)
                            {
                                if (GetBlockCode(rules[index]) == "SYSDOOR")
                                {
                                    id = 1756;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "ALLDOOR")
                                {
                                    id = 1758;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "CLASSIC")
                                {
                                    if (id == 1756)
                                    {
                                        id = 1760;
                                        rules.RemoveAt(index);
                                    }
                                    else if (id == 1758)
                                    {
                                        id = 1764;
                                        rules.RemoveAt(index);
                                    }
                                    else
                                        index++;
                                }
                                else if (GetBlockCode(rules[index]) == "SE")
                                {
                                    if (id == 1756)
                                    {
                                        id = 1762;
                                        rules.RemoveAt(index);
                                    }
                                    else if (id == 1758)
                                    {
                                        id = 1766;
                                        rules.RemoveAt(index);
                                    }
                                    else
                                        index++;
                                }
                                else
                                    index++;
                            }
                            return id;
                        }
                    case "DRPREP":
                        {
                            int id = 1732;
                            int index = 0;
                            while (index < rules.Count)
                            {
                                if (GetBlockCode(rules[index]) == "SYSDOOR")
                                {
                                    id = 1726;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "CLASSIC")
                                {
                                    if (id == 1726)
                                    {
                                        id = 1728;
                                        rules.RemoveAt(index);
                                    }
                                    else
                                        index++;
                                }
                                else if (GetBlockCode(rules[index]) == "SE")
                                {
                                    if (id == 1726)
                                    {
                                        id = 1730;
                                        rules.RemoveAt(index);
                                    }
                                    else
                                        index++;
                                }
                                else
                                    index++;
                            }
                            return id;
                        }
                    case "DRCRIMP":
                        {
                            int id = 1482;
                            int index = 0;
                            while (index < rules.Count)
                            {
                                if (GetBlockCode(rules[index]) == "SYSDOOR")
                                {
                                    id = 1697;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "ALLDOOR")
                                {
                                    id = 1699;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "CLASSIC")
                                {
                                    if (id == 1697)
                                    {
                                        id = 1701;
                                        rules.RemoveAt(index);
                                    }
                                    else if (id == 1699)
                                    {
                                        id = 1703;
                                        rules.RemoveAt(index);
                                    }
                                    else
                                        index++;
                                }
                                else if (GetBlockCode(rules[index]) == "SE")
                                {
                                    if (id == 1697)
                                    {
                                        id = 1705;
                                        rules.RemoveAt(index);
                                    }
                                    else if (id == 1699)
                                    {
                                        id = 1707;
                                        rules.RemoveAt(index);
                                    }
                                    else
                                        index++;
                                }
                                else
                                    index++;
                            }
                            return id;
                        }
                    case "DRASM":
                        {
                            int id = 1575;
                            int index = 0;
                            while (index < rules.Count)
                            {
                                if (GetBlockCode(rules[index]) == "CLASSIC")
                                {
                                    id = 1530;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "SE")
                                {
                                    id = 1532;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "SYSDOOR")
                                {
                                    id = 1560;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "ENTDOOR")
                                {
                                    id = 1562;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "FBMPG")
                                {
                                    id = 1689;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "BBMPG")
                                {
                                    id = 1687;
                                    rules.RemoveAt(index);
                                }
                                else if (GetBlockCode(rules[index]) == "GLASS")
                                {
                                    id = 1528;
                                    index++;
                                }
                                else
                                    index++;
                            }
                            return id;
                        }
                    default:
                        return 0;
                }
        }

        static private int WriteField(int wheretoinsert, Part p)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "configurators/" + configid.ToString() + "/wizard_field_add.json");
            request.ContentType = "application/json";
            request.ContentLength = 0;
            request.Method = "Post";
            string data = "{ \"add_below\" : \"" + wheretoinsert.ToString()
                + "\", \"type\" : \"hidden"
                + "\", \"name\" : \"" + p.partnum
                + "\", \"enabled\" : \"" + (p.keepwhens.Count > 0 ? "off" : "on")
                + "\", \"output\" : \"on\"";
            if (!String.IsNullOrEmpty(p.qty))
                data += ", \"value\" : \"" + p.qty + "\"";
            else
                data += ", \"vaue\" : \"0\"";
            if (p.as_asm)
                data += ", \"as_asm\" : \"on\"";
            data += "}";

            request.ContentLength = sizeof(Byte) * data.Length;
            StreamWriter writer = new StreamWriter(request.GetRequestStream());
            writer.Write(data);
            writer.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string content = new StreamReader(response.GetResponseStream()).ReadToEnd();

            content = content.Replace("{", "").Replace("}", "").Replace("\"", "");
            int result_id = 0;
            foreach (string val in content.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string key = val.Split(new char[] { ':' })[0];
                string value = val.Split(new char[] { ':' })[1];
                switch (key)
                {
                    case "id":
                        result_id = Int32.Parse(value.ToUpper());
                        break;
                }
            }

            return result_id;
        }

        static private int WriteOverride(int field_id)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "fields/" + field_id.ToString() + "/key_override_add.json");
            request.ContentType = "application/json";
            request.ContentLength = 0;
            request.Method = "Post";
            string data = "{ \"key\" : \"enabled\" }";

            request.ContentLength = sizeof(Byte) * data.Length;
            StreamWriter writer = new StreamWriter(request.GetRequestStream());
            writer.Write(data);
            writer.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string content = new StreamReader(response.GetResponseStream()).ReadToEnd();

            content = content.Replace("{", "").Replace("}", "").Replace("\"", "");
            int result_id = 0;
            foreach (string val in content.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string key = val.Split(new char[] { ':' })[0];
                string value = val.Split(new char[] { ':' })[1];
                switch (key)
                {
                    case "id":
                        result_id = Int32.Parse(value.ToUpper());
                        break;
                }
            }

            return result_id;

        }

        static private int WriteCondition(int override_id, string value)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "conditions.json");
            request.ContentType = "application/json";
            request.ContentLength = 0;
            request.Method = "Post";
            string data = "{ \"condition\" : { \"override_id\" : \"" + override_id.ToString() + "\", \"active\" : \"1\", \"value\" : \"" + value.Replace("\"", "'") + "\" } }";

            request.ContentLength = sizeof(Byte) * data.Length;
            StreamWriter writer = new StreamWriter(request.GetRequestStream());
            writer.Write(data);
            writer.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string content = new StreamReader(response.GetResponseStream()).ReadToEnd();

            content = content.Replace("{", "").Replace("}", "").Replace("\"", "");
            int result_id = 0;
            foreach (string val in content.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string key = val.Split(new char[] { ':' })[0];
                string v = val.Split(new char[] { ':' })[1];
                switch (key)
                {
                    case "id":
                        result_id = Int32.Parse(v.ToUpper());
                        break;
                }
            }

            return result_id;
        }

        static private void CreateParts(DataSet ds)
        {
            Dictionary<string, Dictionary<string, List<Part>>> MOM = new Dictionary<string, Dictionary<string, List<Part>>>();

            int max = 89;
            int count = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string code = ds.Tables[0].Rows[i][7].ToString();
                if (!MOM.ContainsKey(code))
                    MOM[code] = new Dictionary<string, List<Part>>();
                string op = ds.Tables[0].Rows[i][5].ToString();
                if (!MOM[code].ContainsKey(op))
                    MOM[code][op] = new List<Part>();
                Part newp = new Part(ds.Tables[0].Rows[i][1].ToString());
                newp.as_asm = ds.Tables[0].Rows[i][5].ToString() == "5";
                newp.ParseRule(ds.Tables[0].Rows[i][8].ToString().TrimEnd());
                newp.ParseQty(ds.Tables[0].Rows[i][9].ToString().TrimEnd());
                bool found = false;
                foreach (Part p in MOM[code][op])
                {
                    if (p.Equals(newp))
                        found = true;
                }
                if (!found)
                {
                    MOM[code][op].Add(newp);
                    count++;
                    if (count == max)
                        break;
                }
            }

            count = 0;
            foreach (string code in MOM.Keys)
            {
                foreach (string op in MOM[code].Keys)
                {
                    foreach (Part p in MOM[code][op])
                    {
                        count++;
                        int wheretoinsert = WhereToInsert(code, op, ref p.keepwhens);
                        bool debug = true;
                        if (debug)
                        {
                            string output = p.partnum + " - " + wheretoinsert.ToString() + " - " + p.qty + " - ";
                            string r = "";
                            r = "";
                            foreach (string rule in p.keepwhens)
                            {
                                if (r.Length > 0)
                                    r += ", ";
                                r += rule;
                            }
                            output += r;
                            Console.WriteLine(output);
                        }
                        else
                        {
                            int field_id = WriteField(wheretoinsert, p);
                            if (p.keepwhens.Count > 0)
                            {
                                int override_id = WriteOverride(field_id);
                                foreach (string rule in p.keepwhens)
                                    WriteCondition(override_id, rule);
                            }

                            if (url == "http://172.16.6.160/wbc/")
                            {
                                SqlCommand comm = new SqlCommand("INSERT INTO _wbc_inserted_parts (partnum, held) VALUES (@partnum, 0)");
                                comm.Parameters.AddWithValue("partnum", p.partnum);

                                using (SqlConnection sqlConnection = new SqlConnection(String.Format("Data Source = {0}; Initial Catalog = {1}; User ID = {2}; Password = {3};", "SQL", "MfgSys803", "rails", "hJ*G6_!pZ2")))
                                {
                                    comm.Connection = sqlConnection;

                                    sqlConnection.Open();

                                    comm.ExecuteNonQuery();

                                    sqlConnection.Close();
                                }
                            }
                        }

                        if (count >= max)
                            break;
                    }
                    if (count >= max)
                        break;
                }
                if (count >= max)
                    break;
            }
        }

        static private void WriteSheet(DataSet ds)
        {
            string lastpartnum = "";
            int oddeven = 1;
            using (ExcelPackage pck = new ExcelPackage(new FileInfo("C:\\ClassicMOM1.xlsx")))
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Materials");
                ws.Cells[1, 1].Value = "Asm";
                ws.Cells[1, 2].Value = "Part #";
                ws.Cells[1, 3].Value = "Qty";
                ws.Cells[1, 4].Value = "Mtl Seq";
                ws.Cells[1, 5].Value = "Op";
                ws.Cells[1, 6].Value = "As Asm";
                ws.Cells[1, 7].Value = "Code";
                ws.Cells[1, 8].Value = "Qty Per";
                ws.Cells[1, 9].Value = "Keep Whens";

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ws.Cells["A" + (i + 2).ToString()].Value = ds.Tables[0].Rows[i][0].ToString();
                    ws.Cells["B" + (i + 2).ToString()].Value = ds.Tables[0].Rows[i][1].ToString();
                    ws.Cells["C" + (i + 2).ToString()].Value = ds.Tables[0].Rows[i][2].ToString();
                    ws.Cells["D" + (i + 2).ToString()].Value = ds.Tables[0].Rows[i][3].ToString();
                    ws.Cells["E" + (i + 2).ToString()].Value = ds.Tables[0].Rows[i][5].ToString();
                    ws.Cells["F" + (i + 2).ToString()].Value = ds.Tables[0].Rows[i][6].ToString();
                    ws.Cells["G" + (i + 2).ToString()].Value = ds.Tables[0].Rows[i][7].ToString();
                    if (!String.IsNullOrEmpty(ds.Tables[0].Rows[i][9].ToString()))
                    {
                        string rule = ds.Tables[0].Rows[i][9].ToString().TrimEnd();
                        string s;
                        if (rule[0] == '{')
                        {
                            string path = rule.Substring(1, rule.Length - 2).Replace("E:", @"\\sql");
                            StreamReader sr = new StreamReader(path);
                            s = Helper.CleanRule(Helper.TranslateRule(sr.ReadToEnd()));
                        }
                        else
                            s = Helper.CleanRule(Helper.TranslateRule(rule));
                        if (s[0] == '(' && s[s.Length - 1] == ')')
                            s = s.Substring(1, s.Length - 2);
                        ws.Cells["H" + (i + 2).ToString()].Value = s;
                    }
                    else
                        ws.Cells["H" + (i + 2).ToString()].Value = "";

                    if (!String.IsNullOrEmpty(ds.Tables[0].Rows[i][8].ToString()))
                    {
                        string rule = ds.Tables[0].Rows[i][8].ToString().TrimEnd();
                        string trule = "";
                        if (rule[0] == '{')
                        {
                            string path = rule.Substring(1, rule.Length - 2).Replace("E:", @"\\sql");
                            StreamReader sr = new StreamReader(path);
                            trule = Helper.CleanRule(Helper.TranslateRule(sr.ReadToEnd()));
                        }
                        else
                        {
                            trule = Helper.CleanRule(Helper.TranslateRule(rule));
                        }
                        List<string> rules = new List<string>();
                        foreach (string s in trule.Split(new string[] { " && " }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            string b = s;
                            if (b[0] == '(' && b[b.Length - 1] == ')')
                                b = b.Substring(1, b.Length-2);
                            if (b != "IN({field_6}[result], 1, 2, 5, 3, 6, 4, 9, 11, 10, 13, 12, 7, 8)")
                                rules.Add(b.Trim());
                            //                                        ws.Cells["H" + (i + 1).ToString()].Value = TranslateRule(sr.ReadToEnd());
                        }
                        int index = 0;
                        rules = Helper.SortRules(rules);
                        foreach (string s in rules)
                        {
                            ws.Cells[i + 2, 9 + index].Value = s;
                            ws.Column(9 + index).Width = 50;
                            index++;
                        }
                        //ws.Cells[i + 2, 9].Value = trule;
                        //ws.Column(9).Width = 50;
                    }
                    //                            else
                    //                              ws.Cells["I" + (i+1).ToString()].Value = "";



                    ws.Column(8).Width = 25;
                    /*                                        if (i > 0 && ws.Cells["B" + (i + 2).ToString()].Value.ToString() == ws.Cells["B" + (i + 1).ToString()].Value.ToString())
                                                            {
                                                                if (ws.Cells["B" + (i + 2).ToString()].Value.ToString() != lastpartnum)
                                                                {
                                                                    lastpartnum = ws.Cells["B" + (i + 2).ToString()].Value.ToString();
                                                                    if (oddeven == 1)
                                                                        oddeven = 2;
                                                                    else
                                                                        oddeven = 1;
                                                                }

                                                                ws.Row(i + 1).Hidden = false;
                                                                ws.Row(i + 2).Hidden = false;
                                                                if (oddeven == 1)
                                                                {
                                                                    ws.Row(i + 1).Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                                    ws.Row(i + 1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                                                                    ws.Row(i + 2).Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                                    ws.Row(i + 2).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                                                                }
                                                                else
                                                                {
                                                                    ws.Row(i + 1).Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                                    ws.Row(i + 1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCyan);
                                                                    ws.Row(i + 2).Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                                    ws.Row(i + 2).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCyan);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                ws.Row(i + 2).Hidden = true;
                                                            }*/
                    ws.Row(i + 2).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Row(i + 2).Style.Border.Top.Style = ExcelBorderStyle.Thin;


                }
                ws.Column(1).AutoFit();
                ws.Column(2).AutoFit();
                ws.Column(3).AutoFit();
                ws.Column(4).AutoFit();
                ws.Column(5).AutoFit();
                ws.Column(6).AutoFit();
                ws.Column(7).AutoFit();
                pck.Save();
            }
        }

    }

}
