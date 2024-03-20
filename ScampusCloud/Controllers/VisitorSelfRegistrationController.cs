using ScampusCloud.Models;
using ScampusCloud.Repository.AccessGroup;
using ScampusCloud.Repository.Service;
using ScampusCloud.Repository.VisitorSelfRegistration;
using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Controllers
{
    public class VisitorSelfRegistrationController : Controller
    {

        VisitorSelfRegistrationModel _VisitorSelfRegistrationModel = new VisitorSelfRegistrationModel();
        private readonly VisitorSelfRegistrationRepository _VisitorSelfRegistrationRepository;

        public VisitorSelfRegistrationController()
        {
            _VisitorSelfRegistrationRepository = new VisitorSelfRegistrationRepository();
        }

        // GET: VisitorSelfRegistration
        public ActionResult AddEditVisitorSelfRegistration()
        {
            try
            {
                _VisitorSelfRegistrationModel.CreatedBy = SessionManager.UserId;
                _VisitorSelfRegistrationModel.ModifiedBy = SessionManager.UserId;
                _VisitorSelfRegistrationModel.CompanyId = SessionManager.CompanyId;
                _VisitorSelfRegistrationModel.lstVisitorTypeDropdown = _VisitorSelfRegistrationRepository.BindVisitorTypeDropDown(SessionManager.CompanyId.ToString());
                _VisitorSelfRegistrationModel.lstVisitorReasonDropdown = _VisitorSelfRegistrationRepository.BindVisitorReasonDropDown(SessionManager.CompanyId.ToString());

                List<SelectListItem> groupA = new List<SelectListItem>();
                groupA = BindAvailableServiceAccessGroupDropDown(SessionManager.CompanyId.ToString(), _VisitorSelfRegistrationModel.Id, "AvailableService");
                ViewBag.groupA = groupA;
                List<object> groupB = new List<object>();
                ViewBag.groupB = BindAvailableServiceAccessGroupDropDown(SessionManager.CompanyId.ToString(), _VisitorSelfRegistrationModel.Id, "AssignedGroup"); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(_VisitorSelfRegistrationModel);
        }

        [HttpPost]
        public ActionResult AddEditVisitorSelfRegistration(VisitorSelfRegistrationModel VisitorSelfRegistrationModel, string saveAndExit = "")
        {

            _VisitorSelfRegistrationModel.CreatedBy = SessionManager.UserId;
            _VisitorSelfRegistrationModel.ModifiedBy = SessionManager.UserId;
            _VisitorSelfRegistrationModel.CompanyId = SessionManager.CompanyId;
            _VisitorSelfRegistrationModel.Id = Convert.ToInt32(VisitorSelfRegistrationModel.Id);
            _VisitorSelfRegistrationModel.FirstName = VisitorSelfRegistrationModel.FirstName;
            _VisitorSelfRegistrationModel.LastName = VisitorSelfRegistrationModel.LastName;
            _VisitorSelfRegistrationModel.Code = VisitorSelfRegistrationModel.Code;
            _VisitorSelfRegistrationModel.CompanyName = VisitorSelfRegistrationModel.CompanyName;
            _VisitorSelfRegistrationModel.VisitorreasonIds = VisitorSelfRegistrationModel.VisitorreasonIds;
            _VisitorSelfRegistrationModel.VisitorTypeIds = VisitorSelfRegistrationModel.VisitorTypeIds;
            _VisitorSelfRegistrationModel.PhoneNumber = VisitorSelfRegistrationModel.PhoneNumber;
            _VisitorSelfRegistrationModel.EmailId = VisitorSelfRegistrationModel.EmailId;
            _VisitorSelfRegistrationModel.NationalId = VisitorSelfRegistrationModel.NationalId;
            _VisitorSelfRegistrationModel.HostEmployeeCode = VisitorSelfRegistrationModel.HostEmployeeCode;
            _VisitorSelfRegistrationModel.AccessCardNumber = VisitorSelfRegistrationModel.AccessCardNumber;
            _VisitorSelfRegistrationModel.ValidFrom = VisitorSelfRegistrationModel.ValidFrom;
            _VisitorSelfRegistrationModel.ValidTo = VisitorSelfRegistrationModel.ValidTo;
            _VisitorSelfRegistrationModel.CheckId = VisitorSelfRegistrationModel.CheckId;
            _VisitorSelfRegistrationModel.CheckOut = VisitorSelfRegistrationModel.CheckOut;
            _VisitorSelfRegistrationModel.StatusId = VisitorSelfRegistrationModel.StatusId;
            _VisitorSelfRegistrationModel.VisitorRegNumber = VisitorSelfRegistrationModel.VisitorRegNumber;
            _VisitorSelfRegistrationModel.ServiceIds = VisitorSelfRegistrationModel.ServiceIds;
            _VisitorSelfRegistrationModel.ImagePath = VisitorSelfRegistrationModel.ImagePath;
            _VisitorSelfRegistrationModel.ImageBase64 = VisitorSelfRegistrationModel.ImageBase64;

            _VisitorSelfRegistrationModel.IsActive = true;
            if (VisitorSelfRegistrationModel.Id > 0)
            {
                VisitorSelfRegistrationModel.ActionType = "Update";
            }
            else
            {
                _VisitorSelfRegistrationModel.ActionType = "Insert";
            }

            VisitorSelfRegistrationModel = _VisitorSelfRegistrationRepository.AddEdit_VisitorSelfRegistration(_VisitorSelfRegistrationModel);

            if (!string.IsNullOrEmpty(saveAndExit))
            {
                return RedirectToAction("AddEditVisitorSelfRegistration", "AddEditVisitorSelfRegistration");
            }
            else
            {
                return RedirectToAction("AddEditVisitorSelfRegistration");
            }
        }
        #region Private Method
        private List<SelectListItem> BindVisitorTypeDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _VisitorSelfRegistrationRepository.BindVisitorTypeDropDown(CompanyId);
            return drpList;
        }

        private List<SelectListItem> BindVisitorReasonDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _VisitorSelfRegistrationRepository.BindVisitorReasonDropDown(CompanyId);
            return drpList;
        }
        private List<SelectListItem> BindAvailableServiceAccessGroupDropDown(string CompanyId, int Id, string ActionType)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _VisitorSelfRegistrationRepository.BindAvailableServiceAccessGroupDropDown(CompanyId, Id, ActionType);
            return drpList;
        }
        #endregion
    }
}