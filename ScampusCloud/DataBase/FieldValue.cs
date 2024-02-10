using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScampusCloud.DataBase
{
    public enum DataTypes
    {
        Text,
        Numeric,
        Date,
        Boolean,
        Structured
    }
    public class FieldValue
    {
        #region private fields
        private string _columnName;
        private object _columnValue;
        private bool _isIDentity;
        private DataTypes _columnType;
        private string _typeName;
        #endregion

        #region properties
        internal DataTypes ColumnType
        {
            get { return _columnType; }
            set { _columnType = value; }
        }

        public bool IsIDentity
        {
            get { return _isIDentity; }
            set { _isIDentity = value; }
        }

        public object ColumnValue
        {
            get { return _columnValue; }
            set { _columnValue = value; }
        }

        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
        public string TypeName
        {
            get { return _typeName; }
            set { _typeName = value; }
        }
        #endregion

        #region Constructors
        public FieldValue()
        {
        }

        public FieldValue(string columnName, object columnValue, DataTypes columnType, bool isIdentity,string typeName)
        {
            _columnName = columnName;
            _columnValue = columnValue;
            _columnType = columnType;
            _isIDentity = isIdentity;
            _typeName = typeName;
        }
        #endregion
    }
}