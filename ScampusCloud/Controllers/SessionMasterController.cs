using ClosedXML.Excel;
using ScampusCloud.Models;
using ScampusCloud.Repository.SessionMaster;
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
    public class SessionMasterController : Controller
    {
        #region Variable Declaration
        private readonly SessionMasterRepository _SessionMasterRepository;
        SessionMasterModel _SessionMasterModel = new SessionMasterModel();
        #endregion
        #region CTOR
        public SessionMasterController()
        {
            _SessionMasterRepository = new SessionMasterRepository();
        }
        #endregion

        #region Method
        // GET: SessionMaster
        public ActionResult SessionMaster()
        {
            ViewData["paging_size"] = 10;
            if (ViewData["currentPage"] == null)
                ViewData["currentPage"] = 1;
            string searchtxt = "NA";
            int totals = Convert.ToInt32(_SessionMasterRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
            ViewData["totalrecords"] = totals;
            return View();
        }
        public ActionResult AddEditSessionMaster(string ID = "")
        {
            try
            {

                _SessionMasterModel.CreatedBy = SessionManager.UserId;
                _SessionMasterModel.ModifiedBy = SessionManager.UserId;
                _SessionMasterModel.CompanyId = SessionManager.CompanyId;

                if (!string.IsNullOrEmpty(ID) && ID != "0")
                {
                    #region Get Entity by id
                    _SessionMasterModel.ActionType = "Edit";
                    _SessionMasterModel.Id = Convert.ToInt32(ID);
                    _SessionMasterModel = _SessionMasterRepository.AddEdit_SessionMaster(_SessionMasterModel);
                    if (_SessionMasterModel != null)
                    {
                        _SessionMasterModel.IsEdit = true;
                        SessionManager.Code = _SessionMasterModel.Code;
                    }
                    else
                    {
                        _SessionMasterModel = new SessionMasterModel();
                        ViewBag.NoRecordExist = true;
                        _SessionMasterModel.Response_Message = "No record found";
                        SessionManager.Code = null;
                    }
                    #endregion
                }
                //// If url Apeended with Querystring ID=0 then Redirect into current Action
                else if (!string.IsNullOrEmpty(ID) && ID == "0")
                {
                    return RedirectToAction("AddEditSessionMaster");
                }
                else
                {
                    _SessionMasterModel.IsEdit = false;
                    _SessionMasterModel.IsActive = true;
                    SessionManager.Code = null;
                }
                return View(_SessionMasterModel);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }

        }
        [HttpPost]
        public ActionResult AddEditSessionMaster(SessionMasterModel _SessionMasterModel, string saveAndExit = "")
        {
            if (_SessionMasterModel.Id > 0)
            {
                _SessionMasterModel.ActionType = "Update";
            }
            else
            {
                _SessionMasterModel.ActionType = "Insert";
            }
            var officemaster = _SessionMasterRepository.AddEdit_SessionMaster(_SessionMasterModel);
            if (!string.IsNullOrEmpty(saveAndExit))
            {
                return RedirectToAction("SessionMaster", "SessionMaster");
            }
            else if (_SessionMasterModel.IsEdit == true)
            {
                return RedirectToAction("AddEditSessionMaster", new { ID = _SessionMasterModel.Id });
            }
            else
            {
                return RedirectToAction("AddEditSessionMaster");
            }
            //return RedirectToAction("SessionMaster", "SessionMaster");
        }

        public ActionResult SessionMasterList(int page = 1, int pagesize = 10, string searchtxt = "")
        {
            try
            {
                searchtxt = string.IsNullOrEmpty(searchtxt) ? "" : searchtxt;
                int totals = Convert.ToInt32(_SessionMasterRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
                var lstCountries = _SessionMasterRepository.GetSessionMasterList(searchtxt, page, pagesize, SessionManager.CompanyId.ToString());
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
                    strHTML.Append("<th class='datatable-cell'>StartTime</th>");
                    strHTML.Append("<th class='datatable-cell'>EndTime</th>");
                    strHTML.Append("<th class='datatable-cell'>Status</th>");
                    strHTML.Append("<th class='datatable-cell'>Action</th>");
                    strHTML.Append("</tr>");
                    strHTML.Append("</thead>");
                    strHTML.Append("<tbody class='datatable-body custom-scroll'>");
                    foreach (var item in lstCountries)
                    {
                        string DeleteConfirmationEvent = "DeleteConfirmation('" + item.Id + "','SessionMaster','SessionMaster','Delete')";
                        strHTML.Append("<tr>");
                        strHTML.Append("<td>" + item.Code + "</td>");
                        strHTML.Append("<td>" + item.Name + "</td>");
                        strHTML.Append("<td>" + item.Description + "</td>");
                        strHTML.Append("<td>" + item.StartTime + "</td>");
                        strHTML.Append("<td>" + item.EndTime + "</td>");
                        if (item.IsActive)
                            strHTML.Append("<td><span><span class='label font-weight-bold label-lg label-light-primary label-inline'>Active</span></span></td>");
                        else
                            strHTML.Append("<td><span><span class='label font-weight-bold label-lg label-light-danger label-inline'>InActive</span></span></td>");

                        strHTML.Append("<td>");
                        strHTML.Append("<a class='btn btn-sm btn-icon btn-lg-light btn-text-primary btn-hover-light-primary mr-3' href= '/SessionMaster/AddEditSessionMaster?ID=" + item.Id + "'><i class='flaticon-edit'></i></a>");
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


            //return Json(SessionMaster, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SessionMasterListCount()
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
                _SessionMasterModel.ActionType = "Delete";
                _SessionMasterModel.Id = Convert.ToInt32(Id);
                var response = _SessionMasterRepository.AddEdit_SessionMaster(_SessionMasterModel);

                return RedirectToAction("SessionMaster", "SessionMaster");
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        //[HttpPost]
        //public string PreviewSelectedImage(IFormFile file)
        //{
        //    string base64OfThumbImg = string.Empty, originalImageBase64 = string.Empty;
        //    if (file != null)
        //    {
        //        //originalImageBase64 = ImageCompressor.OriginalBase64String(file).Result;
        //        base64OfThumbImg = ImageCompressor.GetBase64StringAsync(file, 300, 300).Result;
        //    }
        //    return base64OfThumbImg;
        //}

        [HttpGet]
        public FileResult Export(string searchtxt = "")
        {
            DataTable dt = new DataTable("SessionMaster");
            try
            {
                dt = _SessionMasterRepository.GetSessionMasterData_Export(searchtxt, SessionManager.CompanyId.ToString());
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SessionMaster.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }
        #endregion
    }
}