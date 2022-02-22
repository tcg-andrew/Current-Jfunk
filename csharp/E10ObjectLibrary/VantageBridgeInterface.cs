#region Usings

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

#endregion

namespace ObjectLibrary
{
    public class XmlQuoteMtl
    {
        public string Name { get; set; }
        public Decimal Qty { get; set; }
        public Boolean AsAsm { get; set; }
        public Boolean PhantomBom { get; set; }
        public Decimal Scrap { get; set; }

        public XmlQuoteMtl(string name, Decimal qty)
        {
            Name = name;
            Qty = qty;
            Scrap = 0;
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            XmlQuoteMtl p = (XmlQuoteMtl)obj;
            if (p.Name != Name)
                return false;
            else if (p.Qty != Qty)
                return false;
            return true;
        }
        public override int GetHashCode()
        {
            int q = 0;
            for (int i = 0; i < Name.Length; i++)
            {
                q += Convert.ToInt32(Name[i]);
            }
            return q + Convert.ToInt32(Qty);
        }
    }

    public class XmlQuoteOpr
    {
        public string Name { get; set; }
        public int Seq { get; set; }
        public List<XmlQuoteMtl> Materials {get;set;}

        public XmlQuoteOpr(string name)
        {
            Name = name;
            Materials = new List<XmlQuoteMtl>();
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            XmlQuoteOpr p = (XmlQuoteOpr)obj;
            if (p.Name != Name)
                return false;
            else if (p.Materials.Count != Materials.Count)
                return false;
            else
            {
                for (int i = 0; i < Materials.Count; i++)
                {
                    if (!Materials[i].Equals(p.Materials[i]))
                        return false;
                }
            }
            return true;
        }
        public override int GetHashCode()
        {
            int q = 0;
            for (int i = 0; i < Name.Length; i++)
            {
                q += Convert.ToInt32(Name[i]);
            }
            return q;
        }
    }

    public class XmlQuoteAsm
    {
        public string Name { get; set; }
        public int Qty { get; set; }
        public List<XmlQuoteOpr> Operations { get; set; }
        public string Description { get; set; }
        public int Number05 { get; set; }
        public int Number06 { get; set; }
        public string Shortchar01 { get; set; }
        public List<XmlQuoteMtl> SubAssemblies { get; set; }
        public string NumDoors { get; set; }
        public string Swing { get; set; }

        public int AsmSeq { get; set; }
        public string Template { get; set; }
        public bool AsAsm { get; set; }
        public Dictionary<string, string> Replacements { get; set; }

        public XmlQuoteAsm(string name)
        {
            Name = name;
            Qty = 1;
            Operations = new List<XmlQuoteOpr>();
            SubAssemblies = new List<XmlQuoteMtl>();
            Template = "";
            NumDoors = "NA";
            Swing = "NA";
            Description = "Missing kit assembly";
            Replacements = new Dictionary<string, string>();
        }

        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            XmlQuoteAsm p = (XmlQuoteAsm)obj;
            if (p.Name != Name)
                return false;
            else if (p.Operations.Count != Operations.Count)
                return false;
            else
            {
                for (int i = 0; i < Operations.Count; i++)
                {
                    if (!Operations[i].Equals(p.Operations[i]))
                        return false;
                }
            }
            return true;
            
        }
        public override int GetHashCode()
        {
            int q = 0;
            for (int i = 0; i < Name.Length; i++)
            {
                q += Convert.ToInt32(Name[i]);
            }
            return q + Qty;
        }

        public void ParseReplacements(string data)
        {
            try
            {
                foreach (string str in data.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] split = str.Split(new char[] { ':' });
                    Replacements.Add(split[0], split[1]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("XmlQuoteAsm.ParseReplacements: " + data + " = " + ex.Message);
            }
        }
    }

    public class XmlQuoteLine
    {
        public string PartNum { get; set; }
        public int Qty { get; set; }
        public string Template { get; set; }
        public string Description { get; set; }

        public XmlQuoteLine(string part, int qty, string template, string desc = "")
        {
            PartNum = part;
            Qty = qty;
            Template = template;
            Description = desc;
        }

        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            XmlQuoteLine l = (XmlQuoteLine)obj;
            if (l.PartNum != PartNum)
                return false;
            if (l.Template != Template)
                return false;

