#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ModuleBase
{
    public class GridColumn
    {
        #region Values

        private string _name;
        private Type _type;

        #endregion

        #region Properties

        public bool Visible { get; set; }
        public bool ReadOnly { get; set; }
        public bool Frozen { get; set; }
        public string Name { get { return _name; } }
        public Type DataType { get { return _type; } }
        public string Format { get; set; }

        #endregion

        #region Constructors

        public GridColumn()
        {
            _type = typeof(string);
            Format = "";
            ReadOnly = true;
            Visible = true;
        }

        public GridColumn(string name) : this()
        {
            _name = name;
        }

        public GridColumn(string name, Type t)
            : this()
        {
            _name = name;
            _type = t;
        }

        #endregion
    }
}
