using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Models
{
    public class ServiceModel
    {
        public int Id { get; set; }
        public string Name { get; set;}
        public Guid? CompanyId { get; set; }
        public string Code { get; set;}
        public string Description { get; set;}
        public int? DepartmentId { get; set; }
        public string Department { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public string EmployeeIds { get; set; }
        public string EmployeeName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime dtCreatedDate { get; set; }
        public DateTime dtModifiedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string ActionType { get; set; }
        public bool? IsEdit { get; set; }
        public List<SelectListItem> lstDepartment { get; set; }
    }
}