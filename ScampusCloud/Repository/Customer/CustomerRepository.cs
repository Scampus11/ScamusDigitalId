using ScampusCloud.DataBase;
using ScampusCloud.Models;
using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ScampusCloud.Repository.Customer
{
	public class CustomerRepository
	{
		private GeneralMethods objgm = new GeneralMethods();
		public CustomerModel AddEdit_Customer(CustomerModel _CustomerModel)
		{
			try
			{
				QueryBuilder objQueryBuilder = new QueryBuilder
				{
					TableName = _CustomerModel.GetType().Name,
					StoredProcedureName = @"SP_Customer_CURD",
					SetQueryType = QueryBuilder.QueryType.SELECT
				};
				if (_CustomerModel.ActionType == "Edit" || _CustomerModel.ActionType == "Remote")
				{
					objQueryBuilder.AddFieldValue("@Id", _CustomerModel.Id, DataTypes.Numeric, false);
					objQueryBuilder.AddFieldValue("@EmailId", _CustomerModel.EmailId, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@CompanyId", _CustomerModel.CompanyId, DataTypes.Text, false);
				}
				else
				{
					objQueryBuilder.AddFieldValue("@Id", _CustomerModel.Id, DataTypes.Numeric, false);
					objQueryBuilder.AddFieldValue("@CompanyId", _CustomerModel.CompanyId, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@Name", _CustomerModel.Name, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@Address", _CustomerModel.Address, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@Website", _CustomerModel.Website, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@EmailId", _CustomerModel.EmailId, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@PhoneNumber", _CustomerModel.PhoneNumber, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@CreatedBy", _CustomerModel.CreatedBy, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@ModifiedBy", _CustomerModel.ModifiedBy, DataTypes.Text, false);

					objQueryBuilder.AddFieldValue("@AdminUserEmailId", _CustomerModel.AdminUserEmailId, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@AdminUserPhoneNumber", _CustomerModel.AdminUserPhoneNumber, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@AdminUserPassword", EncryptionDecryption.GetEncrypt(_CustomerModel.AdminUserPassword), DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@Isactive", _CustomerModel.Isactive, DataTypes.Boolean, false);
                    objQueryBuilder.AddFieldValue("@ImagePath", _CustomerModel.ImagePath, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@ImageBase64", _CustomerModel.ImageBase64, DataTypes.Text, false);
                }
				objQueryBuilder.AddFieldValue("@ActionType", _CustomerModel.ActionType, DataTypes.Text, false);

				return objgm.ExecuteObjectUsingSp<CustomerModel>(objQueryBuilder);
			}
			catch (Exception ex)
			{
				ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
				throw;
			}
		}

		public List<CustomerModel> GetCustomerList(string searchtxt = "", int page = 1, int pagesize = 10, string CompanyId = null)
		{
			try
			{
				QueryBuilder objQueryBuilder = new QueryBuilder
				{
					TableName = "Tbl_Mstr_Customer",
					StoredProcedureName = @"SP_GetCustomerData",
					SetQueryType = QueryBuilder.QueryType.SELECT
				};
				objQueryBuilder.AddFieldValue("@Search", searchtxt, DataTypes.Text, false);
				objQueryBuilder.AddFieldValue("@page", page, DataTypes.Numeric, false);
				objQueryBuilder.AddFieldValue("@pagesize", pagesize, DataTypes.Numeric, false);
				objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
				return objgm.GetListUsingSp<CustomerModel>(objQueryBuilder);
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
					TableName = "Tbl_Mstr_Customer",
					StoredProcedureName = @"SP_GetCustomerData_Count",
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

		public DataTable GetCustomerData_Export(string searchtxt = "", string CompanyId = null)
		{
			try
			{
				QueryBuilder objQueryBuilder = new QueryBuilder
				{
					TableName = "Customer",
					StoredProcedureName = @"SP_DownloadCustomerData_Export",
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