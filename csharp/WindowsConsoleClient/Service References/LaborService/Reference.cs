﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WindowsConsoleClient.LaborService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="startactivityresult", Namespace="http://services.it.tcg/epicor/laborservice")]
    [System.SerializableAttribute()]
    public partial class startactivityresult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="endactivityresult", Namespace="http://services.it.tcg/epicor/laborservice")]
    [System.SerializableAttribute()]
    public partial class endactivityresult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="startindirectresult", Namespace="http://services.it.tcg/epicor/laborservice")]
    [System.SerializableAttribute()]
    public partial class startindirectresult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="endindirectresult", Namespace="http://services.it.tcg/epicor/laborservice")]
    [System.SerializableAttribute()]
    public partial class endindirectresult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="clockinresult", Namespace="http://services.it.tcg/epicor/laborservice")]
    [System.SerializableAttribute()]
    public partial class clockinresult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="clockoutresult", Namespace="http://services.it.tcg/epicor/laborservice")]
    [System.SerializableAttribute()]
    public partial class clockoutresult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="getindirectcodesresult", Namespace="http://services.it.tcg/epicor/laborservice")]
    [System.SerializableAttribute()]
    public partial class getindirectcodesresult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.Dictionary<string, string> epicorField;
        
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
        public System.Collections.Generic.Dictionary<string, string> epicor {
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
    [System.Runtime.Serialization.DataContractAttribute(Name="getresourcecodesresult", Namespace="http://services.it.tcg/epicor/laborservice")]
    [System.SerializableAttribute()]
    public partial class getresourcecodesresult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.Dictionary<string, string> epicorField;
        
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
        public System.Collections.Generic.Dictionary<string, string> epicor {
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
    [System.Runtime.Serialization.DataContractAttribute(Name="getstringresult", Namespace="http://services.it.tcg/epicor/laborservice")]
    [System.SerializableAttribute()]
    public partial class getstringresult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string epicorField;
        
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
        public string epicor {
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://services.it.tcg/epicor/laborservice", ConfigurationName="LaborService.ILaborService")]
    public interface ILaborService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/startactivity", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/startactivityResponse")]
        WindowsConsoleClient.LaborService.startactivityresult startactivity(string company, string plant, string employeeid, string jobnum, string assemblyseq, string oprseq);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/startactivity", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/startactivityResponse")]
        System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.startactivityresult> startactivityAsync(string company, string plant, string employeeid, string jobnum, string assemblyseq, string oprseq);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/endactivity", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/endactivityResponse")]
        WindowsConsoleClient.LaborService.endactivityresult endactivity(string company, string plant, string employeeid, string jobnum, string assemblyseq, string oprseq, string qty, string scrap);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/endactivity", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/endactivityResponse")]
        System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.endactivityresult> endactivityAsync(string company, string plant, string employeeid, string jobnum, string assemblyseq, string oprseq, string qty, string scrap);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/startindirect", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/startindirectResponse")]
        WindowsConsoleClient.LaborService.startindirectresult startindirect(string company, string employeeid, string indirectcode, string resourcegroup);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/startindirect", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/startindirectResponse")]
        System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.startindirectresult> startindirectAsync(string company, string employeeid, string indirectcode, string resourcegroup);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/endindirect", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/endindirectResponse")]
        WindowsConsoleClient.LaborService.endindirectresult endindirect(string company, string employeeid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/endindirect", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/endindirectResponse")]
        System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.endindirectresult> endindirectAsync(string company, string employeeid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/clockin", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/clockinResponse")]
        WindowsConsoleClient.LaborService.clockinresult clockin(string company, string employeeid, string shift);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/clockin", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/clockinResponse")]
        System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.clockinresult> clockinAsync(string company, string employeeid, string shift);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/clockout", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/clockoutResponse")]
        WindowsConsoleClient.LaborService.clockoutresult clockout(string company, string employeeid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/clockout", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/clockoutResponse")]
        System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.clockoutresult> clockoutAsync(string company, string employeeid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/getindirectcodes", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/getindirectcodesResponse" +
            "")]
        WindowsConsoleClient.LaborService.getindirectcodesresult getindirectcodes(string company);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/getindirectcodes", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/getindirectcodesResponse" +
            "")]
        System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.getindirectcodesresult> getindirectcodesAsync(string company);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/getresourcecodes", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/getresourcecodesResponse" +
            "")]
        WindowsConsoleClient.LaborService.getresourcecodesresult getresourcecodes(string company);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/getresourcecodes", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/getresourcecodesResponse" +
            "")]
        System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.getresourcecodesresult> getresourcecodesAsync(string company);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/getvacationhours", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/getvacationhoursResponse" +
            "")]
        WindowsConsoleClient.LaborService.getstringresult getvacationhours(string company, string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/getvacationhours", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/getvacationhoursResponse" +
            "")]
        System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.getstringresult> getvacationhoursAsync(string company, string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/getempname", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/getempnameResponse")]
        WindowsConsoleClient.LaborService.getstringresult getempname(string company, string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/laborservice/ILaborService/getempname", ReplyAction="http://services.it.tcg/epicor/laborservice/ILaborService/getempnameResponse")]
        System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.getstringresult> getempnameAsync(string company, string id);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILaborServiceChannel : WindowsConsoleClient.LaborService.ILaborService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LaborServiceClient : System.ServiceModel.ClientBase<WindowsConsoleClient.LaborService.ILaborService>, WindowsConsoleClient.LaborService.ILaborService {
        
        public LaborServiceClient() {
        }
        
        public LaborServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LaborServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LaborServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LaborServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public WindowsConsoleClient.LaborService.startactivityresult startactivity(string company, string plant, string employeeid, string jobnum, string assemblyseq, string oprseq) {
            return base.Channel.startactivity(company, plant, employeeid, jobnum, assemblyseq, oprseq);
        }
        
        public System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.startactivityresult> startactivityAsync(string company, string plant, string employeeid, string jobnum, string assemblyseq, string oprseq) {
            return base.Channel.startactivityAsync(company, plant, employeeid, jobnum, assemblyseq, oprseq);
        }
        
        public WindowsConsoleClient.LaborService.endactivityresult endactivity(string company, string plant, string employeeid, string jobnum, string assemblyseq, string oprseq, string qty, string scrap) {
            return base.Channel.endactivity(company, plant, employeeid, jobnum, assemblyseq, oprseq, qty, scrap);
        }
        
        public System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.endactivityresult> endactivityAsync(string company, string plant, string employeeid, string jobnum, string assemblyseq, string oprseq, string qty, string scrap) {
            return base.Channel.endactivityAsync(company, plant, employeeid, jobnum, assemblyseq, oprseq, qty, scrap);
        }
        
        public WindowsConsoleClient.LaborService.startindirectresult startindirect(string company, string employeeid, string indirectcode, string resourcegroup) {
            return base.Channel.startindirect(company, employeeid, indirectcode, resourcegroup);
        }
        
        public System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.startindirectresult> startindirectAsync(string company, string employeeid, string indirectcode, string resourcegroup) {
            return base.Channel.startindirectAsync(company, employeeid, indirectcode, resourcegroup);
        }
        
        public WindowsConsoleClient.LaborService.endindirectresult endindirect(string company, string employeeid) {
            return base.Channel.endindirect(company, employeeid);
        }
        
        public System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.endindirectresult> endindirectAsync(string company, string employeeid) {
            return base.Channel.endindirectAsync(company, employeeid);
        }
        
        public WindowsConsoleClient.LaborService.clockinresult clockin(string company, string employeeid, string shift) {
            return base.Channel.clockin(company, employeeid, shift);
        }
        
        public System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.clockinresult> clockinAsync(string company, string employeeid, string shift) {
            return base.Channel.clockinAsync(company, employeeid, shift);
        }
        
        public WindowsConsoleClient.LaborService.clockoutresult clockout(string company, string employeeid) {
            return base.Channel.clockout(company, employeeid);
        }
        
        public System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.clockoutresult> clockoutAsync(string company, string employeeid) {
            return base.Channel.clockoutAsync(company, employeeid);
        }
        
        public WindowsConsoleClient.LaborService.getindirectcodesresult getindirectcodes(string company) {
            return base.Channel.getindirectcodes(company);
        }
        
        public System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.getindirectcodesresult> getindirectcodesAsync(string company) {
            return base.Channel.getindirectcodesAsync(company);
        }
        
        public WindowsConsoleClient.LaborService.getresourcecodesresult getresourcecodes(string company) {
            return base.Channel.getresourcecodes(company);
        }
        
        public System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.getresourcecodesresult> getresourcecodesAsync(string company) {
            return base.Channel.getresourcecodesAsync(company);
        }
        
        public WindowsConsoleClient.LaborService.getstringresult getvacationhours(string company, string id) {
            return base.Channel.getvacationhours(company, id);
        }
        
        public System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.getstringresult> getvacationhoursAsync(string company, string id) {
            return base.Channel.getvacationhoursAsync(company, id);
        }
        
        public WindowsConsoleClient.LaborService.getstringresult getempname(string company, string id) {
            return base.Channel.getempname(company, id);
        }
        
        public System.Threading.Tasks.Task<WindowsConsoleClient.LaborService.getstringresult> getempnameAsync(string company, string id) {
            return base.Channel.getempnameAsync(company, id);
        }
    }
}
