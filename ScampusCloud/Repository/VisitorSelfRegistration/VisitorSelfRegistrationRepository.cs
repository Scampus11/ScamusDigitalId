using ScampusCloud.DataBase;
using ScampusCloud.Models;
using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Repository.VisitorSelfRegistration
{

    public class VisitorSelfRegistrationRepository
    {
        private GeneralMethods objgm = new GeneralMethods();

        public VisitorSelfRegistrationModel AddEdit_VisitorSelfRegistration(VisitorSelfRegistrationModel _VisitorSelfRegistrationModel)
        {
            try
            {
                QueryBuilder objQueryBuilder = new QueryBuilder
                {
                    TableName = _VisitorSelfRegistrationModel.GetType().Name,
                    StoredProcedureName = @"SP_VisitorSelfRegistration_CURD",
                    SetQueryType = QueryBuilder.QueryType.SELECT
                };

                objQueryBuilder.AddFieldValue("@Id", _VisitorSelfRegistrationModel.Id, DataTypes.Numeric, false);
                objQueryBuilder.AddFieldValue("@Firstname", _VisitorSelfRegistrationModel.FirstName, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@Lastname", _VisitorSelfRegistrationModel.LastName, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@CompanyId", _VisitorSelfRegistrationModel.CompanyId, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@Code", _VisitorSelfRegistrationModel.Code, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@CompanyName", _VisitorSelfRegistrationModel.CompanyName, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@VisitorreasonIds", _VisitorSelfRegistrationModel.VisitorreasonIds, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@VisitorTypeIds", _VisitorSelfRegistrationModel.VisitorTypeIds, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@PhoneNumber", _VisitorSelfRegistrationModel.PhoneNumber, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@EmailId", _VisitorSelfRegistrationModel.EmailId, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@NationalId", _VisitorSelfRegistrationModel.NationalId, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@HostEmployeeCode", _VisitorSelfRegistrationModel.HostEmployeeCode, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@AccessCardNumber", _VisitorSelfRegistrationModel.AccessCardNumber, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@ValidFrom", _VisitorSelfRegistrationModel.ValidFrom, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@ValidTo", _VisitorSelfRegistrationModel.ValidTo, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@CheckId", _VisitorSelfRegistrationModel.CheckId, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@CheckOut", _VisitorSelfRegistrationModel.CheckOut, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@StatusId", _VisitorSelfRegistrationModel.StatusId, DataTypes.Numeric, false);
                objQueryBuilder.AddFieldValue("@VisitorRegNumber", _VisitorSelfRegistrationModel.VisitorRegNumber, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@ServiceIds", _VisitorSelfRegistrationModel.ServiceIds, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@VisitorPreRegStatus", _VisitorSelfRegistrationModel.VisitorPreRegStatus, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@ImagePath", _VisitorSelfRegistrationModel.ImagePath, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@ImageBase64", _VisitorSelfRegistrationModel.ImageBase64, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@Isactive", _VisitorSelfRegistrationModel.IsActive, DataTypes.Boolean, false);
                objQueryBuilder.AddFieldValue("@CreatedBy", _VisitorSelfRegistrationModel.CreatedBy, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@ModifiedBy", _VisitorSelfRegistrationModel.ModifiedBy, DataTypes.Text, false);
                objQueryBuilder.AddFieldValue("@ActionType", _VisitorSelfRegistrationModel.ActionType, DataTypes.Text, false);
                return objgm.ExecuteObjectUsingSp<VisitorSelfRegistrationModel>(objQueryBuilder);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public List<SelectListItem> BindVisitorTypeDropDown(string CompanyId)
        {
            QueryBuilder objQueryBuilder = new QueryBuilder
            {
                TableName = "Tbl_Mstr_VisitorType_Master",
                StoredProcedureName = @"Sps_Load_VisitorType_Dropdown",
                SetQueryType = QueryBuilder.QueryType.SELECT,
            };
            objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
            return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
        }

        public List<SelectListItem> BindVisitorReasonDropDown(string CompanyId)
        {
            QueryBuilder objQueryBuilder = new QueryBuilder
            {
                TableName = "Tbl_Mstr_VisitorReason_Master",
                StoredProcedureName = @"Sps_Load_VisitorReason_Dropdown",
                SetQueryType = QueryBuilder.QueryType.SELECT,
            };
            objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
            return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
        }
        public List<SelectListItem> BindAvailableServiceAccessGroupDropDown(string CompanyId, int Id, String ActionType)
        {
            QueryBuilder objQueryBuilder = new QueryBuilder
            {
                TableName = "Tbl_Mstr_Service_Master",
                StoredProcedureName = @"Sps_Load_Available_Service_Dropdown",
                SetQueryType = QueryBuilder.QueryType.SELECT,
            };
            objQueryBuilder.AddFieldValue("@Id", Id, DataTypes.Numeric, false);
            objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
            objQueryBuilder.AddFieldValue("@ActionType", ActionType, DataTypes.Text, false);
            return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
        }
    }
}