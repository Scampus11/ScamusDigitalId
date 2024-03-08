using Newtonsoft.Json;
using ScampusCloud.Models;
using ScampusCloud.Repository;
using ScampusCloud.Repository.AccessGroup;
using ScampusCloud.Repository.Staff;
using ScampusCloud.Repository.StudentAccess;
using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Controllers
{
    public class StaffAccessController : Controller
    {
        #region Variable Declaration
        private readonly StaffRepository _StaffRepository;
        private readonly AccessGroupRepository _AccessGroupRepository;
        private readonly StaffAccessRepository _StaffAccessRepository;
        StaffAccessGroupModel _StaffAccessGroupModel = new StaffAccessGroupModel();
        StaffAccessMasterModel _StaffAccessMasterModel = new StaffAccessMasterModel();
        #endregion

        public StaffAccessController()
        {
            _StaffRepository = new StaffRepository();
            _StaffAccessRepository = new StaffAccessRepository();
            _AccessGroupRepository = new AccessGroupRepository();
        }

        // GET: StaffAccess
        public ActionResult StaffAccess()
        {
            ViewData["paging_size"] = 10;
            if (ViewData["currentPage"] == null)
                ViewData["currentPage"] = 1;
            string searchtxt = "NA";
            int totals = Convert.ToInt32(_AccessGroupRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
            ViewData["totalrecords"] = totals;

            _StaffAccessGroupModel.lstDepartment = BindStaffDepartmentDropDown(SessionManager.CompanyId.ToString());
            List<SelectListItem> groupA = new List<SelectListItem>();
            groupA = BindAvailableAccessGroupDropDown(SessionManager.CompanyId.ToString());
            ViewBag.groupA = groupA;
            List<object> groupB = new List<object>();
            ViewBag.groupB = groupB;
            return View(_StaffAccessGroupModel);
        }

        public ActionResult StaffAccessGroupList(int page = 1, int pagesize = 10, string searchtxt = "", int DepartmentId = 0)
        {
            try
            {
                searchtxt = string.IsNullOrEmpty(searchtxt) ? "" : searchtxt;
                int totals = Convert.ToInt32(_StaffAccessRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
                var lstStaffAccessGroup = _StaffAccessRepository.GetStaffAccessList(searchtxt, page, pagesize,  DepartmentId, SessionManager.CompanyId.ToString());
                Session["totalrecords"] = Convert.ToString(totals);
                Session["paging_size"] = Convert.ToString(pagesize);
                ViewData["totalrecords"] = totals;
                ViewData["paging_size"] = pagesize;
                StringBuilder strHTML = new StringBuilder();
                string checkboxAllClick = "SelectAllStaff()";
                if (lstStaffAccessGroup.Count > 0)
                {
                    strHTML.Append("<table class='table table-head-custom table-vertical-center datatable-bordered datatable-head-custom datatable-table' id='kt_advance_table_widget_1'>");
                    strHTML.Append("<thead class='datatable-head'>");
                    strHTML.Append("<tr class='datatable-row'>");
                    strHTML.Append("<th class=pl-0 style=width: 20px><label class=checkbox checkbox-lg checkbox-single><input id='chkAll' type=checkbox value=1 onclick="+checkboxAllClick+"><span></span></input></label></th>");
                    strHTML.Append("<th class='datatable-cell'>Staff Id</th>");
                    strHTML.Append("<th class='datatable-cell'>Staff Name</th>");
                    strHTML.Append("<th class='datatable-cell'>Department </th>");
                    strHTML.Append("<th class='datatable-cell'>Access Group </th>");
                    //strHTML.Append("<th class='datatable-cell'>Block Group</th>");
                    strHTML.Append("<th class='datatable-cell'>Action</th>");
                    strHTML.Append("</tr>");
                    strHTML.Append("</thead>");
                    strHTML.Append("<tbody class='datatable-body custom-scroll'>");
                    foreach (var item in lstStaffAccessGroup)
                    {
                        string ImgSrc = string.Empty;
                        string PhotoImgSrc = string.Empty;
                        string checkboxClick = "SelectStaff()";
                        string DeleteConfirmationEvent = "DeleteConfirmation('" + item.StaffId + "','StaffAccess','StaffAccess','Delete')";

                        string fileExtension = Path.GetExtension(item.ImagePath);

                        if (!string.IsNullOrEmpty(item.ImageBase64))
                            PhotoImgSrc = "data:image/" + fileExtension.TrimStart('.') + ";base64," + item.ImageBase64;

                        strHTML.Append("<tr>");
                        strHTML.Append("<td class=pl-0 py-6><label class=checkbox checkbox-lg checkbox-single><input type=checkbox value=1 class=chk_" + item.StaffId + " data-staffid=" + item.StaffId + " data-staffname=" + item.StaffName + " onclick=" + checkboxClick +" /><span></span></label></td>");
                        strHTML.Append("<td class=pl-0><span class=text-dark-75 font-weight-bolder d-block font-size-lg>" + item.StaffId + "</span></td>");
                        strHTML.Append("<td style='width:250px;'><span><div class='d-flex align-items-center'><div class='symbol symbol-40 flex-shrink-0'><img src='" + PhotoImgSrc + "' style='height:40px;border-radius:100%;border:1px solid;' alt='photo'></div>" +
                            "<div class='ml-4'>" +
                            "<a href='#' class='font-size-sm text-dark-50 text-hover-primary'>" + item.StaffName + "</a></div></div></span></td>");
                        strHTML.Append("<td class=pl-0 py-6>" + item.Department + "</td>");
                        strHTML.Append("<td class=pl-0 py-6>" + item.AccessGroup + "</td>");
                        strHTML.Append("<td>");
                        strHTML.Append("<a class='btn btn-sm btn-icon btn-lg-light btn-text-primary btn-hover-light-primary mr-3' href= '/StaffAccess/AddEditStaffAccess?ID=" + item.Id + "'><i class='flaticon-edit'></i></a>");
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


            //return Json(Reader, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StaffAccessGroupListCount()
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

        public ActionResult AssignAccessGroup(List<StaffAccessGroupModel> data)
        {
            _StaffAccessGroupModel.CreatedBy = SessionManager.UserId;
            _StaffAccessGroupModel.ModifiedBy = SessionManager.UserId;
            _StaffAccessGroupModel.CompanyId = SessionManager.CompanyId;
            _StaffAccessGroupModel.ActionType = "Assign";
            _StaffAccessGroupModel.IsActive = true;
            var test = _StaffAccessRepository.Assign_StaffAccessGroup(data, _StaffAccessGroupModel);
            return Json(new { success = true });
        }
        [HttpPost]
        public ActionResult RemoveAccessGroup(string StaffIds)
        {
            _StaffAccessGroupModel.CreatedBy = SessionManager.UserId;
            _StaffAccessGroupModel.ModifiedBy = SessionManager.UserId;
            _StaffAccessGroupModel.CompanyId = SessionManager.CompanyId;
            _StaffAccessGroupModel.ActionType = "Remove";
            _StaffAccessGroupModel.StaffIds = StaffIds;
            var test = _StaffAccessRepository.Assign_StaffAccessGroup(null, _StaffAccessGroupModel);
            return Json(new { success = true });
        }

        public ActionResult AddEditStaffAccess(string ID = "")
        {
            try
            {

                _StaffAccessMasterModel.CreatedBy = SessionManager.UserId;
                _StaffAccessMasterModel.ModifiedBy = SessionManager.UserId;
                _StaffAccessMasterModel.CompanyId = SessionManager.CompanyId;



                if (!string.IsNullOrEmpty(ID) && ID != "0")
                {
                    #region Get Entity by id
                    _StaffAccessMasterModel.ActionType = "Edit";
                    _StaffAccessMasterModel.Id = Convert.ToInt32(ID);
                    _StaffAccessMasterModel.ActionType = "Edit";
                    _StaffAccessMasterModel = _StaffAccessRepository.AddEdit_StaffAccessGroup(_StaffAccessMasterModel);
                    if (_StaffAccessMasterModel != null)
                    {
                        _StaffAccessMasterModel.IsEdit = true;
                    }
                    else
                    {
                        _StaffAccessMasterModel = new StaffAccessMasterModel();
                        ViewBag.NoRecordExist = true;
                        SessionManager.Code = null;
                    }
                    #endregion
                }
                //// If url Apeended with Querystring ID=0 then Redirect into current Action
                else if (!string.IsNullOrEmpty(ID) && ID == "0")
                {
                    return RedirectToAction("AddEditReader");
                }
                else
                {
                    _StaffAccessMasterModel.IsEdit = false;
                    _StaffAccessMasterModel.IsActive = true;
                    SessionManager.Code = null;
                }
                List<SelectListItem> groupA = new List<SelectListItem>();
                groupA = _StaffAccessRepository.BindAvailableAccessGroupDropDown(_StaffAccessMasterModel.AccessGroupControlId, "AvailableGroup");
                ViewBag.groupA = groupA;
                List<SelectListItem> groupB = new List<SelectListItem>();
                ViewBag.groupB = _StaffAccessRepository.BindAvailableAccessGroupDropDown(_StaffAccessMasterModel.AccessGroupControlId, "AssignedGroup");
                return View(_StaffAccessMasterModel);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }

        }

        [HttpPost]
        public ActionResult AddEditStaffAccess(StaffAccessMasterModel staffAccessMasterModel, string saveAndExit = "")
        {

            _StaffAccessMasterModel.CreatedBy = SessionManager.UserId;
            _StaffAccessMasterModel.ModifiedBy = SessionManager.UserId;
            _StaffAccessMasterModel.CompanyId = SessionManager.CompanyId;
            _StaffAccessMasterModel.Id = Convert.ToInt32(staffAccessMasterModel.Id);
            _StaffAccessMasterModel.AccessGroupId = staffAccessMasterModel.AccessGroupId;
            _StaffAccessMasterModel.StaffId = staffAccessMasterModel.StaffId;
            _StaffAccessMasterModel.IsActive = true;
            _StaffAccessMasterModel.ActionType = "Update";
            _StaffAccessMasterModel = _StaffAccessRepository.AddEdit_StaffAccessGroup(_StaffAccessMasterModel);

            if (!string.IsNullOrEmpty(saveAndExit))
            {
                return RedirectToAction("StaffAccess", "StaffAccess");
            }
            else if (staffAccessMasterModel.IsEdit == true)
            {
                return RedirectToAction("AddEditStaffAccess", new { ID = staffAccessMasterModel.Id });
            }
            else
            {
                return RedirectToAction("AddEditStaffAccess");
            }
        }

        [HttpPost]
        public ActionResult Delete(string Id)
        {
            try
            {
                _StaffAccessMasterModel.ActionType = "Delete";
                _StaffAccessMasterModel.StaffId = Id;
                var response = _StaffAccessRepository.AddEdit_StaffAccessGroup(_StaffAccessMasterModel);

                return RedirectToAction("StaffAccess", "StaffAccess");
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        #region Private Method
        private List<SelectListItem> BindStaffDepartmentDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StaffRepository.BindStaffDepartmentDropDown(CompanyId);
            return drpList;
        }

        private List<SelectListItem> BindAvailableAccessGroupDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _AccessGroupRepository.BindAvailableAccessGroupDropDown(CompanyId);
            return drpList;
        }
        private List<SelectListItem> BindAccessGroupDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _AccessGroupRepository.BindAccessGroupDropDown(CompanyId);
            return drpList;
        }
        #endregion
    }
}