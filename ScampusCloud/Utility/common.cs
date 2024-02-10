using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ScampusCloud.Utility
{
    public static class common
    {
        #region common messages
        // Connection string name
        public static string DATE_FORMAT = "MM/dd/yyyy";
        #endregion

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public static DataTable ListToDataTable<T>(this IEnumerable<T> list) where T : class
        {
            DataTable dtOutput = new DataTable("tblOutput");

            //if the list is empty, return empty data table
            if (list.Count() == 0)
                return dtOutput;

            //get the list of  public properties and add them as columns to the output table           
            PropertyInfo[] properties = list.FirstOrDefault().GetType().
                GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo propertyInfo in properties)
                dtOutput.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);

            //populate rows
            DataRow dr;
            //iterate through all the objects in the list and add them as rows to the table
            foreach (T t in list)
            {
                dr = dtOutput.NewRow();
                //iterate through all the properties of the current object and set their values to data row
                foreach (PropertyInfo propertyInfo in properties)
                {
                    dr[propertyInfo.Name] = propertyInfo.GetValue(t, null);
                }
                dtOutput.Rows.Add(dr);
            }
            return dtOutput;
        }
    }
}