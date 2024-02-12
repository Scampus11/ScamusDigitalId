using ScampusCloud.DataBase;
using ScampusCloud.Models;
using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Repository.AccessGroup
{
    public class AccessGroupRepository
    {
        private GeneralMethods objgm = new GeneralMethods();

        public List<SelectListItem> BindAccessGroupTypeDropDown(string CompanyId)
        {
            QueryBuilder objQueryBuilder = new QueryBuilder
            {
                TableName = "Tbl_Mstr_AccessGroup_Type_Master",
                StoredProcedureName = @"Sps_Load_AccessGroup_Type_Dropdown",
                SetQueryType = QueryBuilder.QueryType.SELECT,
            };
            objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
            return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
        }
        public AccessGroupModel AddEdit_AccessGroup(AccessGroupModel _AccessGroupModel)
        {
            try
            {
                QueryBuilder objQueryBuilder = new QueryBuilder
                {
                    TableName = _AccessGroupModel.GetType().Name,
                    StoredProcedureName = @"SP_AccessGroup_CURD",
                    SetQueryType = QueryBuilder.QueryType.SELECT
                };
                if (_AccessGroupModel.ActionType == "Edit" ||  _AccessGroupModel.ActionType == "Remote")
                {
                    objQueryBuilder.AddFieldValue("@Id", _AccessGroupModel.Id, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@Code", _AccessGroupModel.Code, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@CompanyId", _AccessGroupModel.CompanyId, DataTypes.Text, false);
                }
                else
                {
                    objQueryBuilder.AddFieldValue("@Id", _AccessGroupModel.Id, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@Name", _AccessGroupModel.Name, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@CompanyId", _AccessGroupModel.CompanyId, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@Code", _AccessGroupModel.Code, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@Description", _AccessGroupModel.Description, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@AccessGroupTypeId", _AccessGroupModel.AccessGroupTypeId, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@IsCanteen", _AccessGroupModel.Is_Canteen, DataTypes.Boolean, false);
                    objQueryBuilder.AddFieldValue("@Isactive", _AccessGroupModel.IsActive, DataTypes.Boolean, false);
                    objQueryBuilder.AddFieldValue("@CreatedBy", _AccessGroupModel.CreatedBy, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@ModifiedBy", _AccessGroupModel.ModifiedBy, DataTypes.Text, false);
                    if (_AccessGroupModel.lstAccessGroupLevel != null && _AccessGroupModel.lstAccessGroupLevel.Count > 0)
                    {
                        DataTable dt = common.ToDataTable(_AccessGroupModel.lstAccessGroupLevel);
                        dt.Columns.Remove("ID");
                        dt.Columns.Remove("CompanyId");
                        dt.Columns.Remove("AccessGroupId");
                        dt.Columns.Remove("IsDeleted");
                        dt.Columns.Remove("IsActive");
                        dt.Columns.Remove("dtCreatedDate");
                        dt.Columns.Remove("dtModifiedDate");
                        dt.Columns.Remove("CreatedBy");
                        dt.Columns.Remove("ModifiedBy");
                        dt.Columns.Remove("DoorGroup");
                        dt.Columns.Remove("Session");
                        objQueryBuilder.AddFieldValue("@TempTable", dt, DataTypes.Structured, false, "AccessGroupLevelTableType");
                    }
                }
                objQueryBuilder.AddFieldValue("@ActionType", _AccessGroupModel.ActionType, DataTypes.Text, false);
                return objgm.ExecuteObjectUsingSp<AccessGroupModel>(objQueryBuilder);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public List<SelectListItem> BindDoorGroupDropDown(string CompanyId)
        {
            QueryBuilder objQueryBuilder = new QueryBuilder
            {
                TableName = "Tbl_Mstr_DoorGroup_Master",
                StoredProcedureName = @"Sps_Load_DoorGroup_Dropdown",
                SetQueryType = QueryBuilder.QueryType.SELECT,
            };
            objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
            return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
        }

        public List<SelectListItem> BindSessionDropDown(string CompanyId)
        {
            QueryBuilder objQueryBuilder = new QueryBuilder
            {
                TableName = "Tbl_Mstr_Session_Master",
                StoredProcedureName = @"Sps_Load_Session_Dropdown",
                SetQueryType = QueryBuilder.QueryType.SELECT,
            };
            objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
            return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
        }

        public List<AccessGroupModel> GetAccessGroupList(string searchtxt = "", int page = 1, int pagesize = 10, string CompanyId = null)
        {
            try
            {
                QueryBuilder objQueryBuilder = new QueryBuilder
                {
                    TableName = "Tbl_Mstr_AccessGroup_Master",
                    StoredProcedureName = @"SP_GetAccessGroupData",
                    SetQueryType = QueryBuilder.QueryType.SELECT
                };
                objQueryBuilder.AddFieldValue("@Search", searchtxt, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@page", page, DataTypes.Numeric, false);
                objQueryBuilder.AddFieldValue("@pagesize", pagesize, DataTypes.Numeric, false);
                objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
                return objgm.GetListUsingSp<AccessGroupModel>(objQueryBuilder);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }


        public List<AccessGroupLevelModel> GetAccessGroupLevelList(int? Id,string CompanyId = null)
        {
            try
            {
                QueryBuilder objQueryBuilder = new QueryBuilder
                {
                    TableName = "Tbl_Mstr_AccessGroup_level_Master",
                    StoredProcedureName = @"SP_GetAccessGroupLevelData",
                    SetQueryType = QueryBuilder.QueryType.SELECT
                };
                objQueryBuilder.AddFieldValue("@AccessGroupId", Id, DataTypes.Numeric, false);
                objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
                return objgm.GetListUsingSp<AccessGroupLevelModel>(objQueryBuilder);
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
                    TableName = "Tbl_Mstr_AccessGroup_Master",
                    StoredProcedureName = @"SP_GetAccessGroupData_Count",
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
        public DataTable GetAccessGroupData_Export(string searchtxt = "", string CompanyId = null)
        {
            try
            {
                QueryBuilder objQueryBuilder = new QueryBuilder
                {
                    TableName = "Tbl_Mstr_AccessGroup_Master",
                    StoredProcedureName = @"SP_DownloadAceessGroupData_Export",
                    SetQueryType = QueryBuilder.QueryType.SELECT
                };
                objQueryBuilder.AddFieldValue("@Search", searchtxt, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
                return objgm.ExecuteDataTableUsingSp(objQueryBuilder, true);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }
    }
}