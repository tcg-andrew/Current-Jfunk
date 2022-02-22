#region Usings

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using ObjectLibrary;
using System;

#endregion

namespace ServiceLibrary
{
    #region Service Contracts

    [ServiceContract(Namespace = "http://services.it.tcg/epicor/quoteservice")]
    public interface IQuoteService
    {
        [OperationContract]
        quotegetresult getquoteinfo(string username, string password, string systemid, int numdoors, string doorsizeid, string constructionid, string colorid, string lightid, string handleid, string shelvingid, string shelfcolorid, string shelfpostcolorid,
            string door1hinge, string door1lock, string door2hinge, string door2lock, string door3hinge, string door3lock, string door4hinge, string door4lock, string door5hinge, string door5lock, string door6hinge, string door6lock, string door7hinge, string door7lock,
            string door8hinge, string door8lock, string door9hinge, string door9lock, string door10hinge, string door10lock, string door11hinge, string door11lock, string door12hinge, string door12lock, string door13hinge, string door13lock, string door14hinge,
            string door14lock, string door15hinge, string door15lock, string door16hinge, string door16lock, string door17hinge, string door17lock, string door18hinge, string door18lock, string door19hinge, string door19lock, string door20hinge, string door20lock,
            string frame1shelftypeid, string frame1shelfcolorid, string frame1postcolorid, int frame1shelfqty, int frame1postqty, int frame1lanedivqty, int frame1perimguardqty, int frame1glidesheetqty, int frame1ptmqty, int frame1baseqty, int frame1extbracketqty,
            string frame2shelftypeid, string frame2shelfcolorid, string frame2postcolorid, int frame2shelfqty, int frame2postqty, int frame2lanedivqty, int frame2perimguardqty, int frame2glidesheetqty, int frame2ptmqty, int frame2baseqty, int frame2extbracketqty,
            string frame3shelftypeid, string frame3shelfcolorid, string frame3postcolorid, int frame3shelfqty, int frame3postqty, int frame3lanedivqty, int frame3perimguardqty, int frame3glidesheetqty, int frame3ptmqty, int frame3baseqty, int frame3extbracketqty,
            string frame4shelftypeid, string frame4shelfcolorid, string frame4postcolorid, int frame4shelfqty, int frame4postqty, int frame4lanedivqty, int frame4perimguardqty, int frame4glidesheetqty, int frame4ptmqty, int frame4baseqty, int frame4extbracketqty,
            decimal price, decimal prices, decimal discount, decimal adjamt, string adjcode, decimal adjamts, string adjcodes, string inlay, string pushbar, string silkscreen, string frontkick, string backkick, string fbgqty, string bbgqty, string singleel, string led);
    }

    #endregion

    #region Data Contracts

    [DataContract(Namespace = "http://services.it.tcg/epicor/quoteservice")]
    public class quotegetresult
    {
        [DataMember(Order = 0)]
        public string exception { get; set; }

        [DataMember(Order = 1)]
        public List<quoteinfo> epicor { get; set; }

        public quotegetresult()
        {
            epicor = new List<quoteinfo>();
        }
    }

    #endregion

    #region Data Formats

    public class quoteinfo
    {
        public string description { get; set; }
        public string price { get; set; }
        public Dictionary<string, string> weights { get; set; }

        public quoteinfo()
        {
            description = "";
            price = "";
            weights = new Dictionary<string, string>();
        }

        public quoteinfo(QuoteInfo q)
        {
            description = q.Description;
            price = q.Price;
            weights = new Dictionary<string, string>();
            foreach (string key in q.FreightWeight.Keys)
                weights.Add(key, q.FreightWeight[key]);
        }
    }

    #endregion
}
