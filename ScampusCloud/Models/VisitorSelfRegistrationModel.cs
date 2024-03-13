using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Models
{
    public class VisitorSelfRegistrationModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid? CompanyId { get; set; }
        public string Code { get; set; }
        public string CompanyName { get; set; }
        public string VisitorreasonIds { get; set; }
        public string VisitorTypeIds { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }
        public string NationalId { get; set; }
        public string HostEmployeeCode { get; set; }
        public string AccessCardNumber { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string CheckId { get; set; }
        public string CheckOut { get; set; }
        public int? StatusId { get; set; }
        public string VisitorRegNumber { get; set; }
        public string ServiceIds { get; set; }
        public string VisitorPreRegStatus { get; set; }
        public string ImagePath { get; set; }
        public string ImageBase64 { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime dtCreatedDate { get; set; }
        public DateTime dtModifiedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string ActionType { get; set; }
        public bool? IsEdit { get; set; }
        public List<SelectListItem> lstVisitorTypeDropdown { get; set; }
        public List<SelectListItem> lstVisitorReasonDropdown { get; set; }
    }
}