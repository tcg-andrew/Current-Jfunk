namespace Styleline.WinAnalyzer.DAL.Entities
{
    using System;
    using System.ComponentModel;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.Runtime.CompilerServices;

    [Table(Name="dbo.Temperatures")]
    public class Temperature : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private int _Id;
        private EntitySet<Line> _Lines;
        private EntitySet<PowerTable> _PowerTables;
        private string _TempCode;
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(string.Empty);

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        public Temperature()
        {
            this._Lines = new EntitySet<Line>(new Action<Line>(this.attach_Lines), new Action<Line>(this.detach_Lines));
            this._PowerTables = new EntitySet<PowerTable>(new Action<PowerTable>(this.attach_PowerTables), new Action<PowerTable>(this.detach_PowerTables));
        }

        private void attach_Lines(Line entity)
        {
            this.SendPropertyChanging();
            entity.Temperature = this;
        }

        private void attach_PowerTables(PowerTable entity)
        {
            this.SendPropertyChanging();
            entity.Temperature = this;
        }

        private void detach_Lines(Line entity)
        {
            this.SendPropertyChanging();
            entity.Temperature = null;
        }

        private void detach_PowerTables(PowerTable entity)
        {
            this.SendPropertyChanging();
            entity.Temperature = null;
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

        [Association(Name="Temperature_Line", Storage="_Lines", ThisKey="Id", OtherKey="TemperatureId")]
        public EntitySet<Line> Lines
        {
            get
            {
                return this._Lines;
            }
            set
            {
                this._Lines.Assign(value);
            }
        }

        [Association(Name="Temperature_PowerTable", Storage="_PowerTables", ThisKey="Id", OtherKey="TemperatureId")]
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

        [Column(Storage="_TempCode", DbType="VarChar(6) NOT NULL", CanBeNull=false)]
        public string TempCode
        {
            get
            {
                return this._TempCode;
            }
            set
            {
                if (this._TempCode != value)
                {
                    this.SendPropertyChanging();
                    this._TempCode = value;
                    this.SendPropertyChanged("TempCode");
                }
            }
        }
    }
}

