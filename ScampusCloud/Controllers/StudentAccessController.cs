using ScampusCloud.Models;
using ScampusCloud.Repository.AccessGroup;
using ScampusCloud.Repository.Reader;
using ScampusCloud.Repository.Student;
using ScampusCloud.Repository.StudentAccess;
using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Controllers
{
    [SessionTimeoutAttribute]
    public class StudentAccessController : Controller
    {
        #region Variable Declaration
        private readonly StudentRepository _StudentRepository;
        private readonly AccessGroupRepository _AccessGroupRepository;
        private readonly StudentAccessRepository _StudentAccessRepository;
        StudentAccessGroupModel _StudentAccessGroupModel = new StudentAccessGroupModel();
        #endregion

        public StudentAccessController()
        {
            _StudentRepository = new StudentRepository();
            _AccessGroupRepository = new AccessGroupRepository();
            _StudentAccessRepository = new StudentAccessRepository();
        }

        // GET: StudentAccess
        public ActionResult StudentAccess()
        {
            ViewData["paging_size"] = 10;
            if (ViewData["currentPage"] == null)
                ViewData["currentPage"] = 1;
            string searchtxt = "NA";
            int totals = Convert.ToInt32(_AccessGroupRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
            ViewData["totalrecords"] = totals;
            _StudentAccessGroupModel.lstCampus = BindCampusDropdown(SessionManager.CompanyId.ToString());
            _StudentAccessGroupModel.lstYear = BindYearDropDown(SessionManager.CompanyId.ToString());
            _StudentAccessGroupModel.lstAdmissionType = BindAdmissionTypeDropDown(SessionManager.CompanyId.ToString());
            _StudentAccessGroupModel.lstAccessGroupDropdown = BindAccessGroupTypeDropDown(SessionManager.CompanyId.ToString());

            List<SelectListItem> groupA = new List<SelectListItem>();
            groupA = BindAccessGroupDropDown(SessionManager.CompanyId.ToString());

            //List<object> groupA = new List<object>();
            //groupA.Add(new { Name = "Australia", Code = "AU" });
            //groupA.Add(new { Name = "Bermuda", Code = "BM" });
            //groupA.Add(new { Name = "Canada", Code = "CA" });
            //groupA.Add(new { Name = "Cameroon", Code = "CM" });
            //groupA.Add(new { Name = "Denmark", Code = "DK" });
            //groupA.Add(new { Name = "France", Code = "FR" });
            //groupA.Add(new { Name = "Finland", Code = "FI" });
            //groupA.Add(new { Name = "Germany", Code = "DE" });
            //groupA.Add(new { Name = "Hong Kong", Code = "HK" });
            ViewBag.groupA = groupA;
            List<object> groupB = new List<object>();
            ViewBag.groupB = groupB;
            return View(_StudentAccessGroupModel);
        }
        public ActionResult StudentAccessGroupList(int page = 1, int pagesize = 10, string searchtxt = "", int CampusId = 0, int CollegeId = 0, int DepartmentId = 0, int YearId = 0, int AdmissionTypeId = 0)
        {
            try
            {
                searchtxt = string.IsNullOrEmpty(searchtxt) ? "" : searchtxt;
                int totals = Convert.ToInt32(_StudentAccessRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
                var lstCountries = _StudentAccessRepository.GetStudentAccessList(searchtxt, page, pagesize, CampusId, CollegeId, DepartmentId, YearId, AdmissionTypeId, SessionManager.CompanyId.ToString());
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
                    strHTML.Append("<th class='datatable-cell'><span><input type=\"checkbox\" /></span></th>");
                    strHTML.Append("<th class='datatable-cell'>Student Id</th>");
                    strHTML.Append("<th class='datatable-cell'>Student Name</th>");
                    strHTML.Append("<th class='datatable-cell'>College</th>");
                    strHTML.Append("<th class='datatable-cell'>Department </th>");
                    strHTML.Append("<th class='datatable-cell'>Admission Type</th>");
                    strHTML.Append("<th class='datatable-cell'>Campus</th>");
                    strHTML.Append("<th class='datatable-cell'>Batch Year</th>");
                    strHTML.Append("<th class='datatable-cell'>Access Group </th>");
                    //strHTML.Append("<th class='datatable-cell'>Canteen</th>");
                    //strHTML.Append("<th class='datatable-cell'>Block Group</th>");
                    strHTML.Append("<th class='datatable-cell'>Action</th>");
                    strHTML.Append("</tr>");
                    strHTML.Append("</thead>");
                    strHTML.Append("<tbody class='datatable-body custom-scroll'>");
                    foreach (var item in lstCountries)
                    {
                        string DeleteConfirmationEvent = "DeleteConfirmation('" + item.Id + "','StudentAccess','StudentAccess','Delete')";
                        strHTML.Append("<tr>");
                        strHTML.Append("<td><input type=\"checkbox\" />&nbsp;</td>");
                        strHTML.Append("<td>" + item.StudentId + "</td>");
                        strHTML.Append("<td>" + item.StudentName + "</td>");
                        strHTML.Append("<td>" + item.College + "</td>");
                        strHTML.Append("<td>" + item.Department + "</td>");
                        strHTML.Append("<td>" + item.AdmissionType + "</td>");
                        strHTML.Append("<td>" + item.Campus + "</td>");
                        strHTML.Append("<td>" + item.BatchYear + "</td>");
                        strHTML.Append("<td>" + item.AccessGroup + "</td>");
                        //if (item.IsActive)
                        //    strHTML.Append("<td><span><span class='label font-weight-bold label-lg label-light-primary label-inline'>Active</span></span></td>");
                        //else
                        //    strHTML.Append("<td><span><span class='label font-weight-bold label-lg label-light-danger label-inline'>InActive</span></span></td>");
                        strHTML.Append("<td>");
                        strHTML.Append("<a class='btn btn-sm btn-icon btn-lg-light btn-text-primary btn-hover-light-primary mr-3' href= '/StudentAccess/AddEditStudentAccess?ID=" + item.Id + "'><i class='flaticon-edit'></i></a>");
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

        public ActionResult StudentAccessGroupListCount()
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
        private List<SelectListItem> BindCampusDropdown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StudentRepository.BindCampusDropdown(CompanyId);
            return drpList;
        }
        private List<SelectListItem> BindYearDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StudentRepository.BindYearDropDown(CompanyId);
            return drpList;
        }
        private List<SelectListItem> BindAdmissionTypeDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _StudentRepository.BindAdmissionTypeDropDown(CompanyId);
            return drpList;
        }
        private List<SelectListItem> BindAccessGroupTypeDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _AccessGroupRepository.BindAccessGroupTypeDropDown(CompanyId);
            return drpList;
        }
        private List<SelectListItem> BindAccessGroupDropDown(string CompanyId)
        {
            List<SelectListItem> drpList = new List<SelectListItem>();
            drpList = _AccessGroupRepository.BindAccessGroupDropDown(CompanyId);
            return drpList;
        }
    }
}