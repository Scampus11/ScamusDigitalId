using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Models
{
    public class StaffAccessGroupModel
    {
        public int? Id { get; set; }

        [Column(TypeName = "varchar")]
        public string StaffId { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        [Column(TypeName = "varchar")]
        public string StaffName { get; set; }
        public Guid? CompanyId { get; set; }
        public int? AccessGroupId { get; set; }
        public int? BlockId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime dtCreatedDate { get; set; }
        public DateTime dtModifiedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string AccessGroup { get; set; }
        public bool? IsEdit { get; set; }
        public string ActionType { get; set; }
        public string StaffIds { get; set; }
        public string Department { get; set; }
        public int DepartmentId { get; set; }
        public List<SelectListItem> lstDepartment { get; set; }
    }
    public class StaffAccessMasterModel
    {
        public int Id { get; set; }
        public string StaffId { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        [Column(TypeName = "varchar")]
        public string StaffName { get; set; }
        public Guid? CompanyId { get; set; }
        public bool IsActive { get; set; }
        public DateTime dtCreatedDate { get; set; }
        public DateTime dtModifiedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsEdit { get; set; }
        public string ActionType { get; set; }
        public int AccessGroupControlId { get; set; }
        public string AccessGroupId { get; set; }
    }
}