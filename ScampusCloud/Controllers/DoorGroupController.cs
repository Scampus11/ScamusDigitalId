using ClosedXML.Excel;
using ScampusCloud.Models;
using ScampusCloud.Repository.DoorGroup;
using ScampusCloud.Repository.Reader;
using ScampusCloud.Repository.Staff;
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
    public class DoorGroupController : Controller
    {
        DoorGroupModel _DoorGroupModel = new DoorGroupModel();
        private readonly DoorGroupRepository _DoorGroupRepository;

        public DoorGroupController()
        {
            _DoorGroupRepository = new DoorGroupRepository();
        }

        // GET: DoorGroup
        public ActionResult DoorGroup()
        {
            ViewData["paging_size"] = 10;
            if (ViewData["currentPage"] == null)
                ViewData["currentPage"] = 1;
            string searchtxt = "NA";
            int totals = Convert.ToInt32(_DoorGroupRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
            ViewData["totalrecords"] = totals;
            return View();
        }
        public ActionResult AddEditDoorGroup(string ID = "")
        {
            try
            {

                _DoorGroupModel.CreatedBy = SessionManager.UserId;
                _DoorGroupModel.ModifiedBy = SessionManager.UserId;
                _DoorGroupModel.CompanyId = SessionManager.CompanyId;

                if (!string.IsNullOrEmpty(ID) && ID != "0")
                {
                    #region Get Entity by id
                    _DoorGroupModel.ActionType = "Edit";
                    _DoorGroupModel.Id = Convert.ToInt32(ID);
                    _DoorGroupModel = _DoorGroupRepository.AddEdit_DoorGroup(_DoorGroupModel);
                    if (_DoorGroupModel != null)
                    {
                        _DoorGroupModel.IsEdit = true;
                        SessionManager.Code = _DoorGroupModel.Code;
                    }
                    else
                    {
                        _DoorGroupModel = new DoorGroupModel();
                        ViewBag.NoRecordExist = true;
                        _DoorGroupModel.Response_Message = "No record found";
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
                    _DoorGroupModel.IsEdit = false;
                    _DoorGroupModel.IsActive = true;
                    SessionManager.Code = null;
                }
                _DoorGroupModel.lstReader = BindReaderDropDown(SessionManager.CompanyId.ToString());
                return View(_DoorGroupModel);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }

        }

        [HttpPost]
        public ActionResult AddEditDoorGroup(DoorGroupModel _DoorGroupModel, string saveAndExit = "")
        {
            if (_DoorGroupModel.Id > 0)
            {
                _DoorGroupModel.ActionType = "Update";
            }
            else
            {
                _DoorGroupModel.ActionType = "Insert";
            }
            var officemaster = _DoorGroupRepository.AddEdit_DoorGroup(_DoorGroupModel);
            if (!string.IsNullOrEmpty(saveAndExit))
            {
                return RedirectToAction("DoorGroup", "DoorGroup");
            }
            else if (_DoorGroupModel.IsEdit == true)
            {
                return RedirectToAction("AddEditDoorGroup", new { ID = _DoorGroupModel.Id });
            }
            else
            {
                return RedirectToAction("AddEditDoorGroup");
            }
        }

        private List<SelectListItem> BindReaderDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _DoorGroupRepository.BindReaderDropDown(CompanyId);
            return drpList;
        }

        public ActionResult DoorGroupList(int page = 1, int pagesize = 10, string searchtxt = "")
        {
            try
            {
                searchtxt = string.IsNullOrEmpty(searchtxt) ? "" : searchtxt;
                int totals = Convert.ToInt32(_DoorGroupRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
                var lstCountries = _DoorGroupRepository.GetDoorGroupList(searchtxt, page, pagesize, SessionManager.CompanyId.ToString());
                Session["totalrecords"] = Convert.ToString(totals);
                Session["paging_size"] = Convert.ToString(pagesize);
                ViewData["totalrecords"] = totals;
                ViewData["paging_size"] = pagesize;
                StringBuilder strHTML = new StringBuilder();
                if (lstCountries.Count > 0)
                {
                    strHTML.Append("<table class='datatable-bordered datatable-head-custom datatable-table' id='kt_datatable'>");
                    strHTML.Append("<thead class='datatable-head'>");
                    strHTML.Append("<tr class='datatable-row'>");
                    strHTML.Append("<th class='datatable-cell'>Code</th>");
                    strHTML.Append("<th class='datatable-cell'>Name</th>");
                    strHTML.Append("<th class='datatable-cell'>Description</th>");
                    strHTML.Append("<th class='datatable-cell'>Reader Type</th>");
                    
                    strHTML.Append("<th class='datatable-cell'>Status</th>");
                    strHTML.Append("<th class='datatable-cell'>Action</th>");
                    strHTML.Append("</tr>");
                    strHTML.Append("</thead>");
                    strHTML.Append("<tbody class='datatable-body custom-scroll'>");
                    foreach (var item in lstCountries)
                    {
                        string DeleteConfirmationEvent = "DeleteConfirmation('" + item.Id + "','DoorGroup','DoorGroup','Delete')";
                        strHTML.Append("<tr>");
                        strHTML.Append("<td>" + item.Code + "</td>");
                        strHTML.Append("<td>" + item.Name + "</td>");
                        strHTML.Append("<td>" + item.Description + "</td>");
                        strHTML.Append("<td>" + item.ReaderTypes + "</td>");
                        if (item.IsActive)
                            strHTML.Append("<td><span><span class='label font-weight-bold label-lg label-light-primary label-inline'>Active</span></span></td>");
                        else
                            strHTML.Append("<td><span><span class='label font-weight-bold label-lg label-light-danger label-inline'>InActive</span></span></td>");

                        strHTML.Append("<td>");
                        strHTML.Append("<a class='btn btn-sm btn-icon btn-lg-light btn-text-primary btn-hover-light-primary mr-3' href= '/DoorGroup/AddEditDoorGroup?ID=" + item.Id + "'><i class='flaticon-edit'></i></a>");
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

        public ActionResult DoorGroupListCount()
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

        [HttpPost]
        public ActionResult Delete(string Id)
        {
            try
            {
                _DoorGroupModel.ActionType = "Delete";
                _DoorGroupModel.Id = Convert.ToInt32(Id);
                var response = _DoorGroupRepository.AddEdit_DoorGroup(_DoorGroupModel);
                return RedirectToAction("DoorGroup", "DoorGroup");
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
            DataTable dt = new DataTable("DoorGroup");
            try
            {
                dt = _DoorGroupRepository.GetReaderData_Export(searchtxt, SessionManager.CompanyId.ToString());
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DoorGroup.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }
    }
}