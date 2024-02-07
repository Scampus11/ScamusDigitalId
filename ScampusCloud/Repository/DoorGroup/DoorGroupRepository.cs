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

namespace ScampusCloud.Repository.DoorGroup
{
    public class DoorGroupRepository
    {
        private GeneralMethods objgm = new GeneralMethods();
        public List<SelectListItem> BindReaderDropDown(string CompanyId)
        {
            QueryBuilder objQueryBuilder = new QueryBuilder
            {
                TableName = "Tbl_Mstr_Reader_Master",
                StoredProcedureName = @"Sps_Load_Reader_Dropdown",
                SetQueryType = QueryBuilder.QueryType.SELECT,
            };
            objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
            return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
        }

        public DoorGroupModel AddEdit_DoorGroup(DoorGroupModel _DoorGroupModel)
        {
            try
            {
                QueryBuilder objQueryBuilder = new QueryBuilder
                {
                    TableName = _DoorGroupModel.GetType().Name,
                    StoredProcedureName = @"SP_DoorGroup_CURD",
                    SetQueryType = QueryBuilder.QueryType.SELECT
                };
                if (_DoorGroupModel.ActionType == "Remote")
                {
                    objQueryBuilder.AddFieldValue("@Code", _DoorGroupModel.Code, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@CompanyId", _DoorGroupModel.CompanyId, DataTypes.Text, false);
                }
                else
                {
                    objQueryBuilder.AddFieldValue("@Id", _DoorGroupModel.Id, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@Name", _DoorGroupModel.Name, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@CompanyId", _DoorGroupModel.CompanyId, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@Code", _DoorGroupModel.Code, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@Description", _DoorGroupModel.Description, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@ReaderId", _DoorGroupModel.ReaderId, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@Isactive", _DoorGroupModel.IsActive, DataTypes.Boolean, false);
                    objQueryBuilder.AddFieldValue("@CreatedBy", _DoorGroupModel.CreatedBy, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@ModifiedBy", _DoorGroupModel.ModifiedBy, DataTypes.Text, false);
                }
                objQueryBuilder.AddFieldValue("@ActionType", _DoorGroupModel.ActionType, DataTypes.Text, false);

                return objgm.ExecuteObjectUsingSp<DoorGroupModel>(objQueryBuilder);
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
                    TableName = "Tbl_Mstr_DoorGroup_Master",
                    StoredProcedureName = @"SP_GetDoorGroupData_Count",
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

        public List<DoorGroupModel> GetDoorGroupList(string searchtxt = "", int page = 1, int pagesize = 10, string CompanyId = null)
        {
            try
            {
                QueryBuilder objQueryBuilder = new QueryBuilder
                {
                    TableName = "Tbl_Mstr_DoorGroup_Master",
                    StoredProcedureName = @"SP_GetDoorGroupData",
                    SetQueryType = QueryBuilder.QueryType.SELECT
                };
                objQueryBuilder.AddFieldValue("@Search", searchtxt, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@page", page, DataTypes.Numeric, false);
                objQueryBuilder.AddFieldValue("@pagesize", pagesize, DataTypes.Numeric, false);
                objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
                return objgm.GetListUsingSp<DoorGroupModel>(objQueryBuilder);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }
        public DataTable GetReaderData_Export(string searchtxt = "", string CompanyId = null)
        {
            try
            {
                QueryBuilder objQueryBuilder = new QueryBuilder
                {
                    TableName = "DoorGroup",
                    StoredProcedureName = @"SP_DownloadDoorGroupData_Export",
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