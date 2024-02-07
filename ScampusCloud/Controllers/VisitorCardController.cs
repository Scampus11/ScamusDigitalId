using ClosedXML.Excel;
using ScampusCloud.Models;
using ScampusCloud.Repository.VisitorCard;
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
    public class VisitorCardController : Controller
    {
        #region Variable Declaration
        private readonly VisitorCardRepository _VisitorCardRepository;
        VisitorCardModel _VisitorCardModel = new VisitorCardModel();
        #endregion
        #region CTOR
        public VisitorCardController()
        {
            _VisitorCardRepository = new VisitorCardRepository();
        }
        #endregion

        #region Method
        // GET: VisitorCard
        public ActionResult VisitorCard()
        {
            ViewData["paging_size"] = 10;
            if (ViewData["currentPage"] == null)
                ViewData["currentPage"] = 1;
            string searchtxt = "NA";
            int totals = Convert.ToInt32(_VisitorCardRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
            ViewData["totalrecords"] = totals;
            return View();
        }
        public ActionResult AddEditVisitorCard(string ID = "")
        {
            try
            {

                _VisitorCardModel.CreatedBy = SessionManager.UserId;
                _VisitorCardModel.ModifiedBy = SessionManager.UserId;
                _VisitorCardModel.CompanyId = SessionManager.CompanyId;

                if (!string.IsNullOrEmpty(ID) && ID != "0")
                {
                    #region Get Entity by id
                    _VisitorCardModel.ActionType = "Edit";
                    _VisitorCardModel.Id = Convert.ToInt32(ID);
                    _VisitorCardModel = _VisitorCardRepository.AddEdit_VisitorCard(_VisitorCardModel);
                    if (_VisitorCardModel != null)
                    {
                        _VisitorCardModel.IsEdit = true;
                        SessionManager.Code = _VisitorCardModel.Code;
                    }
                    else
                    {
                        _VisitorCardModel = new VisitorCardModel();
                        ViewBag.NoRecordExist = true;
                        _VisitorCardModel.Response_Message = "No record found";
                        SessionManager.Code = null;
                    }
                    #endregion
                }
                //// If url Apeended with Querystring ID=0 then Redirect into current Action
                else if (!string.IsNullOrEmpty(ID) && ID == "0")
                {
                    return RedirectToAction("AddEditVisitorCard");
                }
                else
                {
                    _VisitorCardModel.IsEdit = false;
                    _VisitorCardModel.IsActive = true;
                    SessionManager.Code = null;
                }
                _VisitorCardModel.lstCardStatus = BindCardStatusDropDown(SessionManager.CompanyId.ToString());
                return View(_VisitorCardModel);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }

        }
        [HttpPost]
        public ActionResult AddEditVisitorCard(VisitorCardModel _VisitorCardModel, string saveAndExit = "")
        {
            if (_VisitorCardModel.Id > 0)
            {
                _VisitorCardModel.ActionType = "Update";
            }
            else
            {
                _VisitorCardModel.ActionType = "Insert";
            }
            var officemaster = _VisitorCardRepository.AddEdit_VisitorCard(_VisitorCardModel);
            if (!string.IsNullOrEmpty(saveAndExit))
            {
                return RedirectToAction("VisitorCard", "VisitorCard");
            }
            else if (_VisitorCardModel.IsEdit == true)
            {
                return RedirectToAction("AddEditVisitorCard", new { ID = _VisitorCardModel.Id });
            }
            else
            {
                return RedirectToAction("AddEditVisitorCard");
            }
            //return RedirectToAction("VisitorCard", "VisitorCard");
        }

        public ActionResult VisitorCardList(int page = 1, int pagesize = 10, string searchtxt = "")
        {
            try
            {
                searchtxt = string.IsNullOrEmpty(searchtxt) ? "" : searchtxt;
                int totals = Convert.ToInt32(_VisitorCardRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
                var lstCountries = _VisitorCardRepository.GetVisitorCardList(searchtxt, page, pagesize, SessionManager.CompanyId.ToString());
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
                    strHTML.Append("<th class='datatable-cell'>Number</th>");
                    strHTML.Append("<th class='datatable-cell'>Status</th>");
                    strHTML.Append("<th class='datatable-cell'>Action</th>");
                    strHTML.Append("</tr>");
                    strHTML.Append("</thead>");
                    strHTML.Append("<tbody class='datatable-body custom-scroll'>");
                    foreach (var item in lstCountries)
                    {
                        string DeleteConfirmationEvent = "DeleteConfirmation('" + item.Id + "','VisitorCard','VisitorCard','Delete')";
                        strHTML.Append("<tr>");
                        strHTML.Append("<td>" + item.Code + "</td>");
                        strHTML.Append("<td>" + item.Name + "</td>");
                        strHTML.Append("<td>" + item.Number + "</td>");
                        if (item.IsActive)
                            strHTML.Append("<td><span><span class='label font-weight-bold label-lg label-light-primary label-inline'>Active</span></span></td>");
                        else
                            strHTML.Append("<td><span><span class='label font-weight-bold label-lg label-light-danger label-inline'>InActive</span></span></td>");

                        strHTML.Append("<td>");
                        strHTML.Append("<a class='btn btn-sm btn-icon btn-lg-light btn-text-primary btn-hover-light-primary mr-3' href= '/VisitorCard/AddEditVisitorCard?ID=" + item.Id + "'><i class='flaticon-edit'></i></a>");
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


            //return Json(VisitorCard, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VisitorCardListCount()
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
                _VisitorCardModel.ActionType = "Delete";
                _VisitorCardModel.Id = Convert.ToInt32(Id);
                var response = _VisitorCardRepository.AddEdit_VisitorCard(_VisitorCardModel);

                return RedirectToAction("VisitorCard", "VisitorCard");
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
            DataTable dt = new DataTable("VisitorCard");
            try
            {
                dt = _VisitorCardRepository.GetVisitorCardData_Export(searchtxt, SessionManager.CompanyId.ToString());
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "VisitorCard.xlsx");
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

        private List<SelectListItem> BindCardStatusDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _VisitorCardRepository.BindCardStatusDropDown(CompanyId);
            return drpList;
        }
    }
}