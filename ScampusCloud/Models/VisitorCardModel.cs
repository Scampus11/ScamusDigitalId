using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Models
{
    public class VisitorCardModel : ResponseMessage
    {
        public int? Id { get; set; }

        public Guid? CompanyId { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }

        public string CardId { get; set; }
        public string Description { get; set; }
        public string Number { get; set; }
        public string CardStatus { get; set; }
        public int CardStatusId { get; set; }
        public List<SelectListItem> lstCardStatus { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime dtCreatedDate { get; set; }
        public DateTime dtModifiedDate { get; set; }

        [Required(ErrorMessage = "Enter Code id")]
        [Remote(action: "IsVisitorCardCodeExist", controller: "RemoteValidation", HttpMethod = "POST", ErrorMessage = "Code is already in use.")]
        public string Code { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string ActionType { get; set; }
    }
}