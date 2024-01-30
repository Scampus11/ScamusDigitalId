using ClosedXML.Excel;
using ScampusCloud.Models;
using ScampusCloud.Repository.Student;
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

namespace ScampusCloud.Controllers
{
    [SessionTimeoutAttribute]
    public class StudentController : Controller
    {

        #region Variable Declaration
        private readonly StudentRepository _StudentRepository;
        StudentModel _StudentModel = new StudentModel();
        #endregion

        #region CTOR
        public StudentController()
        {
            _StudentRepository = new StudentRepository();
        }
        #endregion

        #region Method
        // GET: Student
        public ActionResult Student()
        {
            ViewData["paging_size"] = 10;
            if (ViewData["currentPage"] == null)
                ViewData["currentPage"] = 1;
            string searchtxt = "NA";
            int totals = Convert.ToInt32(_StudentRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
            ViewData["totalrecords"] = totals;
            return View();
        }
        public ActionResult AddEditStudent(string ID = "")
        {
            try
            {

                _StudentModel.CreatedBy = SessionManager.UserId;
                _StudentModel.ModifiedBy = SessionManager.UserId;
                _StudentModel.CompanyId = SessionManager.CompanyId;

                if (!string.IsNullOrEmpty(ID) && ID != "0")
                {
                    #region Get Entity by id
                    _StudentModel.ActionType = "Edit";
                    _StudentModel.Id = Convert.ToInt32(ID);
                    _StudentModel = _StudentRepository.AddEdit_Student(_StudentModel);
                    if (_StudentModel != null)
                    {
                        _StudentModel.IsEdit = true;
                        SessionManager.StudentId = _StudentModel.StudentId;
                        SessionManager.EmailId = _StudentModel.EmailId;
                        SessionManager.Code = _StudentModel.Code;
                    }
                    else
                    {
                        _StudentModel = new StudentModel();
                        ViewBag.NoRecordExist = true;
                        _StudentModel.Response_Message = "No record found";
                        SessionManager.StudentId = null;
                        SessionManager.EmailId = null;
                        SessionManager.Code = null;
                    }
                    #endregion
                }
                //// If url Apeended with Querystring ID=0 then Redirect into current Action
                else if (!string.IsNullOrEmpty(ID) && ID == "0")
                {
                    return RedirectToAction("AddEditStudent");
                }
                else
                {
                    _StudentModel.IsEdit = false;
                    _StudentModel.Isactive = true;
                    SessionManager.StudentId = null;
                    SessionManager.EmailId = null;
                    SessionManager.Code = null;
                }
                _StudentModel.lstCampus = BindCampusDropdown(SessionManager.CompanyId.ToString());
                _StudentModel.lstYear = BindYearDropDown(SessionManager.CompanyId.ToString());
                _StudentModel.lstProgram = BindProgramDropDown(SessionManager.CompanyId.ToString());
                _StudentModel.lstDegreeType = BindDegreeTypeDropDown(SessionManager.CompanyId.ToString());
                _StudentModel.lstAdmissionType = BindAdmissionTypeDropDown(SessionManager.CompanyId.ToString());
                _StudentModel.lstCardStatus = BindCardStatusDropDown(SessionManager.CompanyId.ToString());
                //_StudentModel.lstCanteen = BindCanteenDropDown(SessionManager.CompanyId.ToString());
                return View(_StudentModel);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }

        }
        [HttpPost]
        public ActionResult AddEditStudent(StudentModel _StudentModel, string saveAndExit = "")
        {
            if (_StudentModel.Id > 0)
            {
                _StudentModel.ActionType = "Update";
            }
            else
            {
                _StudentModel.ActionType = "Insert";
            }
            var officemaster = _StudentRepository.AddEdit_Student(_StudentModel);
            if (!string.IsNullOrEmpty(saveAndExit))
            {
                return RedirectToAction("Student", "Student");
            }
            else if (_StudentModel.IsEdit == true)
            {
                return RedirectToAction("AddEditStudent", new { ID = _StudentModel.Id });
            }
            else
            {
                return RedirectToAction("AddEditStudent");
            }
            //return RedirectToAction("Student", "Student");
        }

        public ActionResult StudentList(int page = 1, int pagesize = 10, string searchtxt = "")
        {
            try
            {
                searchtxt = string.IsNullOrEmpty(searchtxt) ? "" : searchtxt;
                int totals = Convert.ToInt32(_StudentRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
                var lstStudent = _StudentRepository.GetStudentList(searchtxt, page, pagesize, SessionManager.CompanyId.ToString());
                Session["totalrecords"] = Convert.ToString(totals);
                Session["paging_size"] = Convert.ToString(pagesize);
                ViewData["totalrecords"] = totals;
                ViewData["paging_size"] = pagesize;
                StringBuilder strHTML = new StringBuilder();
                if (lstStudent.Count > 0)
                {
                    strHTML.Append("<table class='datatable-bordered datatable-head-custom datatable-table' id='kt_datatable'>");
                    strHTML.Append("<thead class='datatable-head'>");
                    strHTML.Append("<tr class='datatable-row'>");
                    strHTML.Append("<th class='datatable-cell'>Code</th>");
                    strHTML.Append("<th class='datatable-cell'>Name</th>");
                    strHTML.Append("<th class='datatable-cell'>Student ID</th>");
                    strHTML.Append("<th class='datatable-cell'>Email</th>");
                    strHTML.Append("<th class='datatable-cell'>Department</th>");
                    strHTML.Append("<th class='datatable-cell hide'>Job Title</th>");
                    strHTML.Append("<th class='datatable-cell'>Action</th>");
                    strHTML.Append("</tr>");
                    strHTML.Append("</thead>");
                    strHTML.Append("<tbody class='datatable-body custom-scroll'>");
                    foreach (var item in lstStudent)
                    {
                        string ImgSrc = string.Empty;
                        string DeleteConfirmationEvent = "DeleteConfirmation('" + item.StudentId + "','Student','Student','Delete')";
                        string CardManagementEvent = "CardManagement('" + item.Id + "')";
                        //if (!string.IsNullOrEmpty(item.Image64byte))
                        //    ImgSrc = "data:image/jpg;base64," + item.Image64byte;
                        //else
                        //    ImgSrc = "data:image/jpg;base64," + Constant.DefaultPersonIconBase64String;

                        strHTML.Append("<tr>");
                        strHTML.Append("<td>" + item.Code + "</td>");
                        strHTML.Append("<td style='width:250px;'><span><div class='d-flex align-items-center'><div class='symbol symbol-40 flex-shrink-0'><img src='" + ImgSrc + "' style='height:40px;border-radius:100%;border:1px solid;' alt='photo'></div>" +
                            "<div class='ml-4'>" +
                            "<a href='#' class='font-size-sm text-dark-50 text-hover-primary'>" + item.FirstName + "</a></div></div></span></td>");
                        strHTML.Append("<td>" + item.StudentId + "</td>");
                        strHTML.Append("<td>" + (item.EmailId ?? "NA") + "</td>");
                        strHTML.Append("<td>" + (item.DepartmentName ?? "NA") + "</td>");
                        strHTML.Append("<td class='hide'>" + (item.JobTitleName ?? "NA") + "</td>");
                        strHTML.Append("<td>");
                        strHTML.Append("<a class='btn btn-sm btn-icon btn-lg-light btn-text-primary btn-hover-light-primary mr-3' href= '/Student/AddEditStudent?ID=" + item.Id + "'><i class='flaticon-edit'></i></a>");
                        strHTML.Append("<a class='btn btn-sm btn-icon btn-lg-light btn-text-primary btn-hover-light-primary mr-3'  onclick=" + CardManagementEvent + "><i class='far fa-id-card'></i></a>");
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


            //return Json(Student, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StudentListCount()
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
                _StudentModel.ActionType = "Delete";
                _StudentModel.Id = Convert.ToInt32(Id);
                var response = _StudentRepository.AddEdit_Student(_StudentModel);

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
            DataTable dt = new DataTable("Student");
            try
            {
                dt = _StudentRepository.GetStudentData_Export(searchtxt, SessionManager.CompanyId.ToString());
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Student.xlsx");
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
        private List<SelectListItem> BindCampusDropdown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StudentRepository.BindCampusDropdown(CompanyId);
            return drpList;
        }
        private List<SelectListItem> BindStudentDepartmentDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StudentRepository.BindStudentDepartmentDropDown(CompanyId);
            return drpList;
        }
        private List<SelectListItem> BindProgramDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StudentRepository.BindProgramDropDown(CompanyId);
            return drpList;
        }

        private List<SelectListItem> BindDegreeTypeDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StudentRepository.BindDegreeTypeDropDown(CompanyId);
            return drpList;
        }
        private List<SelectListItem> BindAdmissionTypeDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StudentRepository.BindAdmissionTypeDropDown(CompanyId);
            return drpList;
        }
        private List<SelectListItem> BindCardStatusDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StudentRepository.BindCardStatusDropDown(CompanyId);
            return drpList;
        }
        private List<SelectListItem> BindCanteenDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StudentRepository.BindCanteenDropDown(CompanyId);
            return drpList;
        }
        private List<SelectListItem> BindYearDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StudentRepository.BindYearDropDown(CompanyId);
            return drpList;
        }
        [HttpPost]
        public ActionResult GetColleageByCampus(string ScampusId)
        {
            int CampusId;
            List<SelectListItem> colleageNames = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(ScampusId))
            {
                CampusId = Convert.ToInt32(ScampusId);
                colleageNames = _StudentRepository.BindColleageDropDown(SessionManager.CompanyId.ToString(), CampusId);
            }
            return Json(colleageNames, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDepartmentByColleage(string ColleageId)
        {
            int colleageId;
            List<SelectListItem> departmentNames = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(ColleageId))
            {
                colleageId = Convert.ToInt32(ColleageId);
                departmentNames = _StudentRepository.BindDepartmentDropDown(SessionManager.CompanyId.ToString(), colleageId);
            }
            return Json(departmentNames, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetAdmissionTypeShort(string AdmissionTypeId)
        {
            int admissionTypeId;
            List<SelectListItem> admissionShortTypeNames = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(AdmissionTypeId))
            {
                admissionTypeId = Convert.ToInt32(AdmissionTypeId);
                admissionShortTypeNames = _StudentRepository.BindAdmissionTypeShortDropDown(SessionManager.CompanyId.ToString(), admissionTypeId);
            }
            return Json(admissionShortTypeNames, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

}