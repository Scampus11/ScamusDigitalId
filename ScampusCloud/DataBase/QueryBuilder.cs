using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ScampusCloud.DataBase
{
    public class QueryBuilder
    {
        #region private members
        private string _tableName = string.Empty;
        private List<FieldValue> _fieldValueCollection = new List<FieldValue>();
        private string _storedProcedureName = string.Empty;
        public enum QueryType
        {
            INSERT = 1,
            UPDATE = 2,
            DELETE = 3,
            SELECT = 4
        }

        public Enum SetQueryType;
        #endregion

        #region properties
        internal List<FieldValue> FieldValueCollection
        {
            get { return _fieldValueCollection; }
            //set { _fieldValueCollection = value; }
        }

        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        public string StoredProcedureName
        {
            get { return _storedProcedureName; }
            set { _storedProcedureName = value; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// This function adds a new column in the FieldValueCollection
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <param name="columnValue">Value of that column</param>
        /// <param name="columnType">Data type of the column</param>
        public void AddFieldValue(string columnName, object columnValue, DataTypes columnType, bool isIdentity,string typeName = "")
        {
            FieldValue field = new FieldValue(columnName, columnValue, columnType, isIdentity,typeName);
            FieldValueCollection.Add(field);
        }

        public string InsertQuery()
        {
            StringBuilder srInsert = new StringBuilder();
            string columns = " (";
            string values = " Values (";

            try
            {
                if (TableName == string.Empty)
                {
                    throw new Exception("TableName does not specified.");
                }

                if (FieldValueCollection == null)
                {
                    throw new Exception("No columns specified.");
                }

                srInsert.Append("Insert into ");
                srInsert.Append(TableName);

                foreach (FieldValue objField in FieldValueCollection)
                {
                    if (objField.ColumnValue != null)
                    {
                        columns += objField.ColumnName + ",";

                        switch (objField.ColumnType)
                        {
                            case DataTypes.Numeric:
                                values += objField.ColumnValue == DBNull.Value ? "NULL" : Convert.ToString(objField.ColumnValue);
                                values += ",";
                                break;
                            case DataTypes.Date:
                                values += " '" + Convert.ToString(objField.ColumnValue) + "',";
                                break;
                            case DataTypes.Text:
                                values += " '" + Convert.ToString(Convert.ToString(objField.ColumnValue).Replace("'", "''")) + "',";
                                break;
                            case DataTypes.Boolean:
                                values += (objField.ColumnValue == DBNull.Value ? "NULL" : Convert.ToString(objField.ColumnValue) == "False" ? "0" : "1") + ",";
                                break;
                        }
                    }
                }

                columns = columns.Remove(columns.Length - 1);   // remove last comma
                columns += ")";

                values = values.Remove(values.Length - 1);   // remove last comma
                values += ")";


                srInsert.Append(columns);
                srInsert.Append(values);

            }
            catch
            {
                throw;
            }
            return srInsert.ToString();
        }

        /// <summary>
        /// This will escape the special character.
        /// </summary>
        /// <param name="strQuery"></param>
        /// <returns></returns>
        private string HandleSpecialCharacters(string strQuery)
        {
            // Single quote.
            strQuery = strQuery.Replace("'", "''");
            return strQuery;
        }


        public string UpdateRecord(FieldValue whereClauseFieldValue)
        {
            StringBuilder srUpdate = new StringBuilder();
            string columns = " ";

            try
            {
                if (TableName == string.Empty)
                {
                    throw new Exception("TableName does not specified.");
                }

                if (FieldValueCollection.Count <= 0)
                {
                    throw new Exception("No columns specified.");
                }

                srUpdate.Append("Update ");
                srUpdate.Append(TableName);
                srUpdate.Append(" set ");

                foreach (FieldValue objField in FieldValueCollection)
                {
                    columns += objField.ColumnName + "=";

                    switch (objField.ColumnType)
                    {
                        case DataTypes.Numeric:

                            columns += objField.ColumnValue == DBNull.Value ? "NULL" : Convert.ToString(objField.ColumnValue) == String.Empty ? "0" : Convert.ToString(objField.ColumnValue);
                            columns += ",";
                            break;
                        case DataTypes.Date:
                            columns += "'" + Convert.ToString(objField.ColumnValue) + "',";
                            break;
                        case DataTypes.Text:
                            columns += "'" + Convert.ToString(objField.ColumnValue).Replace("'", "''") + "',";
                            break;
                        case DataTypes.Boolean:
                            columns += (objField.ColumnValue == DBNull.Value ? "NULL" : Convert.ToString(objField.ColumnValue) == "False" ? "0" : "1") + ",";
                            break;
                    }
                }

                columns = columns.Remove(columns.Length - 1);   // remove last comma
                srUpdate.Append(columns);
                srUpdate.Append(" where ");
                srUpdate.Append(whereClauseFieldValue.ColumnName + "=");

                string strValue = string.Empty;
                switch (whereClauseFieldValue.ColumnType)
                {
                    case DataTypes.Numeric:
                        strValue = Convert.ToString(whereClauseFieldValue.ColumnValue);
                        break;
                    case DataTypes.Date:
                        strValue = "'" + Convert.ToString(whereClauseFieldValue.ColumnValue) + "'";
                        break;
                    case DataTypes.Text:
                        strValue = "'" + Convert.ToString(whereClauseFieldValue.ColumnValue).Replace("'", "''") + "'";
                        break;

                }
                srUpdate.Append(strValue);

            }
            catch
            {
                throw;
            }
            return Convert.ToString(srUpdate);
        }

        public string UpdateRecord(List<FieldValue> whereClauseFieldValues)
        {
            StringBuilder srUpdate = new StringBuilder();
            string columns = " ";

            try
            {
                if (TableName == string.Empty)
                {
                    throw new Exception("TableName does not specified.");
                }

                if (FieldValueCollection.Count <= 0)
                {
                    throw new Exception("No columns specified.");
                }

                srUpdate.Append("Update ");
                srUpdate.Append(TableName);
                srUpdate.Append(" set ");

                foreach (FieldValue objField in FieldValueCollection)
                {
                    if (objField.ColumnValue != null)
                    {
                        columns += objField.ColumnName + "=";

                        switch (objField.ColumnType)
                        {
                            case DataTypes.Numeric:
                                columns += objField.ColumnValue == DBNull.Value ? "NULL" : Convert.ToString(objField.ColumnValue);
                                columns += ",";
                                break;
                            case DataTypes.Date:
                                columns += (Convert.ToDateTime(objField.ColumnValue).ToString(common.DATE_FORMAT) == "01/01/1753" ? "NULL" : " '" + Convert.ToString(objField.ColumnValue) + "' ") + ",";
                                break;
                            case DataTypes.Text:
                                columns += " '" + Convert.ToString(Convert.ToString(objField.ColumnValue).Replace("'", "''")) + "',";
                                break;
                            case DataTypes.Boolean:
                                columns += (objField.ColumnValue == DBNull.Value ? "NULL" : Convert.ToString(objField.ColumnValue) == "False" ? "0" : "1") + ",";
                                break;
                        }
                    }
                }

                columns = columns.Remove(columns.Length - 1);   // remove last comma
                srUpdate.Append(columns);
                srUpdate.Append(" where ");


                string strValue = string.Empty;
                foreach (FieldValue wherefield in whereClauseFieldValues)
                {
                    //srUpdate.Append(wherefield.ColumnName + "=");
                    strValue += wherefield.ColumnName + "=";

                    switch (wherefield.ColumnType)
                    {
                        case DataTypes.Numeric:
                            strValue += Convert.ToString(wherefield.ColumnValue);
                            break;
                        case DataTypes.Date:
                            strValue += "'" + Convert.ToString(wherefield.ColumnValue) + "'";
                            break;
                        case DataTypes.Text:
                            strValue += "'" + Convert.ToString(wherefield.ColumnValue).Replace("'", "''") + "'";
                            break;
                    }
                    strValue += " and ";

                }
                strValue = strValue.Remove(strValue.Length - 5);    // remove last " and "
                srUpdate.Append(strValue);

            }
            catch
            {
                throw;
            }
            return srUpdate.ToString();
        }

        public string DeleteRecord(List<FieldValue> whereClauseFieldValues)
        {
            StringBuilder srDelete = new StringBuilder();

            try
            {
                if (TableName == string.Empty)
                {
                    throw new Exception("TableName does not specified.");
                }

                srDelete.Append("Delete from ");
                srDelete.Append(_tableName);
                srDelete.Append(" where ");
                //srDelete.Append(whereClauseFieldValue.ColumnName + "=");

                string strValue = string.Empty;
                foreach (FieldValue wherefield in whereClauseFieldValues)
                {
                    //srUpdate.Append(wherefield.ColumnName + "=");
                    strValue += wherefield.ColumnName + "=";
                    switch (wherefield.ColumnType)
                    {
                        case DataTypes.Numeric:
                            strValue += Convert.ToString(wherefield.ColumnValue);
                            break;
                        case DataTypes.Date:
                            strValue += "'" + Convert.ToString(wherefield.ColumnValue) + "'";
                            break;
                        case DataTypes.Text:
                            strValue += "'" + Convert.ToString(wherefield.ColumnValue).Replace("'", "''") + "'";
                            break;
                    }
                    strValue += " and ";

                }
                strValue = strValue.Remove(strValue.Length - 5);    // remove last " and "
                srDelete.Append(strValue);

            }
            catch
            {
                throw;
            }
            return srDelete.ToString();
        }

        public string DeleteRecord(FieldValue whereClauseFieldValue)
        {
            StringBuilder srDelete = new StringBuilder();

            try
            {
                if (TableName == string.Empty)
                {
                    throw new Exception("TableName does not specified.");
                }

                srDelete.Append("Delete from ");
                srDelete.Append(_tableName);
                srDelete.Append(" where ");
                srDelete.Append(whereClauseFieldValue.ColumnName + "=");

                string strValue = string.Empty;
                switch (whereClauseFieldValue.ColumnType)
                {
                    case DataTypes.Numeric:
                        strValue += Convert.ToString(whereClauseFieldValue.ColumnValue);
                        break;
                    case DataTypes.Date:
                        strValue += "'" + Convert.ToString(whereClauseFieldValue.ColumnValue) + "'";
                        break;
                    case DataTypes.Text:
                        strValue += "'" + Convert.ToString(whereClauseFieldValue.ColumnValue).Replace("'", "''") + "'";
                        break;
                }
                srDelete.Append(strValue);

            }
            catch
            {
                throw;
            }
            return srDelete.ToString();
        }
        #endregion
    }
}