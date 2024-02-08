using ScampusCloud.DataBase;
using ScampusCloud.Models;
using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
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
                if (_AccessGroupModel.ActionType == "Remote")
                {
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
                    objQueryBuilder.AddFieldValue("@Isactive", _AccessGroupModel.IsActive, DataTypes.Boolean, false);
                    objQueryBuilder.AddFieldValue("@CreatedBy", _AccessGroupModel.CreatedBy, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@ModifiedBy", _AccessGroupModel.ModifiedBy, DataTypes.Text, false);
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
    }
}