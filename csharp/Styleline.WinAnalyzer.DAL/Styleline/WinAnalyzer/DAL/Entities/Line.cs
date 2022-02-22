namespace Styleline.WinAnalyzer.DAL.Entities
{
    using System;
    using System.ComponentModel;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.Runtime.CompilerServices;

    [Table(Name="dbo.Lines")]
    public class Line : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private int _Id;
        private string _LineCode;
        private EntityRef<Styleline.WinAnalyzer.DAL.Entities.Temperature> _Temperature = new EntityRef<Styleline.WinAnalyzer.DAL.Entities.Temperature>();
        private int _TemperatureId;
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(string.Empty);

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

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

        [Column(Storage="_LineCode", DbType="VarChar(2) NOT NULL", CanBeNull=false)]
        public string LineCode
        {
            get
            {
                return this._LineCode;
            }
            set
            {
                if (this._LineCode != value)
                {
                    this.SendPropertyChanging();
                    this._LineCode = value;
                    this.SendPropertyChanged("LineCode");
                }
            }
        }

        [Association(Name="Temperature_Line", Storage="_Temperature", ThisKey="TemperatureId", OtherKey="Id", IsForeignKey=true)]
        public Styleline.WinAnalyzer.DAL.Entities.Temperature Temperature
        {
            get
            {
                return this._Temperature.Entity;
            }
            set
            {
                Styleline.WinAnalyzer.DAL.Entities.Temperature previousValue = this._Temperature.Entity;
                if ((previousValue != value) || !this._Temperature.HasLoadedOrAssignedValue)
                {
                    this.SendPropertyChanging();
                    if (previousValue != null)
                    {
                        this._Temperature.Entity = null;
                        previousValue.Lines.Remove(this);
                    }
                    this._Temperature.Entity = value;
                    if (value != null)
                    {
                        value.Lines.Add(this);
                        this._TemperatureId = value.Id;
                    }
                    else
                    {
                        this._TemperatureId = 0;
                    }
                    this.SendPropertyChanged("Temperature");
                }
            }
        }

        [Column(Storage="_TemperatureId", DbType="Int NOT NULL")]
        public int TemperatureId
        {
            get
            {
                return this._TemperatureId;
            }
            set
            {
                if (this._TemperatureId != value)
                {
                    if (this._Temperature.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.SendPropertyChanging();
                    this._TemperatureId = value;
                    this.SendPropertyChanged("TemperatureId");
                }
            }
        }
    }
}

