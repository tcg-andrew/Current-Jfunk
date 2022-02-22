namespace Styleline.WinAnalyzer.DAL.Repositories
{
    using Styleline.WinAnalyzer.DAL;
    using Styleline.WinAnalyzer.DAL.Entities;
    using System;
    using System.Collections.Generic;

    public interface IPowerAnalyzerRepository
    {
        IEnumerable<PowerTable> GetCurrentReferenceSet();
        IEnumerable<DoorCount> GetDoorCounts();
        IEnumerable<FrameType> GetFrameTypes();
        IEnumerable<Line> GetLines();
        IEnumerable<Model> GetModels();
        PowerTable GetPowerTable(int doorCountId, int modelId, int lineId, int frameTypeId, int voltageId);
        IEnumerable<Temperature> GetTemperatures();
        IEnumerable<Voltage> GetVoltages();
        int InsertReading(int powerTableId, string jobNumber, string assemblyNumber, char type, string employeeNo, decimal voltage, decimal wattage, decimal pf, decimal ohms, decimal amps, bool autoEntry, ref int? maxRecording);

        PowerAnalyzerDataContext Context { get; set; }
    }
}