            return l.PartNum == PartNum && l.Template == Template;
        }
    }


    public class XmlQuoteParse
    {
        public string System { get; set; }
        public string Temperature { get; set; }
        public string Color { get; set; }
        public string Texture { get; set; }
        public string Lights { get; set; }
        public string Locks { get; set; }
        public string Handle { get; set; }

        public string ProdCode { get; set; }
        public List<XmlQuoteAsm> Assemblies { get; set; }
        public Dictionary<string, int> Lines { get; set; }
        public List<XmlQuoteLine> _Lines { get; set; }
        public Decimal Weight { get; set; }
        public Decimal ShelfPrice { get; set; }
        public string Replace { get; set; }
        public string Description { get; set; }
        public string Character01 { get; set; }
        public string Character03 { get; set; }
        public string Character05 { get; set; }


        public XmlQuoteParse(XmlDocument doc)
        {
            Assemblies = new List<XmlQuoteAsm>();
            Lines = new Dictionary<string, int>();
            _Lines = new List<XmlQuoteLine>();

            foreach (XmlNode node in doc.ChildNodes[1].ChildNodes)
            {
                ParseNode(node, null, null);
            }

        }

        private void ParseNode(XmlNode node, XmlQuoteAsm asm, XmlQuoteOpr opr)
        {
            string name = node.Name;
            if (node.Name.Substring(0, 1) == "X")
            {
                name = node.Name.Substring(1);
            }
            if (name == "prodgroup")
                ProdCode = node.InnerText;
            else if (name == "weight")
                Weight = Decimal.Parse(node.InnerText);
            else if (name == "asmdesc")
            {
                if (asm != null)
                    asm.Description = node.InnerText;
            }
            else if (name == "description")
                Description = node.InnerText;
            else if (name == "ShelfPrice")
                ShelfPrice = Decimal.Parse(node.InnerText);
            else if (name == "number05")
                asm.Number05 = Int32.Parse(node.InnerText);
            else if (name == "number06")
                asm.Number06 = Int32.Parse(node.InnerText);
            else if (name == "shortchar01")
                asm.Shortchar01 = node.InnerText;
            else if (name == "character01")
                Character01 = node.InnerText;
            else if (name == "character03")
                Character03 = node.InnerText;
            else if (name == "character05")
                Character05 = node.InnerText;
            else if (name == "system")
                System = node.InnerText;
            else if (name == "temperature")
                Temperature = node.InnerText;
            else if (name == "color")
                Color = node.InnerText;
            else if (name == "numdoors")
                asm.NumDoors = node.InnerText;
            else if (name == "swing")
                asm.Swing = node.InnerText;
            else if (name == "texture")
                Texture = node.InnerText;
            else if (name == "lights")
                Lights = node.InnerText;
            else if (name == "locks")
                Locks = node.InnerText;
            else if (name == "handle")
                Handle = node.InnerText;
            else if (name == "template")
                asm.Template = node.InnerText;
            else if (name == "asasm")
                asm.AsAsm = node.InnerText == "1";
            else if (name == "replace")
                asm.ParseReplacements(node.InnerText);
            else if (name == "value")
                asm.Qty = Int32.Parse(node.InnerText);
            else if (name.StartsWith("LINES-"))
            {
                foreach (XmlNode n in node.ChildNodes)
                {
                    XmlQuoteLine newline = new XmlQuoteLine(n.Name.Substring(1), (int)Decimal.Parse(n.InnerText), "SHELF");
                    int indexOf = _Lines.IndexOf(newline);
                    if (indexOf >= 0)
                        _Lines[indexOf].Qty += newline.Qty;
                    else
                        _Lines.Add(newline);

                    if (!Lines.ContainsKey(n.Name.Substring(1)))
                        Lines[n.Name.Substring(1)] = (int)Decimal.Parse(n.InnerText);
                    else
                        Lines[n.Name.Substring(1)] += (int)Decimal.Parse(n.InnerText);
                }
            }
            else if (name.StartsWith("ASM-"))
            {
                XmlQuoteAsm newasm = new XmlQuoteAsm(name.Substring(4));
                foreach (XmlNode n in node.ChildNodes)
                {
                    ParseNode(n, newasm, null);
                }

                if (newasm.Template == "FX")
                {
                    XmlQuoteLine newline = new XmlQuoteLine(newasm.Shortchar01, 1, "FX", newasm.Description);

                    int indexOf = _Lines.IndexOf(newline);
                    if (indexOf >= 0)
                        _Lines[indexOf].Qty += 1;
                    else
                        _Lines.Add(newline);
                }
                if (newasm.Template == "RM")
                {
                    XmlQuoteLine newline = new XmlQuoteLine(name.Substring(4), 1, "RM");
                    int indexOf = _Lines.IndexOf(newline);
                    if (indexOf >= 0)
                        _Lines[indexOf].Qty += 1;
                    else
                        _Lines.Add(newline);
                }
                {
                    int indexOf = Assemblies.IndexOf(newasm);
                    if (indexOf >= 0)
                        Assemblies[indexOf].Qty += newasm.Qty;
                    else
                        Assemblies.Add(newasm);
                }
            }
            else if (name.StartsWith("OPR-"))
            {
                if (asm != null)
                {
                    XmlQuoteOpr newopr = new XmlQuoteOpr(name.Substring(4));
                    asm.Operations.Add(newopr);
                    foreach (XmlNode n in node.ChildNodes)
                        ParseNode(n, asm, newopr);
                }
            }
            else if (asm != null && (opr != null || asm.Template == "TC"))
            {
                name = name.Replace("-first", "").Replace("-second", "").Replace("-third", "").Replace("-fourth", "").Replace("-fifth", "");
                if (node.ChildNodes.Count == 1)
                {
                    XmlQuoteMtl newmtl = new XmlQuoteMtl(name, Decimal.Parse(node.InnerText));
                    if (asm.Template == "TC")
                        asm.SubAssemblies.Add(newmtl);
                    else
                        opr.Materials.Add(newmtl);
                }
                else
                {
                    XmlQuoteMtl newmtl = new XmlQuoteMtl(name, 0);
                    foreach (XmlNode n in node.ChildNodes)
                    {
                        if (n.Name == "value")
                            newmtl.Qty = Decimal.Parse(n.InnerText);
                        else if (n.Name == "asasm")
                            newmtl.AsAsm = n.InnerText == "1";
                        else if (n.Name == "phantombom")
                            newmtl.PhantomBom = n.InnerText == "1";
                        else if (n.Name == "scrap")
                            newmtl.Scrap = Decimal.Parse(n.InnerText);
                    }

                    if (newmtl.PhantomBom || !newmtl.AsAsm)
                    {
                        opr.Materials.Add(newmtl);
                    }
                    else
                    {
                        int indexOf = asm.SubAssemblies.IndexOf(newmtl);
                        if (indexOf >= 0)
                            asm.SubAssemblies[indexOf].Qty += newmtl.Qty;
                        else
                            asm.SubAssemblies.Add(newmtl);
                    }
                }
            }
            else
            {
                foreach (XmlNode n in node.ChildNodes)
                    ParseNode(n, asm, opr);
            }
        }
    }

    #region Public Class QuoteLine
    public class QuoteLine
    {
        #region Properties

        public string PartNum { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public bool IsConfig { get; set; }
        public bool IsFauxConfig { get; set; }
        public Dictionary<string, string> ConfigValues { get; set; }
        public decimal Price { get; set; }
        public decimal PriceAdj { get; set; }
        public string PriceAdj_Code { get; set; }
        public string PriceAdj_Desc { get; set; }
        public string QuoteComment { get; set; }
        public string JobComment { get; set; }
        public decimal Discount { get; set; }
        public QuoteLine parentLine { get; set; }
        public int lineNum { get; set; }
        public bool Process { get; set; }
//        public bool MatrixPriced { get; set; }
        public bool forceprice { get; set; }
        public bool shelfprice { get; set; }
        public string ModlinePartNum { get; set; }
        public int SystemWeight { get; set; }
        public int RailsID { get; set; }
        public bool FirstModline { get; set; }
        public Dictionary<string, Dictionary<string, decimal>> MOM { get; set; }
        public bool custom_mom = false;
        public XmlQuoteParse xml { get; set; }
        public string Template { get; set; }

        public string ActualPartNum
        {
            get
            {
                return (PartNum == "MISCPART" && !String.IsNullOrEmpty(ModlinePartNum) ? ModlinePartNum : PartNum);
            }
        }
        #endregion

        #region Constructors

        public QuoteLine()
        {
            ConfigValues = new Dictionary<string, string>();
            Process = true;
            MOM = new Dictionary<string, Dictionary<string, decimal>>();
        }

        public QuoteLine(string partnum, int qty, string description, decimal discount, string q_comment, string j_comment, decimal price, decimal priceadj, string priceadj_code, string priceadj_desc, string nco, DateTime created_at, string modline, string sys_weight, int linenum, int railsid, string template, bool firstmod = false, bool custom = false, XmlQuoteParse wbcresult = null, bool force_price = false, bool shelf_price = false/*, Boolean matrix_price*/)
        {
            MOM = new Dictionary<string, Dictionary<string, decimal>>();
            custom_mom = custom;
            xml = wbcresult;

            forceprice = force_price;
            shelfprice = shelf_price;
            ConfigValues = new Dictionary<string, string>();
            PartNum = partnum;
            Quantity = qty;
            Description = description;
            Discount = discount;
            QuoteComment = q_comment;
            if (!String.IsNullOrEmpty(nco))
                QuoteComment = "NCO: " + nco + "\n" + QuoteComment;
            JobComment = j_comment;
            Price = price;
            PriceAdj = priceadj;
            PriceAdj_Code = priceadj_code;
            PriceAdj_Desc = priceadj_desc;
            ModlinePartNum = modline;
            int weight = 0;
            Int32.TryParse(sys_weight, out weight);
            SystemWeight = weight;
            Process = true;
            lineNum = linenum;
            RailsID = railsid;
            Template = template;
//            MatrixPriced = matrix_price;

            IsConfig = PartNum == "SYSTEM" || PartNum == "SHELVING"; 
            IsFauxConfig = PartNum.StartsWith("00W") || PartNum.StartsWith("00H") || PartNum.StartsWith("01H") || PartNum.StartsWith("02H") || PartNum.StartsWith("03H") || partnum.StartsWith("04H");

            if (PriceAdj_Code == "MATRIX" || PartNum == "SHIP" || partnum == "QUICKSHIP" || partnum == "SHIPLFTGT" || (PartNum.StartsWith("SHIP") && Price == 0))
            {
                forceprice = true;
                PriceAdj = price + priceadj;
                Price = PriceAdj;
            }
            /*else if (custom)
            {
                forceprice = true;
                PriceAdj = price + priceadj;
            }*/
            else if (PartNum == "MISCPART" && !String.IsNullOrEmpty(modline))
            {
                forceprice = true;
                lineNum = 0;
                if (firstmod)
                    PriceAdj = (price / qty) + priceadj;
                else
                    PriceAdj = 0;
            }
            else if (((created_at.Year < 2014 || (created_at.Year == 2014 && (created_at.Month < 6 || (created_at.Month == 6 && created_at.Day <= 8)))) || (created_at.Year == 2016 && (created_at.Month < 5 || (created_at.Month == 5 && created_at.Day <= 15)))) && created_at.AddDays(60) >= DateTime.Now)
            {
                forceprice = true;
                PriceAdj = price;
            }
            //        PriceAdj = (price + priceadj) / ((100 - discount) / 100);
            //        PriceAdj = (price / ((100 - discount) / 100) + priceadj);
        }

        #endregion

        #region Private Methods

        private string TranslateBacketColorID(string id)
        {
            switch (id)
            {
                case "1": return "GALVANIZED";
                case "2": return "BLACK";
                default: return "";
            }
        }
        private string TranslateTypeID(string id)
        {
            switch (id)
            {
                case "77": return "DOOR/FRAME SYSTEM";
                case "78": return "DOOR ONLY";
                case "79": return "FRAME ONLY";
                case "80": return "S//D PLUS";
                case "84": return "R//M PLUS";
                default: throw new Exception("Undefined System ID");
            }
        }

        private string TranslateSizeID(string id)
        {
            switch (id)
            {
                case "68": return "G1";
                case "69": return "G2";
                case "70": return "G3";
                case "62": return "K1";
                case "64": return "K2";
                case "67": return "K3";
                case "65": return "S2";
                case "63": return "S3";
                case "66": return "S5";
                case "73": return "D1";
                case "74": return "D2";
                case "71": return "D3";
                case "85": return "D5";
                case "75": return "R1";
                case "76": return "R2";
                case "72": return "R3";
                case "86": return "R5";
                default: throw new Exception("Undefined DoorSize ID");
            }
        }

        private string TranslateConstructionID(string id)
        {
            switch (id)
            {
                case "17": return "CLASSIC PLUS (2 pane E-coat gas-filled non-heated)";
                case "18": return "20//20 PLUS (2 pane E-coat gas-filled heated)";
                case "27": return "CLASSIC PLUS FREEZER (3 pane E-coat heated)";
                case "22": return "CLASSIC NT (2 pane unheated)";
                case "23": return "CLASSIC 20//20 (2 pane heated)";
                case "91": return "CLASSIC LT (3 pane heated)";
                case "19": return "HYBRIDOOR (2 pane E-coat gas-filled non-heated)";
                case "28": return "HYBRIDOOR FREEZER (3 pane E-coat heated)";
                case "20": return "S//E PLUS (2 pane E-coat gas-filled non-heated)";
                case "24": return "S//E NT (2 pane unheated)";
                case "92": return "S//E LT (3 pane heated)";
                case "93": return "S//E NT (2 pane E-coat unheated)";
                case "26": return "CAL-20 (3 pane E-coat unheated)";
                default: throw new Exception("Undefined Construction ID");
            }
        }

        private string TranslateColorID(string id)
        {
            switch (id)
            {
                case "1": return "SATIN SILVER";
                case "6": return "SMOOTH SATIN SILVER";
                case "2": return "SATIN BLACK";
                case "7": return "SMOOTH SATIN BLACK";
                case "3": return "BRIGHT SILVER";
                case "8": return "SMOOTH BRIGHT SILVER";
                case "5": return "BRIGHT BLACK";
                case "9": return "SMOOTH BRIGHT BLACK";
                case "4": return "BRIGHT GOLD";
                case "94": return "CHERRY TOMATO";
                case "95": return "ANTIQUE SILVER";
                case "96": return "GRANITE";
                case "97": return "FOREST GREEN";
                case "90": return "STANDARD";
                default: throw new Exception("Undefined Color ID");
            }
        }

        private string TranslateLightID(string id)
        {
            switch (id)
            {
                case "36": return "STANDARD T-8";
                case "37": return "LED";
                case "38": return "ALS";
                case "39": return "RECESSED ALS";
                case "35": return "NONE";
                default: throw new Exception("Undefined Light ID");
            }
        }

        private string TranslateLEDID(string id)
        {
            switch (id)
            {
                case "98": return "STANDARD";
                case "99": return "GEN 3";
                case "100": return "HIGH OUTPUT";
                case "101": return "ELECKRA";
                case "102": return "ELECKRA";
                case "109": return "SLOAN";
                case "115": return "SLOAN";
                case "113": return "USLED";
                case "114": return "LED80";
                case "116": return "LED80";
                default: throw new Exception("Undefined LED ID");
            }
        }

        private string TranslateHandleID(string id)
        {
            switch (id)
            {
                case "32": return "STANDARD";
                case "33": return "FULL-LENGTH";
                case "34": return "NONE";
                default: throw new Exception("Undefined Handle ID");
            }
        }

        private string TranslateShelvingID(string id)
        {
            try
            {
                return TranslateShelfType(id);
            }
            catch (Exception)
            {
                throw new Exception("Undefined Shelf Type ID");
            }
        }

        private string TranslateShelfColorID(string id)
        {
            try
            {
                return TranslateShelfColor(id);
            }
            catch (Exception)
            {
                throw new Exception("Undefined Shelf Color ID");
            }
        }

        private string TranslatePostColorID(string id)
        {
            try
            {
                return TranslateShelfPostColor(id);
            }
            catch (Exception)
            {
                throw new Exception("Undefined Shelf Post Color ID");
            }
        }

        private static string TranslateShelfType(string id)
        {
            switch (id)
            {
                case "0": return "";
                case "89": return "NONE";
                case "55": return "27 IN DEEP STANDARD";
                case "56": return "36 IN DEEP SLIDE-TRAC";
                case "57": return "36 IN DEEP SUPER SLIDE-TRAC";
                case "58": return "43 IN DEEP SUPER SLIDE-TRAC";
                case "59": return "60 IN DEEP SUPER SLIDE-TRAC";
                default: throw new Exception("Undefined");
            }
        }

        private static string TranslateShelfColor(string id)
        {
            switch (id)
            {
                case "0": return "";
                case "45": return "WHITE";
                case "46": return "BLACK";
                case "87": return "NONE";
                default: throw new Exception("Undefined");
            }
        }

        private static string TranslateShelfPostColor(string id)
        {
            switch (id)
            {
                case "0": return "";
                case "88": return "NONE";
                case "42": return "GALVANIZED";
                case "43": return "WHITE";
                case "44": return "BLACK";
                default: throw new Exception("Undefined");
            }
        }

        #endregion

        #region Public Methods

        public void PopulateSystemConfigValues(string type_id, int num_doors, string size_id, string construction_id, string finish_id, string light_id, string led_id, string handle_id, string inlay_color, string shelf_type_id, string shelf_color_id, string post_color_id, string hinge01,
            string hinge02, string hinge03, string hinge04, string hinge05, string hinge06, string hinge07, string hinge08, string hinge09, string hinge10, string hinge11, string hinge12, string hinge13, string hinge14, string hinge15, string hinge16,
            string hinge17, string hinge18, string hinge19, string hinge20, string lock01, string lock02, string lock03, string lock04, string lock05, string lock06, string lock07, string lock08, string lock09, string lock10, string lock11, string lock12,
            string lock13, string lock14, string lock15, string lock16, string lock17, string lock18, string lock19, string lock20, string removepushbar, string singleendlight, string silkscreencolor, string kickplate_front, string kickplate_back,
            string bmprgrdqty_front, string bmprgrdqty_back, string bmprgrdloc_front, string bmprgrdloc_back, string flangeless, string bracket_color_id, string single_locks)
        {
            ConfigValues.Clear();
            if (num_doors == 0)
                num_doors = 1;
            ConfigValues.Add("P01_CMB_SYSTEMSELECTION", TranslateTypeID(type_id));
            ConfigValues.Add("P01_CMB_SYSDOORNUM", String.Format("{0:00}", (num_doors == 0 ? 1 : num_doors)));
            ConfigValues.Add("P01_CMB_SYSDOORSIZE", TranslateSizeID(size_id));
            ConfigValues.Add("P01_CMB_SYSDOORCONSTRUCT", TranslateConstructionID(construction_id));
            ConfigValues.Add("P01_CMB_SYSCOLOR", TranslateColorID(finish_id));
            ConfigValues.Add("P01_CMB_SYSLIGHTTYPE", TranslateLightID(light_id));
            ConfigValues.Add("P01_CHK_GEN3", led_id != "" && TranslateLEDID(led_id) == "GEN 3" ? "yes" : "no");
            ConfigValues.Add("P01_CHK_HIGHOUTPUTLED", led_id != "" && TranslateLEDID(led_id) == "HIGH OUTPUT" ? "yes" : "no");
            ConfigValues.Add("P01_CHK_ELECKRA", led_id != "" && TranslateLEDID(led_id) == "ELECKRA" ? "yes" : "no" );
            ConfigValues.Add("P01_CHK_SLOAN", led_id != "" && TranslateLEDID(led_id) == "SLOAN" ? "yes" : "no");
            ConfigValues.Add("P01_CHK_USLED", led_id != "" && TranslateLEDID(led_id) == "USLED" ? "yes" : "no");
            ConfigValues.Add("P01_CHK_LED80", led_id != "" && TranslateLEDID(led_id) == "LED80" ? "yes" : "no");
            ConfigValues.Add("P01_CHK_MOTIONSENSOR", led_id != "" && (led_id == "102" || led_id == "115" || led_id == "116") ? "yes" : "no");
            ConfigValues.Add("P01_CMB_BRACKETCOLOR", TranslateBacketColorID(bracket_color_id));
            ConfigValues.Add("P01_CMB_SYSHANDLETYPE", TranslateHandleID(handle_id));
            ConfigValues.Add("P01_CMB_SYSINLAY", inlay_color == "" ? "NONE" : inlay_color.ToUpper());
            ConfigValues.Add("P01_CMB_SHELVING", TranslateShelvingID(shelf_type_id));
            ConfigValues.Add("P01_CMB_SHELFCOLOR", TranslateShelfColorID(shelf_color_id));
            ConfigValues.Add("P01_CMB_SHELFPOSTCOLOR", TranslatePostColorID(post_color_id));
            ConfigValues.Add("P01_CHK_PUSHBAR", removepushbar.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P01_RAD_SYSFLANGE", flangeless.ToLower() == "true" ? "2" : "1");
            ConfigValues.Add("P01_CHK_CHGTOSINGENDLGHT", singleendlight.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P01_CHK_FULLSILKSCREEN", silkscreencolor == "Black" || silkscreencolor == "White" ? "yes" : "no");
            ConfigValues.Add("P01_CMB_FULLSCREENCOLOR", silkscreencolor == "Black" || silkscreencolor == "White" ? silkscreencolor.ToUpper() : "");
            string kickplate = kickplate_front.Length > 0 && kickplate_front != "None" ? kickplate_front : kickplate_back;
            ConfigValues.Add("P01_CMB_KICKPLATES", kickplate == "Regular" ? "STANDARD" : kickplate == "12 Inch" ? "12 IN" : kickplate.Length > 0 ? kickplate.ToUpper() : "");
            ConfigValues.Add("P01_CHK_FRONTKICKPLATES", kickplate_front.Length > 0 && kickplate_front != "None" ? "yes" : "no");
            ConfigValues.Add("P01_CHK_BACKKICKPLATES", kickplate_back.Length > 0 && kickplate_back != "None" ? "yes" : "no");
            ConfigValues.Add("P01_CHK_FRONTBUMPERGRD", bmprgrdqty_front.Length > 0 && Int32.Parse(bmprgrdqty_front) > 0 ? "yes" : "no");
            ConfigValues.Add("P01_CHK_BACKBUMPERGUARD", bmprgrdqty_back.Length > 0 && Int32.Parse(bmprgrdqty_back) > 0 ? "yes" : "no");
            ConfigValues.Add("P01_DEC_FRONTBMPGRD", bmprgrdqty_front.Length > 0 && Int32.Parse(bmprgrdqty_front) > 0 ? bmprgrdqty_front : "0");
            ConfigValues.Add("P01_DEC_BACKBMPGRDS", bmprgrdqty_back.Length > 0 && Int32.Parse(bmprgrdqty_back) > 0 ? bmprgrdqty_back : "0");
            ConfigValues.Add("P01_TXT_FRONTBMPGRDLOC", bmprgrdloc_front);
            ConfigValues.Add("P01_TXT_BACKBMPGRDLOC", bmprgrdloc_back);
            ConfigValues.Add("P01_CHK_DONE", "yes");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR1", hinge01 == "" || hinge01.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR1", lock01 != "" && lock01.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR2", num_doors < 2 || hinge02 == "" || hinge02.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR2", num_doors >= 2 && lock02 != "" && lock02.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR3", num_doors < 3 || hinge03 == "" || hinge03.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR3", num_doors >= 3 && lock03 != "" && lock03.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR4", num_doors < 4 || hinge04 == "" || hinge04.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR4", num_doors >= 4 && lock04 != "" && lock04.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR5", num_doors < 5 || hinge05 == "" || hinge05.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR5", num_doors >= 5 && lock05 != "" && lock05.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR6", num_doors < 6 || hinge06 == "" || hinge06.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR6", num_doors >= 6 && lock06 != "" && lock06.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR7", num_doors < 7 || hinge07 == "" || hinge07.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR7", num_doors >= 7 && lock07 != "" && lock07.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR8", num_doors < 8 || hinge08 == "" || hinge08.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR8", num_doors >= 8 && lock08 != "" && lock08.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR9", num_doors < 9 || hinge09 == "" || hinge09.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR9", num_doors >= 9 && lock09 != "" && lock09.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR10", num_doors < 10 || hinge10 == "" || hinge10.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR10", num_doors >= 10 && lock10 != "" && lock10.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR11", num_doors < 11 || hinge11 == "" || hinge11.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR11", num_doors >= 11 && lock11 != "" && lock11.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR12", num_doors < 12 || hinge12 == "" || hinge12.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR12", num_doors >= 12 && lock12 != "" && lock12.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR13", num_doors < 13 || hinge13 == "" || hinge13.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR13", num_doors >= 13 && lock13 != "" && lock13.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR14", num_doors < 14 || hinge14 == "" || hinge14.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR14", num_doors >= 14 && lock14 != "" && lock14.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR15", num_doors < 15 || hinge15 == "" || hinge15.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR15", num_doors >= 15 && lock15 != "" && lock15.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR16", num_doors < 16 || hinge16 == "" || hinge16.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR16", num_doors >= 16 && lock16 != "" && lock16.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR17", num_doors < 17 || hinge17 == "" || hinge17.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR17", num_doors >= 17 && lock17 != "" && lock17.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR18", num_doors < 18 || hinge18 == "" || hinge18.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR18", num_doors >= 18 && lock18 != "" && lock18.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR19", num_doors < 19 || hinge19 == "" || hinge19.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR19", num_doors >= 19 && lock19 != "" && lock19.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_RAD_SYSHINGEDOOR20", num_doors < 20 || hinge20 == "" || hinge20.Substring(0, 1).ToUpper() == "L" ? "1" : "2");
            ConfigValues.Add("P02_CHK_SYSLOCKDOOR20", num_doors >= 20 && lock20 != "" && lock20.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_CHK_SINGLELOCK", single_locks.ToLower() == "true" ? "yes" : "no");
            ConfigValues.Add("P02_CHK_DONE", "yes");
        }

        public void PopulateShelfConfigValues(QuoteLine parent, int num_doors, string frame1_shelf_type_id, string frame1_shelf_color_id, string frame1_post_color_id, int frame1shelfqty, int frame1postqty, int frame1lanedivqty, int frame1perimgrdqty, int frame1glideshtqty,
            int frame1pricetagmldqty, int frame1baseqty, int frame1extbracketqty, string frame2_shelf_type_id, string frame2_shelf_color_id, string frame2_post_color_id, int frame2shelfqty, int frame2postqty, int frame2lanedivqty, int frame2perimgrdqty, int frame2glideshtqty,
            int frame2pricetagmldqty, int frame2baseqty, int frame2extbracketqty, string frame3_shelf_type_id, string frame3_shelf_color_id, string frame3_post_color_id, int frame3shelfqty, int frame3postqty, int frame3lanedivqty, int frame3perimgrdqty, int frame3glideshtqty,
            int frame3pricetagmldqty, int frame3baseqty, int frame3extbracketqty, string frame4_shelf_type_id, string frame4_shelf_color_id, string frame4_post_color_id, int frame4shelfqty, int frame4postqty, int frame4lanedivqty, int frame4perimgrdqty, int frame4glideshtqty,
            int frame4pricetagmldqty, int frame4baseqty, int frame4extbracketqty)
        {
            parentLine = parent;
            if (num_doors == 0)
                num_doors = 1;
            ConfigValues.Clear();
            ConfigValues.Add("P01_DEC_QUOTELINE", parent.lineNum.ToString());
            ConfigValues.Add("P01_CHK_POPULATE", "yes");
            ConfigValues.Add("P01_CMB_SHELFTYPE1", TranslateShelvingID(frame1_shelf_type_id));
            ConfigValues.Add("P01_CMB_SHELFCOLOR1", TranslateShelfColorID(frame1_shelf_color_id));
            ConfigValues.Add("P01_CMB_POSTCOLOR1", TranslatePostColorID(frame1_post_color_id));
            ConfigValues.Add("P01_DEC_SHELFQTY1", frame1shelfqty.ToString());
            ConfigValues.Add("P01_DEC_POSTQTY1", frame1postqty.ToString());
            ConfigValues.Add("P01_DEC_LANEDIVQTY1", frame1lanedivqty.ToString());
            ConfigValues.Add("P01_DEC_PERIMGUARDQTY1", frame1perimgrdqty.ToString());
            ConfigValues.Add("P01_DEC_GLIDESHEETQTY1", frame1glideshtqty.ToString());
            ConfigValues.Add("P01_DEC_PTMQTY1", frame1pricetagmldqty.ToString());
            ConfigValues.Add("P01_DEC_BASEQTY1", frame1baseqty.ToString());
            ConfigValues.Add("P01_DEC_EXTBRACKET1", frame1extbracketqty.ToString());
            ConfigValues.Add("P01_CMB_SHELFTYPE2", num_doors < 6 ? "" : TranslateShelvingID(frame2_shelf_type_id));
            ConfigValues.Add("P01_CMB_SHELFCOLOR2", num_doors < 6 ? "" : TranslateShelfColorID(frame2_shelf_color_id));
            ConfigValues.Add("P01_CMB_POSTCOLOR2", num_doors < 6 ? "" : TranslatePostColorID(frame2_post_color_id));
            ConfigValues.Add("P01_DEC_SHELFQTY2", num_doors < 6 ? "0" : frame2shelfqty.ToString());
            ConfigValues.Add("P01_DEC_POSTQTY2", num_doors < 6 ? "0" : frame2postqty.ToString());
            ConfigValues.Add("P01_DEC_LANEDIVQTY2", num_doors < 6 ? "0" : frame2lanedivqty.ToString());
            ConfigValues.Add("P01_DEC_PERIMGUARDQTY2", num_doors < 6 ? "0" : frame2perimgrdqty.ToString());
            ConfigValues.Add("P01_DEC_GLIDESHEETQTY2", num_doors < 6 ? "0" : frame2glideshtqty.ToString());
            ConfigValues.Add("P01_DEC_PTMQTY2", num_doors < 6 ? "0" : frame2pricetagmldqty.ToString());
            ConfigValues.Add("P01_DEC_BASEQTY2", num_doors < 6 ? "0" : frame2baseqty.ToString());
            ConfigValues.Add("P01_DEC_EXTBRACKET2", num_doors < 6 ? "0" : frame2extbracketqty.ToString());
            ConfigValues.Add("P01_CMB_SHELFTYPE3", num_doors < 11 ? "" : TranslateShelvingID(frame3_shelf_type_id));
            ConfigValues.Add("P01_CMB_SHELFCOLOR3", num_doors < 11 ? "" : TranslateShelfColorID(frame3_shelf_color_id));
            ConfigValues.Add("P01_CMB_POSTCOLOR3", num_doors < 11 ? "" : TranslatePostColorID(frame3_post_color_id));
            ConfigValues.Add("P01_DEC_SHELFQTY3", num_doors < 11 ? "0" : frame3shelfqty.ToString());
            ConfigValues.Add("P01_DEC_POSTQTY3", num_doors < 11 ? "0" : frame3postqty.ToString());
            ConfigValues.Add("P01_DEC_LANEDIVQTY3", num_doors < 11 ? "0" : frame3lanedivqty.ToString());
            ConfigValues.Add("P01_DEC_PERIMGUARDQTY3", num_doors < 11 ? "0" : frame3perimgrdqty.ToString());
            ConfigValues.Add("P01_DEC_GLIDESHEETQTY3", num_doors < 11 ? "0" : frame3glideshtqty.ToString());
            ConfigValues.Add("P01_DEC_PTMQTY3", num_doors < 11 ? "0" : frame3pricetagmldqty.ToString());
            ConfigValues.Add("P01_DEC_BASEQTY3", num_doors < 11 ? "0" : frame3baseqty.ToString());
            ConfigValues.Add("P01_DEC_EXTBRACKET3", num_doors < 11 ? "0" : frame3extbracketqty.ToString());
            ConfigValues.Add("P01_CMB_SHELFTYPE4", num_doors < 16 ? "" : TranslateShelvingID(frame4_shelf_type_id));
            ConfigValues.Add("P01_CMB_SHELFCOLOR4", num_doors < 16 ? "" : TranslateShelfColorID(frame4_shelf_color_id));
            ConfigValues.Add("P01_CMB_POSTCOLOR4", num_doors < 16 ? "" : TranslatePostColorID(frame4_post_color_id));
            ConfigValues.Add("P01_DEC_SHELFQTY4", num_doors < 16 ? "0" : frame4shelfqty.ToString());
            ConfigValues.Add("P01_DEC_POSTQTY4", num_doors < 16 ? "0" : frame4postqty.ToString());
            ConfigValues.Add("P01_DEC_LANEDIVQTY4", num_doors < 16 ? "0" : frame4lanedivqty.ToString());
            ConfigValues.Add("P01_DEC_PERIMGUARDQTY4", num_doors < 16 ? "0" : frame4perimgrdqty.ToString());
            ConfigValues.Add("P01_DEC_GLIDESHEETQTY4", num_doors < 16 ? "0" : frame4glideshtqty.ToString());
            ConfigValues.Add("P01_DEC_PTMQTY4", num_doors < 16 ? "0" : frame4pricetagmldqty.ToString());
            ConfigValues.Add("P01_DEC_BASEQTY4", num_doors < 16 ? "0" : frame4baseqty.ToString());
            ConfigValues.Add("P01_DEC_EXTBRACKET4", num_doors < 16 ? "0" : frame4extbracketqty.ToString());
            ConfigValues.Add("P01_CHK_DONE", "yes");
        }

        public void UpdateShelfLinenumValue()
        {
            if (ConfigValues.ContainsKey("P01_DEC_QUOTELINE"))
                ConfigValues["P01_DEC_QUOTELINE"] = parentLine.lineNum.ToString();
        }

        #endregion
    }

    #endregion

    public class VantageBridgeInterface
    {
        #region Public Methods

        public int CreateQuote(string username, string password, string railsid, int quotenum, string custid, string shiptonum, string stname, string stadd1, string stadd2, string stadd3, string stcity, string ststate, string stzip, string csr, string comments, List<QuoteLine> lineitems, decimal total_price, string carrier, int transitdays, string stemail, string stemailcc, string salesrepcode, string jobref = "", bool skip_price_validation = false)
        {
            try
            {
                CustomerInterface customerInterface = new CustomerInterface();
                QuoteInterface quoteInterface = new QuoteInterface();
                /// TODO: Verify configuration interface isn't necessary
                //ConfigurationInterface configurationInterface = new ConfigurationInterface();
                QuoteAssemblyInterface quoteAssemblyInterface = new QuoteAssemblyInterface();
                PartInterface partInterface = new PartInterface();
                if (!customerInterface.IsShipToValid(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, custid, shiptonum, stname, stadd1, stadd2, stadd3, stcity, ststate, stzip, stemail, stemailcc))
                    throw new Exception("A Ship To address with this Ship To Num was found, but has different values.  Please enter a unique Ship To Num to create a new Ship To address for this customer, or use the Ship To Search to pick an existing address");
                else
                {
                    if (1 == 0/*quotenum != 0*/)
                    {
              //          quoteInterface.DeleteLineItems(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, quotenum);
//                        quoteInterface.SyncLineData(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), quotenum, lineitems);
                        /// TODO: Resolve reconfiguration methodology
                        //quoteInterface.PrepareReconfig(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), quotenum, lineitems);
                    }
                    else
                        quotenum = quoteInterface.CreateQuote(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, railsid, custid, shiptonum, csr, comments, carrier, transitdays, jobref, salesrepcode);

                    foreach (QuoteLine line in lineitems)
                    {
                        if (line.Process)
                        {
                            if (line.Template != "FX" && !partInterface.PartExists(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, line.ActualPartNum))
                            {
                                throw new Exception("Part # " + line.ActualPartNum + " does not exist.  Notify Engineering");
                            }
                            else
                            {
                                if (line.Template == "FX" && !partInterface.PartExists(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, line.ActualPartNum))
                                {
                                    partInterface.CreatePurchasedPart(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, line.ActualPartNum, line.Description, line.xml.ProdCode, "RDCI", "SFLMAINBIN", 360, true, "Lea");
                                    SQLAccess.SendMail(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), "eng@styleline.com; jfsoftwarellc@gmail.com", "New FX Door Created - " + line.ActualPartNum, "The bridge has automatically created purchased part #" + line.ActualPartNum + ".  Part created with prodcode " + line.xml.ProdCode + ", part class: RGL, supplier: 9310, backflush: true, buyer: Lea.  Please contact Justin Funk with any issues.");
                                }

                                NewQuoteLine lineinfo = quoteInterface.CreateLineItem(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, railsid, quotenum, line.ActualPartNum, line.Description, line.Discount, line.QuoteComment, line.JobComment, line.SystemWeight, (line.custom_mom ? line.xml : null));
                                if (lineinfo.forcePrice)
                                {
                                    line.forceprice = true;
                                    line.PriceAdj += line.Price;
                                }
                                line.lineNum = lineinfo.linenum;
                                line.UpdateShelfLinenumValue();

                                //if (line.IsConfig)
                                //    configurationInterface.UpdateConfiguration(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, quotenum, lineinfo.linenum, line.ActualPartNum, lineinfo.revision, line.ConfigValues);

                                quoteInterface.UpdateLineQuote(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, quotenum, lineinfo.linenum);

                                /*if (line.MatrixPriced)
                                        quoteInterface.UpdateLinePrice(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, quotenum, lineinfo.linenum, line.Price);
                                else */if (line.ActualPartNum == "SHIP" || line.ActualPartNum == "QUICKSHIP" || !String.IsNullOrEmpty(line.PriceAdj_Code) || line.forceprice || line.custom_mom || line.shelfprice)
                                {
                                    if (line.shelfprice)
                                    {
                                        quoteInterface.UpdateLinePrice(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, quotenum, lineinfo.linenum, line.Price);
                                        if (line.PriceAdj > 0 && line.PriceAdj_Code != "MATRIX")
                                            quoteInterface.CreateMiscCharge(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, quotenum, lineinfo.linenum, line.PriceAdj, line.PriceAdj_Code, line.PriceAdj_Desc);
                                    }
                                    else
                                    {
                                        if (line.custom_mom)
                                            quoteInterface.UpdateLinePrice(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, quotenum, lineinfo.linenum, line.Price);
                                        if (line.forceprice /*|| line.custom_mom*/)
                                            quoteInterface.UpdateLinePrice(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, quotenum, lineinfo.linenum, line.PriceAdj);
                                        else if (!String.IsNullOrEmpty(line.PriceAdj_Code))
                                            quoteInterface.CreateMiscCharge(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, quotenum, lineinfo.linenum, line.PriceAdj, line.PriceAdj_Code, line.PriceAdj_Desc);
                                    }
                                }

                                if (partInterface.PartIsSalesKit(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, line.ActualPartNum))
                                    quoteInterface.UpdateKitComponents(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, line.ActualPartNum, quotenum, lineinfo);

                                try
                                {
                                    if (partInterface.PartGetsDetails(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, line.ActualPartNum) && line.ActualPartNum != "SHELVING" && !line.custom_mom)
                                    {
                                        if (line.ActualPartNum != "SHELVING" && !line.custom_mom)
                                            lineinfo.revision = partInterface.GetPartInfo(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, line.ActualPartNum).RevNum;
                                        quoteAssemblyInterface.CreateQuoteAsm(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, quotenum, lineinfo.linenum, line.ActualPartNum, lineinfo.revision);
                                    }
                                    else if (line.custom_mom)
                                    {
                                        quoteAssemblyInterface.CreateFromXml(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, quotenum, lineinfo.linenum, line.PartNum, line.xml, ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString());
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (ex.Message != "A valid Source Method is required.")
                                        throw ex;
                                }
                                quoteInterface.UpdateLineQty(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, quotenum, lineinfo.linenum, line.Quantity);
                                quoteInterface.ReadyToQuote(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, quotenum, lineinfo.linenum, true);
                            }
                        }
                    }
/*                    if (!skip_price_validation)
                    {
                        decimal vantage_total = quoteInterface.GetQuoteTotal(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, quotenum);
                        if (total_price != vantage_total && Math.Abs(total_price - vantage_total) > (1 * lineitems.Count))
                            throw new Exception(String.Format("Vantage Quote (#{2}) total (${0}) does not match Web Quote total (${1})", String.Format("{0:0.00}", vantage_total), String.Format("{0:0.00}", total_price), quotenum.ToString()));
                    }*/
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " (QUOTENUM " + quotenum + ")" + ", inner trace = " + ex.StackTrace);
            }
            return quotenum;
        }

        #endregion
    }
}
