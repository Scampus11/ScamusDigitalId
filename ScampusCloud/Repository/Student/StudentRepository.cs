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
using System.Web.Mvc;

namespace ScampusCloud.Repository.Student
{
	public class StudentRepository
	{
		private GeneralMethods objgm = new GeneralMethods();
		public StudentModel AddEdit_Student(StudentModel _StudentModel)
		{
			try
			{
				QueryBuilder objQueryBuilder = new QueryBuilder
				{
					TableName = _StudentModel.GetType().Name,
					StoredProcedureName = @"SP_Student_CURD",
					SetQueryType = QueryBuilder.QueryType.SELECT
				};
				if (_StudentModel.ActionType == "Edit" || _StudentModel.ActionType == "Delete")
				{
					objQueryBuilder.AddFieldValue("@Id", _StudentModel.Id, DataTypes.Numeric, false);
					objQueryBuilder.AddFieldValue("@CompanyId", _StudentModel.CompanyId, DataTypes.Text, false);
				}
				else if (_StudentModel.ActionType == "Remote")
				{
					objQueryBuilder.AddFieldValue("@Id", _StudentModel.Id, DataTypes.Numeric, false);
					objQueryBuilder.AddFieldValue("@StudentId", _StudentModel.StudentId, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@CompanyId", _StudentModel.CompanyId, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@EmailId", _StudentModel.EmailId, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@Code", _StudentModel.Code, DataTypes.Text, false);
				}
				else
				{
					objQueryBuilder.AddFieldValue("@Id", _StudentModel.Id, DataTypes.Numeric, false);
					objQueryBuilder.AddFieldValue("@StudentId", _StudentModel.StudentId, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@Code", _StudentModel.Code, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@CompanyId", _StudentModel.CompanyId, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@FirstName", _StudentModel.FirstName, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@FatherName", _StudentModel.FatherName, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@GrandFatherName", _StudentModel.GrandFatherName, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@FullNameAmharic", _StudentModel.FullNameAmharic, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@Gender", _StudentModel.Gender, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@DateOfBirth", _StudentModel.BirthDate, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@ColleageId", _StudentModel.CollageId, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@DepartmentId", _StudentModel.DepartmentId, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@CampusId", _StudentModel.CampusId, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@YearId", _StudentModel.YearId, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@ProgramId", _StudentModel.ProgramId, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@DegreeTypeId", _StudentModel.DegreeTypeId, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@AdmissionTypeId", _StudentModel.AdmissionTypeId, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@AdmissionTypeShortId", _StudentModel.AdmissionTypeShortId, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@ValidDateUntil", _StudentModel.ValidDateUntil, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@IssueDate", _StudentModel.IssueDate, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@EmailId", _StudentModel.EmailId, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@CardStatusId", _StudentModel.CardStatusId, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@CardNumber", _StudentModel.CardNumber, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@IsDegitalStatus", _StudentModel.IsDigitalStatus, DataTypes.Boolean, false);
                    objQueryBuilder.AddFieldValue("@IsEnableHostel", _StudentModel.IsEnableHostel, DataTypes.Boolean, false);
                    objQueryBuilder.AddFieldValue("@HostelId", _StudentModel.HostelId, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@BlockId", _StudentModel.BlockId, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@FloorId", _StudentModel.FloorId, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@RoomId", _StudentModel.RoomId, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@BedId", _StudentModel.BedId, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@LockerId", _StudentModel.LockerId, DataTypes.Numeric, false);
                    objQueryBuilder.AddFieldValue("@ImagePath", _StudentModel.ImagePath, DataTypes.Text, false);
                    objQueryBuilder.AddFieldValue("@SignaturePath", _StudentModel.SignaturePath, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@ImageBase64", _StudentModel.ImageBase64, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@SignatureBase64", _StudentModel.SignatureBase64, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@Isactive", _StudentModel.Isactive, DataTypes.Boolean, false);
					objQueryBuilder.AddFieldValue("@CreatedBy", _StudentModel.CreatedBy, DataTypes.Text, false);
					objQueryBuilder.AddFieldValue("@ModifiedBy", _StudentModel.ModifiedBy, DataTypes.Text, false);
				}
				objQueryBuilder.AddFieldValue("@ActionType", _StudentModel.ActionType, DataTypes.Text, false);
				return objgm.ExecuteObjectUsingSp<StudentModel>(objQueryBuilder);
			}
			catch (Exception ex)
			{
				ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
				throw;
			}
		}

		public List<StudentModel> GetStudentList(string searchtxt = "", int page = 1, int pagesize = 10, string CompanyId = null)
		{
			try
			{
				QueryBuilder objQueryBuilder = new QueryBuilder
				{
					TableName = "Tbl_Mstr_Student",
					StoredProcedureName = @"SP_GetStudentData",
					SetQueryType = QueryBuilder.QueryType.SELECT
				};
				objQueryBuilder.AddFieldValue("@Search", searchtxt, DataTypes.Text, false);
				objQueryBuilder.AddFieldValue("@page", page, DataTypes.Numeric, false);
				objQueryBuilder.AddFieldValue("@pagesize", pagesize, DataTypes.Numeric, false);
				objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
				return objgm.GetListUsingSp<StudentModel>(objQueryBuilder);
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
					TableName = "Tbl_Mstr_Student",
					StoredProcedureName = @"SP_GetStudentData_Count",
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

		public DataTable GetStudentData_Export(string searchtxt = "", string CompanyId = null)
		{
			try
			{
				QueryBuilder objQueryBuilder = new QueryBuilder
				{
					TableName = "Student",
					StoredProcedureName = @"SP_DownloadStudentData_Export",
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
		public List<SelectListItem> BindCampusDropdown(string CompanyId)
		{
			QueryBuilder objQueryBuilder = new QueryBuilder
			{
				TableName = "tbl_Campus",
				StoredProcedureName = @"Sps_Load_Campus_Dropdown",
				SetQueryType = QueryBuilder.QueryType.SELECT,
			};
			objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
			return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
		}

		public List<SelectListItem> BindStudentDepartmentDropDown(string CompanyId)
		{
			QueryBuilder objQueryBuilder = new QueryBuilder
			{
				TableName = "tbl_StudentDepartment",
				StoredProcedureName = @"Sps_Load_StudentDepartment_Dropdown",
				SetQueryType = QueryBuilder.QueryType.SELECT,
			};
			objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
			return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
		}

		public List<SelectListItem> BindProgramDropDown(string CompanyId)
		{
			QueryBuilder objQueryBuilder = new QueryBuilder
			{
				TableName = "tbl_Program",
				StoredProcedureName = @"Sps_Load_Program_Dropdown",
				SetQueryType = QueryBuilder.QueryType.SELECT,
			};
			objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
			return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
		}

		public List<SelectListItem> BindDegreeTypeDropDown(string CompanyId)
		{
			QueryBuilder objQueryBuilder = new QueryBuilder
			{
				TableName = "tbl_DegreeType",
				StoredProcedureName = @"Sps_Load_DegreeType_Dropdown",
				SetQueryType = QueryBuilder.QueryType.SELECT,
			};
			objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
			return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
		}
		public List<SelectListItem> BindAdmissionTypeDropDown(string CompanyId)
		{
			QueryBuilder objQueryBuilder = new QueryBuilder
			{
				TableName = "tbl_AdmissionType",
				StoredProcedureName = @"Sps_Load_AdmissionType_Dropdown",
				SetQueryType = QueryBuilder.QueryType.SELECT,
			};
			objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
			return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
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
		public List<SelectListItem> BindCanteenDropDown(string CompanyId)
		{
			QueryBuilder objQueryBuilder = new QueryBuilder
			{
				TableName = "tbl_Canteen",
				StoredProcedureName = @"Sps_Load_Canteen_Dropdown",
				SetQueryType = QueryBuilder.QueryType.SELECT,
			};
			objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
			return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
		}
		public List<SelectListItem> BindYearDropDown(string CompanyId)
		{
			QueryBuilder objQueryBuilder = new QueryBuilder
			{
				TableName = "tbl_Year",
				StoredProcedureName = @"Sps_Load_Year_Dropdown",
				SetQueryType = QueryBuilder.QueryType.SELECT,
			};
			objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
			return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
		}
        public List<SelectListItem> BindColleageDropDown(string CompanyId, int ScampusId)
        {
            QueryBuilder objQueryBuilder = new QueryBuilder
            {
                TableName = "Tbl_Mstr_College_Master",
                StoredProcedureName = @"Sps_Load_College_Dropdown",
                SetQueryType = QueryBuilder.QueryType.SELECT,
            };
            objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
            objQueryBuilder.AddFieldValue("@ScampusId", ScampusId, DataTypes.Numeric, false);
            return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
        }
        public List<SelectListItem> BindDepartmentDropDown(string CompanyId, int ColleageId)
        {
            QueryBuilder objQueryBuilder = new QueryBuilder
            {
                TableName = "Tbl_Mstr_Student_Department_Master",
                StoredProcedureName = @"Sps_Load_Student_Department_Dropdown",
                SetQueryType = QueryBuilder.QueryType.SELECT,
            };
            objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
            objQueryBuilder.AddFieldValue("@CollegeId", ColleageId, DataTypes.Numeric, false);
            return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
        }
        public List<SelectListItem> BindAdmissionTypeShortDropDown(string CompanyId, int AdmissionTypeId)
        {
            QueryBuilder objQueryBuilder = new QueryBuilder
            {
                TableName = "Tbl_Mstr_Admission_Master",
                StoredProcedureName = @"Sps_Load_AdmissionTypeShort_Dropdown",
                SetQueryType = QueryBuilder.QueryType.SELECT,
            };
            objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
            objQueryBuilder.AddFieldValue("@Id", AdmissionTypeId, DataTypes.Numeric, false);
            return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
        }
    }
}