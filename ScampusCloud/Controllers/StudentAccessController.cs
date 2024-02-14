using ScampusCloud.Models;
using ScampusCloud.Repository.Student;
using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Controllers
{
    [SessionTimeoutAttribute]
    public class StudentAccessController : Controller
    {
        #region Variable Declaration
        private readonly StudentRepository _StudentRepository;
        StudentAccessGroupModel _StudentAccessGroupModel = new StudentAccessGroupModel();
        #endregion

        public StudentAccessController()
        {
                _StudentRepository = new StudentRepository();
        }

        // GET: StudentAccess
        public ActionResult StudentAccess()
        {
            _StudentAccessGroupModel.lstCampus = BindCampusDropdown(SessionManager.CompanyId.ToString());
            _StudentAccessGroupModel.lstYear = BindYearDropDown(SessionManager.CompanyId.ToString());
            _StudentAccessGroupModel.lstAdmissionType = BindAdmissionTypeDropDown(SessionManager.CompanyId.ToString());
            return View(_StudentAccessGroupModel);
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
    }
}