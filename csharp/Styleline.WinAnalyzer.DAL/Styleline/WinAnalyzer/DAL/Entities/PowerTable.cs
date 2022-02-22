namespace Styleline.WinAnalyzer.DAL.Entities
{
    using System;
    using System.ComponentModel;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.Runtime.CompilerServices;

    [Table(Name="dbo.PowerTable")]
    public class PowerTable : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private EntityRef<Styleline.WinAnalyzer.DAL.Entities.DoorCount> _DoorCount = new EntityRef<Styleline.WinAnalyzer.DAL.Entities.DoorCount>();
        private int _DoorCountId;
        private string _drdramhi;
        private string _drdramlo;
        private string _drdrohhi;
        private string _drdrohlo;
        private string _drfrohhi;
        private string _drfrohlo;
        private string _drfrwire;
        private string _drglohhi;
        private string _drglohlo;
        private EntityRef<Styleline.WinAnalyzer.DAL.Entities.FrameType> _FrameType = new EntityRef<Styleline.WinAnalyzer.DAL.Entities.FrameType>();
        private int _FrameTypeId;
        private string _frfwohhi;
        private string _frfwohlo;
        private string _frmuohhi;
        private string _frmuohlo;
        private string _frmuwire;
        private string _frstohhi;
        private string _frstohlo;
        private string _frstwire;
        private string _frtfamhi;
        private string _frtfamlo;
        private string _frtfohhi;
        private string _frtfohlo;
        private string _frtmohhi;
        private string _frtmohlo;
        private string _frtsohhi;
        private string _frtsohlo;
        private string _frw1ohhi;
        private string _frw1ohlo;
        private string _frw1wire;
        private string _frw2ohhi;
        private string _frw2ohlo;
        private string _frw2wire;
        private string _consump;
        private string _surface;
        private int _Id;
        private bool _IsDoor;
        private bool _IsFrame;
        private string _item;
        private string _ltampshi;
        private string _ltampslo;
        private EntityRef<Styleline.WinAnalyzer.DAL.Entities.Model> _Model = new EntityRef<Styleline.WinAnalyzer.DAL.Entities.Model>();
        private int? _ModelId;
        private EntityRef<Styleline.WinAnalyzer.DAL.Entities.Revision> _Revision = new EntityRef<Styleline.WinAnalyzer.DAL.Entities.Revision>();
        private int _RevisionId;
        private string _sudfamhi;
        private string _sudfamlo;
        private string _sudlamhi;
        private string _sudlamlo;
        private string _suflamhi;
        private string _suflamlo;
        private string _sumxamhe;
        private string _sumxamlt;
        private string _sumxamto;
        private string _surtamhe;
        private string _surtamlt;
        private EntityRef<Styleline.WinAnalyzer.DAL.Entities.Temperature> _Temperature = new EntityRef<Styleline.WinAnalyzer.DAL.Entities.Temperature>();
        private int? _TemperatureId;
        private EntityRef<Styleline.WinAnalyzer.DAL.Entities.Voltage> _Voltage = new EntityRef<Styleline.WinAnalyzer.DAL.Entities.Voltage>();
        private int _VoltageId;
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

        [Association(Name="DoorCount_PowerTable", Storage="_DoorCount", ThisKey="DoorCountId", OtherKey="Id", IsForeignKey=true)]
        public Styleline.WinAnalyzer.DAL.Entities.DoorCount DoorCount
        {
            get
            {
                return this._DoorCount.Entity;
            }
            set
            {
                Styleline.WinAnalyzer.DAL.Entities.DoorCount previousValue = this._DoorCount.Entity;
                if ((previousValue != value) || !this._DoorCount.HasLoadedOrAssignedValue)
                {
                    this.SendPropertyChanging();
                    if (previousValue != null)
                    {
                        this._DoorCount.Entity = null;
                        previousValue.PowerTables.Remove(this);
                    }
                    this._DoorCount.Entity = value;
                    if (value != null)
                    {
                        value.PowerTables.Add(this);
                        this._DoorCountId = value.Id;
                    }
                    else
                    {
                        this._DoorCountId = 0;
                    }
                    this.SendPropertyChanged("DoorCount");
                }
            }
        }

        [Column(Storage="_DoorCountId", DbType="Int NOT NULL")]
        public int DoorCountId
        {
            get
            {
                return this._DoorCountId;
            }
            set
            {
                if (this._DoorCountId != value)
                {
                    if (this._DoorCount.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.SendPropertyChanging();
                    this._DoorCountId = value;
                    this.SendPropertyChanged("DoorCountId");
                }
            }
        }

        [Column(Storage = "_consump", DbType = "NVarChar(20) NOT NULL", CanBeNull = false)]
        public string consump
        {
            get
            {
                return this._consump;
            }
            set
            {
                if (this._consump != value)
                {
                    this.SendPropertyChanging();
                    this._consump = value;
                    this.SendPropertyChanged("consump");
                }
            }
        }

        [Column(Storage = "_surface", DbType = "NVarchar(10) NOT NULL", CanBeNull = false)]
        public string surface
        {
            get
            {
                return this._surface;
            }
            set
            {
                if (this._surface != value)
                {
                    this.SendPropertyChanging();
                    this._surface = value;
                    this.SendPropertyChanged("surface");
                }
            }
        }

        [Column(Storage="_drdramhi", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
        public string drdramhi
        {
            get
            {
                return this._drdramhi;
            }
            set
            {
                if (this._drdramhi != value)
                {
                    this.SendPropertyChanging();
                    this._drdramhi = value;
                    this.SendPropertyChanged("drdramhi");
                }
            }
        }

        [Column(Storage="_drdramlo", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
        public string drdramlo
        {
            get
            {
                return this._drdramlo;
            }
            set
            {
                if (this._drdramlo != value)
                {
                    this.SendPropertyChanging();
                    this._drdramlo = value;
                    this.SendPropertyChanged("drdramlo");
                }
            }
        }

        [Column(Storage="_drdrohhi", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string drdrohhi
        {
            get
            {
                return this._drdrohhi;
            }
            set
            {
                if (this._drdrohhi != value)
                {
                    this.SendPropertyChanging();
                    this._drdrohhi = value;
                    this.SendPropertyChanged("drdrohhi");
                }
            }
        }

        [Column(Storage="_drdrohlo", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
        public string drdrohlo
        {
            get
            {
                return this._drdrohlo;
            }
            set
            {
                if (this._drdrohlo != value)
                {
                    this.SendPropertyChanging();
                    this._drdrohlo = value;
                    this.SendPropertyChanged("drdrohlo");
                }
            }
        }

        [Column(Storage="_drfrohhi", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string drfrohhi
        {
            get
            {
                return this._drfrohhi;
            }
            set
            {
                if (this._drfrohhi != value)
                {
                    this.SendPropertyChanging();
                    this._drfrohhi = value;
                    this.SendPropertyChanged("drfrohhi");
                }
            }
        }

        [Column(Storage="_drfrohlo", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string drfrohlo
        {
            get
            {
                return this._drfrohlo;
            }
            set
            {
                if (this._drfrohlo != value)
                {
                    this.SendPropertyChanging();
                    this._drfrohlo = value;
                    this.SendPropertyChanged("drfrohlo");
                }
            }
        }

        [Column(Storage="_drfrwire", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string drfrwire
        {
            get
            {
                return this._drfrwire;
            }
            set
            {
                if (this._drfrwire != value)
                {
                    this.SendPropertyChanging();
                    this._drfrwire = value;
                    this.SendPropertyChanged("drfrwire");
                }
            }
        }

        [Column(Storage="_drglohhi", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string drglohhi
        {
            get
            {
                return this._drglohhi;
            }
            set
            {
                if (this._drglohhi != value)
                {
                    this.SendPropertyChanging();
                    this._drglohhi = value;
                    this.SendPropertyChanged("drglohhi");
                }
            }
        }

        [Column(Storage="_drglohlo", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string drglohlo
        {
            get
            {
                return this._drglohlo;
            }
            set
            {
                if (this._drglohlo != value)
                {
                    this.SendPropertyChanging();
                    this._drglohlo = value;
                    this.SendPropertyChanged("drglohlo");
                }
            }
        }

        [Association(Name="FrameType_PowerTable", Storage="_FrameType", ThisKey="FrameTypeId", OtherKey="Id", IsForeignKey=true)]
        public Styleline.WinAnalyzer.DAL.Entities.FrameType FrameType
        {
            get
            {
                return this._FrameType.Entity;
            }
            set
            {
                Styleline.WinAnalyzer.DAL.Entities.FrameType previousValue = this._FrameType.Entity;
                if ((previousValue != value) || !this._FrameType.HasLoadedOrAssignedValue)
                {
                    this.SendPropertyChanging();
                    if (previousValue != null)
                    {
                        this._FrameType.Entity = null;
                        previousValue.PowerTables.Remove(this);
                    }
                    this._FrameType.Entity = value;
                    if (value != null)
                    {
                        value.PowerTables.Add(this);
                        this._FrameTypeId = value.Id;
                    }
                    else
                    {
                        this._FrameTypeId = 0;
                    }
                    this.SendPropertyChanged("FrameType");
                }
            }
        }

        [Column(Storage="_FrameTypeId", DbType="Int NOT NULL")]
        public int FrameTypeId
        {
            get
            {
                return this._FrameTypeId;
            }
            set
            {
                if (this._FrameTypeId != value)
                {
                    if (this._FrameType.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.SendPropertyChanging();
                    this._FrameTypeId = value;
                    this.SendPropertyChanged("FrameTypeId");
                }
            }
        }

        [Column(Storage="_frfwohhi", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string frfwohhi
        {
            get
            {
                return this._frfwohhi;
            }
            set
            {
                if (this._frfwohhi != value)
                {
                    this.SendPropertyChanging();
                    this._frfwohhi = value;
                    this.SendPropertyChanged("frfwohhi");
                }
            }
        }

        [Column(Storage="_frfwohlo", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string frfwohlo
        {
            get
            {
                return this._frfwohlo;
            }
            set
            {
                if (this._frfwohlo != value)
                {
                    this.SendPropertyChanging();
                    this._frfwohlo = value;
                    this.SendPropertyChanged("frfwohlo");
                }
            }
        }

        [Column(Storage="_frmuohhi", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
        public string frmuohhi
        {
            get
            {
                return this._frmuohhi;
            }
            set
            {
                if (this._frmuohhi != value)
                {
                    this.SendPropertyChanging();
                    this._frmuohhi = value;
                    this.SendPropertyChanged("frmuohhi");
                }
            }
        }

        [Column(Storage="_frmuohlo", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
        public string frmuohlo
        {
            get
            {
                return this._frmuohlo;
            }
            set
            {
                if (this._frmuohlo != value)
                {
                    this.SendPropertyChanging();
                    this._frmuohlo = value;
                    this.SendPropertyChanged("frmuohlo");
                }
            }
        }

        [Column(Storage="_frmuwire", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
        public string frmuwire
        {
            get
            {
                return this._frmuwire;
            }
            set
            {
                if (this._frmuwire != value)
                {
                    this.SendPropertyChanging();
                    this._frmuwire = value;
                    this.SendPropertyChanged("frmuwire");
                }
            }
        }

        [Column(Storage="_frstohhi", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string frstohhi
        {
            get
            {
                return this._frstohhi;
            }
            set
            {
                if (this._frstohhi != value)
                {
                    this.SendPropertyChanging();
                    this._frstohhi = value;
                    this.SendPropertyChanged("frstohhi");
                }
            }
        }

        [Column(Storage="_frstohlo", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string frstohlo
        {
            get
            {
                return this._frstohlo;
            }
            set
            {
                if (this._frstohlo != value)
                {
                    this.SendPropertyChanging();
                    this._frstohlo = value;
                    this.SendPropertyChanged("frstohlo");
                }
            }
        }

        [Column(Storage="_frstwire", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string frstwire
        {
            get
            {
                return this._frstwire;
            }
            set
            {
                if (this._frstwire != value)
                {
                    this.SendPropertyChanging();
                    this._frstwire = value;
                    this.SendPropertyChanged("frstwire");
                }
            }
        }

        [Column(Storage="_frtfamhi", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string frtfamhi
        {
            get
            {
                return this._frtfamhi;
            }
            set
            {
                if (this._frtfamhi != value)
                {
                    this.SendPropertyChanging();
                    this._frtfamhi = value;
                    this.SendPropertyChanged("frtfamhi");
                }
            }
        }

        [Column(Storage="_frtfamlo", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string frtfamlo
        {
            get
            {
                return this._frtfamlo;
            }
            set
            {
                if (this._frtfamlo != value)
                {
                    this.SendPropertyChanging();
                    this._frtfamlo = value;
                    this.SendPropertyChanged("frtfamlo");
                }
            }
        }

        [Column(Storage="_frtfohhi", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string frtfohhi
        {
            get
            {
                return this._frtfohhi;
            }
            set
            {
                if (this._frtfohhi != value)
                {
                    this.SendPropertyChanging();
                    this._frtfohhi = value;
                    this.SendPropertyChanged("frtfohhi");
                }
            }
        }

        [Column(Storage="_frtfohlo", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string frtfohlo
        {
            get
            {
                return this._frtfohlo;
            }
            set
            {
                if (this._frtfohlo != value)
                {
                    this.SendPropertyChanging();
                    this._frtfohlo = value;
                    this.SendPropertyChanged("frtfohlo");
                }
            }
        }

        [Column(Storage="_frtmohhi", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
        public string frtmohhi
        {
            get
            {
                return this._frtmohhi;
            }
            set
            {
                if (this._frtmohhi != value)
                {
                    this.SendPropertyChanging();
                    this._frtmohhi = value;
                    this.SendPropertyChanged("frtmohhi");
                }
            }
        }

        [Column(Storage="_frtmohlo", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
        public string frtmohlo
        {
            get
            {
                return this._frtmohlo;
            }
            set
            {
                if (this._frtmohlo != value)
                {
                    this.SendPropertyChanging();
                    this._frtmohlo = value;
                    this.SendPropertyChanged("frtmohlo");
                }
            }
        }

        [Column(Storage="_frtsohhi", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string frtsohhi
        {
            get
            {
                return this._frtsohhi;
            }
            set
            {
                if (this._frtsohhi != value)
                {
                    this.SendPropertyChanging();
                    this._frtsohhi = value;
                    this.SendPropertyChanged("frtsohhi");
                }
            }
        }

        [Column(Storage="_frtsohlo", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string frtsohlo
        {
            get
            {
                return this._frtsohlo;
            }
            set
            {
                if (this._frtsohlo != value)
                {
                    this.SendPropertyChanging();
                    this._frtsohlo = value;
                    this.SendPropertyChanged("frtsohlo");
                }
            }
        }

        [Column(Storage="_frw1ohhi", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string frw1ohhi
        {
            get
            {
                return this._frw1ohhi;
            }
            set
            {
                if (this._frw1ohhi != value)
                {
                    this.SendPropertyChanging();
                    this._frw1ohhi = value;
                    this.SendPropertyChanged("frw1ohhi");
                }
            }
        }

        [Column(Storage="_frw1ohlo", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string frw1ohlo
        {
            get
            {
                return this._frw1ohlo;
            }
            set
            {
                if (this._frw1ohlo != value)
                {
                    this.SendPropertyChanging();
                    this._frw1ohlo = value;
                    this.SendPropertyChanged("frw1ohlo");
                }
            }
        }

        [Column(Storage="_frw1wire", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string frw1wire
        {
            get
            {
                return this._frw1wire;
            }
            set
            {
                if (this._frw1wire != value)
                {
                    this.SendPropertyChanging();
                    this._frw1wire = value;
                    this.SendPropertyChanged("frw1wire");
                }
            }
        }

        [Column(Storage="_frw2ohhi", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
        public string frw2ohhi
        {
            get
            {
                return this._frw2ohhi;
            }
            set
            {
                if (this._frw2ohhi != value)
                {
                    this.SendPropertyChanging();
                    this._frw2ohhi = value;
                    this.SendPropertyChanged("frw2ohhi");
                }
            }
        }

        [Column(Storage="_frw2ohlo", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
        public string frw2ohlo
        {
            get
            {
                return this._frw2ohlo;
            }
            set
            {
                if (this._frw2ohlo != value)
                {
                    this.SendPropertyChanging();
                    this._frw2ohlo = value;
                    this.SendPropertyChanged("frw2ohlo");
                }
            }
        }

        [Column(Storage="_frw2wire", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string frw2wire
        {
            get
            {
                return this._frw2wire;
            }
            set
            {
                if (this._frw2wire != value)
                {
                    this.SendPropertyChanging();
                    this._frw2wire = value;
                    this.SendPropertyChanged("frw2wire");
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

        [Column(Storage="_IsDoor", DbType="Bit NOT NULL")]
        public bool IsDoor
        {
            get
            {
                return this._IsDoor;
            }
            set
            {
                if (this._IsDoor != value)
                {
                    this.SendPropertyChanging();
                    this._IsDoor = value;
                    this.SendPropertyChanged("IsDoor");
                }
            }
        }

        [Column(Storage="_IsFrame", DbType="Bit NOT NULL")]
        public bool IsFrame
        {
            get
            {
                return this._IsFrame;
            }
            set
            {
                if (this._IsFrame != value)
                {
                    this.SendPropertyChanging();
                    this._IsFrame = value;
                    this.SendPropertyChanged("IsFrame");
                }
            }
        }

        [Column(Storage="_item", DbType="VarChar(15)")]
        public string item
        {
            get
            {
                return this._item;
            }
            set
            {
                if (this._item != value)
                {
                    this.SendPropertyChanging();
                    this._item = value;
                    this.SendPropertyChanged("item");
                }
            }
        }

        [Column(Storage="_ltampshi", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
        public string ltampshi
        {
            get
            {
                return this._ltampshi;
            }
            set
            {
                if (this._ltampshi != value)
                {
                    this.SendPropertyChanging();
                    this._ltampshi = value;
                    this.SendPropertyChanged("ltampshi");
                }
            }
        }

        [Column(Storage="_ltampslo", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
        public string ltampslo
        {
            get
            {
                return this._ltampslo;
            }
            set
            {
                if (this._ltampslo != value)
                {
                    this.SendPropertyChanging();
                    this._ltampslo = value;
                    this.SendPropertyChanged("ltampslo");
                }
            }
        }

        [Association(Name="Model_PowerTable", Storage="_Model", ThisKey="ModelId", OtherKey="Id", IsForeignKey=true)]
        public Styleline.WinAnalyzer.DAL.Entities.Model Model
        {
            get
            {
                return this._Model.Entity;
            }
            set
            {
                Styleline.WinAnalyzer.DAL.Entities.Model previousValue = this._Model.Entity;
                if ((previousValue != value) || !this._Model.HasLoadedOrAssignedValue)
                {
                    this.SendPropertyChanging();
                    if (previousValue != null)
                    {
                        this._Model.Entity = null;
                        previousValue.PowerTables.Remove(this);
                    }
                    this._Model.Entity = value;
                    if (value != null)
                    {
                        value.PowerTables.Add(this);
                        this._ModelId = new int?(value.Id);
                    }
                    else
                    {
                        this._ModelId = null;
                    }
                    this.SendPropertyChanged("Model");
                }
            }
        }

        [Column(Storage="_ModelId", DbType="Int")]
        public int? ModelId
        {
            get
            {
                return this._ModelId;
            }
            set
            {
                if (this._ModelId != value)
                {
                    if (this._Model.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.SendPropertyChanging();
                    this._ModelId = value;
                    this.SendPropertyChanged("ModelId");
                }
            }
        }

        [Association(Name="Revision_PowerTable", Storage="_Revision", ThisKey="RevisionId", OtherKey="Id", IsForeignKey=true)]
        public Styleline.WinAnalyzer.DAL.Entities.Revision Revision
        {
            get
            {
                return this._Revision.Entity;
            }
            set
            {
                Styleline.WinAnalyzer.DAL.Entities.Revision previousValue = this._Revision.Entity;
                if ((previousValue != value) || !this._Revision.HasLoadedOrAssignedValue)
                {
                    this.SendPropertyChanging();
                    if (previousValue != null)
                    {
                        this._Revision.Entity = null;
                        previousValue.PowerTables.Remove(this);
                    }
                    this._Revision.Entity = value;
                    if (value != null)
                    {
                        value.PowerTables.Add(this);
                        this._RevisionId = value.Id;
                    }
                    else
                    {
                        this._RevisionId = 0;
                    }
                    this.SendPropertyChanged("Revision");
                }
            }
        }

        [Column(Storage="_RevisionId", DbType="Int NOT NULL")]
        public int RevisionId
        {
            get
            {
                return this._RevisionId;
            }
            set
            {
                if (this._RevisionId != value)
                {
                    if (this._Revision.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.SendPropertyChanging();
                    this._RevisionId = value;
                    this.SendPropertyChanged("RevisionId");
                }
            }
        }

        [Column(Storage="_sudfamhi", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
        public string sudfamhi
        {
            get
            {
                return this._sudfamhi;
            }
            set
            {
                if (this._sudfamhi != value)
                {
                    this.SendPropertyChanging();
                    this._sudfamhi = value;
                    this.SendPropertyChanged("sudfamhi");
                }
            }
        }

        [Column(Storage="_sudfamlo", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
        public string sudfamlo
        {
            get
            {
                return this._sudfamlo;
            }
            set
            {
                if (this._sudfamlo != value)
                {
                    this.SendPropertyChanging();
                    this._sudfamlo = value;
                    this.SendPropertyChanged("sudfamlo");
                }
            }
        }

        [Column(Storage="_sudlamhi", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
        public string sudlamhi
        {
            get
            {
                return this._sudlamhi;
            }
            set
            {
                if (this._sudlamhi != value)
                {
                    this.SendPropertyChanging();
                    this._sudlamhi = value;
                    this.SendPropertyChanged("sudlamhi");
                }
            }
        }

        [Column(Storage="_sudlamlo", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
        public string sudlamlo
        {
            get
            {
                return this._sudlamlo;
            }
            set
            {
                if (this._sudlamlo != value)
                {
                    this.SendPropertyChanging();
                    this._sudlamlo = value;
                    this.SendPropertyChanged("sudlamlo");
                }
            }
        }

        [Column(Storage="_suflamhi", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
        public string suflamhi
        {
            get
            {
                return this._suflamhi;
            }
            set
            {
                if (this._suflamhi != value)
                {
                    this.SendPropertyChanging();
                    this._suflamhi = value;
                    this.SendPropertyChanged("suflamhi");
                }
            }
        }

        [Column(Storage="_suflamlo", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
        public string suflamlo
        {
            get
            {
                return this._suflamlo;
            }
            set
            {
                if (this._suflamlo != value)
                {
                    this.SendPropertyChanging();
                    this._suflamlo = value;
                    this.SendPropertyChanged("suflamlo");
                }
            }
        }

        [Column(Storage="_sumxamhe", DbType="NVarChar(11) NOT NULL", CanBeNull=false)]
        public string sumxamhe
        {
            get
            {
                return this._sumxamhe;
            }
            set
            {
                if (this._sumxamhe != value)
                {
                    this.SendPropertyChanging();
                    this._sumxamhe = value;
                    this.SendPropertyChanged("sumxamhe");
                }
            }
        }

        [Column(Storage="_sumxamlt", DbType="NVarChar(11) NOT NULL", CanBeNull=false)]
        public string sumxamlt
        {
            get
            {
                return this._sumxamlt;
            }
            set
            {
                if (this._sumxamlt != value)
                {
                    this.SendPropertyChanging();
                    this._sumxamlt = value;
                    this.SendPropertyChanged("sumxamlt");
                }
            }
        }

        [Column(Storage="_sumxamto", DbType="NVarChar(11) NOT NULL", CanBeNull=false)]
        public string sumxamto
        {
            get
            {
                return this._sumxamto;
            }
            set
            {
                if (this._sumxamto != value)
                {
                    this.SendPropertyChanging();
                    this._sumxamto = value;
                    this.SendPropertyChanged("sumxamto");
                }
            }
        }

        [Column(Storage="_surtamhe", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
        public string surtamhe
        {
            get
            {
                return this._surtamhe;
            }
            set
            {
                if (this._surtamhe != value)
                {
                    this.SendPropertyChanging();
                    this._surtamhe = value;
                    this.SendPropertyChanged("surtamhe");
                }
            }
        }

        [Column(Storage="_surtamlt", DbType="NVarChar(9) NOT NULL", CanBeNull=false)]
        public string surtamlt
        {
            get
            {
                return this._surtamlt;
            }
            set
            {
                if (this._surtamlt != value)
                {
                    this.SendPropertyChanging();
                    this._surtamlt = value;
                    this.SendPropertyChanged("surtamlt");
                }
            }
        }

        [Association(Name="Temperature_PowerTable", Storage="_Temperature", ThisKey="TemperatureId", OtherKey="Id", IsForeignKey=true)]
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
                        previousValue.PowerTables.Remove(this);
                    }
                    this._Temperature.Entity = value;
                    if (value != null)
                    {
                        value.PowerTables.Add(this);
                        this._TemperatureId = new int?(value.Id);
                    }
                    else
                    {
                        this._TemperatureId = null;
                    }
                    this.SendPropertyChanged("Temperature");
                }
            }
        }

        [Column(Storage="_TemperatureId", DbType="Int")]
        public int? TemperatureId
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

        [Association(Name="Voltage_PowerTable", Storage="_Voltage", ThisKey="VoltageId", OtherKey="Id", IsForeignKey=true)]
        public Styleline.WinAnalyzer.DAL.Entities.Voltage Voltage
        {
            get
            {
                return this._Voltage.Entity;
            }
            set
            {
                Styleline.WinAnalyzer.DAL.Entities.Voltage previousValue = this._Voltage.Entity;
                if ((previousValue != value) || !this._Voltage.HasLoadedOrAssignedValue)
                {
                    this.SendPropertyChanging();
                    if (previousValue != null)
                    {
                        this._Voltage.Entity = null;
                        previousValue.PowerTables.Remove(this);
                    }
                    this._Voltage.Entity = value;
                    if (value != null)
                    {
                        value.PowerTables.Add(this);
                        this._VoltageId = value.Id;
                    }
                    else
                    {
                        this._VoltageId = 0;
                    }
                    this.SendPropertyChanged("Voltage");
                }
            }
        }

        [Column(Storage="_VoltageId", DbType="Int NOT NULL")]
        public int VoltageId
        {
            get
            {
                return this._VoltageId;
            }
            set
            {
                if (this._VoltageId != value)
                {
                    if (this._Voltage.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.SendPropertyChanging();
                    this._VoltageId = value;
                    this.SendPropertyChanged("VoltageId");
                }
            }
        }
    }
}

