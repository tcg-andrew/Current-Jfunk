namespace Styleline.WinAnalyzer.DAL.Repositories
{
    using Styleline.WinAnalyzer.DAL;
    using Styleline.WinAnalyzer.DAL.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class PowerAnalyzerRepository : IPowerAnalyzerRepository
    {
        public PowerAnalyzerRepository() : this(Db.GetContext())
        {
        }

        public PowerAnalyzerRepository(PowerAnalyzerDataContext context)
        {
            this.Context = context;
        }

        public IEnumerable<PowerTable> GetCurrentReferenceSet()
        {
            return (from p in this.Context.PowerTables
                where p.Revision.IsCurrent
                orderby p.Model.ModelCode, p.Temperature.TempCode, p.DoorCount.CountName, p.FrameType.FrameTypeCode
                select p);
        }

        public IEnumerable<DoorCount> GetDoorCounts()
        {
            return (from d in this.Context.DoorCounts
                orderby d.CountName
                select d);
        }

        public IEnumerable<FrameType> GetFrameTypes()
        {
            return (from f in this.Context.FrameTypes
                orderby f.FrameTypeCode
                select f);
        }

        public IEnumerable<Line> GetLines()
        {
            return (from l in this.Context.Lines
                orderby l.LineCode
                select l);
        }

        public IEnumerable<Model> GetModels()
        {
            return (from c in this.Context.Models
                orderby c.ModelCode
                select c);
        }

        public PowerTable GetPowerTable(int doorCountId, int modelId, int lineId, int frameTypeId, int voltageId)
        {
            return (from p in this.Context.PowerTables
                    where (((((p.Model.Id == modelId) && (p.DoorCountId == doorCountId)) && p.Temperature.Lines.Any<Line>(l => (l.Id == lineId))) && (p.FrameType.Id == frameTypeId)) && (p.Voltage.Id == voltageId)) && /*p.Revision.Id == 36*/p.Revision.IsCurrent
                select p).FirstOrDefault<PowerTable>();
        }

        public IEnumerable<Temperature> GetTemperatures()
        {
            return (from t in this.Context.Temperatures
                orderby t.TempCode
                select t);
        }

        public IEnumerable<Voltage> GetVoltages()
        {
            return (from v in this.Context.Voltages
                orderby v.VoltageName
                select v);
        }

        public int InsertReading(int powerTableId, string jobNumber, string assemblyNumber, char type, string employeeNo, decimal voltage, decimal wattage, decimal pf, decimal ohms, decimal amps, bool autoEntry, ref int? maxRecording)
        {
            return this.Context.InsertReading(new int?(powerTableId), jobNumber, assemblyNumber, new char?(type), employeeNo, new decimal?(voltage), new decimal?(wattage), new decimal?(pf), new decimal?(ohms), new decimal?(amps), new bool?(autoEntry), ref maxRecording);
        }

        public PowerAnalyzerDataContext Context { get; set; }
    }
}

