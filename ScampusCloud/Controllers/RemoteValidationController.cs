using ScampusCloud.Models;
using ScampusCloud.Repository.Admission;
using ScampusCloud.Repository.Campus;
using ScampusCloud.Repository.CardStatus;
using ScampusCloud.Repository.College;
using ScampusCloud.Repository.Country;
using ScampusCloud.Repository.Customer;
using ScampusCloud.Repository.DegreeType;
using ScampusCloud.Repository.JobTitle;
using ScampusCloud.Repository.Program;
using ScampusCloud.Repository.Reader;
using ScampusCloud.Repository.SessionMaster;
using ScampusCloud.Repository.Staff;
using ScampusCloud.Repository.StaffDepartment;
using ScampusCloud.Repository.StudentCompany;
using ScampusCloud.Repository.StudentDepartment;
using ScampusCloud.Repository.StudentFacility;
using ScampusCloud.Repository.VisitorAccessLevel;
using ScampusCloud.Repository.VisitorReason;
using ScampusCloud.Repository.VisitorStatus;
using ScampusCloud.Repository.VisitorType;
using ScampusCloud.Repository.Year;
using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Controllers
{
    [SessionTimeoutAttribute]
    public class RemoteValidationController : Controller
    {
        StaffModel Staff = new StaffModel();
        AdmissionModel Admission = new AdmissionModel();
        CampusModel Campus = new CampusModel();
        CardStatusModel CardStatus = new CardStatusModel();
        CollegeModel College = new CollegeModel();
        CountryModel Country = new CountryModel();
        CustomerModel Customer = new CustomerModel();
        DegreeTypeModel DegreeType = new DegreeTypeModel();
        JobTitleModel JobTitle = new JobTitleModel();
        ProgramModel Program = new ProgramModel();
        StaffDepartmentModel StaffDepartment = new StaffDepartmentModel();
        StudentCompanyModel StudentCompany = new StudentCompanyModel();
        StudentDepartmentModel StudentDepartment = new StudentDepartmentModel();
        StudentFacilityModel StudentFacility= new StudentFacilityModel();
        VisitorAccessLevelModel VisitorAccessLevel = new VisitorAccessLevelModel();
        VisitorReasonModel VisitorReason= new VisitorReasonModel();
        VisitorTypeModel VisitorType= new VisitorTypeModel();
        YearModel Year=new YearModel();
        VisitorStatusModel VisitorStatus = new VisitorStatusModel();
        ReaderModel Reader = new ReaderModel();
        SessionMasterModel SessionMaster = new SessionMasterModel();

        public RemoteValidationController()
        {
        }
        #region Staff
        [HttpPost]
        public ActionResult IsStaffIdExist(string StaffId = "")
        {
            StaffRepository _StaffRepository = new StaffRepository();
            string Original_StaffId = SessionManager.StaffId;
            bool IsEditMode = !string.IsNullOrEmpty(Original_StaffId) ? true : false;
            string returnMsg = "";
            
            if (IsEditMode && !string.Equals(Original_StaffId, StaffId))
            {
                Staff.ActionType = "Remote";
                Staff.StaffId = StaffId;
                Staff.CompanyId = SessionManager.CompanyId;
                Staff = _StaffRepository.AddEdit_Staff(Staff);
                returnMsg = $"Staff Id '{StaffId}' is already in use.";
            }
            else if (!IsEditMode)
            {
                Staff.ActionType = "Remote";
                Staff.StaffId = StaffId;
                Staff.CompanyId = SessionManager.CompanyId;
                Staff = _StaffRepository.AddEdit_Staff(Staff);
                returnMsg = $"Staff Id '{StaffId}' is already in use.";
            }
            if (Staff == null)
                return Json(true);
            else if (Staff.StaffId == null)
                return Json(true);
            else
                return Json(false);

        }
        [HttpPost]
        public ActionResult IsEmailIdExist(string EmailId = "")
        {
            StaffRepository _StaffRepository = new StaffRepository();
            string Original_EmailId = SessionManager.EmailId;
            bool IsEditMode = !string.IsNullOrEmpty(Original_EmailId) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_EmailId, EmailId))
            {
                Staff.ActionType = "Remote";
                Staff.EmailId = EmailId;
                Staff.CompanyId = SessionManager.CompanyId;
                Staff = _StaffRepository.AddEdit_Staff(Staff);
                returnMsg = $"Email Id '{EmailId}' is already in use.";
            }
            else if (!IsEditMode)
            {
                Staff.ActionType = "Remote";
                Staff.EmailId = EmailId;
                Staff.CompanyId = SessionManager.CompanyId;
                Staff = _StaffRepository.AddEdit_Staff(Staff);
                returnMsg = $"Email Id '{EmailId}' is already in use.";
            }
            if (Staff == null)
                return Json(true);
            else if (Staff.EmailId == null)
                return Json(true);
            else
                return Json(false);

        }

        [HttpPost]
        public ActionResult IsCodeExist(string Code = "")
        {
            StaffRepository _StaffRepository = new StaffRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                Staff.ActionType = "Remote";
                Staff.Code = Code;
                Staff.CompanyId = SessionManager.CompanyId;
                Staff = _StaffRepository.AddEdit_Staff(Staff);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                Staff.ActionType = "Remote";
                Staff.Code = Code;
                Staff.CompanyId = SessionManager.CompanyId;
                Staff = _StaffRepository.AddEdit_Staff(Staff);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (Staff == null)
                return Json(true);
            else if (Staff.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region Admission
        [HttpPost]
        public ActionResult IsAdmissionCodeExist(string Code = "")
        {
            AdmissionRepository _Repository = new AdmissionRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                Admission.ActionType = "Remote";
                Admission.Code = Code;
                Admission.CompanyId = SessionManager.CompanyId;
                Admission = _Repository.AddEdit_Admission(Admission);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                Admission.ActionType = "Remote";
                Admission.Code = Code;
                Admission.CompanyId = SessionManager.CompanyId;
                Admission = _Repository.AddEdit_Admission(Admission);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (Admission == null)
                return Json(true);
            else if (Admission.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region Campus
        [HttpPost]
        public ActionResult IsCampusCodeExist(string Code = "")
        {
            CampusRepository _Repository = new CampusRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                Campus.ActionType = "Remote";
                Campus.Code = Code;
                Campus.CompanyId = SessionManager.CompanyId;
                Campus = _Repository.AddEdit_Campus(Campus);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                Campus.ActionType = "Remote";
                Campus.Code = Code;
                Campus.CompanyId = SessionManager.CompanyId;
                Campus = _Repository.AddEdit_Campus(Campus);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (Campus == null)
                return Json(true);
            else if (Campus.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region CardStatus
        [HttpPost]
        public ActionResult IsCardStatusCodeExist(string Code = "")
        {
            CardStatusRepository _Repository = new CardStatusRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                CardStatus.ActionType = "Remote";
                CardStatus.Code = Code;
                CardStatus.CompanyId = SessionManager.CompanyId;
                CardStatus = _Repository.AddEdit_CardStatus(CardStatus);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                CardStatus.ActionType = "Remote";
                CardStatus.Code = Code;
                CardStatus.CompanyId = SessionManager.CompanyId;
                CardStatus = _Repository.AddEdit_CardStatus(CardStatus);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (CardStatus == null)
                return Json(true);
            else if (CardStatus.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region College
        [HttpPost]
        public ActionResult IsCollegeCodeExist(string Code = "")
        {
            CollegeRepository _Repository = new CollegeRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                College.ActionType = "Remote";
                College.Code = Code;
                College.CompanyId = SessionManager.CompanyId;
                College = _Repository.AddEdit_College(College);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                College.ActionType = "Remote";
                College.Code = Code;
                College.CompanyId = SessionManager.CompanyId;
                College = _Repository.AddEdit_College(College);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (College == null)
                return Json(true);
            else if (College.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region Country
        [HttpPost]
        public ActionResult IsCountryCodeExist(string Code = "")
        {
            CountryRepository _Repository = new CountryRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                Country.ActionType = "Remote";
                Country.Code = Code;
                Country.CompanyId = SessionManager.CompanyId;
                Country = _Repository.AddEdit_Country(Country);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                Country.ActionType = "Remote";
                Country.Code = Code;
                Country.CompanyId = SessionManager.CompanyId;
                Country = _Repository.AddEdit_Country(Country);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (Country == null)
                return Json(true);
            else if (Country.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region Customer
        [HttpPost]
        public ActionResult IsCustomerEmailIdExist(string EmailId = "")
        {
            CustomerRepository _Repository = new CustomerRepository();
            string Original_EmailId = SessionManager.EmailId;
            bool IsEditMode = !string.IsNullOrEmpty(Original_EmailId) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_EmailId, EmailId))
            {
                Customer.ActionType = "Remote";
                Customer.EmailId = EmailId;
                Customer.CompanyId = SessionManager.CompanyId;
                Customer = _Repository.AddEdit_Customer(Customer);
                returnMsg = $"EmailId '{EmailId}' is already in use.";
            }
            else if (!IsEditMode)
            {
                Customer.ActionType = "Remote";
                Customer.EmailId = EmailId;
                Customer.CompanyId = SessionManager.CompanyId;
                Customer = _Repository.AddEdit_Customer(Customer);
                returnMsg = $"EmailId '{EmailId}' is already in use.";
            }
            if (Customer == null)
                return Json(true);
            else if (Customer.EmailId == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region DegreeType
        [HttpPost]
        public ActionResult IsDegreeTypeCodeExist(string Code = "")
        {
            DegreeTypeRepository _Repository = new DegreeTypeRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                DegreeType.ActionType = "Remote";
                DegreeType.Code = Code;
                DegreeType.CompanyId = SessionManager.CompanyId;
                DegreeType = _Repository.AddEdit_DegreeType(DegreeType);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                DegreeType.ActionType = "Remote";
                DegreeType.Code = Code;
                DegreeType.CompanyId = SessionManager.CompanyId;
                DegreeType = _Repository.AddEdit_DegreeType(DegreeType);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (DegreeType == null)
                return Json(true);
            else if (DegreeType.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region JobTitle
        [HttpPost]
        public ActionResult IsJobTitleCodeExist(string Code = "")
        {
            JobTitleRepository _Repository = new JobTitleRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                JobTitle.ActionType = "Remote";
                JobTitle.Code = Code;
                JobTitle.CompanyId = SessionManager.CompanyId;
                JobTitle = _Repository.AddEdit_JobTitle(JobTitle);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                JobTitle.ActionType = "Remote";
                JobTitle.Code = Code;
                JobTitle.CompanyId = SessionManager.CompanyId;
                JobTitle = _Repository.AddEdit_JobTitle(JobTitle);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (JobTitle == null)
                return Json(true);
            else if (JobTitle.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region JobTitle
        [HttpPost]
        public ActionResult IsProgramCodeExist(string Code = "")
        {
            ProgramRepository _Repository = new ProgramRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                Program.ActionType = "Remote";
                Program.Code = Code;
                Program.CompanyId = SessionManager.CompanyId;
                Program = _Repository.AddEdit_Program(Program);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                Program.ActionType = "Remote";
                Program.Code = Code;
                Program.CompanyId = SessionManager.CompanyId;
                Program = _Repository.AddEdit_Program(Program);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (Program == null)
                return Json(true);
            else if (Program.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region StaffDepartment
        [HttpPost]
        public ActionResult IsStaffDepartmentCodeExist(string Code = "")
        {
            StaffDepartmentRepository _Repository = new StaffDepartmentRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                StaffDepartment.ActionType = "Remote";
                StaffDepartment.Code = Code;
                StaffDepartment.CompanyId = SessionManager.CompanyId;
                StaffDepartment = _Repository.AddEdit_StaffDepartment(StaffDepartment);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                StaffDepartment.ActionType = "Remote";
                StaffDepartment.Code = Code;
                StaffDepartment.CompanyId = SessionManager.CompanyId;
                StaffDepartment = _Repository.AddEdit_StaffDepartment(StaffDepartment);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (StaffDepartment == null)
                return Json(true);
            else if (StaffDepartment.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region StudentCompany
        [HttpPost]
        public ActionResult IsStudentCompanyExist(string Code = "")
        {
            StudentCompanyRepository _Repository = new StudentCompanyRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                StudentCompany.ActionType = "Remote";
                StudentCompany.Code = Code;
                StudentCompany.CompanyId = SessionManager.CompanyId;
                StudentCompany = _Repository.AddEdit_StudentCompany(StudentCompany);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                StudentCompany.ActionType = "Remote";
                StudentCompany.Code = Code;
                StudentCompany.CompanyId = SessionManager.CompanyId;
                StudentCompany = _Repository.AddEdit_StudentCompany(StudentCompany);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (StudentCompany == null)
                return Json(true);
            else if (StudentCompany.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region StudentDepartment
        [HttpPost]
        public ActionResult IsStudentDepartmentCodeExist(string Code = "")
        {
            StudentDepartmentRepository _Repository = new StudentDepartmentRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                StudentDepartment.ActionType = "Remote";
                StudentDepartment.Code = Code;
                StudentDepartment.CompanyId = SessionManager.CompanyId;
                StudentDepartment = _Repository.AddEdit_StudentDepartment(StudentDepartment);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                StudentDepartment.ActionType = "Remote";
                StudentDepartment.Code = Code;
                StudentDepartment.CompanyId = SessionManager.CompanyId;
                StudentDepartment = _Repository.AddEdit_StudentDepartment(StudentDepartment);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (StudentDepartment == null)
                return Json(true);
            else if (StudentDepartment.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region StudentFacility
        [HttpPost]
        public ActionResult IsStudentFacilityCodeExist(string Code = "")
        {
            StudentFacilityRepository _Repository = new StudentFacilityRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                StudentFacility.ActionType = "Remote";
                StudentFacility.Code = Code;
                StudentFacility.CompanyId = SessionManager.CompanyId;
                StudentFacility = _Repository.AddEdit_StudentFacility(StudentFacility);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                StudentFacility.ActionType = "Remote";
                StudentFacility.Code = Code;
                StudentFacility.CompanyId = SessionManager.CompanyId;
                StudentFacility = _Repository.AddEdit_StudentFacility(StudentFacility);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (StudentFacility == null)
                return Json(true);
            else if (StudentFacility.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region VisitorAccessLevel
        [HttpPost]
        public ActionResult IsVisitorAccessLevelCodeExist(string Code = "")
        {
            VisitorAccessLevelRepository _Repository = new VisitorAccessLevelRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                VisitorAccessLevel.ActionType = "Remote";
                VisitorAccessLevel.Code = Code;
                VisitorAccessLevel.CompanyId = SessionManager.CompanyId;
                VisitorAccessLevel = _Repository.AddEdit_VisitorAccessLevel(VisitorAccessLevel);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                VisitorAccessLevel.ActionType = "Remote";
                VisitorAccessLevel.Code = Code;
                VisitorAccessLevel.CompanyId = SessionManager.CompanyId;
                VisitorAccessLevel = _Repository.AddEdit_VisitorAccessLevel(VisitorAccessLevel);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (VisitorAccessLevel == null)
                return Json(true);
            else if (VisitorAccessLevel.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region VisitorReason
        [HttpPost]
        public ActionResult IsVisitorReasonCodeExist(string Code = "")
        {
            VisitorReasonRepository _Repository = new VisitorReasonRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                VisitorReason.ActionType = "Remote";
                VisitorReason.Code = Code;
                VisitorReason.CompanyId = SessionManager.CompanyId;
                VisitorReason = _Repository.AddEdit_VisitorReason(VisitorReason);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                VisitorReason.ActionType = "Remote";
                VisitorReason.Code = Code;
                VisitorReason.CompanyId = SessionManager.CompanyId;
                VisitorReason = _Repository.AddEdit_VisitorReason(VisitorReason);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (VisitorReason == null)
                return Json(true);
            else if (VisitorReason.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region VisitorType
        [HttpPost]
        public ActionResult IsVisitorTypeCodeExist(string Code = "")
        {
            VisitorTypeRepository _Repository = new VisitorTypeRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                VisitorType.ActionType = "Remote";
                VisitorType.Code = Code;
                VisitorType.CompanyId = SessionManager.CompanyId;
                VisitorType = _Repository.AddEdit_VisitorType(VisitorType);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                VisitorType.ActionType = "Remote";
                VisitorType.Code = Code;
                VisitorType.CompanyId = SessionManager.CompanyId;
                VisitorType = _Repository.AddEdit_VisitorType(VisitorType);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (VisitorType == null)
                return Json(true);
            else if (VisitorType.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region Year
        [HttpPost]
        public ActionResult IsYearCodeExist(string Code = "")
        {
            YearRepository _Repository = new YearRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                Year.ActionType = "Remote";
                Year.Code = Code;
                Year.CompanyId = SessionManager.CompanyId;
                Year = _Repository.AddEdit_Year(Year);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                Year.ActionType = "Remote";
                Year.Code = Code;
                Year.CompanyId = SessionManager.CompanyId;
                Year = _Repository.AddEdit_Year(Year);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (Year == null)
                return Json(true);
            else if (Year.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region VisitorStatus
        [HttpPost]
        public ActionResult IsVisitorStatusCodeExist(string Code = "")
        {
            VisitorStatusRepository _Repository = new VisitorStatusRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                VisitorStatus.ActionType = "Remote";
                VisitorStatus.Code = Code;
                VisitorStatus.CompanyId = SessionManager.CompanyId;
                VisitorStatus = _Repository.AddEdit_VisitorStatus(VisitorStatus);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                VisitorStatus.ActionType = "Remote";
                VisitorStatus.Code = Code;
                VisitorStatus.CompanyId = SessionManager.CompanyId;
                VisitorStatus = _Repository.AddEdit_VisitorStatus(VisitorStatus);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (VisitorStatus == null)
                return Json(true);
            else if (VisitorStatus.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region Reader
        [HttpPost]
        public ActionResult IsReaderCodeExist(string Code = "")
        {
            ReaderRepository _Repository = new ReaderRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                Reader.ActionType = "Remote";
                Reader.Code = Code;
                Reader.CompanyId = SessionManager.CompanyId;
                Reader = _Repository.AddEdit_Reader(Reader);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                Reader.ActionType = "Remote";
                Reader.Code = Code;
                Reader.CompanyId = SessionManager.CompanyId;
                Reader = _Repository.AddEdit_Reader(Reader);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (Reader == null)
                return Json(true);
            else if (Reader.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion

        #region Reader
        [HttpPost]
        public ActionResult IsSessionMasterCodeExist(string Code = "")
        {
            SessionMasterRepository _Repository = new SessionMasterRepository();
            string Original_Code = SessionManager.Code;
            bool IsEditMode = !string.IsNullOrEmpty(Original_Code) ? true : false;
            string returnMsg = "";

            if (IsEditMode && !string.Equals(Original_Code, Code))
            {
                SessionMaster.ActionType = "Remote";
                SessionMaster.Code = Code;
                SessionMaster.CompanyId = SessionManager.CompanyId;
                SessionMaster = _Repository.AddEdit_SessionMaster(SessionMaster);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            else if (!IsEditMode)
            {
                SessionMaster.ActionType = "Remote";
                SessionMaster.Code = Code;
                SessionMaster.CompanyId = SessionManager.CompanyId;
                SessionMaster = _Repository.AddEdit_SessionMaster(SessionMaster);
                returnMsg = $"Code '{Code}' is already in use.";
            }
            if (SessionMaster == null)
                return Json(true);
            else if (SessionMaster.Code == null)
                return Json(true);
            else
                return Json(false);

        }
        #endregion
        
    }
}