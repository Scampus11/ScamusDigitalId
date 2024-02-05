using ScampusCloud.Models;
using ScampusCloud.Repository.Reader;
using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Controllers
{
    [SessionTimeoutAttribute]
    public class DoorGroupController : Controller
    {
        DoorGroupModel _DoorGroupModel = new DoorGroupModel();

        public DoorGroupController()
        {
                
        }

        // GET: DoorGroup
        public ActionResult DoorGroup()
        {
            ViewData["paging_size"] = 10;
            if (ViewData["currentPage"] == null)
                ViewData["currentPage"] = 1;
            string searchtxt = "NA";
            int totals = 0;//Convert.ToInt32(_ReaderRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
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
                    //_DoorGroupModel = _ReaderRepository.AddEdit_Reader(_ReaderModel);
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
                return View(_DoorGroupModel);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }

        }
    }
}