namespace Styleline.WinAnalyzer.DAL.Entities
{
    using System;
    using System.ComponentModel;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.Runtime.CompilerServices;

    [Table(Name="dbo.Models")]
    public class Model : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private int _Id;
        private string _ModelCode;
        private EntitySet<PowerTable> _PowerTables;
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(string.Empty);

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        public Model()
        {
            this._PowerTables = new EntitySet<PowerTable>(new Action<PowerTable>(this.attach_PowerTables), new Action<PowerTable>(this.detach_PowerTables));
        }

        private void attach_PowerTables(PowerTable entity)
        {
            this.SendPropertyChanging();
            entity.Model = this;
        }

        private void detach_PowerTables(PowerTable entity)
        {
            this.SendPropertyChanging();
            entity.Model = null;
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

        [Column(Storage="_ModelCode", DbType="VarChar(2) NOT NULL", CanBeNull=false)]
        public string ModelCode
        {
            get
            {
                return this._ModelCode;
            }
            set
            {
                if (this._ModelCode != value)
                {
                    this.SendPropertyChanging();
                    this._ModelCode = value;
                    this.SendPropertyChanged("ModelCode");
                }
            }
        }

        [Association(Name="Model_PowerTable", Storage="_PowerTables", ThisKey="Id", OtherKey="ModelId")]
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

