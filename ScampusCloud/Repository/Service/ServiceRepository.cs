using DocumentFormat.OpenXml.Office2010.Excel;
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

namespace ScampusCloud.Repository.Service
{
    public class ServicesRepository
    {
        private GeneralMethods objgm = new GeneralMethods();

        public List<ServiceModel> GetServicesList(string searchtxt = "", int page = 1, int pagesize = 10, string CompanyId = null)
        {
            try
            {
                QueryBuilder objQueryBuilder = new QueryBuilder
                {
                    TableName = "Tbl_Mstr_Service_Master",
                    StoredProcedureName = @"SP_GetServicesData",
                    SetQueryType = QueryBuilder.QueryType.SELECT
                };
                objQueryBuilder.AddFieldValue("@Search", searchtxt, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@page", page, DataTypes.Numeric, false);
                objQueryBuilder.AddFieldValue("@pagesize", pagesize, DataTypes.Numeric, false);
                objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
                return objgm.GetListUsingSp<ServiceModel>(objQueryBuilder);
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
                    TableName = "Tbl_Mstr_Reader",
                    StoredProcedureName = @"SP_GetServicesData_Count",
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

        public ServiceModel AddEdit_Service(ServiceModel _ServiceModel)
        {
            try
            {
                QueryBuilder objQueryBuilder = new QueryBuilder
                {
                    TableName = _ServiceModel.GetType().Name,
                    StoredProcedureName = @"SP_Services_CURD",
                    SetQueryType = QueryBuilder.QueryType.SELECT
                };
                if (_ServiceModel.ActionType == "Remote")
                {
                    objQueryBuilder.AddFieldValue("@Code", _ServiceModel.Code, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@CompanyId", _ServiceModel.CompanyId, DataTypes.Text, false);
                }
                else
                {
                    objQueryBuilder.AddFieldValue("@Id", _ServiceModel.Id, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@Name", _ServiceModel.Name, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@CompanyId", _ServiceModel.CompanyId, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@Code", _ServiceModel.Code, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@Description", _ServiceModel.Description, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@DeptId", _ServiceModel.DepartmentId, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@StartDateTime", _ServiceModel.StartDateTime, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@EndDateTime", _ServiceModel.EndDateTime, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@EmployeeIds", _ServiceModel.EmployeeIds, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@Isactive", _ServiceModel.IsActive, DataTypes.Boolean, false);
                    objQueryBuilder.AddFieldValue("@CreatedBy", _ServiceModel.CreatedBy, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@ModifiedBy", _ServiceModel.ModifiedBy, DataTypes.Text, false);
                }
                objQueryBuilder.AddFieldValue("@ActionType", _ServiceModel.ActionType, DataTypes.Text, false);

                return objgm.ExecuteObjectUsingSp<ServiceModel>(objQueryBuilder);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public List<SelectListItem> BindAvailableStaffAccessGroupDropDown(string CompanyId, int Id, String ActionType)
        {
            QueryBuilder objQueryBuilder = new QueryBuilder
            {
                TableName = "Tbl_Mstr_Staff_Master",
                StoredProcedureName = @"Sps_Load_Available_Staff_AccessGroup_Dropdown",
                SetQueryType = QueryBuilder.QueryType.SELECT,
            };
            objQueryBuilder.AddFieldValue("@Id", Id, DataTypes.Numeric, false);
            objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
            objQueryBuilder.AddFieldValue("@ActionType", ActionType, DataTypes.Text, false);
            return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
        }

        public DataTable GetServicesData_Export(string searchtxt = "", string CompanyId = null)
        {
            try
            {
                QueryBuilder objQueryBuilder = new QueryBuilder
                {
                    TableName = "Services",
                    StoredProcedureName = @"SP_DownloadServicesData_Export",
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