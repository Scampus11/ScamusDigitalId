using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Models
{
	public class StudentModel : ResponseMessage
	{
		public int? Id { get; set; }
		public Guid? CompanyId { get; set; }

		[Required(ErrorMessage = "Student Id is required")]
		[Remote(action: "IsStudentIdExist", controller: "RemoteValidation", HttpMethod = "POST", ErrorMessage = "Student Id is already in use.")]
		public string StudentId { get; set; }

		[Required(ErrorMessage = "First Name is required")]
		public string FirstName { get; set; }

		public string FatherName { get; set; }
		public string GrandFatherName { get;set; }

		[Required(ErrorMessage = "Code is required")]
		[Remote(action: "IsStudentCodeExist", controller: "RemoteValidation", HttpMethod = "POST", ErrorMessage = "Code is already in use.")]
		public string Code { get; set; }
		public string FullNameAmharic { get; set; }

		[Required(ErrorMessage = "Birth Of Date is required")]
		public string BirthDate { get; set; }
		public string Gender { get; set; }

		[Required(ErrorMessage = "Email Address is required")]
		[RegularExpression(@"^[_a-z0-9-]+(\.[_a-z0-9-]+)*(\+[a-z0-9-]+)?@[a-z0-9-]+(\.[a-z0-9-]+)*$", ErrorMessage = "Invalid Email Address Format")]
		//[Remote(action: "IsStudentEmailIdExist", controller: "RemoteValidation", HttpMethod = "POST", ErrorMessage = "Email Id is already in use.")]
		public string EmailId { get; set; }
		[Required(ErrorMessage = "Password is required")]
		//[RegularExpression(@"(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,20}$", ErrorMessage = "Your password must be at least 6 to 20 characters long")]
		//[RegularExpression(@"(?=.*\d)(?=.*[A-Za-z]).{8,}",
		//    ErrorMessage = "Your password must be at least 8 characters long and contain at least 1 letter and 1 number")]
		public int CollageId { get; set; }
		public List<SelectListItem> lstCollage { get; set; }
		public int DepartmentId { get; set; }
		public List<SelectListItem> lstDepartment { get; set; }
		public int CampusId { get; set; }
		public List<SelectListItem> lstCampus { get; set; }
		public int YearId { get; set; }
		public List<SelectListItem> lstYear { get; set; }
		public int ProgramId { get; set; }
		public List<SelectListItem> lstProgram { get; set; }
		public int DegreeTypeId { get; set; }
		public List<SelectListItem> lstDegreeType { get; set; }
		public int AdmissionTypeId { get; set; }
		public List<SelectListItem> lstAdmissionType { get; set; }
		public int AdmissionTypeShortId { get; set; }
		public List<SelectListItem> lstAdmissionTypeShort { get; set; }
		public int CardStatusId { get; set; }
		public List<SelectListItem> lstCardStatus { get; set; }

		[Required(ErrorMessage = "Issue Date is required")]
		public string IssueDate { get; set; }
		public string ValidDateUntil { get; set; }
		public string CardNumber { get; set; }
		public string ImagePath { get; set; }
		public string SignaturePath { get; set; }
		public string ImageBase64 { get; set; }
		public string SignatureBase64 { get; set; }
		public bool Isactive { get; set; }
		public Guid? CreatedBy { get; set; }
		public Guid? ModifiedBy { get; set; }
		public string ActionType { get; set; }
		public bool? IsEdit { get; set; }

		public string DepartmentName { get; set; }
		public string CampusName { get; set; }
		public bool IsDigitalStatus { get; set; }
        public bool IsEnableHostel { get; set; }
		public int HostelId { get; set; }
        public int BlockId { get; set; }
        public int FloorId { get; set; }
        public int RoomId { get; set; }
        public int BedId { get; set; }
        public int LockerId { get; set; }

    }

}