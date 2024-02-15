using ScampusCloud.Models;
using ScampusCloud.Repository.AccessGroup;
using ScampusCloud.Repository.Reader;
using ScampusCloud.Repository.Student;
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
        StudentAccessGroupModel _StudentAccessGroupModel = new StudentAccessGroupModel();
        #endregion

        public StudentAccessController()
        {
            _StudentRepository = new StudentRepository();
            _AccessGroupRepository = new AccessGroupRepository();
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
            return View(_StudentAccessGroupModel);
        }
        public ActionResult StudentAccessGroupList(int page = 1, int pagesize = 10, string searchtxt = "")
        {
            try
            {
                searchtxt = string.IsNullOrEmpty(searchtxt) ? "" : searchtxt;
                int totals = 0;// Convert.ToInt32(_ReaderRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
                //var lstCountries = null;// ReaderRepository.GetReaderList(searchtxt, page, pagesize, SessionManager.CompanyId.ToString());
                Session["totalrecords"] = Convert.ToString(totals);
                Session["paging_size"] = Convert.ToString(pagesize);
                ViewData["totalrecords"] = totals;
                ViewData["paging_size"] = pagesize;
                StringBuilder strHTML = new StringBuilder();
                //if (lstCountries.Count > 0)
                //{
                strHTML.Append("<table class='datatable-bordered datatable-head-custom datatable-table' id='kt_datatable'>");
                strHTML.Append("<thead class='datatable-head'>");
                strHTML.Append("<tr class='datatable-row'>");
                strHTML.Append("<th class='datatable-cell'>Student Id</th>");
                strHTML.Append("<th class='datatable-cell'>Student Name</th>");
                strHTML.Append("<th class='datatable-cell'>College Name</th>");
                strHTML.Append("<th class='datatable-cell'>Department Name</th>");
                strHTML.Append("<th class='datatable-cell'>Admission Type</th>");
                strHTML.Append("<th class='datatable-cell'>Campus Name</th>");
                strHTML.Append("<th class='datatable-cell'>Batch Year</th>");
                strHTML.Append("<th class='datatable-cell'>Access Group Name</th>");
                strHTML.Append("<th class='datatable-cell'>Canteen Name</th>");
                strHTML.Append("<th class='datatable-cell'>Block Group</th>");
                strHTML.Append("<th class='datatable-cell'>Action</th>");
                strHTML.Append("</tr>");
                strHTML.Append("</thead>");
                strHTML.Append("<tbody class='datatable-body custom-scroll'>");
                //foreach (var item in lstCountries)
                //{
                //    string DeleteConfirmationEvent = "DeleteConfirmation('" + item.Id + "','Reader','Reader','Delete')";
                //    strHTML.Append("<tr>");
                //    strHTML.Append("<td>" + item.Code + "</td>");
                //    strHTML.Append("<td>" + item.Name + "</td>");
                //    strHTML.Append("<td>" + item.ReaderType + "</td>");
                //    if (item.IsActive)
                //        strHTML.Append("<td><span><span class='label font-weight-bold label-lg label-light-primary label-inline'>Active</span></span></td>");
                //    else
                //        strHTML.Append("<td><span><span class='label font-weight-bold label-lg label-light-danger label-inline'>InActive</span></span></td>");

                //    strHTML.Append("<td>");
                //    strHTML.Append("<a class='btn btn-sm btn-icon btn-lg-light btn-text-primary btn-hover-light-primary mr-3' href= '/Reader/AddEditReader?ID=" + item.Id + "'><i class='flaticon-edit'></i></a>");
                //    strHTML.Append("<a id = 'del_" + item.Id + "' class='btn btn-sm btn-icon btn-lg-light btn-text-danger btn-hover-light-danger' onclick=" + DeleteConfirmationEvent + "><i class='flaticon-delete'></i></a>");
                //    strHTML.Append("</td>");
                //    strHTML.Append("</tr>");
                //}
                strHTML.Append("</tbody>");
                strHTML.Append("</table>");
                //}
                //else
                //{
                //    strHTML.Append("<center>No data available in table</center>");
                //}
                return Content(strHTML.ToString());
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }


            //return Json(Reader, JsonRequestBehavior.AllowGet);
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
    }
}