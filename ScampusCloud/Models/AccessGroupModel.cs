using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Models
{
    public class AccessGroupTypeModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        [Column(TypeName = "varchar")]
        public string Name { get; set; }
        public Guid? CompanyId { get; set; }

        [Required(ErrorMessage = "Please enter code")]
        [Remote(action: "IsAccessGroupCodeExist", controller: "RemoteValidation", HttpMethod = "POST", ErrorMessage = "Code is already in use.")]
        [Column(TypeName = "varchar")]
        public string Code { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime dtCreatedDate { get; set; }
        public DateTime dtModifiedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }

    }
    public class AccessGroupModel : ResponseMessage
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        [Column(TypeName = "varchar")]
        public string Name { get; set; }
        public Guid? CompanyId { get; set; }
        [Remote(action: "IsAccessGroupCodeExist", controller: "RemoteValidation", HttpMethod = "POST", ErrorMessage = "Code is already in use.")]
        [Column(TypeName = "varchar")]
        public string Code { get; set; }
        [Column(TypeName = "varchar")]
        public string Description { get; set; }
        public int AccessGroupTypeId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool Is_Canteen { get; set; }
        public DateTime dtCreatedDate { get; set; }
        public DateTime dtModifiedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public List<SelectListItem> lstAccessGroupDropdown { get; set; }
        public List<SelectListItem> lstDoorGroupDropdown { get; set; }
        public List<SelectListItem> lstSessionDropdown { get; set; }
        public List<AccessGroupLevelModel> lstAccessGroupLevel { get; set; }
        public bool IsEdit { get; set; }
        public string ActionType { get; set; }
        public int? DoorGroupId { get; set; }
        public int? SessionId { get; set; }
        public string CanteenType { get; set; }
        public string DoorGroupIds { get; set; }
        public string SessionIds { get; set; }
    }
    public class AccessGroupLevelModel
    {
        public int? Id { get; set; }
        public Guid? CompanyId { get; set; }
        public int AccessGroupId { get; set; }
        public int DoorGroupId { get; set; }
        public int SessionId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime dtCreatedDate { get; set; }
        public DateTime dtModifiedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string DoorGroup { get; set; }
        public string Session { get; set; }
    }
}