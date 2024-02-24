using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Models
{
    public class StudentAccessGroupModel
    {
        public int? Id { get; set; }
     
        [Column(TypeName = "varchar")]
        public string StudentId { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        [Column(TypeName = "varchar")]
        public string StudentName { get; set; }
        public Guid? CompanyId { get; set; }
        public int? AccessGroupId { get; set; }
        public int? CanteenTypeId { get; set; }
        public int? BlockId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime dtCreatedDate { get; set; }
        public DateTime dtModifiedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public int CollageId { get; set; }
        public string College { get; set; }
        public string Department { get; set; }
        public string AdmissionType { get; set; }
        public string Campus { get; set; }
        public string BatchYear { get; set; }
        public string AccessGroup { get; set; }
        public List<SelectListItem> lstCollage { get; set; }
        public int DepartmentId { get; set; }
        public List<SelectListItem> lstDepartment { get; set; }
        public int CampusId { get; set; }
        public List<SelectListItem> lstCampus { get; set; }
        public int YearId { get; set; }
        public List<SelectListItem> lstYear { get; set; }
        public int AdmissionTypeId { get; set; }
        public List<SelectListItem> lstAdmissionType { get; set; }
        public string Gender { get; set; }
        public int AccessGroupTypeId { get; set; }
        public List<SelectListItem> lstAccessGroupDropdown { get; set; }
        public string CanteenType { get; set; }
        public bool? IsEdit { get; set; }
        public string ActionType { get; set; }
        public string StudentIds { get; set; }
    }

    public class StudentAccessMasterModel
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        [Column(TypeName = "varchar")]
        public string StudentName { get; set; }
        public Guid? CompanyId { get; set; }
        public int AccessGroupTypeId { get; set; }
        public List<SelectListItem> lstAccessGroupDropdown { get; set; }
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