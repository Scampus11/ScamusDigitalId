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

namespace ScampusCloud.Repository.VisitorCard
{
	public class VisitorCardRepository
	{
		private GeneralMethods objgm = new GeneralMethods();
		public VisitorCardModel AddEdit_VisitorCard(VisitorCardModel _VisitorCardModel)
		{
			try
			{
				QueryBuilder objQueryBuilder = new QueryBuilder
				{
					TableName = _VisitorCardModel.GetType().Name,
					StoredProcedureName = @"SP_VisitorCard_CURD",
					SetQueryType = QueryBuilder.QueryType.SELECT
				};
				if (_VisitorCardModel.ActionType == "Remote")
				{
					objQueryBuilder.AddFieldValue("@Code", _VisitorCardModel.Code, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@CompanyId", _VisitorCardModel.CompanyId, DataTypes.Text, false);
				}
				else
				{
					objQueryBuilder.AddFieldValue("@Id", _VisitorCardModel.Id, DataTypes.Numeric, false);
					objQueryBuilder.AddFieldValue("@Name", _VisitorCardModel.Name, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@CompanyId", _VisitorCardModel.CompanyId, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@CardId", _VisitorCardModel.CardId, DataTypes.Numeric, false);
					objQueryBuilder.AddFieldValue("@Code", _VisitorCardModel.Code, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@Description", _VisitorCardModel.Description, DataTypes.Numeric, false);
					objQueryBuilder.AddFieldValue("@Number", _VisitorCardModel.Number, DataTypes.Numeric, false);
					objQueryBuilder.AddFieldValue("@CardStatusId", _VisitorCardModel.CardStatusId, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@Isactive", _VisitorCardModel.IsActive, DataTypes.Boolean, false);
					objQueryBuilder.AddFieldValue("@CreatedBy", _VisitorCardModel.CreatedBy, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@ModifiedBy", _VisitorCardModel.ModifiedBy, DataTypes.Text, false);
				}
				objQueryBuilder.AddFieldValue("@ActionType", _VisitorCardModel.ActionType, DataTypes.Text, false);

				return objgm.ExecuteObjectUsingSp<VisitorCardModel>(objQueryBuilder);
			}
			catch (Exception ex)
			{
				ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
				throw;
			}
		}

		public List<VisitorCardModel> GetVisitorCardList(string searchtxt = "", int page = 1, int pagesize = 10, string CompanyId = null)
		{
			try
			{
				QueryBuilder objQueryBuilder = new QueryBuilder
				{
					TableName = "Tbl_Mstr_VisitorCard",
					StoredProcedureName = @"SP_GetVisitorCardData",
					SetQueryType = QueryBuilder.QueryType.SELECT
				};
				objQueryBuilder.AddFieldValue("@Search", searchtxt, DataTypes.Text, false);
				objQueryBuilder.AddFieldValue("@page", page, DataTypes.Numeric, false);
				objQueryBuilder.AddFieldValue("@pagesize", pagesize, DataTypes.Numeric, false);
				objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
				return objgm.GetListUsingSp<VisitorCardModel>(objQueryBuilder);
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
					TableName = "Tbl_Mstr_VisitorCard",
					StoredProcedureName = @"SP_GetVisitorCardData_Count",
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

		public DataTable GetVisitorCardData_Export(string searchtxt = "", string CompanyId = null)
		{
			try
			{
				QueryBuilder objQueryBuilder = new QueryBuilder
				{
					TableName = "VisitorCard",
					StoredProcedureName = @"SP_DownloadVisitorCardData_Export",
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
		public List<SelectListItem> BindCardStatusDropDown(string CompanyId)
		{
			QueryBuilder objQueryBuilder = new QueryBuilder
			{
				TableName = "tbl_CardStatus",
				StoredProcedureName = @"Sps_Load_CardStatus_Dropdown",
				SetQueryType = QueryBuilder.QueryType.SELECT,
			};
			objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
			return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
		}
	}
}