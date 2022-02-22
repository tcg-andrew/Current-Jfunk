namespace Styleline.WinAnalyzer.CommPipe
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    [DataContract]
    public class AnalyzerRequest
    {
        [DataMember]
        public string AssemblyNumber { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string EmployeeNumber { get; set; }

        [DataMember]
        public string Frame { get; set; }

        [DataMember]
        public bool IsFrame { get; set; }

        [DataMember]
        public string ItemNumber { get; set; }

        [DataMember]
        public string JobNumber { get; set; }

        [DataMember]
        public int JobQty { get; set; }

        [DataMember]
        public string Line { get; set; }

        [DataMember]
        public string Model { get; set; }

        [DataMember]
        public string NumberOfDoors { get; set; }

        [DataMember]
        public bool RequestValues { get; set; }

        [DataMember]
        public string Voltage { get; set; }
    }
}

