using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Models
{
	public class CustomerModel : ResponseMessage
	{
		public CustomerModel()
		{
		}

		public int? Id { get; set; }

		public Guid? CompanyId { get; set; }

		[Required(ErrorMessage = "Name is required")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Address is required")]
		public string Address { get; set; }

		[Required(ErrorMessage = "Web Site is required")]
		[RegularExpression(@"(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})$", ErrorMessage = "Invalid Company Site Format")]
		public string Website { get; set; }

		[Required(ErrorMessage = "Email Address is required")]
		[RegularExpression(@"^[_a-z0-9-]+(\.[_a-z0-9-]+)*(\+[a-z0-9-]+)?@[a-z0-9-]+(\.[a-z0-9-]+)*$", ErrorMessage = "Invalid Email Address Format")]
		[Remote(action: "IsCustomerEmailIdExist", controller: "RemoteValidation", HttpMethod = "POST", ErrorMessage = "Customer is already in use.")]
		public string EmailId { get; set; }

		[Required(ErrorMessage = "Phone Number is required")]
		[RegularExpression(@"^[+]{1}[0-9]{2}[0-9]{10}$", ErrorMessage = "Invalid Phone Number Format")]
		public string PhoneNumber { get; set; }
		public bool Isactive { get; set; }
		public Guid? CreatedBy { get; set; }
		public Guid? ModifiedBy { get; set; }
		public string ActionType { get; set; }
		public bool? IsEdit { get; set; }

		[Required(ErrorMessage = "Admin User Email Address is required")]
		[RegularExpression(@"^[_a-z0-9-]+(\.[_a-z0-9-]+)*(\+[a-z0-9-]+)?@[a-z0-9-]+(\.[a-z0-9-]+)*$", ErrorMessage = "Invalid Admin User Email Address Format")]
		public string AdminUserEmailId { get; set; }

		[Required(ErrorMessage = "Admin User Phone Number is required")]
		//[RegularExpression(@"^[+]{1}[0-9]{2}[0-9]{10}$", ErrorMessage = "Invalid Admin User Phone Number Format")]
		public string AdminUserPhoneNumber { get; set; }

		[Required(ErrorMessage = "Admin User Password is required")]
		//[RegularExpression(@"(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,20}$", ErrorMessage = "Your password must be at least 6 to 20 characters long")]
		//[RegularExpression(@"(?=.*\d)(?=.*[A-Za-z]).{8,}",
		//    ErrorMessage = "Your password must be at least 8 characters long and contain at least 1 letter and 1 number")]
		public string AdminUserPassword { get; set; }
        public string ImagePath { get; set; }
        public string ImageBase64 { get; set; }
    }
}