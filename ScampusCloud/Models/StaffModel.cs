using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Models
{
    public class StaffModel : ResponseMessage
    {
		public int? Id { get; set; }
		public Guid? CompanyId { get; set; }

		[Required(ErrorMessage = "Staff Id is required")]
		[Remote(action: "IsStaffIdExist", controller: "RemoteValidation",HttpMethod ="POST",ErrorMessage = "Staff Id is already in use.")]
		public string StaffId { get; set; }

		[Required(ErrorMessage = "Full Name is required")]
		public string FullName { get; set; }

		[Required(ErrorMessage = "Code is required")]
		[Remote(action: "IsCodeExist", controller: "RemoteValidation", HttpMethod = "POST", ErrorMessage = "Code is already in use.")]
		public string Code { get; set; }
		public string FullNameAmharic { get; set; }

		[Required(ErrorMessage = "Birth Of Date is required")]
		public string BirthDate { get; set; }	
		public string Gender { get; set; }	
		public string BloodGroup { get; set; }
		public string PersonalPhone { get; set; }

		[Required(ErrorMessage = "Email Address is required")]
		//[RegularExpression(@"^[_a-z0-9-]+(\.[_a-z0-9-]+)*(\+[a-z0-9-]+)?@[a-z0-9-]+(\.[a-z0-9-]+)*$", ErrorMessage = "Invalid Email Address Format")]
		[Remote(action: "IsEmailIdExist", controller: "RemoteValidation", HttpMethod = "POST", ErrorMessage = "Email Id is already in use.")]
		public string EmailId { get; set; }
		[Required(ErrorMessage = "Password is required")]
		//[RegularExpression(@"(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,20}$", ErrorMessage = "Your password must be at least 6 to 20 characters long")]
		//[RegularExpression(@"(?=.*\d)(?=.*[A-Za-z]).{8,}",
		//    ErrorMessage = "Your password must be at least 8 characters long and contain at least 1 letter and 1 number")]
		public string Password { get; set; }
		public string Address { get; set; }
		public int CountryId { get; set; }
		public List<SelectListItem> lstCountry { get; set; }
		public int DepartmentId { get; set; }
		public List<SelectListItem> lstDepartment { get; set; }
		public int JobTitleId { get; set; }
		public List<SelectListItem> lstJobTitle { get; set; }
		public int CompanyMstrId { get; set; }
		public List<SelectListItem> lstCompany { get; set; }
		public int FacilityId { get; set; }
		public List<SelectListItem> lstFacility { get; set; }
		public int CanteenId { get; set; }
		public List<SelectListItem> lstCanteen { get; set; }
		public int CardStatusId { get; set; }
		public List<SelectListItem> lstCardStatus { get; set; }

		[Required(ErrorMessage = "Issue Date is required")]
		public string IssueDate { get; set; }
		public string UID { get; set; }
		public string VehicleNumber { get; set; }
		[Required(ErrorMessage = "Card Number is required")]
		public string CardNumber { get; set; }
		public string Woreda { get; set; }
		public string SubCity { get; set; }
		public string HouseNumber { get; set; }
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
		public string JobTitleName { get; set; }

	}

}