#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Epicor.Mfg.Core;
using System.Reflection;

#endregion

namespace ModuleBase
{
    public enum ModuleState { Unloaded, Loading, Loaded, Saving, Printing, Saved }
    public enum ModuleAction { Save, Print, Favorite, Refresh }

    public abstract class Module
    {
        #region Values

        protected DataTable _data;
        protected ModuleState _state;
        protected DateTime _lastUpdate;
        protected List<ModuleAction> _actions;
        protected List<GridColumn> _columns;
        protected Session _session;
        protected string _server;
        protected string _database;

        #endregion

        #region Properties

        public ModuleState State { get { return _state; } }
        public DateTime LastUpdate { get { return _lastUpdate; } }
        public DataTable Data { get { return _data; } }
        public List<ModuleAction> Actions { get { return _actions; } }
        public List<GridColumn> Columns { get { return _columns; } }
        public Session Session { get { return _session; } }
        public string Server { get { return _server; } }
        public string Database { get { return _database; } }

        #endregion

        #region Constructor

        public Module(Session session, string server, string database)
        {
            _state = ModuleState.Unloaded;
            _data = new DataTable();
            _actions = new List<ModuleAction>();
            _columns = new List<GridColumn>();
            _session = session;
            _server = server;
            _database = database;

        }

        #endregion

        #region Public Methods

        public bool LoadData(Dictionary<string, object> args)
        {
            _state = ModuleState.Loading;

            _data = DataMethod(args);

            _lastUpdate = DateTime.Now;
            _state = ModuleState.Loaded;

            return true;
        }

        public bool SaveData()
        {
            _state = ModuleState.Saving;

            SaveMethod();

            _state = ModuleState.Saved;
            return true;
        }

        public void Unload()
        {
            _state = ModuleState.Unloaded;
            _data = null;
        }

        public void Presave()
        {
            _state = ModuleState.Saving;
        }

        #endregion

        #region Protected Methods

        protected abstract DataTable DataMethod(Dictionary<string, object> args);

        protected abstract bool SaveMethod();

        protected DataTable GenerateGridTable()
        {
            DataTable dt = new DataTable();
            foreach (GridColumn column in _columns)
            {
                System.Data.DataColumn c = new DataColumn();
                c.ColumnName = column.Name;
                c.ReadOnly = column.ReadOnly;
                c.DataType = column.DataType;
                dt.Columns.Add(c);
            }
            return dt;
        }

        #endregion
    }
}
