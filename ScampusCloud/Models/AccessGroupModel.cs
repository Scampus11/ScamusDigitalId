using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Models
{
    public class AccessGroupTypeModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }
        public Guid? CompanyId { get; set; }
        [Remote(action: "IsDoorGroupCodeExist", controller: "RemoteValidation", HttpMethod = "POST", ErrorMessage = "Code is already in use.")]
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
        public string Name { get; set; }
        public Guid? CompanyId { get; set; }
        [Remote(action: "IsDoorGroupCodeExist", controller: "RemoteValidation", HttpMethod = "POST", ErrorMessage = "Code is already in use.")]
        public string Code { get; set; }
        public string Description { get; set; }
        public int AccessGroupTypeId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsSAG { get; set; }
        public DateTime dtCreatedDate { get; set; }
        public DateTime dtModifiedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public List<SelectListItem> lstAccessGroupDropdown { get; set; }
        public List<SelectListItem> lstDoorGroupDropdown { get; set; }
        public List<AccessGroupLevelModel> lstAccessGroupLevel { get; set; }
        public bool IsEdit { get; set; }
        public string ActionType { get; set; }
        public int DoorGroupId { get; set; }
        public int SessionId { get; set; }
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
    }
}