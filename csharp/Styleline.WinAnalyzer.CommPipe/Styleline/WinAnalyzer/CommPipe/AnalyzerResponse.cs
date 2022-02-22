namespace Styleline.WinAnalyzer.CommPipe
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    [DataContract]
    public class AnalyzerResponse
    {
        [DataMember]
        public decimal Amps { get; set; }

        [DataMember]
        public string EmployeeNumber { get; set; }

        [DataMember]
        public decimal Ohms { get; set; }

        [DataMember]
        public decimal OhmsHigh { get; set; }

        [DataMember]
        public decimal OhmsLow { get; set; }

        [DataMember]
        public decimal PF { get; set; }

        [DataMember]
        public int PowerTableId { get; set; }

        [DataMember]
        public int RecordedCount { get; set; }

        [DataMember]
        public int RecordedId { get; set; }

        [DataMember]
        public AnalyzerRequest Request { get; set; }

        [DataMember]
        public string Revision { get; set; }

        [DataMember]
        public decimal Volts { get; set; }

        [DataMember]
        public decimal Watts { get; set; }
    }
}

