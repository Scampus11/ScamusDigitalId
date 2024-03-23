using ScampusCloud.DataBase;
using ScampusCloud.Models;
using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ScampusCloud.Repository.Visitor
{
    public class VisitorRepository
    {
        private GeneralMethods objgm = new GeneralMethods();

        public List<VisitorSelfRegistrationModel> GetVisitorList(string searchtxt = "", int page = 1, int pagesize = 10, string CompanyId = null)
        {
            try
            {
                QueryBuilder objQueryBuilder = new QueryBuilder
                {
                    TableName = "Tbl_Mstr_Visitor_Master",
                    StoredProcedureName = @"SP_GetVisitorData",
                    SetQueryType = QueryBuilder.QueryType.SELECT
                };
                objQueryBuilder.AddFieldValue("@Search", searchtxt, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@page", page, DataTypes.Numeric, false);
                objQueryBuilder.AddFieldValue("@pagesize", pagesize, DataTypes.Numeric, false);
                objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
                return objgm.GetListUsingSp<VisitorSelfRegistrationModel>(objQueryBuilder);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public string GetAllCount(string searchtxt = "", string CompanyId = null)
        {
            try
            {
                QueryBuilder objQueryBuilder = new QueryBuilder
                {
                    TableName = "Tbl_Mstr_Visitor_Master",
                    StoredProcedureName = @"SP_GetVisitorData_Count",
                    SetQueryType = QueryBuilder.QueryType.SELECT,
                };
                objQueryBuilder.AddFieldValue("@Search", searchtxt, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
                return objgm.ExcecuteScalarUsingSp(objQueryBuilder);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }

        }

    }
}