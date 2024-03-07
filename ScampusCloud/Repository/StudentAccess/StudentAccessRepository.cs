using ScampusCloud.DataBase;
using ScampusCloud.Models;
using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

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
                    StoredProcedureName = @"SP_GetStudentAccessGroup_Count",
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
        public StudentAccessGroupModel Assign_StudentAccessGroup(List<StudentAccessGroupModel> data, StudentAccessGroupModel _AccessGroupModel)
        {
            try
            {
                QueryBuilder objQueryBuilder = new QueryBuilder
                {
                    TableName = _AccessGroupModel.GetType().Name,
                    StoredProcedureName = @"SP_Assign_StudentAccessGroup",
                    SetQueryType = QueryBuilder.QueryType.SELECT
                };
                objQueryBuilder.AddFieldValue("@CompanyId", _AccessGroupModel.CompanyId, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@Isactive", _AccessGroupModel.IsActive, DataTypes.Boolean, false);
                objQueryBuilder.AddFieldValue("@CreatedBy", _AccessGroupModel.CreatedBy, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@ModifiedBy", _AccessGroupModel.ModifiedBy, DataTypes.Text, false);
                if (data != null && data.Count > 0)
                {
                    DataTable dt = common.ToDataTable(data);
                    dt.Columns.Remove("ID");
                    dt.Columns.Remove("CompanyId");
                    dt.Columns.Remove("BlockId");
                    dt.Columns.Remove("IsDeleted");
                    dt.Columns.Remove("IsActive");
                    dt.Columns.Remove("dtCreatedDate");
                    dt.Columns.Remove("dtModifiedDate");
                    dt.Columns.Remove("CreatedBy");
                    dt.Columns.Remove("ModifiedBy");
                    dt.Columns.Remove("lstCollage");
                    dt.Columns.Remove("DepartmentId");
                    dt.Columns.Remove("lstDepartment");
                    dt.Columns.Remove("CampusId");
                    dt.Columns.Remove("lstCampus");
                    dt.Columns.Remove("YearId");
                    dt.Columns.Remove("lstYear");
                    dt.Columns.Remove("AdmissionTypeId");
                    dt.Columns.Remove("lstAdmissionType");
                    dt.Columns.Remove("Gender");
                    dt.Columns.Remove("AccessGroupTypeId");
                    dt.Columns.Remove("lstAccessGroupDropdown");
                    dt.Columns.Remove("CollageId");
                    dt.Columns.Remove("College");
                    dt.Columns.Remove("Department");
                    dt.Columns.Remove("AdmissionType");
                    dt.Columns.Remove("Campus");
                    dt.Columns.Remove("BatchYear");
                    dt.Columns.Remove("AccessGroupId");
                    dt.Columns.Remove("CanteenType");
                    dt.Columns.Remove("IsEdit");
                    dt.Columns.Remove("ActionType");
                    dt.Columns.Remove("StudentIds");
                    dt.Columns.Remove("ImagePath");
                    dt.Columns.Remove("ImageBase64");
                    objQueryBuilder.AddFieldValue("@TempTable", dt, DataTypes.Structured, false, "StudentAccessGroupType");
                }
                objQueryBuilder.AddFieldValue("@StudentIds", _AccessGroupModel.StudentIds, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@ActionType", _AccessGroupModel.ActionType, DataTypes.Text, false);
                return objgm.ExecuteObjectUsingSp<StudentAccessGroupModel>(objQueryBuilder);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public StudentAccessMasterModel AddEdit_StudentAccessGroup(StudentAccessMasterModel _StudentAccessMasterModel)
        {
            try
            {
                QueryBuilder objQueryBuilder = new QueryBuilder
                {
                    TableName = _StudentAccessMasterModel.GetType().Name,
                    StoredProcedureName = @"SP_StudentAccessGroup_CURD",
                    SetQueryType = QueryBuilder.QueryType.SELECT
                };
                if (_StudentAccessMasterModel.ActionType == "Remote" || _StudentAccessMasterModel.ActionType == "Edit")
                {
                    objQueryBuilder.AddFieldValue("@Id", _StudentAccessMasterModel.Id, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@CompanyId", _StudentAccessMasterModel.CompanyId, DataTypes.Text, false);
                }
                else
                {
                    objQueryBuilder.AddFieldValue("@Id", _StudentAccessMasterModel.Id, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@StudentId", _StudentAccessMasterModel.StudentId, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@CompanyId", _StudentAccessMasterModel.CompanyId, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@CanteenType", _StudentAccessMasterModel.AccessGroupTypeId, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@AccessGroupId", _StudentAccessMasterModel.AccessGroupId, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@Isactive", _StudentAccessMasterModel.IsActive, DataTypes.Boolean, false);
                    objQueryBuilder.AddFieldValue("@CreatedBy", _StudentAccessMasterModel.CreatedBy, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@ModifiedBy", _StudentAccessMasterModel.ModifiedBy, DataTypes.Text, false);
                }
                objQueryBuilder.AddFieldValue("@ActionType", _StudentAccessMasterModel.ActionType, DataTypes.Text, false);

                return objgm.ExecuteObjectUsingSp<StudentAccessMasterModel>(objQueryBuilder);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public List<SelectListItem> BindAvailableAccessGroupDropDown(int Id,string ActionType,string GroupName)
        {
            QueryBuilder objQueryBuilder = new QueryBuilder
            {
                TableName = "Tbl_Mstr_AccessGroup_Master",
                StoredProcedureName = @"SP_GetAvailableAssgined_Group",
                SetQueryType = QueryBuilder.QueryType.SELECT,
            };
            objQueryBuilder.AddFieldValue("@Id", Id, DataTypes.Numeric, false);
            objQueryBuilder.AddFieldValue("@ActionType", ActionType, DataTypes.Text, false);
            objQueryBuilder.AddFieldValue("@GroupName", GroupName, DataTypes.Text, false);
            return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
        }
    }
}