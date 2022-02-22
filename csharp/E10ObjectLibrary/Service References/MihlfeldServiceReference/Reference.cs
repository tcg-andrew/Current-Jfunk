﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ObjectLibrary.MihlfeldServiceReference {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="https://www.mihlfeld.com", ConfigurationName="MihlfeldServiceReference.OpenSourceSoap")]
    public interface OpenSourceSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="https://www.mihlfeld.com/Get_Requestor_ID_List", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable Get_Requestor_ID_List(string Username, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://www.mihlfeld.com/Get_Rates", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable Get_Rates(
                    string Username, 
                    string Password, 
                    string BD1_NMFC, 
                    string BD1_Weight, 
                    string BD2_NMFC, 
                    string BD2_Weight, 
                    string BD3_NMFC, 
                    string BD3_Weight, 
                    string BD4_NMFC, 
                    string BD4_Weight, 
                    string BD5_NMFC, 
                    string BD5_Weight, 
                    string Origin_PostCode, 
                    string Destination_PostCode, 
                    string Direction, 
                    string Ship_Date, 
                    string Bill_Terms, 
                    int Requestor_ID);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://www.mihlfeld.com/Get_Rates_withCountry", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable Get_Rates_withCountry(
                    string Username, 
                    string Password, 
                    string BD1_NMFC, 
                    string BD1_Weight, 
                    string BD2_NMFC, 
                    string BD2_Weight, 
                    string BD3_NMFC, 
                    string BD3_Weight, 
                    string BD4_NMFC, 
                    string BD4_Weight, 
                    string BD5_NMFC, 
                    string BD5_Weight, 
                    string Origin_PostCode, 
                    string Destination_PostCode, 
                    string Origin_Country, 
                    string Destination_Country, 
                    string Direction, 
                    string Ship_Date, 
                    string Bill_Terms, 
                    int Requestor_ID);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface OpenSourceSoapChannel : ObjectLibrary.MihlfeldServiceReference.OpenSourceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class OpenSourceSoapClient : System.ServiceModel.ClientBase<ObjectLibrary.MihlfeldServiceReference.OpenSourceSoap>, ObjectLibrary.MihlfeldServiceReference.OpenSourceSoap {
        
        public OpenSourceSoapClient() {
        }
        
        public OpenSourceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public OpenSourceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OpenSourceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OpenSourceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Data.DataTable Get_Requestor_ID_List(string Username, string Password) {
            return base.Channel.Get_Requestor_ID_List(Username, Password);
        }
        
        public System.Data.DataTable Get_Rates(
                    string Username, 
                    string Password, 
                    string BD1_NMFC, 
                    string BD1_Weight, 
                    string BD2_NMFC, 
                    string BD2_Weight, 
                    string BD3_NMFC, 
                    string BD3_Weight, 
                    string BD4_NMFC, 
                    string BD4_Weight, 
                    string BD5_NMFC, 
                    string BD5_Weight, 
                    string Origin_PostCode, 
                    string Destination_PostCode, 
                    string Direction, 
                    string Ship_Date, 
                    string Bill_Terms, 
                    int Requestor_ID) {
            return base.Channel.Get_Rates(Username, Password, BD1_NMFC, BD1_Weight, BD2_NMFC, BD2_Weight, BD3_NMFC, BD3_Weight, BD4_NMFC, BD4_Weight, BD5_NMFC, BD5_Weight, Origin_PostCode, Destination_PostCode, Direction, Ship_Date, Bill_Terms, Requestor_ID);
        }
        
        public System.Data.DataTable Get_Rates_withCountry(
                    string Username, 
                    string Password, 
                    string BD1_NMFC, 
                    string BD1_Weight, 
                    string BD2_NMFC, 
                    string BD2_Weight, 
                    string BD3_NMFC, 
                    string BD3_Weight, 
                    string BD4_NMFC, 
                    string BD4_Weight, 
                    string BD5_NMFC, 
                    string BD5_Weight, 
                    string Origin_PostCode, 
                    string Destination_PostCode, 
                    string Origin_Country, 
                    string Destination_Country, 
                    string Direction, 
                    string Ship_Date, 
                    string Bill_Terms, 
                    int Requestor_ID) {
            return base.Channel.Get_Rates_withCountry(Username, Password, BD1_NMFC, BD1_Weight, BD2_NMFC, BD2_Weight, BD3_NMFC, BD3_Weight, BD4_NMFC, BD4_Weight, BD5_NMFC, BD5_Weight, Origin_PostCode, Destination_PostCode, Origin_Country, Destination_Country, Direction, Ship_Date, Bill_Terms, Requestor_ID);
        }
    }
}
