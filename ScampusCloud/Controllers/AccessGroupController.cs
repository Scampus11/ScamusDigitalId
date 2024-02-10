using ClosedXML.Excel;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using ScampusCloud.Models;
using ScampusCloud.Repository.AccessGroup;
using ScampusCloud.Repository.DoorGroup;
using ScampusCloud.Repository.Reader;
using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Controllers
{
    [SessionTimeoutAttribute]
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
            int totals = Convert.ToInt32(_AccessGroupRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
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
                    _AccessGroupModel = _AccessGroupRepository.AddEdit_AccessGroup(_AccessGroupModel);

                    _AccessGroupModel.lstAccessGroupLevel = _AccessGroupRepository.GetAccessGroupLevelList(_AccessGroupModel.Id, Convert.ToString(_AccessGroupModel.CompanyId));

                    Session["data"] = _AccessGroupModel.lstAccessGroupLevel;

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
                _AccessGroupModel.lstSessionDropdown = BindSessionDropDown(SessionManager.CompanyId.ToString());
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
            _AccessGroupModel.lstAccessGroupLevel = (List<AccessGroupLevelModel>)Session["data"];
            var officemaster = _AccessGroupRepository.AddEdit_AccessGroup(_AccessGroupModel);
            if (!string.IsNullOrEmpty(saveAndExit))
            {
                return RedirectToAction("AccessGroup", "AccessGroup");
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

       

        [HttpPost]
        public PartialViewResult AccessGroupLevel(List<AccessGroupLevelModel> lstAccessGroupLevel)
        {
            List<AccessGroupLevelModel> ListName;
            if (Session["data"] != null)
            {
                ListName = (List<AccessGroupLevelModel>)Session["data"];
                foreach (var item in lstAccessGroupLevel)
                {
                    bool IsExistsAccessGroupLevel = ListName.Any(x => x.DoorGroup.Equals(item.DoorGroup) && x.Session.Equals(item.Session));
                    if (!IsExistsAccessGroupLevel)
                    {
                        ListName.Add(item);
                    }
                }
            }
            else
            {
                ListName = lstAccessGroupLevel;
            }
            Session["data"] = ListName;
            return PartialView("_AccessGroupLevel", ListName);
        }

        [HttpPost]
        public ActionResult RemoveAccessGroupLevel(string DoorGroup)
        {
            List<AccessGroupLevelModel> ListName = new List<AccessGroupLevelModel>();
            if (Session["data"] != null)
            {
                ListName = (List<AccessGroupLevelModel>)Session["data"];
                // Add new items to the existing list
                //foreach (var item in lstAccessGroupLevel)
                //{
                //    ListName.Add(item);
                //}
            }

            // Find the item to remove
            var itemToRemove = ListName.FirstOrDefault(item => item.DoorGroup == DoorGroup);

            // If item found, remove it from the list
            if (itemToRemove != null)
            {
                ListName.Remove(itemToRemove);
            }

            // Update the session with the modified list
            Session["data"] = ListName;

            return PartialView("_AccessGroupLevel", ListName);
        }

        public ActionResult AccessGroupList(int page = 1, int pagesize = 10, string searchtxt = "")
        {
            try
            {
                searchtxt = string.IsNullOrEmpty(searchtxt) ? "" : searchtxt;
                int totals = Convert.ToInt32(_AccessGroupRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
                var lstAccessGroup = _AccessGroupRepository.GetAccessGroupList(searchtxt, page, pagesize, SessionManager.CompanyId.ToString());
                Session["totalrecords"] = Convert.ToString(totals);
                Session["paging_size"] = Convert.ToString(pagesize);
                ViewData["totalrecords"] = totals;
                ViewData["paging_size"] = pagesize;
                StringBuilder strHTML = new StringBuilder();
                if (lstAccessGroup.Count > 0)
                {
                    strHTML.Append("<table class='datatable-bordered datatable-head-custom datatable-table' id='kt_datatable'>");
                    strHTML.Append("<thead class='datatable-head'>");
                    strHTML.Append("<tr class='datatable-row'>");
                    strHTML.Append("<th class='datatable-cell'>Name</th>");
                    strHTML.Append("<th class='datatable-cell'>Canteen Type</th>");
                    strHTML.Append("<th class='datatable-cell'>Description</th>");
                    strHTML.Append("<th class='datatable-cell'>Action</th>");
                    strHTML.Append("</tr>");
                    strHTML.Append("</thead>");
                    strHTML.Append("<tbody class='datatable-body custom-scroll'>");
                    foreach (var item in lstAccessGroup)
                    {
                        string DeleteConfirmationEvent = "DeleteConfirmation('" + item.Id + "','AccessGroup','AccessGroup','Delete')";
                        strHTML.Append("<tr>");
                        strHTML.Append("<td>" + item.Name + "</td>");
                        strHTML.Append("<td>" + item.CanteenType + "</td>");
                        strHTML.Append("<td>" + item.Description + "</td>");
                        strHTML.Append("<td>");
                        strHTML.Append("<a class='btn btn-sm btn-icon btn-lg-light btn-text-primary btn-hover-light-primary mr-3' href= '/AccessGroup/AddEditAccessGroup?ID=" + item.Id + "'><i class='flaticon-edit'></i></a>");
                        strHTML.Append("<a id = 'del_" + item.Id + "' class='btn btn-sm btn-icon btn-lg-light btn-text-danger btn-hover-light-danger' onclick=" + DeleteConfirmationEvent + "><i class='flaticon-delete'></i></a>");
                        strHTML.Append("</td>");
                        strHTML.Append("</tr>");
                    }
                    strHTML.Append("</tbody>");
                    strHTML.Append("</table>");
                }
                else
                {
                    strHTML.Append("<center>No data available in table</center>");
                }
                return Content(strHTML.ToString());
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public ActionResult AccessGroupListCount()
        {
            try
            {
                string result = string.Empty;
                int totals = Convert.ToInt32(Session["totalrecords"]);
                int pagesize = Convert.ToInt32(Session["paging_size"]);
                ViewData["paging_size"] = Convert.ToInt32(Session["paging_size"]);

                int noofpages = 1;
                if (totals > 0 && pagesize > 0)
                    noofpages = (totals / pagesize) + (totals % pagesize != 0 ? 1 : 0);
                result = "{\"noofpages\":" + noofpages + ",\"NoOfTotalRecords\":" + totals + "}";

                return Content((result));
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }
        [HttpGet]
        public FileResult Export(string searchtxt = "")
        {
            DataTable dt = new DataTable("Reader");
            try
            {
                dt = _AccessGroupRepository.GetAccessGroupData_Export(searchtxt, SessionManager.CompanyId.ToString());
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AccessGroup.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        #region Private method
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

        private List<SelectListItem> BindSessionDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _AccessGroupRepository.BindSessionDropDown(CompanyId);
            return drpList;
        }
        #endregion
    }
}