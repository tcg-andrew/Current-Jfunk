﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TCGEpicorForExcel2007.JobServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="jobgetmismatchresult", Namespace="http://services.it.tcg/epicor/jobservice")]
    [System.SerializableAttribute()]
    public partial class jobgetmismatchresult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string exceptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ObjectLibrary.Job[] epicorField;
        
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
        public ObjectLibrary.Job[] epicor {
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
    [System.Runtime.Serialization.DataContractAttribute(Name="jobgetstringresult", Namespace="http://services.it.tcg/epicor/jobservice")]
    [System.SerializableAttribute()]
    public partial class jobgetstringresult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
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
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://services.it.tcg/epicor/jobservice", ConfigurationName="JobServiceReference.IJobService")]
    public interface IJobService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/jobservice/IJobService/GetJobsWithMismatchedDates", ReplyAction="http://services.it.tcg/epicor/jobservice/IJobService/GetJobsWithMismatchedDatesRe" +
            "sponse")]
        TCGEpicorForExcel2007.JobServiceReference.jobgetmismatchresult GetJobsWithMismatchedDates();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/jobservice/IJobService/UpdateJobDates", ReplyAction="http://services.it.tcg/epicor/jobservice/IJobService/UpdateJobDatesResponse")]
        TCGEpicorForExcel2007.JobServiceReference.jobgetstringresult UpdateJobDates(string username, string password, ObjectLibrary.Job[] jobs);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IJobServiceChannel : TCGEpicorForExcel2007.JobServiceReference.IJobService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class JobServiceClient : System.ServiceModel.ClientBase<TCGEpicorForExcel2007.JobServiceReference.IJobService>, TCGEpicorForExcel2007.JobServiceReference.IJobService {
        
        public JobServiceClient() {
        }
        
        public JobServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public JobServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public JobServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public JobServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public TCGEpicorForExcel2007.JobServiceReference.jobgetmismatchresult GetJobsWithMismatchedDates() {
            return base.Channel.GetJobsWithMismatchedDates();
        }
        
        public TCGEpicorForExcel2007.JobServiceReference.jobgetstringresult UpdateJobDates(string username, string password, ObjectLibrary.Job[] jobs) {
            return base.Channel.UpdateJobDates(username, password, jobs);
        }
    }
}