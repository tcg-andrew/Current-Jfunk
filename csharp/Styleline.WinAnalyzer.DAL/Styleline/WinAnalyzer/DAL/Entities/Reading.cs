namespace Styleline.WinAnalyzer.DAL.Entities
{
    using System;
    using System.Data.Linq.Mapping;

    [Table(Name="dbo.Readings")]
    public class Reading
    {
        private decimal _Amps;
        private string _AssemblyNumber;
        private int _AssemblyRecording;
        private bool _AutoEntry;
        private string _EmployeeNo;
        private string _entrtime;
        private DateTime _EntryDate;
        private int _Id;
        private string _JobNumber;
        private decimal? _Lineno;
        private decimal _Ohms;
        private decimal _Pf;
        private int _PowerTableId;
        private string _Sono;
        private char _Type;
        private decimal _Voltage;
        private decimal _Wattage;

        [Column(Storage="_Amps", DbType="Decimal(18,3) NOT NULL")]
        public decimal Amps
        {
            get
            {
                return this._Amps;
            }
            set
            {
                if (this._Amps != value)
                {
                    this._Amps = value;
                }
            }
        }

        [Column(Storage="_AssemblyNumber", DbType="VarChar(10) NOT NULL", CanBeNull=false)]
        public string AssemblyNumber
        {
            get
            {
                return this._AssemblyNumber;
            }
            set
            {
                if (this._AssemblyNumber != value)
                {
                    this._AssemblyNumber = value;
                }
            }
        }

        [Column(Storage="_AssemblyRecording", DbType="Int NOT NULL")]
        public int AssemblyRecording
        {
            get
            {
                return this._AssemblyRecording;
            }
            set
            {
                if (this._AssemblyRecording != value)
                {
                    this._AssemblyRecording = value;
                }
            }
        }

        [Column(Storage="_AutoEntry", DbType="Bit NOT NULL")]
        public bool AutoEntry
        {
            get
            {
                return this._AutoEntry;
            }
            set
            {
                if (this._AutoEntry != value)
                {
                    this._AutoEntry = value;
                }
            }
        }

        [Column(Storage="_EmployeeNo", DbType="NVarChar(6) NOT NULL", CanBeNull=false)]
        public string EmployeeNo
        {
            get
            {
                return this._EmployeeNo;
            }
            set
            {
                if (this._EmployeeNo != value)
                {
                    this._EmployeeNo = value;
                }
            }
        }

        [Column(Storage="_entrtime", DbType="NVarChar(8)")]
        public string entrtime
        {
            get
            {
                return this._entrtime;
            }
            set
            {
                if (this._entrtime != value)
                {
                    this._entrtime = value;
                }
            }
        }

        [Column(Storage="_EntryDate", DbType="DateTime NOT NULL")]
        public DateTime EntryDate
        {
            get
            {
                return this._EntryDate;
            }
            set
            {
                if (this._EntryDate != value)
                {
                    this._EntryDate = value;
                }
            }
        }

        [Column(Storage="_Id", AutoSync=AutoSync.Always, DbType="Int NOT NULL IDENTITY", IsDbGenerated=true)]
        public int Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                if (this._Id != value)
                {
                    this._Id = value;
                }
            }
        }

        [Column(Storage="_JobNumber", DbType="VarChar(20) NOT NULL", CanBeNull=false)]
        public string JobNumber
        {
            get
            {
                return this._JobNumber;
            }
            set
            {
                if (this._JobNumber != value)
                {
                    this._JobNumber = value;
                }
            }
        }

        [Column(Name="[Lineno]", Storage="_Lineno", DbType="Decimal(4,0)")]
        public decimal? Lineno
        {
            get
            {
                return this._Lineno;
            }
            set
            {
                decimal? someDecimal = this._Lineno;
                decimal? someOtherDecimal = value;
                if ((someDecimal.GetValueOrDefault() != someOtherDecimal.GetValueOrDefault()) || (someDecimal.HasValue != someOtherDecimal.HasValue))
                {
                    this._Lineno = value;
                }
            }
        }

        [Column(Storage="_Ohms", DbType="Decimal(18,2) NOT NULL")]
        public decimal Ohms
        {
            get
            {
                return this._Ohms;
            }
            set
            {
                if (this._Ohms != value)
                {
                    this._Ohms = value;
                }
            }
        }

        [Column(Storage="_Pf", DbType="Decimal(18,3) NOT NULL")]
        public decimal Pf
        {
            get
            {
                return this._Pf;
            }
            set
            {
                if (this._Pf != value)
                {
                    this._Pf = value;
                }
            }
        }

        [Column(Storage="_PowerTableId", DbType="Int NOT NULL")]
        public int PowerTableId
        {
            get
            {
                return this._PowerTableId;
            }
            set
            {
                if (this._PowerTableId != value)
                {
                    this._PowerTableId = value;
                }
            }
        }

        [Column(Storage="_Sono", DbType="NVarChar(10)")]
        public string Sono
        {
            get
            {
                return this._Sono;
            }
            set
            {
                if (this._Sono != value)
                {
                    this._Sono = value;
                }
            }
        }

        [Column(Storage="_Type", DbType="NVarChar(1) NOT NULL")]
        public char Type
        {
            get
            {
                return this._Type;
            }
            set
            {
                if (this._Type != value)
                {
                    this._Type = value;
                }
            }
        }

        [Column(Storage="_Voltage", DbType="Decimal(18,2) NOT NULL")]
        public decimal Voltage
        {
            get
            {
                return this._Voltage;
            }
            set
            {
                if (this._Voltage != value)
                {
                    this._Voltage = value;
                }
            }
        }

        [Column(Storage="_Wattage", DbType="Decimal(18,2) NOT NULL")]
        public decimal Wattage
        {
            get
            {
                return this._Wattage;
            }
            set
            {
                if (this._Wattage != value)
                {
                    this._Wattage = value;
                }
            }
        }
    }
}

