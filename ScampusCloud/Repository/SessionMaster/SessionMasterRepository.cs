using ScampusCloud.DataBase;
using ScampusCloud.Models;
using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ScampusCloud.Repository.SessionMaster
{
	public class SessionMasterRepository
	{
		private GeneralMethods objgm = new GeneralMethods();
		public SessionMasterModel AddEdit_SessionMaster(SessionMasterModel _SessionMasterModel)
		{
			try
			{
				QueryBuilder objQueryBuilder = new QueryBuilder
				{
					TableName = _SessionMasterModel.GetType().Name,
					StoredProcedureName = @"SP_SessionMaster_CURD",
					SetQueryType = QueryBuilder.QueryType.SELECT
				};
				if (_SessionMasterModel.ActionType == "Remote")
				{
					objQueryBuilder.AddFieldValue("@Code", _SessionMasterModel.Code, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@CompanyId", _SessionMasterModel.CompanyId, DataTypes.Text, false);
				}
				else
				{
					objQueryBuilder.AddFieldValue("@Id", _SessionMasterModel.Id, DataTypes.Numeric, false);
					objQueryBuilder.AddFieldValue("@Name", _SessionMasterModel.Name, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@CompanyId", _SessionMasterModel.CompanyId, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@Code", _SessionMasterModel.Code, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@Description", _SessionMasterModel.Description, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@StartTime", _SessionMasterModel.StartTime, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@EndTime", _SessionMasterModel.EndTime, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@Isactive", _SessionMasterModel.IsActive, DataTypes.Boolean, false);
					objQueryBuilder.AddFieldValue("@CreatedBy", _SessionMasterModel.CreatedBy, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@ModifiedBy", _SessionMasterModel.ModifiedBy, DataTypes.Text, false);
				}
				objQueryBuilder.AddFieldValue("@ActionType", _SessionMasterModel.ActionType, DataTypes.Text, false);

				return objgm.ExecuteObjectUsingSp<SessionMasterModel>(objQueryBuilder);
			}
			catch (Exception ex)
			{
				ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
				throw;
			}
		}

		public List<SessionMasterModel> GetSessionMasterList(string searchtxt = "", int page = 1, int pagesize = 10, string CompanyId = null)
		{
			try
			{
				QueryBuilder objQueryBuilder = new QueryBuilder
				{
					TableName = "Tbl_Mstr_SessionMaster",
					StoredProcedureName = @"SP_GetSessionMasterData",
					SetQueryType = QueryBuilder.QueryType.SELECT
				};
				objQueryBuilder.AddFieldValue("@Search", searchtxt, DataTypes.Text, false);
				objQueryBuilder.AddFieldValue("@page", page, DataTypes.Numeric, false);
				objQueryBuilder.AddFieldValue("@pagesize", pagesize, DataTypes.Numeric, false);
				objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
				return objgm.GetListUsingSp<SessionMasterModel>(objQueryBuilder);
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
					TableName = "Tbl_Mstr_SessionMaster",
					StoredProcedureName = @"SP_GetSessionMasterData_Count",
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

		public DataTable GetSessionMasterData_Export(string searchtxt = "", string CompanyId = null)
		{
			try
			{
				QueryBuilder objQueryBuilder = new QueryBuilder
				{
					TableName = "SessionMaster",
					StoredProcedureName = @"SP_DownloadSessionMasterData_Export",
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