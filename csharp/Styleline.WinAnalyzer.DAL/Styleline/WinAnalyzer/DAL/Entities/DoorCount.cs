namespace Styleline.WinAnalyzer.DAL.Entities
{
    using System;
    using System.ComponentModel;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.Runtime.CompilerServices;

    [Table(Name="dbo.DoorCounts")]
    public class DoorCount : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private string _CountName;
        private int _Id;
        private int _Number;
        private EntitySet<PowerTable> _PowerTables;
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(string.Empty);

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        public DoorCount()
        {
            this._PowerTables = new EntitySet<PowerTable>(new Action<PowerTable>(this.attach_PowerTables), new Action<PowerTable>(this.detach_PowerTables));
        }

        private void attach_PowerTables(PowerTable entity)
        {
            this.SendPropertyChanging();
            entity.DoorCount = this;
        }

        private void detach_PowerTables(PowerTable entity)
        {
            this.SendPropertyChanging();
            entity.DoorCount = null;
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

        [Column(Storage="_CountName", DbType="VarChar(2) NOT NULL", CanBeNull=false)]
        public string CountName
        {
            get
            {
                return this._CountName;
            }
            set
            {
                if (this._CountName != value)
                {
                    this.SendPropertyChanging();
                    this._CountName = value;
                    this.SendPropertyChanged("CountName");
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

        [Column(Storage="_Number", DbType="Int NOT NULL")]
        public int Number
        {
            get
            {
                return this._Number;
            }
            set
            {
                if (this._Number != value)
                {
                    this.SendPropertyChanging();
                    this._Number = value;
                    this.SendPropertyChanged("Number");
                }
            }
        }

        [Association(Name="DoorCount_PowerTable", Storage="_PowerTables", ThisKey="Id", OtherKey="DoorCountId")]
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
    }
}

