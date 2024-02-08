using DocumentFormat.OpenXml.EMMA;
using ScampusCloud.Models;
using ScampusCloud.Repository.AccessGroup;
using ScampusCloud.Repository.DoorGroup;
using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Controllers
{
    public class AccessGroupController : Controller
    {
        AccessGroupModel _AccessGroupModel = new AccessGroupModel();
        private readonly AccessGroupRepository _AccessGroupRepository;

        public AccessGroupController()
        {
            _AccessGroupRepository = new AccessGroupRepository();
        }

        // GET: AccessGroup
        public ActionResult AccessGroup()
        {

            ViewData["paging_size"] = 10;
            if (ViewData["currentPage"] == null)
                ViewData["currentPage"] = 1;
            string searchtxt = "NA";
            int totals = 0; //Convert.ToInt32(_DoorGroupRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
            ViewData["totalrecords"] = totals;
            return View();
        }

        public ActionResult AddEditAccessGroup(string ID = "")
        {
            try
            {
                _AccessGroupModel.CreatedBy = SessionManager.UserId;
                _AccessGroupModel.ModifiedBy = SessionManager.UserId;
                _AccessGroupModel.CompanyId = SessionManager.CompanyId;

                if (!string.IsNullOrEmpty(ID) && ID != "0")
                {
                    #region Get Entity by id
                    _AccessGroupModel.ActionType = "Edit";
                    _AccessGroupModel.Id = Convert.ToInt32(ID);
                    //_DoorGroupModel = _DoorGroupRepository.AddEdit_DoorGroup(_DoorGroupModel);
                    if (_AccessGroupModel != null)
                    {
                        _AccessGroupModel.IsEdit = true;
                        SessionManager.Code = _AccessGroupModel.Code;
                    }
                    else
                    {
                        _AccessGroupModel = new AccessGroupModel();
                        ViewBag.NoRecordExist = true;
                        _AccessGroupModel.Response_Message = "No record found";
                        SessionManager.Code = null;
                    }
                    #endregion
                }
                //// If url Apeended with Querystring ID=0 then Redirect into current Action
                else if (!string.IsNullOrEmpty(ID) && ID == "0")
                {
                    return RedirectToAction("AddEditAccessGroup");
                }
                else
                {
                    _AccessGroupModel.IsEdit = false;
                    _AccessGroupModel.IsActive = true;
                    SessionManager.Code = null;
                }
                _AccessGroupModel.lstAccessGroupDropdown = BindAccessGroupTypeDropDown(SessionManager.CompanyId.ToString());
                _AccessGroupModel.lstDoorGroupDropdown = BindDoorGroupDropDown(SessionManager.CompanyId.ToString());
                return View(_AccessGroupModel);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        [HttpPost]
        public ActionResult AddEditAccessGroup(AccessGroupModel _AccessGroupModel, string saveAndExit = "")
        {
            if (_AccessGroupModel.Id > 0)
            {
                _AccessGroupModel.ActionType = "Update";
            }
            else
            {
                _AccessGroupModel.ActionType = "Insert";
            }
            var officemaster = _AccessGroupRepository.AddEdit_AccessGroup(_AccessGroupModel);
            if (!string.IsNullOrEmpty(saveAndExit))
            {
                return RedirectToAction("DoorGroup", "DoorGroup");
            }
            else if (_AccessGroupModel.IsEdit == true)
            {
                return RedirectToAction("AddEditAccessGroup", new { ID = _AccessGroupModel.Id });
            }
            else
            {
                return RedirectToAction("AddEditAccessGroup");
            }
        }

        private List<SelectListItem> BindAccessGroupTypeDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _AccessGroupRepository.BindAccessGroupTypeDropDown(CompanyId);
            return drpList;
        }
        private List<SelectListItem> BindDoorGroupDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _AccessGroupRepository.BindDoorGroupDropDown(CompanyId);
            return drpList;
        }

        [HttpPost]
        public PartialViewResult AccessGroupLevel(List<AccessGroupLevelModel> lstAccessGroupLevel)
        {
            return PartialView("_AccessGroupLevel", lstAccessGroupLevel);
        }

    }
}