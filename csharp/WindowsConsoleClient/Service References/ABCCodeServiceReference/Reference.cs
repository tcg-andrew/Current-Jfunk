﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WindowsConsoleClient.ABCCodeServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="abccodegetresult", Namespace="http://services.it.tcg/epicor/abccodeservice")]
    [System.SerializableAttribute()]
    public partial class abccodegetresult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private WindowsConsoleClient.ABCCodeServiceReference.abccode[] epicorField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string exceptionField;
        
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
        public WindowsConsoleClient.ABCCodeServiceReference.abccode[] epicor {
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
    [System.Runtime.Serialization.DataContractAttribute(Name="abccode", Namespace="http://schemas.datacontract.org/2004/07/ServiceLibrary")]
    [System.SerializableAttribute()]
    public partial class abccode : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string codeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int freqField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal mincostField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal minvolField;
        
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
        public string code {
            get {
                return this.codeField;
            }
            set {
                if ((object.ReferenceEquals(this.codeField, value) != true)) {
                    this.codeField = value;
                    this.RaisePropertyChanged("code");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int freq {
            get {
                return this.freqField;
            }
            set {
                if ((this.freqField.Equals(value) != true)) {
                    this.freqField = value;
                    this.RaisePropertyChanged("freq");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal mincost {
            get {
                return this.mincostField;
            }
            set {
                if ((this.mincostField.Equals(value) != true)) {
                    this.mincostField = value;
                    this.RaisePropertyChanged("mincost");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal minvol {
            get {
                return this.minvolField;
            }
            set {
                if ((this.minvolField.Equals(value) != true)) {
                    this.minvolField = value;
                    this.RaisePropertyChanged("minvol");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="abccodeupdaterequest", Namespace="http://services.it.tcg/epicor/abccodeservice")]
    [System.SerializableAttribute()]
    public partial class abccodeupdaterequest : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private WindowsConsoleClient.ABCCodeServiceReference.abccode[] epicorField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string passwordField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string usernameField;
        
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
        public WindowsConsoleClient.ABCCodeServiceReference.abccode[] epicor {
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
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string password {
            get {
                return this.passwordField;
            }
            set {
                if ((object.ReferenceEquals(this.passwordField, value) != true)) {
                    this.passwordField = value;
                    this.RaisePropertyChanged("password");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string username {
            get {
                return this.usernameField;
            }
            set {
                if ((object.ReferenceEquals(this.usernameField, value) != true)) {
                    this.usernameField = value;
                    this.RaisePropertyChanged("username");
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
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://services.it.tcg/epicor/abccodeservice", ConfigurationName="ABCCodeServiceReference.IABCCodeService")]
    public interface IABCCodeService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/abccodeservice/IABCCodeService/getallabccodes", ReplyAction="http://services.it.tcg/epicor/abccodeservice/IABCCodeService/getallabccodesRespon" +
            "se")]
        WindowsConsoleClient.ABCCodeServiceReference.abccodegetresult getallabccodes();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/abccodeservice/IABCCodeService/updateabccodes", ReplyAction="http://services.it.tcg/epicor/abccodeservice/IABCCodeService/updateabccodesRespon" +
            "se")]
        string updateabccodes(WindowsConsoleClient.ABCCodeServiceReference.abccodeupdaterequest data);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IABCCodeServiceChannel : WindowsConsoleClient.ABCCodeServiceReference.IABCCodeService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ABCCodeServiceClient : System.ServiceModel.ClientBase<WindowsConsoleClient.ABCCodeServiceReference.IABCCodeService>, WindowsConsoleClient.ABCCodeServiceReference.IABCCodeService {
        
        public ABCCodeServiceClient() {
        }
        
        public ABCCodeServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ABCCodeServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ABCCodeServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ABCCodeServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public WindowsConsoleClient.ABCCodeServiceReference.abccodegetresult getallabccodes() {
            return base.Channel.getallabccodes();
        }
        
        public string updateabccodes(WindowsConsoleClient.ABCCodeServiceReference.abccodeupdaterequest data) {
            return base.Channel.updateabccodes(data);
        }
    }
}
