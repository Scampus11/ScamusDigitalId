using ClosedXML.Excel;
using ScampusCloud.Models;
using ScampusCloud.Repository.Reader;
using ScampusCloud.Repository.Staff;
using ScampusCloud.Repository.Visitor;
using ScampusCloud.Repository.VisitorSelfRegistration;
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
    public class VisitorController : Controller
    {
        VisitorSelfRegistrationModel _VisitorSelfRegistrationModel = new VisitorSelfRegistrationModel();
        private readonly VisitorRepository _VisitorRepository;
        private readonly VisitorSelfRegistrationRepository _VisitorSelfRegistrationRepository;

        public VisitorController()
        {
            _VisitorRepository = new VisitorRepository();
            _VisitorSelfRegistrationRepository = new VisitorSelfRegistrationRepository();
        }
        // GET: Visitor
        public ActionResult Visitor()
        {
            ViewData["paging_size"] = 10;
            if (ViewData["currentPage"] == null)
                ViewData["currentPage"] = 1;
            string searchtxt = "NA";
            int totals = Convert.ToInt32(_VisitorRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
            ViewData["totalrecords"] = totals;
            return View();
        }
        public ActionResult VisitorList(int page = 1, int pagesize = 10, string searchtxt = "")
        {
            try
            {
                searchtxt = string.IsNullOrEmpty(searchtxt) ? "" : searchtxt;
                int totals = Convert.ToInt32(_VisitorRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
                var lstVisitor = _VisitorRepository.GetVisitorList(searchtxt, page, pagesize, SessionManager.CompanyId.ToString());
                Session["totalrecords"] = Convert.ToString(totals);
                Session["paging_size"] = Convert.ToString(pagesize);
                ViewData["totalrecords"] = totals;
                ViewData["paging_size"] = pagesize;
                StringBuilder strHTML = new StringBuilder();
                if (lstVisitor.Count > 0)
                {
                    strHTML.Append("<table class='datatable-bordered datatable-head-custom datatable-table' id='kt_datatable'>");
                    strHTML.Append("<thead class='datatable-head'>");
                    strHTML.Append("<tr class='datatable-row'>");
                    strHTML.Append("<th class='datatable-cell'>Photo</th>");
                    strHTML.Append("<th class='datatable-cell'>Visitor Reg No</th>");
                    strHTML.Append("<th class='datatable-cell'>Name</th>");
                    strHTML.Append("<th class='datatable-cell'>EmailId</th>");
                    strHTML.Append("<th class='datatable-cell'>PhoneNumber</th>");
                    strHTML.Append("<th class='datatable-cell'>Visitor Type</th>");
                    strHTML.Append("<th class='datatable-cell'>Visitor Reason</th>");
                    strHTML.Append("<th class='datatable-cell'>Visitor Register Status</th>");
                    strHTML.Append("<th class='datatable-cell'>Status</th>");
                    strHTML.Append("<th class='datatable-cell'>Action</th>");
                    strHTML.Append("</tr>");
                    strHTML.Append("</thead>");
                    strHTML.Append("<tbody class='datatable-body custom-scroll'>");
                    foreach (var item in lstVisitor)
                    {
                        string ImgSrc = string.Empty;
                        string PhotoImgSrc = string.Empty;
                        string DeleteConfirmationEvent = "DeleteConfirmation('" + item.Id + "','Visitor','Visitor','Delete')";
                        string fileExtension = Path.GetExtension(item.ImagePath);
                        if (!string.IsNullOrEmpty(item.ImageBase64))
                            PhotoImgSrc = "data:image/png;base64," + item.ImageBase64;
                        strHTML.Append("<tr>");
                        strHTML.Append("<td style='width:250px;'><span><div class='d-flex align-items-center'><div class='symbol symbol-40 flex-shrink-0'><img src='" + PhotoImgSrc + "' style='height:40px;border-radius:100%;border:1px solid;' alt='photo'></div>" +
                             "<div class='ml-4'>" +
                             "<a href='#' class='font-size-sm text-dark-50 text-hover-primary'>" + item.Fullname + "</a></div></div></span></td>");
                        strHTML.Append("<td>" + item.VisitorRegNumber + "</td>");
                        strHTML.Append("<td>" + item.Fullname + "</td>");
                        strHTML.Append("<td>" + item.EmailId + "</td>");
                        strHTML.Append("<td>" + item.PhoneNumber + "</td>");
                        strHTML.Append("<td>" + item.VisitorType + "</td>");
                        strHTML.Append("<td>" + item.VisitorReason + "</td>");
                        strHTML.Append("<td>" + item.VisitorPreRegStatus + "</td>");
                        if (item.IsActive)
                            strHTML.Append("<td><span><span class='label font-weight-bold label-lg label-light-primary label-inline'>Active</span></span></td>");
                        else
                            strHTML.Append("<td><span><span class='label font-weight-bold label-lg label-light-danger label-inline'>InActive</span></span></td>");
                        strHTML.Append("<td>");
                        strHTML.Append("<a class='btn btn-sm btn-icon btn-lg-light btn-text-primary btn-hover-light-primary mr-3' href= '/Visitor/AddEditVisitor?ID=" + item.Id + "'><i class='flaticon-edit'></i></a>");
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

        public ActionResult VisitorListCount()
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

        public ActionResult AddEditVisitor(string ID = "")
        {

            _VisitorSelfRegistrationModel.CreatedBy = SessionManager.UserId;
            _VisitorSelfRegistrationModel.ModifiedBy = SessionManager.UserId;
            _VisitorSelfRegistrationModel.CompanyId = SessionManager.CompanyId;

            if (!string.IsNullOrEmpty(ID) && ID != "0")
            {
                #region Get Entity by id
                _VisitorSelfRegistrationModel.ActionType = "Edit";
                _VisitorSelfRegistrationModel.Id = Convert.ToInt32(ID);
                _VisitorSelfRegistrationModel = _VisitorSelfRegistrationRepository.AddEdit_VisitorSelfRegistration(_VisitorSelfRegistrationModel);
                if (_VisitorSelfRegistrationModel != null)
                {
                    _VisitorSelfRegistrationModel.IsEdit = true;
                    SessionManager.Code = _VisitorSelfRegistrationModel.Code;
                }
                else
                {
                    _VisitorSelfRegistrationModel = new VisitorSelfRegistrationModel();
                    ViewBag.NoRecordExist = true;
                   // _VisitorSelfRegistrationModel.Response_Message = "No record found";
                    SessionManager.Code = null;
                }
                #endregion
            }
            else if (!string.IsNullOrEmpty(ID) && ID == "0")
            {
                return RedirectToAction("AddEditVisitor");
            }
            else
            {
                _VisitorSelfRegistrationModel.IsEdit = false;
                _VisitorSelfRegistrationModel.IsActive = true;
                SessionManager.Code = null;
            }
            _VisitorSelfRegistrationModel.lstVisitorTypeDropdown = _VisitorSelfRegistrationRepository.BindVisitorTypeDropDown(SessionManager.CompanyId.ToString());
            _VisitorSelfRegistrationModel.lstVisitorReasonDropdown = _VisitorSelfRegistrationRepository.BindVisitorReasonDropDown(SessionManager.CompanyId.ToString());
            _VisitorSelfRegistrationModel.lstVisitorStatusDropdown = _VisitorRepository.BindVisitorStatusDropDown(SessionManager.CompanyId.ToString());
            List<SelectListItem> groupA = new List<SelectListItem>();
            groupA = BindAvailableServiceAccessGroupDropDown(SessionManager.CompanyId.ToString(), _VisitorSelfRegistrationModel.Id, "AvailableService");
            ViewBag.groupA = groupA;
            List<object> groupB = new List<object>();
            ViewBag.groupB = BindAvailableServiceAccessGroupDropDown(SessionManager.CompanyId.ToString(), _VisitorSelfRegistrationModel.Id, "AssignedGroup");
            _VisitorSelfRegistrationModel.IsActive = true;
            return View(_VisitorSelfRegistrationModel);
        }

        [HttpPost]
        public ActionResult AddEditVisitor(VisitorSelfRegistrationModel VisitorSelfRegistrationModel, string saveAndExit = "")
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
                _VisitorSelfRegistrationModel.ActionType = "Update";
            }
            else
            {
                _VisitorSelfRegistrationModel.ActionType = "Insert";
            }

            VisitorSelfRegistrationModel = _VisitorSelfRegistrationRepository.AddEdit_VisitorSelfRegistration(_VisitorSelfRegistrationModel);

            if (!string.IsNullOrEmpty(saveAndExit))
            {
                return RedirectToAction("Visitor", "Visitor", new { area = "" });

            }
            else
            {
                return RedirectToAction("Visitor", "Visitor", new { area = "" });
            }
        }

        [HttpGet]
        public FileResult Export(string searchtxt = "")
        {
            DataTable dt = new DataTable("Visitor");
            try
            {
                dt = _VisitorRepository.GetVisitorData_Export(searchtxt, SessionManager.CompanyId.ToString());
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Visitor.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        [HttpPost]
        public ActionResult Delete(string Id)
        {
            try
            {
                _VisitorSelfRegistrationModel.ActionType = "Delete";
                _VisitorSelfRegistrationModel.Id = Convert.ToInt32(Id);
                var response = _VisitorSelfRegistrationRepository.AddEdit_VisitorSelfRegistration(_VisitorSelfRegistrationModel);

                return RedirectToAction("Visitor", "Visitor");
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
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
        private List<SelectListItem> BindVisitorStatusDropdown(string CompanyId)
        {
            List<SelectListItem> visitorStatusList = new List<SelectListItem>();
            visitorStatusList = _VisitorRepository.BindVisitorStatusDropDown(CompanyId);
            return visitorStatusList;
        }
        #endregion

    }
}