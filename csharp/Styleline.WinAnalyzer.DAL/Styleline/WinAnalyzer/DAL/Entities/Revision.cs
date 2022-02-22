namespace Styleline.WinAnalyzer.DAL.Entities
{
    using System;
    using System.ComponentModel;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.Runtime.CompilerServices;

    [Table(Name="dbo.Revisions")]
    public class Revision : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private int _Id;
        private bool _IsCurrent;
        private EntitySet<PowerTable> _PowerTables;
        private string _RevisionName;
        private int _RevisionNumber;
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(string.Empty);

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        public Revision()
        {
            this._PowerTables = new EntitySet<PowerTable>(new Action<PowerTable>(this.attach_PowerTables), new Action<PowerTable>(this.detach_PowerTables));
        }

        private void attach_PowerTables(PowerTable entity)
        {
            this.SendPropertyChanging();
            entity.Revision = this;
        }

        private void detach_PowerTables(PowerTable entity)
        {
            this.SendPropertyChanging();
            entity.Revision = null;
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

        [Column(Storage="_IsCurrent", DbType="Bit NOT NULL")]
        public bool IsCurrent
        {
            get
            {
                return this._IsCurrent;
            }
            set
            {
                if (this._IsCurrent != value)
                {
                    this.SendPropertyChanging();
                    this._IsCurrent = value;
                    this.SendPropertyChanged("IsCurrent");
                }
            }
        }

        [Association(Name="Revision_PowerTable", Storage="_PowerTables", ThisKey="Id", OtherKey="RevisionId")]
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

        [Column(Storage="_RevisionName", DbType="VarChar(10)")]
        public string RevisionName
        {
            get
            {
                return this._RevisionName;
            }
            set
            {
                if (this._RevisionName != value)
                {
                    this.SendPropertyChanging();
                    this._RevisionName = value;
                    this.SendPropertyChanged("RevisionName");
                }
            }
        }

        [Column(Storage="_RevisionNumber", DbType="Int NOT NULL")]
        public int RevisionNumber
        {
            get
            {
                return this._RevisionNumber;
            }
            set
            {
                if (this._RevisionNumber != value)
                {
                    this.SendPropertyChanging();
                    this._RevisionNumber = value;
                    this.SendPropertyChanged("RevisionNumber");
                }
            }
        }
    }
}

