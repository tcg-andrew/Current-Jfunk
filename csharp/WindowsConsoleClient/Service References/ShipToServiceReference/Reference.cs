﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WindowsConsoleClient.ShipToServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="shiptogetresult", Namespace="http://services.it.tcg/epicor/shiptoservice")]
    [System.SerializableAttribute()]
    public partial class shiptogetresult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string exceptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private WindowsConsoleClient.ShipToServiceReference.shipto[] epicorField;
        
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
        public WindowsConsoleClient.ShipToServiceReference.shipto[] epicor {
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
    [System.Runtime.Serialization.DataContractAttribute(Name="shipto", Namespace="http://schemas.datacontract.org/2004/07/ServiceLibrary")]
    [System.SerializableAttribute()]
    public partial class shipto : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string address1Field;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string address2Field;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string address3Field;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string cityField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string countryField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string shiptonumField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string stateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string zipField;
        
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
        public string address1 {
            get {
                return this.address1Field;
            }
            set {
                if ((object.ReferenceEquals(this.address1Field, value) != true)) {
                    this.address1Field = value;
                    this.RaisePropertyChanged("address1");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string address2 {
            get {
                return this.address2Field;
            }
            set {
                if ((object.ReferenceEquals(this.address2Field, value) != true)) {
                    this.address2Field = value;
                    this.RaisePropertyChanged("address2");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string address3 {
            get {
                return this.address3Field;
            }
            set {
                if ((object.ReferenceEquals(this.address3Field, value) != true)) {
                    this.address3Field = value;
                    this.RaisePropertyChanged("address3");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string city {
            get {
                return this.cityField;
            }
            set {
                if ((object.ReferenceEquals(this.cityField, value) != true)) {
                    this.cityField = value;
                    this.RaisePropertyChanged("city");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string country {
            get {
                return this.countryField;
            }
            set {
                if ((object.ReferenceEquals(this.countryField, value) != true)) {
                    this.countryField = value;
                    this.RaisePropertyChanged("country");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string shiptonum {
            get {
                return this.shiptonumField;
            }
            set {
                if ((object.ReferenceEquals(this.shiptonumField, value) != true)) {
                    this.shiptonumField = value;
                    this.RaisePropertyChanged("shiptonum");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string state {
            get {
                return this.stateField;
            }
            set {
                if ((object.ReferenceEquals(this.stateField, value) != true)) {
                    this.stateField = value;
                    this.RaisePropertyChanged("state");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string zip {
            get {
                return this.zipField;
            }
            set {
                if ((object.ReferenceEquals(this.zipField, value) != true)) {
                    this.zipField = value;
                    this.RaisePropertyChanged("zip");
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
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://services.it.tcg/epicor/shiptoservice", ConfigurationName="ShipToServiceReference.IShipToService")]
    public interface IShipToService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/shiptoservice/IShipToService/getshiptobycustid", ReplyAction="http://services.it.tcg/epicor/shiptoservice/IShipToService/getshiptobycustidRespo" +
            "nse")]
        WindowsConsoleClient.ShipToServiceReference.shiptogetresult getshiptobycustid(string custid);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IShipToServiceChannel : WindowsConsoleClient.ShipToServiceReference.IShipToService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ShipToServiceClient : System.ServiceModel.ClientBase<WindowsConsoleClient.ShipToServiceReference.IShipToService>, WindowsConsoleClient.ShipToServiceReference.IShipToService {
        
        public ShipToServiceClient() {
        }
        
        public ShipToServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ShipToServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ShipToServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ShipToServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public WindowsConsoleClient.ShipToServiceReference.shiptogetresult getshiptobycustid(string custid) {
            return base.Channel.getshiptobycustid(custid);
        }
    }
}
