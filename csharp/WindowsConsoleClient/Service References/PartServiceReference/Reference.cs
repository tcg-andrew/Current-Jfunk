﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WindowsConsoleClient.PartServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="partgetallresult", Namespace="http://services.it.tcg/epicor/partservice")]
    [System.SerializableAttribute()]
    public partial class partgetallresult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string exceptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private WindowsConsoleClient.PartServiceReference.part[] epicorField;
        
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
        public WindowsConsoleClient.PartServiceReference.part[] epicor {
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
    [System.Runtime.Serialization.DataContractAttribute(Name="part", Namespace="http://schemas.datacontract.org/2004/07/ServiceLibrary")]
    [System.SerializableAttribute()]
    public partial class part : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string descField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal freightclassField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string groupField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool nonstockField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string partclassField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string partnumField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal priceField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string typeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string unitField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal weightField;
        
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
        public string desc {
            get {
                return this.descField;
            }
            set {
                if ((object.ReferenceEquals(this.descField, value) != true)) {
                    this.descField = value;
                    this.RaisePropertyChanged("desc");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal freightclass {
            get {
                return this.freightclassField;
            }
            set {
                if ((this.freightclassField.Equals(value) != true)) {
                    this.freightclassField = value;
                    this.RaisePropertyChanged("freightclass");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string group {
            get {
                return this.groupField;
            }
            set {
                if ((object.ReferenceEquals(this.groupField, value) != true)) {
                    this.groupField = value;
                    this.RaisePropertyChanged("group");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool nonstock {
            get {
                return this.nonstockField;
            }
            set {
                if ((this.nonstockField.Equals(value) != true)) {
                    this.nonstockField = value;
                    this.RaisePropertyChanged("nonstock");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string partclass {
            get {
                return this.partclassField;
            }
            set {
                if ((object.ReferenceEquals(this.partclassField, value) != true)) {
                    this.partclassField = value;
                    this.RaisePropertyChanged("partclass");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string partnum {
            get {
                return this.partnumField;
            }
            set {
                if ((object.ReferenceEquals(this.partnumField, value) != true)) {
                    this.partnumField = value;
                    this.RaisePropertyChanged("partnum");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal price {
            get {
                return this.priceField;
            }
            set {
                if ((this.priceField.Equals(value) != true)) {
                    this.priceField = value;
                    this.RaisePropertyChanged("price");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string type {
            get {
                return this.typeField;
            }
            set {
                if ((object.ReferenceEquals(this.typeField, value) != true)) {
                    this.typeField = value;
                    this.RaisePropertyChanged("type");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string unit {
            get {
                return this.unitField;
            }
            set {
                if ((object.ReferenceEquals(this.unitField, value) != true)) {
                    this.unitField = value;
                    this.RaisePropertyChanged("unit");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal weight {
            get {
                return this.weightField;
            }
            set {
                if ((this.weightField.Equals(value) != true)) {
                    this.weightField = value;
                    this.RaisePropertyChanged("weight");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="partgetneedingupdateresult", Namespace="http://services.it.tcg/epicor/partservice")]
    [System.SerializableAttribute()]
    public partial class partgetneedingupdateresult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string exceptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private WindowsConsoleClient.PartServiceReference.part1[] epicorField;
        
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
        public WindowsConsoleClient.PartServiceReference.part1[] epicor {
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
    [System.Runtime.Serialization.DataContractAttribute(Name="part", Namespace="http://schemas.datacontract.org/2004/07/ServiceLibrary.NeedingUpdate")]
    [System.SerializableAttribute()]
    public partial class part1 : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string descField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal minimumqtyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal monthlyusageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string partnumField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal percentdiffField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string plantField;
        
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
        public string desc {
            get {
                return this.descField;
            }
            set {
                if ((object.ReferenceEquals(this.descField, value) != true)) {
                    this.descField = value;
                    this.RaisePropertyChanged("desc");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal minimumqty {
            get {
                return this.minimumqtyField;
            }
            set {
                if ((this.minimumqtyField.Equals(value) != true)) {
                    this.minimumqtyField = value;
                    this.RaisePropertyChanged("minimumqty");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal monthlyusage {
            get {
                return this.monthlyusageField;
            }
            set {
                if ((this.monthlyusageField.Equals(value) != true)) {
                    this.monthlyusageField = value;
                    this.RaisePropertyChanged("monthlyusage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string partnum {
            get {
                return this.partnumField;
            }
            set {
                if ((object.ReferenceEquals(this.partnumField, value) != true)) {
                    this.partnumField = value;
                    this.RaisePropertyChanged("partnum");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal percentdiff {
            get {
                return this.percentdiffField;
            }
            set {
                if ((this.percentdiffField.Equals(value) != true)) {
                    this.percentdiffField = value;
                    this.RaisePropertyChanged("percentdiff");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string plant {
            get {
                return this.plantField;
            }
            set {
                if ((object.ReferenceEquals(this.plantField, value) != true)) {
                    this.plantField = value;
                    this.RaisePropertyChanged("plant");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="partsminqtyupdaterequest", Namespace="http://services.it.tcg/epicor/partservice")]
    [System.SerializableAttribute()]
    public partial class partsminqtyupdaterequest : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string usernameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string passwordField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private WindowsConsoleClient.PartServiceReference.part1[] epicorField;
        
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
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
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
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public WindowsConsoleClient.PartServiceReference.part1[] epicor {
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
    [System.Runtime.Serialization.DataContractAttribute(Name="partupdateresult", Namespace="http://services.it.tcg/epicor/partservice")]
    [System.SerializableAttribute()]
    public partial class partupdateresult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://services.it.tcg/epicor/partservice", ConfigurationName="PartServiceReference.IPartService")]
    public interface IPartService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/partservice/IPartService/getallparts", ReplyAction="http://services.it.tcg/epicor/partservice/IPartService/getallpartsResponse")]
        WindowsConsoleClient.PartServiceReference.partgetallresult getallparts(int limit);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/partservice/IPartService/getpartsneedingupdate", ReplyAction="http://services.it.tcg/epicor/partservice/IPartService/getpartsneedingupdateRespo" +
            "nse")]
        WindowsConsoleClient.PartServiceReference.partgetneedingupdateresult getpartsneedingupdate();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/partservice/IPartService/updatepartsminqty", ReplyAction="http://services.it.tcg/epicor/partservice/IPartService/updatepartsminqtyResponse")]
        WindowsConsoleClient.PartServiceReference.partupdateresult updatepartsminqty(WindowsConsoleClient.PartServiceReference.partsminqtyupdaterequest request);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPartServiceChannel : WindowsConsoleClient.PartServiceReference.IPartService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PartServiceClient : System.ServiceModel.ClientBase<WindowsConsoleClient.PartServiceReference.IPartService>, WindowsConsoleClient.PartServiceReference.IPartService {
        
        public PartServiceClient() {
        }
        
        public PartServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PartServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PartServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PartServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public WindowsConsoleClient.PartServiceReference.partgetallresult getallparts(int limit) {
            return base.Channel.getallparts(limit);
        }
        
        public WindowsConsoleClient.PartServiceReference.partgetneedingupdateresult getpartsneedingupdate() {
            return base.Channel.getpartsneedingupdate();
        }
        
        public WindowsConsoleClient.PartServiceReference.partupdateresult updatepartsminqty(WindowsConsoleClient.PartServiceReference.partsminqtyupdaterequest request) {
            return base.Channel.updatepartsminqty(request);
        }
    }
}
