//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WindowsConsoleClient.QuoteServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="quotegetresult", Namespace="http://services.it.tcg/epicor/quoteservice")]
    [System.SerializableAttribute()]
    public partial class quotegetresult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string exceptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private WindowsConsoleClient.QuoteServiceReference.quoteinfo[] epicorField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string exception {
            get {
                return this.exceptionField;
            }
            set {
                if ((object.ReferenceEquals(this.exceptionField, value) != true)) {
                    this.exceptionField = value;
                    this.RaisePropertyChanged("exception");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public WindowsConsoleClient.QuoteServiceReference.quoteinfo[] epicor {
            get {
                return this.epicorField;
            }
            set {
                if ((object.ReferenceEquals(this.epicorField, value) != true)) {
                    this.epicorField = value;
                    this.RaisePropertyChanged("epicor");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="quoteinfo", Namespace="http://schemas.datacontract.org/2004/07/ServiceLibrary")]
    [System.SerializableAttribute()]
    public partial class quoteinfo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string descriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string priceField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string description {
            get {
                return this.descriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.descriptionField, value) != true)) {
                    this.descriptionField = value;
                    this.RaisePropertyChanged("description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string price {
            get {
                return this.priceField;
            }
            set {
                if ((object.ReferenceEquals(this.priceField, value) != true)) {
                    this.priceField = value;
                    this.RaisePropertyChanged("price");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://services.it.tcg/epicor/quoteservice", ConfigurationName="QuoteServiceReference.IQuoteService")]
    public interface IQuoteService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/quoteservice/IQuoteService/getquoteinfo", ReplyAction="http://services.it.tcg/epicor/quoteservice/IQuoteService/getquoteinfoResponse")]
        WindowsConsoleClient.QuoteServiceReference.quotegetresult getquoteinfo(
                    string username, 
                    string password, 
                    int custid, 
                    string partnum, 
                    string revision, 
                    string shelvingpartnum, 
                    string shelvingrevision, 
                    bool isconfigured, 
                    bool needsdetails, 
                    int systemid, 
                    int numdoors, 
                    int doorsizeid, 
                    int constructionid, 
                    int colorid, 
                    int lightid, 
                    int handleid, 
                    int shelvingid, 
                    int shelfcolorid, 
                    int shelfpostcolorid, 
                    int frame1shelftypeid, 
                    int frame1shelfcolorid, 
                    int frame1postcolorid, 
                    int frame1shelfqty, 
                    int frame1postqty, 
                    int frame1lanedivqty, 
                    int frame1perimguardqty, 
                    int frame1glidesheetqty, 
                    int frame1ptmqty, 
                    int frame1baseqty, 
                    int frame1extbracketqty, 
                    int frame2shelftypeid, 
                    int frame2shelfcolorid, 
                    int frame2postcolorid, 
                    int frame2shelfqty, 
                    int frame2postqty, 
                    int frame2lanedivqty, 
                    int frame2perimguardqty, 
                    int frame2glidesheetqty, 
                    int frame2ptmqty, 
                    int frame2baseqty, 
                    int frame2extbracketqty, 
                    int frame3shelftypeid, 
                    int frame3shelfcolorid, 
                    int frame3postcolorid, 
                    int frame3shelfqty, 
                    int frame3postqty, 
                    int frame3lanedivqty, 
                    int frame3perimguardqty, 
                    int frame3glidesheetqty, 
                    int frame3ptmqty, 
                    int frame3baseqty, 
                    int frame3extbracketqty, 
                    int frame4shelftypeid, 
                    int frame4shelfcolorid, 
                    int frame4postcolorid, 
                    int frame4shelfqty, 
                    int frame4postqty, 
                    int frame4lanedivqty, 
                    int frame4perimguardqty, 
                    int frame4glidesheetqty, 
                    int frame4ptmqty, 
                    int frame4baseqty, 
                    int frame4extbracketqty);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IQuoteServiceChannel : WindowsConsoleClient.QuoteServiceReference.IQuoteService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class QuoteServiceClient : System.ServiceModel.ClientBase<WindowsConsoleClient.QuoteServiceReference.IQuoteService>, WindowsConsoleClient.QuoteServiceReference.IQuoteService {
        
        public QuoteServiceClient() {
        }
        
        public QuoteServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public QuoteServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public QuoteServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public QuoteServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public WindowsConsoleClient.QuoteServiceReference.quotegetresult getquoteinfo(
                    string username, 
                    string password, 
                    int custid, 
                    string partnum, 
                    string revision, 
                    string shelvingpartnum, 
                    string shelvingrevision, 
                    bool isconfigured, 
                    bool needsdetails, 
                    int systemid, 
                    int numdoors, 
                    int doorsizeid, 
                    int constructionid, 
                    int colorid, 
                    int lightid, 
                    int handleid, 
                    int shelvingid, 
                    int shelfcolorid, 
                    int shelfpostcolorid, 
                    int frame1shelftypeid, 
                    int frame1shelfcolorid, 
                    int frame1postcolorid, 
                    int frame1shelfqty, 
                    int frame1postqty, 
                    int frame1lanedivqty, 
                    int frame1perimguardqty, 
                    int frame1glidesheetqty, 
                    int frame1ptmqty, 
                    int frame1baseqty, 
                    int frame1extbracketqty, 
                    int frame2shelftypeid, 
                    int frame2shelfcolorid, 
                    int frame2postcolorid, 
                    int frame2shelfqty, 
                    int frame2postqty, 
                    int frame2lanedivqty, 
                    int frame2perimguardqty, 
                    int frame2glidesheetqty, 
                    int frame2ptmqty, 
                    int frame2baseqty, 
                    int frame2extbracketqty, 
                    int frame3shelftypeid, 
                    int frame3shelfcolorid, 
                    int frame3postcolorid, 
                    int frame3shelfqty, 
                    int frame3postqty, 
                    int frame3lanedivqty, 
                    int frame3perimguardqty, 
                    int frame3glidesheetqty, 
                    int frame3ptmqty, 
                    int frame3baseqty, 
                    int frame3extbracketqty, 
                    int frame4shelftypeid, 
                    int frame4shelfcolorid, 
                    int frame4postcolorid, 
                    int frame4shelfqty, 
                    int frame4postqty, 
                    int frame4lanedivqty, 
                    int frame4perimguardqty, 
                    int frame4glidesheetqty, 
                    int frame4ptmqty, 
                    int frame4baseqty, 
                    int frame4extbracketqty) {
            return base.Channel.getquoteinfo(username, password, custid, partnum, revision, shelvingpartnum, shelvingrevision, isconfigured, needsdetails, systemid, numdoors, doorsizeid, constructionid, colorid, lightid, handleid, shelvingid, shelfcolorid, shelfpostcolorid, frame1shelftypeid, frame1shelfcolorid, frame1postcolorid, frame1shelfqty, frame1postqty, frame1lanedivqty, frame1perimguardqty, frame1glidesheetqty, frame1ptmqty, frame1baseqty, frame1extbracketqty, frame2shelftypeid, frame2shelfcolorid, frame2postcolorid, frame2shelfqty, frame2postqty, frame2lanedivqty, frame2perimguardqty, frame2glidesheetqty, frame2ptmqty, frame2baseqty, frame2extbracketqty, frame3shelftypeid, frame3shelfcolorid, frame3postcolorid, frame3shelfqty, frame3postqty, frame3lanedivqty, frame3perimguardqty, frame3glidesheetqty, frame3ptmqty, frame3baseqty, frame3extbracketqty, frame4shelftypeid, frame4shelfcolorid, frame4postcolorid, frame4shelfqty, frame4postqty, frame4lanedivqty, frame4perimguardqty, frame4glidesheetqty, frame4ptmqty, frame4baseqty, frame4extbracketqty);
        }
    }
}
