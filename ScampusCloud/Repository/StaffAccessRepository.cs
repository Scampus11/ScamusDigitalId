﻿using ScampusCloud.DataBase;
using ScampusCloud.Models;
using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ScampusCloud.Repository
{
    public class StaffAccessRepository
    {
        private GeneralMethods objgm = new GeneralMethods();
        public List<StaffAccessGroupModel> GetStaffAccessList(string searchtxt = "", int page = 1, int pagesize = 10, int DepartmentId = 0,string CompanyId = null)
        {
            try
            {
                QueryBuilder objQueryBuilder = new QueryBuilder
                {
                    TableName = "Tbl_Mstr_Staff_Master",
                    StoredProcedureName = @"SP_GetStaffAccessGroupData",
                    SetQueryType = QueryBuilder.QueryType.SELECT
                };
                objQueryBuilder.AddFieldValue("@Search", searchtxt, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@page", page, DataTypes.Numeric, false);
                objQueryBuilder.AddFieldValue("@pagesize", pagesize, DataTypes.Numeric, false);
                objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@DepartmentId", DepartmentId, DataTypes.Numeric, false);
                return objgm.GetListUsingSp<StaffAccessGroupModel>(objQueryBuilder);
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
                    TableName = "Tbl_Mstr_Staff_Master",
                    StoredProcedureName = @"SP_GetStaffAccessGroup_Count",
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
        public StaffAccessGroupModel Assign_StaffAccessGroup(List<StaffAccessGroupModel> data, StaffAccessGroupModel _AccessGroupModel)
        {
            try
            {
                QueryBuilder objQueryBuilder = new QueryBuilder
                {
                    TableName = _AccessGroupModel.GetType().Name,
                    StoredProcedureName = @"SP_Assign_StaffAccessGroup",
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
                    dt.Columns.Remove("DepartmentId");
                    dt.Columns.Remove("lstDepartment");
                    dt.Columns.Remove("AccessGroupTypeId");
                    dt.Columns.Remove("lstAccessGroupDropdown");
                    dt.Columns.Remove("Department");
                    dt.Columns.Remove("AccessGroupId");
                    dt.Columns.Remove("CanteenType");
                    dt.Columns.Remove("IsEdit");
                    dt.Columns.Remove("ActionType");
                    dt.Columns.Remove("StaffIds");
                    objQueryBuilder.AddFieldValue("@TempTable", dt, DataTypes.Structured, false, "StaffAccessGroupType");
                }
                objQueryBuilder.AddFieldValue("@StaffIds", _AccessGroupModel.StaffIds, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@ActionType", _AccessGroupModel.ActionType, DataTypes.Text, false);
                return objgm.ExecuteObjectUsingSp<StaffAccessGroupModel>(objQueryBuilder);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }
    }
}