﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TCGEpicorForExcel2007.PowerAnalyzerServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PowerAnalyzerSetting", Namespace="http://schemas.datacontract.org/2004/07/ObjectLibrary")]
    [System.SerializableAttribute()]
    public partial class PowerAnalyzerSetting : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold DoorDoorAmpsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold DoorDoorOhmsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold DoorFrameOhmsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DoorFrameWireField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold DoorGlassOhmsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold FrameAmpsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold FrameMullionOhmsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FrameMullionWireField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold FrameOhmsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold FrameStainlessOhmsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FrameStainlessWireField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold FrameTotalMullionOhmsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold FrameTotalStainlessOhmsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FrameTypeCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold FrameWrap1OhmsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FrameWrap1WireField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold FrameWrap2OhmsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FrameWrap2WireField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold FrameWrapOhmsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int HighVoltageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsDoorField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsFrameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ItemField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold LightAmpsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int LowVoltageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ModelCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int NumberDoorsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold SummaryDoorAndFrameAmpsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold SummaryDoorAndFrameAndLampAmpsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold SummaryFrameAndLightAmpsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SummaryMaxAmpsHeaterField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SummaryMaxAmpsLightsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SummaryMaxAmpsTotalField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SummaryRatedAmpsHeaterField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SummaryRatedAmpsLightsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TemperatureCodeField;
        
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
        public TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold DoorDoorAmps {
            get {
                return this.DoorDoorAmpsField;
            }
            set {
                if ((object.ReferenceEquals(this.DoorDoorAmpsField, value) != true)) {
                    this.DoorDoorAmpsField = value;
                    this.RaisePropertyChanged("DoorDoorAmps");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold DoorDoorOhms {
            get {
                return this.DoorDoorOhmsField;
            }
            set {
                if ((object.ReferenceEquals(this.DoorDoorOhmsField, value) != true)) {
                    this.DoorDoorOhmsField = value;
                    this.RaisePropertyChanged("DoorDoorOhms");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold DoorFrameOhms {
            get {
                return this.DoorFrameOhmsField;
            }
            set {
                if ((object.ReferenceEquals(this.DoorFrameOhmsField, value) != true)) {
                    this.DoorFrameOhmsField = value;
                    this.RaisePropertyChanged("DoorFrameOhms");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DoorFrameWire {
            get {
                return this.DoorFrameWireField;
            }
            set {
                if ((object.ReferenceEquals(this.DoorFrameWireField, value) != true)) {
                    this.DoorFrameWireField = value;
                    this.RaisePropertyChanged("DoorFrameWire");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold DoorGlassOhms {
            get {
                return this.DoorGlassOhmsField;
            }
            set {
                if ((object.ReferenceEquals(this.DoorGlassOhmsField, value) != true)) {
                    this.DoorGlassOhmsField = value;
                    this.RaisePropertyChanged("DoorGlassOhms");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold FrameAmps {
            get {
                return this.FrameAmpsField;
            }
            set {
                if ((object.ReferenceEquals(this.FrameAmpsField, value) != true)) {
                    this.FrameAmpsField = value;
                    this.RaisePropertyChanged("FrameAmps");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold FrameMullionOhms {
            get {
                return this.FrameMullionOhmsField;
            }
            set {
                if ((object.ReferenceEquals(this.FrameMullionOhmsField, value) != true)) {
                    this.FrameMullionOhmsField = value;
                    this.RaisePropertyChanged("FrameMullionOhms");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FrameMullionWire {
            get {
                return this.FrameMullionWireField;
            }
            set {
                if ((object.ReferenceEquals(this.FrameMullionWireField, value) != true)) {
                    this.FrameMullionWireField = value;
                    this.RaisePropertyChanged("FrameMullionWire");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold FrameOhms {
            get {
                return this.FrameOhmsField;
            }
            set {
                if ((object.ReferenceEquals(this.FrameOhmsField, value) != true)) {
                    this.FrameOhmsField = value;
                    this.RaisePropertyChanged("FrameOhms");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold FrameStainlessOhms {
            get {
                return this.FrameStainlessOhmsField;
            }
            set {
                if ((object.ReferenceEquals(this.FrameStainlessOhmsField, value) != true)) {
                    this.FrameStainlessOhmsField = value;
                    this.RaisePropertyChanged("FrameStainlessOhms");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FrameStainlessWire {
            get {
                return this.FrameStainlessWireField;
            }
            set {
                if ((object.ReferenceEquals(this.FrameStainlessWireField, value) != true)) {
                    this.FrameStainlessWireField = value;
                    this.RaisePropertyChanged("FrameStainlessWire");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold FrameTotalMullionOhms {
            get {
                return this.FrameTotalMullionOhmsField;
            }
            set {
                if ((object.ReferenceEquals(this.FrameTotalMullionOhmsField, value) != true)) {
                    this.FrameTotalMullionOhmsField = value;
                    this.RaisePropertyChanged("FrameTotalMullionOhms");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold FrameTotalStainlessOhms {
            get {
                return this.FrameTotalStainlessOhmsField;
            }
            set {
                if ((object.ReferenceEquals(this.FrameTotalStainlessOhmsField, value) != true)) {
                    this.FrameTotalStainlessOhmsField = value;
                    this.RaisePropertyChanged("FrameTotalStainlessOhms");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FrameTypeCode {
            get {
                return this.FrameTypeCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.FrameTypeCodeField, value) != true)) {
                    this.FrameTypeCodeField = value;
                    this.RaisePropertyChanged("FrameTypeCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold FrameWrap1Ohms {
            get {
                return this.FrameWrap1OhmsField;
            }
            set {
                if ((object.ReferenceEquals(this.FrameWrap1OhmsField, value) != true)) {
                    this.FrameWrap1OhmsField = value;
                    this.RaisePropertyChanged("FrameWrap1Ohms");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FrameWrap1Wire {
            get {
                return this.FrameWrap1WireField;
            }
            set {
                if ((object.ReferenceEquals(this.FrameWrap1WireField, value) != true)) {
                    this.FrameWrap1WireField = value;
                    this.RaisePropertyChanged("FrameWrap1Wire");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold FrameWrap2Ohms {
            get {
                return this.FrameWrap2OhmsField;
            }
            set {
                if ((object.ReferenceEquals(this.FrameWrap2OhmsField, value) != true)) {
                    this.FrameWrap2OhmsField = value;
                    this.RaisePropertyChanged("FrameWrap2Ohms");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FrameWrap2Wire {
            get {
                return this.FrameWrap2WireField;
            }
            set {
                if ((object.ReferenceEquals(this.FrameWrap2WireField, value) != true)) {
                    this.FrameWrap2WireField = value;
                    this.RaisePropertyChanged("FrameWrap2Wire");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold FrameWrapOhms {
            get {
                return this.FrameWrapOhmsField;
            }
            set {
                if ((object.ReferenceEquals(this.FrameWrapOhmsField, value) != true)) {
                    this.FrameWrapOhmsField = value;
                    this.RaisePropertyChanged("FrameWrapOhms");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int HighVoltage {
            get {
                return this.HighVoltageField;
            }
            set {
                if ((this.HighVoltageField.Equals(value) != true)) {
                    this.HighVoltageField = value;
                    this.RaisePropertyChanged("HighVoltage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsDoor {
            get {
                return this.IsDoorField;
            }
            set {
                if ((this.IsDoorField.Equals(value) != true)) {
                    this.IsDoorField = value;
                    this.RaisePropertyChanged("IsDoor");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsFrame {
            get {
                return this.IsFrameField;
            }
            set {
                if ((this.IsFrameField.Equals(value) != true)) {
                    this.IsFrameField = value;
                    this.RaisePropertyChanged("IsFrame");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Item {
            get {
                return this.ItemField;
            }
            set {
                if ((object.ReferenceEquals(this.ItemField, value) != true)) {
                    this.ItemField = value;
                    this.RaisePropertyChanged("Item");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold LightAmps {
            get {
                return this.LightAmpsField;
            }
            set {
                if ((object.ReferenceEquals(this.LightAmpsField, value) != true)) {
                    this.LightAmpsField = value;
                    this.RaisePropertyChanged("LightAmps");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int LowVoltage {
            get {
                return this.LowVoltageField;
            }
            set {
                if ((this.LowVoltageField.Equals(value) != true)) {
                    this.LowVoltageField = value;
                    this.RaisePropertyChanged("LowVoltage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ModelCode {
            get {
                return this.ModelCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.ModelCodeField, value) != true)) {
                    this.ModelCodeField = value;
                    this.RaisePropertyChanged("ModelCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int NumberDoors {
            get {
                return this.NumberDoorsField;
            }
            set {
                if ((this.NumberDoorsField.Equals(value) != true)) {
                    this.NumberDoorsField = value;
                    this.RaisePropertyChanged("NumberDoors");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold SummaryDoorAndFrameAmps {
            get {
                return this.SummaryDoorAndFrameAmpsField;
            }
            set {
                if ((object.ReferenceEquals(this.SummaryDoorAndFrameAmpsField, value) != true)) {
                    this.SummaryDoorAndFrameAmpsField = value;
                    this.RaisePropertyChanged("SummaryDoorAndFrameAmps");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold SummaryDoorAndFrameAndLampAmps {
            get {
                return this.SummaryDoorAndFrameAndLampAmpsField;
            }
            set {
                if ((object.ReferenceEquals(this.SummaryDoorAndFrameAndLampAmpsField, value) != true)) {
                    this.SummaryDoorAndFrameAndLampAmpsField = value;
                    this.RaisePropertyChanged("SummaryDoorAndFrameAndLampAmps");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TCGEpicorForExcel2007.PowerAnalyzerServiceReference.SettingThreshold SummaryFrameAndLightAmps {
            get {
                return this.SummaryFrameAndLightAmpsField;
            }
            set {
                if ((object.ReferenceEquals(this.SummaryFrameAndLightAmpsField, value) != true)) {
                    this.SummaryFrameAndLightAmpsField = value;
                    this.RaisePropertyChanged("SummaryFrameAndLightAmps");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SummaryMaxAmpsHeater {
            get {
                return this.SummaryMaxAmpsHeaterField;
            }
            set {
                if ((object.ReferenceEquals(this.SummaryMaxAmpsHeaterField, value) != true)) {
                    this.SummaryMaxAmpsHeaterField = value;
                    this.RaisePropertyChanged("SummaryMaxAmpsHeater");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SummaryMaxAmpsLights {
            get {
                return this.SummaryMaxAmpsLightsField;
            }
            set {
                if ((object.ReferenceEquals(this.SummaryMaxAmpsLightsField, value) != true)) {
                    this.SummaryMaxAmpsLightsField = value;
                    this.RaisePropertyChanged("SummaryMaxAmpsLights");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SummaryMaxAmpsTotal {
            get {
                return this.SummaryMaxAmpsTotalField;
            }
            set {
                if ((object.ReferenceEquals(this.SummaryMaxAmpsTotalField, value) != true)) {
                    this.SummaryMaxAmpsTotalField = value;
                    this.RaisePropertyChanged("SummaryMaxAmpsTotal");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SummaryRatedAmpsHeater {
            get {
                return this.SummaryRatedAmpsHeaterField;
            }
            set {
                if ((object.ReferenceEquals(this.SummaryRatedAmpsHeaterField, value) != true)) {
                    this.SummaryRatedAmpsHeaterField = value;
                    this.RaisePropertyChanged("SummaryRatedAmpsHeater");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SummaryRatedAmpsLights {
            get {
                return this.SummaryRatedAmpsLightsField;
            }
            set {
                if ((object.ReferenceEquals(this.SummaryRatedAmpsLightsField, value) != true)) {
                    this.SummaryRatedAmpsLightsField = value;
                    this.RaisePropertyChanged("SummaryRatedAmpsLights");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TemperatureCode {
            get {
                return this.TemperatureCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.TemperatureCodeField, value) != true)) {
                    this.TemperatureCodeField = value;
                    this.RaisePropertyChanged("TemperatureCode");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="SettingThreshold", Namespace="http://schemas.datacontract.org/2004/07/ObjectLibrary")]
    [System.SerializableAttribute()]
    public partial class SettingThreshold : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string HighField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LowField;
        
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
        public string High {
            get {
                return this.HighField;
            }
            set {
                if ((object.ReferenceEquals(this.HighField, value) != true)) {
                    this.HighField = value;
                    this.RaisePropertyChanged("High");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Low {
            get {
                return this.LowField;
            }
            set {
                if ((object.ReferenceEquals(this.LowField, value) != true)) {
                    this.LowField = value;
                    this.RaisePropertyChanged("Low");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="powertableupdateresult", Namespace="http://services.it.tcg/epicor/poweranalyzerservice")]
    [System.SerializableAttribute()]
    public partial class powertableupdateresult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
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
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://services.it.tcg/epicor/poweranalyzerservice", ConfigurationName="PowerAnalyzerServiceReference.IPowerAnalyzerService")]
    public interface IPowerAnalyzerService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.it.tcg/epicor/poweranalyzerservice/IPowerAnalyzerService/UpdatePo" +
            "werTable", ReplyAction="http://services.it.tcg/epicor/poweranalyzerservice/IPowerAnalyzerService/UpdatePo" +
            "werTableResponse")]
        TCGEpicorForExcel2007.PowerAnalyzerServiceReference.powertableupdateresult UpdatePowerTable(TCGEpicorForExcel2007.PowerAnalyzerServiceReference.PowerAnalyzerSetting[] settings);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPowerAnalyzerServiceChannel : TCGEpicorForExcel2007.PowerAnalyzerServiceReference.IPowerAnalyzerService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PowerAnalyzerServiceClient : System.ServiceModel.ClientBase<TCGEpicorForExcel2007.PowerAnalyzerServiceReference.IPowerAnalyzerService>, TCGEpicorForExcel2007.PowerAnalyzerServiceReference.IPowerAnalyzerService {
        
        public PowerAnalyzerServiceClient() {
        }
        
        public PowerAnalyzerServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PowerAnalyzerServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PowerAnalyzerServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PowerAnalyzerServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public TCGEpicorForExcel2007.PowerAnalyzerServiceReference.powertableupdateresult UpdatePowerTable(TCGEpicorForExcel2007.PowerAnalyzerServiceReference.PowerAnalyzerSetting[] settings) {
            return base.Channel.UpdatePowerTable(settings);
        }
    }
}