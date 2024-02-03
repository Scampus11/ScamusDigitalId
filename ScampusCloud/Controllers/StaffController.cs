using ClosedXML.Excel;
using ScampusCloud.Models;
using ScampusCloud.Repository.Staff;
using ScampusCloud.Repository.Login;
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
using static ScampusCloud.RouteConfig;
using ScampusCloud.Repository.Student;

namespace ScampusCloud.Controllers
{
    [SessionTimeoutAttribute]
    public class StaffController : Controller
    {

        #region Variable Declaration
        private readonly StaffRepository _StaffRepository;
        StaffModel _StaffModel = new StaffModel();
        #endregion

        #region CTOR
        public StaffController()
        {
            _StaffRepository = new StaffRepository();
        }
        #endregion

        #region Method
        // GET: Staff
        public ActionResult Staff()
        {
            ViewData["paging_size"] = 10;
            if (ViewData["currentPage"] == null)
                ViewData["currentPage"] = 1;
            string searchtxt = "NA";
            int totals = Convert.ToInt32(_StaffRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
            ViewData["totalrecords"] = totals;
            return View();
        }
        public ActionResult AddEditStaff(string ID = "")
        {
            try
            {

                _StaffModel.CreatedBy = SessionManager.UserId;
                _StaffModel.ModifiedBy = SessionManager.UserId;
                _StaffModel.CompanyId = SessionManager.CompanyId;

                if (!string.IsNullOrEmpty(ID) && ID != "0")
                {
                    #region Get Entity by id
                    _StaffModel.ActionType = "Edit";
                    _StaffModel.Id = Convert.ToInt32(ID);
                    _StaffModel = _StaffRepository.AddEdit_Staff(_StaffModel);
                    if (_StaffModel != null)
                    {
                        _StaffModel.IsEdit = true;
                        SessionManager.StaffId= _StaffModel.StaffId;
                        SessionManager.EmailId = _StaffModel.EmailId;
                        SessionManager.Code = _StaffModel.Code;
                        _StaffModel.Password = !string.IsNullOrEmpty(_StaffModel.Password) ? EncryptionDecryption.GetDecrypt(_StaffModel.Password) : string.Empty;
                        //if (model.Code == null)
                        //    model.Code = "";
                        //HttpContext.Session.SetString("Original_Id", model.Code);
                    }
                    else
                    {
                        _StaffModel = new StaffModel();
                        ViewBag.NoRecordExist = true;
                        _StaffModel.Response_Message = "No record found";
                        SessionManager.StaffId = null;
                        SessionManager.EmailId = null;
                        SessionManager.Code = null;
                    }
                    #endregion
                }
                //// If url Apeended with Querystring ID=0 then Redirect into current Action
                else if (!string.IsNullOrEmpty(ID) && ID == "0")
                {
                    return RedirectToAction("AddEditStaff");
                }
                else
                {
                    _StaffModel.IsEdit = false;
                    _StaffModel.Isactive = true;
                    SessionManager.StaffId = null;
                    SessionManager.EmailId = null;
                    SessionManager.Code = null;
                }
                _StaffModel.lstCountry = BindCountryDropdown(SessionManager.CompanyId.ToString());
                _StaffModel.lstDepartment = BindStaffDepartmentDropDown(SessionManager.CompanyId.ToString());
                _StaffModel.lstJobTitle = BindJobTitleDropDown(SessionManager.CompanyId.ToString());
                _StaffModel.lstCompany = BindStudentCompanyDropDown(SessionManager.CompanyId.ToString());
                _StaffModel.lstFacility = BindFacilityDropDown(SessionManager.CompanyId.ToString());
                _StaffModel.lstCardStatus = BindCardStatusDropDown(SessionManager.CompanyId.ToString());
                _StaffModel.lstCanteen = BindCanteenDropDown(SessionManager.CompanyId.ToString());
                return View(_StaffModel);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }

        }
        [HttpPost]
        public ActionResult AddEditStaff(StaffModel _StaffModel, string saveAndExit = "")
        {
            if (_StaffModel.Id > 0)
            {
                _StaffModel.ActionType = "Update";
            }
            else
            {
                _StaffModel.ActionType = "Insert";
            }

            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                if (file.ContentLength > 0)
                {
                    int fileSize = file.ContentLength;
                    string fileName = file.FileName;
                    string mimeType = file.ContentType;
                    System.IO.Stream fileContent = file.InputStream;
                    file.SaveAs(Server.MapPath("~/Images/Student/" + fileName)); //File will be saved in application root
                    if (Request.Files.AllKeys[i].Equals("uploadPhotoFile"))
                    {
                        _StaffModel.ImagePath = "~/Images/" + fileName;
                    }
                    else
                    {
                        _StaffModel.SignaturePath = "~/Images/" + fileName;
                    }
                }

            }

            var officemaster = _StaffRepository.AddEdit_Staff(_StaffModel);
            if (!string.IsNullOrEmpty(saveAndExit))
            {
                return RedirectToAction("Staff", "Staff");
            }
            else if (_StaffModel.IsEdit == true)
            {
                return RedirectToAction("AddEditStaff", new { ID = _StaffModel.Id });
            }
            else
            {
                return RedirectToAction("AddEditStaff");
            }
            //return RedirectToAction("Staff", "Staff");
        }

