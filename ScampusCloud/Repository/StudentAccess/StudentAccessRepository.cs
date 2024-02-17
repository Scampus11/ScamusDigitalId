using ScampusCloud.DataBase;
using ScampusCloud.Models;
using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ScampusCloud.Repository.StudentAccess
{
    public class StudentAccessRepository 
    {
        private GeneralMethods objgm = new GeneralMethods();

        public List<StudentAccessGroupModel> GetStudentAccessList(string searchtxt = "", int page = 1, int pagesize = 10,int CampusId = 0, int CollegeId = 0, int DepartmentId = 0, int YearId = 0, int AdmissionTypeId = 0, string CompanyId = null)
        {
            try
            {
                QueryBuilder objQueryBuilder = new QueryBuilder
                {
                    TableName = "Tbl_Mstr_Student_Master",
                    StoredProcedureName = @"SP_GetStudentAccessGroupData",
                    SetQueryType = QueryBuilder.QueryType.SELECT
                };
                objQueryBuilder.AddFieldValue("@Search", searchtxt, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@page", page, DataTypes.Numeric, false);
                objQueryBuilder.AddFieldValue("@pagesize", pagesize, DataTypes.Numeric, false);
                objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@CampusId", CampusId, DataTypes.Numeric, false);
                objQueryBuilder.AddFieldValue("@CollegeId", CollegeId, DataTypes.Numeric, false);
                objQueryBuilder.AddFieldValue("@DepartmentId", DepartmentId, DataTypes.Numeric, false);
                objQueryBuilder.AddFieldValue("@Year", YearId, DataTypes.Numeric, false);
                objQueryBuilder.AddFieldValue("@AdmissionTypeId", AdmissionTypeId, DataTypes.Numeric, false);
                return objgm.GetListUsingSp<StudentAccessGroupModel>(objQueryBuilder);
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
                    TableName = "Tbl_Mstr_Student_Master",
                    StoredProcedureName = @"SP_GetStudentAccessGroupData",
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