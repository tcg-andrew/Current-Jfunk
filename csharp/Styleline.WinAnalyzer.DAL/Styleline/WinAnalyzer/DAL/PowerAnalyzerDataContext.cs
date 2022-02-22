namespace Styleline.WinAnalyzer.DAL
{
    using System;
    using System.Data;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.Reflection;
    using Styleline.WinAnalyzer.DAL.Properties;
    using Styleline.WinAnalyzer.DAL.Entities;

    [Database(Name="PowerAnalyzer")]
    public class PowerAnalyzerDataContext : DataContext
    {
        private static MappingSource mappingSource = new AttributeMappingSource();

        public PowerAnalyzerDataContext() : base(Settings.Default.PowerAnalyzerConnectionString1, mappingSource)
        {
        }

        public PowerAnalyzerDataContext(IDbConnection connection) : base(connection, mappingSource)
        {
        }

        public PowerAnalyzerDataContext(string connection) : base(connection, mappingSource)
        {
        }

        public PowerAnalyzerDataContext(IDbConnection connection, MappingSource mappingSource) : base(connection, mappingSource)
        {
        }

        public PowerAnalyzerDataContext(string connection, MappingSource mappingSource) : base(connection, mappingSource)
        {
        }

        [Function(Name="dbo.InsertReading")]
        public int InsertReading([Parameter(Name="PowerTableId", DbType="Int")] int? powerTableId, [Parameter(Name="JobNumber", DbType="VarChar(20)")] string jobNumber, [Parameter(Name="AssemblyNumber", DbType="VarChar(10)")] string assemblyNumber, [Parameter(Name="Type", DbType="Char(1)")] char? type, [Parameter(Name="EmployeeNo", DbType="NVarChar(6)")] string employeeNo, [Parameter(Name="Voltage", DbType="Decimal")] decimal? voltage, [Parameter(Name="Wattage", DbType="Decimal")] decimal? wattage, [Parameter(Name="Pf", DbType="Decimal")] decimal? pf, [Parameter(Name="Ohms", DbType="Decimal")] decimal? ohms, [Parameter(Name="Amps", DbType="Decimal")] decimal? amps, [Parameter(Name="AutoEntry", DbType="Bit")] bool? autoEntry, [Parameter(Name="MaxRecording", DbType="Int")] ref int? maxRecording)
        {
            IExecuteResult result = base.ExecuteMethodCall(this, (MethodInfo) MethodBase.GetCurrentMethod(), new object[] { powerTableId, jobNumber, assemblyNumber, type, employeeNo, voltage, wattage, pf, ohms, amps, autoEntry, (int?) maxRecording });
            maxRecording = (int?) result.GetParameterValue(11);
            return (int) result.ReturnValue;
        }

        public Table<DoorCount> DoorCounts
        {
            get
            {
                return base.GetTable<DoorCount>();
            }
        }

        public Table<FrameType> FrameTypes
        {
            get
            {
                return base.GetTable<FrameType>();
            }
        }

        public Table<Line> Lines
        {
            get
            {
                return base.GetTable<Line>();
            }
        }

        public Table<Model> Models
        {
            get
            {
                return base.GetTable<Model>();
            }
        }

        public Table<PowerTable> PowerTables
        {
            get
            {
                return base.GetTable<PowerTable>();
            }
        }

        public Table<Reading> Readings
        {
            get
            {
                return base.GetTable<Reading>();
            }
        }

        public Table<Revision> Revisions
        {
            get
            {
                return base.GetTable<Revision>();
            }
        }

        public Table<Temperature> Temperatures
        {
            get
            {
                return base.GetTable<Temperature>();
            }
        }

        public Table<Voltage> Voltages
        {
            get
            {
                return base.GetTable<Voltage>();
            }
        }
    }
}

