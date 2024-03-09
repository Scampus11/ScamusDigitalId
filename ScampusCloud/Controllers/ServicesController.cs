using ClosedXML.Excel;
using ScampusCloud.Models;
using ScampusCloud.Repository;
using ScampusCloud.Repository.AccessGroup;
using ScampusCloud.Repository.Reader;
using ScampusCloud.Repository.Service;
using ScampusCloud.Repository.Staff;
using ScampusCloud.Repository.Student;
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
    public class ServicesController : Controller
    {
        private readonly ServicesRepository _ServicesRepository;
        private readonly StaffRepository _StaffRepository;
        private readonly AccessGroupRepository _AccessGroupRepository;
        ServiceModel _ServiceModel = new ServiceModel();

        public ServicesController()
        {
            _ServicesRepository = new ServicesRepository();
            _StaffRepository = new StaffRepository();
            _AccessGroupRepository = new AccessGroupRepository();
        }

        // GET: Service
        public ActionResult Services()
        {
            ViewData["paging_size"] = 10;
            if (ViewData["currentPage"] == null)
                ViewData["currentPage"] = 1;
            string searchtxt = "NA";
            int totals = 0; //Convert.ToInt32(_StudentRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
            ViewData["totalrecords"] = totals;
            return View();
        }
        public ActionResult ServicesList(int page = 1, int pagesize = 10, string searchtxt = "")
        {
            try
            {
                searchtxt = string.IsNullOrEmpty(searchtxt) ? "" : searchtxt;
                int totals = Convert.ToInt32(_ServicesRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
                var lstServiceList = _ServicesRepository.GetServicesList(searchtxt, page, pagesize, SessionManager.CompanyId.ToString());
                Session["totalrecords"] = Convert.ToString(totals);
                Session["paging_size"] = Convert.ToString(pagesize);
                ViewData["totalrecords"] = totals;
                ViewData["paging_size"] = pagesize;
                StringBuilder strHTML = new StringBuilder();
                if (lstServiceList.Count > 0)
                {
                    strHTML.Append("<table class='datatable-bordered datatable-head-custom datatable-table' id='kt_datatable'>");
                    strHTML.Append("<thead class='datatable-head'>");
                    strHTML.Append("<tr class='datatable-row'>");
                    strHTML.Append("<th class='datatable-cell'>Service Name</th>");
                    strHTML.Append("<th class='datatable-cell'>Service Description</th>");
                    strHTML.Append("<th class='datatable-cell'>Start Date Time</th>");
                    strHTML.Append("<th class='datatable-cell'>End Date Time</th>");
                    strHTML.Append("<th class='datatable-cell'>Department</th>");
                    strHTML.Append("<th class='datatable-cell'>Employee Name</th>");
                    strHTML.Append("<th class='datatable-cell'>Status</th>");
                    strHTML.Append("<th class='datatable-cell'>Action</th>");
                    strHTML.Append("</tr>");
                    strHTML.Append("</thead>");
                    strHTML.Append("<tbody class='datatable-body custom-scroll'>");
                    foreach (var item in lstServiceList)
                    {
                        string DeleteConfirmationEvent = "DeleteConfirmation('" + item.Id + "','Services','Services','Delete')";
                        strHTML.Append("<tr>");
                        strHTML.Append("<td>" + item.Name + "</td>");
                        strHTML.Append("<td>" + item.Description + "</td>");
                        strHTML.Append("<td>" + item.StartDateTime + "</td>");
                        strHTML.Append("<td>" + item.EndDateTime + "</td>");
                        strHTML.Append("<td>" + item.Department + "</td>");
                        strHTML.Append("<td>" + item.EmployeeName + "</td>");
                        if (item.IsActive)
                            strHTML.Append("<td><span><span class='label font-weight-bold label-lg label-light-primary label-inline'>Active</span></span></td>");
                        else
                            strHTML.Append("<td><span><span class='label font-weight-bold label-lg label-light-danger label-inline'>InActive</span></span></td>");

                        strHTML.Append("<td>");
                        strHTML.Append("<a class='btn btn-sm btn-icon btn-lg-light btn-text-primary btn-hover-light-primary mr-3' href= '/Services/AddEditServices?ID=" + item.Id + "'><i class='flaticon-edit'></i></a>");
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

        public ActionResult ServicesListCount()
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

        public ActionResult AddEditServices(string ID = "")
        {
            try
            {
                _ServiceModel.CreatedBy = SessionManager.UserId;
                _ServiceModel.ModifiedBy = SessionManager.UserId;
                _ServiceModel.CompanyId = SessionManager.CompanyId;

                if (!string.IsNullOrEmpty(ID) && ID != "0")
                {
                    #region Get Entity by id
                    _ServiceModel.ActionType = "Edit";
                    _ServiceModel.Id = Convert.ToInt32(ID);
                    _ServiceModel = _ServicesRepository.AddEdit_Service(_ServiceModel);
                    if (_ServiceModel != null)
                    {
                        _ServiceModel.IsEdit = true;
                        SessionManager.Code = _ServiceModel.Code;
                    }
                    else
                    {
                        _ServiceModel = new ServiceModel();
                        ViewBag.NoRecordExist = true;
                        //_ServiceModel.Response_Message = "No record found";
                        SessionManager.Code = null;
                    }
                    #endregion
                }
                //// If url Apeended with Querystring ID=0 then Redirect into current Action
                else if (!string.IsNullOrEmpty(ID) && ID == "0")
                {
                    return RedirectToAction("AddEditServices");
                }
                else
                {
                    _ServiceModel.IsEdit = false;
                    _ServiceModel.IsActive = true;
                    SessionManager.Code = null;
                }
                _ServiceModel.lstDepartment = BindStaffDepartmentDropDown(SessionManager.CompanyId.ToString());

                List<SelectListItem> groupA = new List<SelectListItem>();
                groupA = BindAvailableStaffAccessGroupDropDown(SessionManager.CompanyId.ToString(), _ServiceModel.Id, "AvailableGroup");
                ViewBag.groupA = groupA;
                List<object> groupB = new List<object>();
                ViewBag.groupB = BindAvailableStaffAccessGroupDropDown(SessionManager.CompanyId.ToString(), _ServiceModel.Id, "AssignedGroup"); ;

                return View(_ServiceModel);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }

        }

        [HttpPost]
        public ActionResult AddEditServices(ServiceModel ServiceModel, string saveAndExit = "")
        {

            _ServiceModel.CreatedBy = SessionManager.UserId;
            _ServiceModel.ModifiedBy = SessionManager.UserId;
            _ServiceModel.CompanyId = SessionManager.CompanyId;
            _ServiceModel.Id = Convert.ToInt32(ServiceModel.Id);
            _ServiceModel.Name = ServiceModel.Name;
            _ServiceModel.Code = ServiceModel.Code;
            _ServiceModel.Description = ServiceModel.Description;
            _ServiceModel.DepartmentId = ServiceModel.DepartmentId;
            _ServiceModel.StartDateTime = ServiceModel.StartDateTime;
            _ServiceModel.EndDateTime = ServiceModel.EndDateTime;
            _ServiceModel.EmployeeIds = ServiceModel.EmployeeIds;
            _ServiceModel.IsActive = true;
            if(_ServiceModel.Id > 0)
            {
                _ServiceModel.ActionType = "Update";
            }
            else
            {
                _ServiceModel.ActionType = "Insert";
            }
            
            _ServiceModel = _ServicesRepository.AddEdit_Service(_ServiceModel);

            if (!string.IsNullOrEmpty(saveAndExit))
            {
                return RedirectToAction("Services", "Services");
            }
            else if (ServiceModel.IsEdit == true)
            {
                return RedirectToAction("AddEditServices", new { ID = ServiceModel.Id });
            }
            else
            {
                return RedirectToAction("AddEditServices");
            }
        }

        [HttpGet]
        public FileResult Export(string searchtxt = "")
        {
            DataTable dt = new DataTable("Services");
            try
            {
                dt = _ServicesRepository.GetServicesData_Export(searchtxt, SessionManager.CompanyId.ToString());
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Services.xlsx");
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
                _ServiceModel.ActionType = "Delete";
                _ServiceModel.Id = Convert.ToInt32(Id);
                var response = _ServicesRepository.AddEdit_Service(_ServiceModel);

                return RedirectToAction("Services", "Services");
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        private List<SelectListItem> BindStaffDepartmentDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StaffRepository.BindStaffDepartmentDropDown(CompanyId);
            return drpList;
        }
        private List<SelectListItem> BindAvailableStaffAccessGroupDropDown(string CompanyId,int Id,string ActionType)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _ServicesRepository.BindAvailableStaffAccessGroupDropDown(CompanyId,Id,ActionType);
            return drpList;
        }
    }
}