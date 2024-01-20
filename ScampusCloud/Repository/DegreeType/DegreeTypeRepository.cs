﻿using ScampusCloud.DataBase;
using ScampusCloud.Models;
using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ScampusCloud.Repository.DegreeType
{
	public class DegreeTypeRepository
	{
		private GeneralMethods objgm = new GeneralMethods();
		public DegreeTypeModel AddEdit_DegreeType(DegreeTypeModel _DegreeTypeModel)
		{
			try
			{
				QueryBuilder objQueryBuilder = new QueryBuilder
				{
					TableName = _DegreeTypeModel.GetType().Name,
					StoredProcedureName = @"SP_DegreeType_CURD",
					SetQueryType = QueryBuilder.QueryType.SELECT
				};

				objQueryBuilder.AddFieldValue("@Id", _DegreeTypeModel.Id, DataTypes.Numeric, false);
				objQueryBuilder.AddFieldValue("@Name", _DegreeTypeModel.Name, DataTypes.Text, false);
				objQueryBuilder.AddFieldValue("@CompanyId", _DegreeTypeModel.CompanyId, DataTypes.Text, false);
				objQueryBuilder.AddFieldValue("@Code", _DegreeTypeModel.Code, DataTypes.Text, false);
				objQueryBuilder.AddFieldValue("@Isactive", _DegreeTypeModel.IsActive, DataTypes.Boolean, false);
				objQueryBuilder.AddFieldValue("@CreatedBy", _DegreeTypeModel.CreatedBy, DataTypes.Text, false);
				objQueryBuilder.AddFieldValue("@ModifiedBy", _DegreeTypeModel.ModifiedBy, DataTypes.Text, false);
				objQueryBuilder.AddFieldValue("@ActionType", _DegreeTypeModel.ActionType, DataTypes.Text, false);

				return objgm.ExecuteObjectUsingSp<DegreeTypeModel>(objQueryBuilder);
			}
			catch (Exception ex)
			{
				ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
				throw;
			}
		}

		public List<DegreeTypeModel> GetDegreeTypeList(string searchtxt = "", int page = 1, int pagesize = 10)
		{
			try
			{
				QueryBuilder objQueryBuilder = new QueryBuilder
				{
					TableName = "Tbl_Mstr_DegreeType",
					StoredProcedureName = @"SP_GetDegreeTypeData",
					SetQueryType = QueryBuilder.QueryType.SELECT
				};
				objQueryBuilder.AddFieldValue("@Search", searchtxt, DataTypes.Text, false);
				objQueryBuilder.AddFieldValue("@page", page, DataTypes.Numeric, false);
				objQueryBuilder.AddFieldValue("@pagesize", pagesize, DataTypes.Numeric, false);
				return objgm.GetListUsingSp<DegreeTypeModel>(objQueryBuilder);
			}
			catch (Exception ex)
			{
				ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
				throw;
			}
		}

		public string GetAllCount(string searchtxt = "")
		{
			try
			{
				QueryBuilder objQueryBuilder = new QueryBuilder
				{
					TableName = "Tbl_Mstr_DegreeType",
					StoredProcedureName = @"SP_GetDegreeTypeData_Count",
					SetQueryType = QueryBuilder.QueryType.SELECT,
				};
				objQueryBuilder.AddFieldValue("@Search", searchtxt, DataTypes.Text, false);
				return objgm.ExcecuteScalarUsingSp(objQueryBuilder);
			}
			catch (Exception ex)
			{
				ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
				throw;
			}

		}

		public DataTable GetDegreeTypeData_Export(string searchtxt = "")
		{
			try
			{
				QueryBuilder objQueryBuilder = new QueryBuilder
				{
					TableName = "DegreeType",
					StoredProcedureName = @"SP_DownloadDegreeTypeData_Export",
					SetQueryType = QueryBuilder.QueryType.SELECT
				};
				objQueryBuilder.AddFieldValue("@Search", searchtxt, DataTypes.Text, false);
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