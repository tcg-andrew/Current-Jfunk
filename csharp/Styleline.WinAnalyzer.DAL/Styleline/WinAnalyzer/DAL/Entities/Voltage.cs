namespace Styleline.WinAnalyzer.DAL.Entities
{
    using System;
    using System.ComponentModel;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.Runtime.CompilerServices;

    [Table(Name="dbo.Voltages")]
    public class Voltage : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private string _HighVoltage;
        private int _Id;
        private string _LowVoltage;
        private EntitySet<PowerTable> _PowerTables;
        private string _VoltageName;
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(string.Empty);

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        public Voltage()
        {
            this._PowerTables = new EntitySet<PowerTable>(new Action<PowerTable>(this.attach_PowerTables), new Action<PowerTable>(this.detach_PowerTables));
        }

        private void attach_PowerTables(PowerTable entity)
        {
            this.SendPropertyChanging();
            entity.Voltage = this;
        }

        private void detach_PowerTables(PowerTable entity)
        {
            this.SendPropertyChanging();
            entity.Voltage = null;
        }

        protected virtual void SendPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void SendPropertyChanging()
        {
            if (this.PropertyChanging != null)
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        [Column(Storage="_HighVoltage", DbType="VarChar(3) NOT NULL", CanBeNull=false)]
        public string HighVoltage
        {
            get
            {
                return this._HighVoltage;
            }
            set
            {
                if (this._HighVoltage != value)
                {
                    this.SendPropertyChanging();
                    this._HighVoltage = value;
                    this.SendPropertyChanged("HighVoltage");
                }
            }
        }

        [Column(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
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
                    this.SendPropertyChanging();
                    this._Id = value;
                    this.SendPropertyChanged("Id");
                }
            }
        }

        [Column(Storage="_LowVoltage", DbType="VarChar(3) NOT NULL", CanBeNull=false)]
        public string LowVoltage
        {
            get
            {
                return this._LowVoltage;
            }
            set
            {
                if (this._LowVoltage != value)
                {
                    this.SendPropertyChanging();
                    this._LowVoltage = value;
                    this.SendPropertyChanged("LowVoltage");
                }
            }
        }

        [Association(Name="Voltage_PowerTable", Storage="_PowerTables", ThisKey="Id", OtherKey="VoltageId")]
        public EntitySet<PowerTable> PowerTables
        {
            get
            {
                return this._PowerTables;
            }
            set
            {
                this._PowerTables.Assign(value);
            }
        }

        [Column(Storage="_VoltageName", DbType="VarChar(3) NOT NULL", CanBeNull=false)]
        public string VoltageName
        {
            get
            {
                return this._VoltageName;
            }
            set
            {
                if (this._VoltageName != value)
                {
                    this.SendPropertyChanging();
                    this._VoltageName = value;
                    this.SendPropertyChanged("VoltageName");
                }
            }
        }
    }
}

