//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WindowsConsoleClient.FreightRateServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="freightrategetresult", Namespace="http://services.it.tcg/epicor/freightrateservice")]
    [System.SerializableAttribute()]
    public partial class freightrategetresult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string exceptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private WindowsConsoleClient.FreightRateServiceReference.freightrate[] epicorField;
        
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
        public WindowsConsoleClient.FreightRateServiceReference.freightrate[] epicor {
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
    [System.Runtime.Serialization.DataContractAttribute(Name="freightrate", Namespace="http://schemas.datacontract.org/2004/07/ServiceLibrary")]
    [System.SerializableAttribute()]
    public partial class freightrate : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string carrierField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string jointlineField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal totalcostField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int transitdaysField;
        
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
        public string carrier {
            get {
                return this.carrierField;
            }
            set {
                if ((object.ReferenceEquals(this.carrierField, value) != true)) {
                    this.carrierField = value;
                    this.RaisePropertyChanged("carrier");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string jointline {
            get {
                return this.jointlineField;
            }
            set {
                if ((object.ReferenceEquals(this.jointlineField, value) != true)) {
                    this.jointlineField = value;
                    this.RaisePropertyChanged("jointline");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal totalcost {
            get {
                return this.totalcostField;
            }
            set {
                if ((this.totalcostField.Equals(value) != true)) {
                    this.totalcostField = value;
                    this.RaisePropertyChanged("totalcost");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int transitdays {
            get {
                return this.transitdaysField;
            }
            set {
                if ((this.transitdaysField.Equals(value) != true)) {
                    this.transitdaysField = value;
                    this.RaisePropertyChanged("transitdays");
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
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://services.it.tcg/epicor/partservice", ConfigurationName="FreightRateServiceReference.IFreightRateService")]
    public interface IFreightRateService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/partservice/IFreightRateService/getrate", ReplyAction="http://services.it.tcg/epicor/partservice/IFreightRateService/getrateResponse")]
        WindowsConsoleClient.FreightRateServiceReference.freightrategetresult getrate(string classcode1, string weight1, string classcode2, string weight2, string classcode3, string weight3, string classcode4, string weight4, string classcode5, string weight5, string fromzip, string tozip, int location);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IFreightRateServiceChannel : WindowsConsoleClient.FreightRateServiceReference.IFreightRateService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FreightRateServiceClient : System.ServiceModel.ClientBase<WindowsConsoleClient.FreightRateServiceReference.IFreightRateService>, WindowsConsoleClient.FreightRateServiceReference.IFreightRateService {
        
        public FreightRateServiceClient() {
        }
        
        public FreightRateServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FreightRateServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FreightRateServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FreightRateServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public WindowsConsoleClient.FreightRateServiceReference.freightrategetresult getrate(string classcode1, string weight1, string classcode2, string weight2, string classcode3, string weight3, string classcode4, string weight4, string classcode5, string weight5, string fromzip, string tozip, int location) {
            return base.Channel.getrate(classcode1, weight1, classcode2, weight2, classcode3, weight3, classcode4, weight4, classcode5, weight5, fromzip, tozip, location);
        }
    }
}