        public ActionResult StaffList(int page = 1, int pagesize = 10, string searchtxt = "")
        {
            try
            {
                searchtxt = string.IsNullOrEmpty(searchtxt) ? "" : searchtxt;
                int totals = Convert.ToInt32(_StaffRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
                var lstStaff = _StaffRepository.GetStaffList(searchtxt, page, pagesize, SessionManager.CompanyId.ToString());
                Session["totalrecords"] = Convert.ToString(totals);
                Session["paging_size"] = Convert.ToString(pagesize);
                ViewData["totalrecords"] = totals;
                ViewData["paging_size"] = pagesize;
                StringBuilder strHTML = new StringBuilder();
                if (lstStaff.Count > 0)
                {
                    strHTML.Append("<table class='datatable-bordered datatable-head-custom datatable-table' id='kt_datatable'>");
                    strHTML.Append("<thead class='datatable-head'>");
                    strHTML.Append("<tr class='datatable-row'>");
                    strHTML.Append("<th class='datatable-cell'>Code</th>");
                    strHTML.Append("<th class='datatable-cell'>Name</th>");
                    strHTML.Append("<th class='datatable-cell'>Staff ID</th>");
                    strHTML.Append("<th class='datatable-cell'>Email</th>");
                    strHTML.Append("<th class='datatable-cell'>Department</th>");
                    strHTML.Append("<th class='datatable-cell hide'>Job Title</th>");
                    strHTML.Append("<th class='datatable-cell'>Action</th>");
                    strHTML.Append("</tr>");
                    strHTML.Append("</thead>");
                    strHTML.Append("<tbody class='datatable-body custom-scroll'>");
                    foreach (var item in lstStaff)
                    {
                        string ImgSrc = string.Empty;
                        string PhotoImgSrc = string.Empty;
                        string DeleteConfirmationEvent = "DeleteConfirmation('" + item.StaffId + "','Staff','Staff','Delete')";
                        string CardManagementEvent = "CardManagement('" + item.Id + "')";
                        string fileExtension = Path.GetExtension(item.ImagePath);

                        if (!string.IsNullOrEmpty(item.ImageBase64))
                            PhotoImgSrc = "data:image/" + fileExtension.TrimStart('.') + ";base64," + item.ImageBase64;

                        strHTML.Append("<tr>");
                        strHTML.Append("<td>" + item.Code + "</td>");
                        strHTML.Append("<td style='width:250px;'><span><div class='d-flex align-items-center'><div class='symbol symbol-40 flex-shrink-0'><img src='" + PhotoImgSrc + "' style='height:40px;border-radius:100%;border:1px solid;' alt='photo'></div>" +
                             "<div class='ml-4'>" +
                             "<a href='#' class='font-size-sm text-dark-50 text-hover-primary'>" + item.FullName + "</a></div></div></span></td>");
                        strHTML.Append("<td>" + item.StaffId + "</td>");
                        strHTML.Append("<td>" + (item.EmailId ?? "NA") + "</td>");
                        strHTML.Append("<td>" + (item.DepartmentName ?? "NA") + "</td>");
                        strHTML.Append("<td class='hide'>" + (item.JobTitleName ?? "NA") + "</td>");
                        strHTML.Append("<td>");
                        strHTML.Append("<a class='btn btn-sm btn-icon btn-lg-light btn-text-primary btn-hover-light-primary mr-3' href= '/Staff/AddEditStaff?ID=" + item.Id + "'><i class='flaticon-edit'></i></a>");
                       // strHTML.Append("<a class='btn btn-sm btn-icon btn-lg-light btn-text-primary btn-hover-light-primary mr-3'  onclick=" + CardManagementEvent + "><i class='far fa-id-card'></i></a>");
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


            //return Json(Staff, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StaffListCount()
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
                _StaffModel.ActionType = "Delete";
                _StaffModel.Id = Convert.ToInt32(Id);
                var response = _StaffRepository.AddEdit_Staff(_StaffModel);

                return RedirectToAction("Staff", "Staff");
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        [HttpPost]
        public ActionResult RemovePhotoSignature(string Id, string PhotoType)
        {
            try
            {
                _StaffModel.ActionType = PhotoType;
                _StaffModel.Id = Convert.ToInt32(Id);
                _StaffModel.CompanyId = SessionManager.CompanyId;
                var response = _StaffRepository.AddEdit_Staff(_StaffModel);

                return RedirectToAction("Student", "Student");
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
            DataTable dt = new DataTable("Staff");
            try
            {
                dt = _StaffRepository.GetStaffData_Export(searchtxt,SessionManager.CompanyId.ToString());
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Staff.xlsx");
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
        #region Private method
        private List<SelectListItem> BindCountryDropdown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StaffRepository.BindCountryDropDown(CompanyId);
            return drpList;
        }
        private List<SelectListItem> BindStaffDepartmentDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StaffRepository.BindStaffDepartmentDropDown(CompanyId);
            return drpList;
        }
        private List<SelectListItem> BindJobTitleDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StaffRepository.BindJobTitleDropDown(CompanyId);
            return drpList;
        }

        private List<SelectListItem> BindStudentCompanyDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StaffRepository.BindStudentCompanyDropDown(CompanyId);
            return drpList;
        }
        private List<SelectListItem> BindFacilityDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StaffRepository.BindFacilityDropDown(CompanyId);
            return drpList;
        }
        private List<SelectListItem> BindCardStatusDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StaffRepository.BindCardStatusDropDown(CompanyId);
            return drpList;
        }
        private List<SelectListItem> BindCanteenDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StaffRepository.BindCanteenDropDown(CompanyId);
            return drpList;
        }
        #endregion

    }
}