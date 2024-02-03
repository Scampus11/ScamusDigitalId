using ClosedXML.Excel;
using ScampusCloud.Models;
using ScampusCloud.Repository.Customer;
using ScampusCloud.Repository.Login;
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
    public class CustomerController : Controller
    {

        #region Variable Declaration
        private readonly CustomerRepository _customerRepository;
        CustomerModel _CustomerModel = new CustomerModel();
        #endregion

        #region CTOR
        public CustomerController()
        {
            _customerRepository = new CustomerRepository();
        }
        #endregion

        #region Method
        // GET: Customer
        public ActionResult Customer()
        {
            ViewData["paging_size"] = 10;
            if (ViewData["currentPage"] == null)
                ViewData["currentPage"] = 1;
            string searchtxt = "NA";
            int totals = Convert.ToInt32(_customerRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
            ViewData["totalrecords"] = totals;
            return View();
        }
        public ActionResult AddEditCustomer(string ID = "")
        {
            try
            {
                _CustomerModel.CreatedBy = SessionManager.UserId;
                _CustomerModel.ModifiedBy = SessionManager.UserId;
                _CustomerModel.CompanyId = SessionManager.CompanyId;

                if (!string.IsNullOrEmpty(ID) && ID != "0")
                {
                    #region Get Entity by id
                    _CustomerModel.ActionType = "Edit";
                    _CustomerModel.Id = Convert.ToInt32(ID);
                    _CustomerModel = _customerRepository.AddEdit_Customer(_CustomerModel);
                    if (_CustomerModel != null)
                    {
                        _CustomerModel.IsEdit = true;
                        _CustomerModel.AdminUserPassword = !string.IsNullOrEmpty(_CustomerModel.AdminUserPassword) ? EncryptionDecryption.GetDecrypt(_CustomerModel.AdminUserPassword) : string.Empty;
                        SessionManager.EmailId = _CustomerModel.EmailId;
                    }
                    else
                    {
                        _CustomerModel = new CustomerModel();
                        ViewBag.NoRecordExist = true;
                        _CustomerModel.Response_Message = "No record found";
                        SessionManager.EmailId = null;
                    }
                    #endregion
                }
                //// If url Apeended with Querystring ID=0 then Redirect into current Action
                else if (!string.IsNullOrEmpty(ID) && ID == "0")
                {
                    return RedirectToAction("AddEditCustomer");
                }
                else
                {
                    _CustomerModel.IsEdit = false;
                    _CustomerModel.Isactive = true;
                    SessionManager.EmailId = null;
                }
                return View(_CustomerModel);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, this.GetType().Name + " : " + MethodBase.GetCurrentMethod().Name);
                throw;
            }

        }
        [HttpPost]
        public ActionResult AddEditCustomer(CustomerModel _CustomerModel, string saveAndExit = "")
        {
            if (_CustomerModel.Id > 0)
            {
                _CustomerModel.ActionType = "Update";
            }
            else
            {
                _CustomerModel.ActionType = "Insert";
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
                    file.SaveAs(Server.MapPath("~/Images/" + fileName)); //File will be saved in application root
                    if (Request.Files.AllKeys[i].Equals("uploadPhotoFile"))
                    {
                        _CustomerModel.ImagePath = "~/Images/" + fileName;
                    }
                }
            }
            var officemaster = _customerRepository.AddEdit_Customer(_CustomerModel);
            if (!string.IsNullOrEmpty(saveAndExit))
            {
                return RedirectToAction("Customer", "Customer");
            }
            else if (_CustomerModel.IsEdit == true)
            {
                return RedirectToAction("AddEditCustomer", new { ID = _CustomerModel.Id });
            }
            else
            {
                return RedirectToAction("AddEditCustomer");
            }
            //return RedirectToAction("Customer", "Customer");
        }

        public ActionResult CustomerList(int page = 1, int pagesize = 10, string searchtxt = "")
        {
            try
            {
                searchtxt = string.IsNullOrEmpty(searchtxt) ? "" : searchtxt;
                int totals = Convert.ToInt32(_customerRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
                var lstAccessGroup = _customerRepository.GetCustomerList(searchtxt, page, pagesize, SessionManager.CompanyId.ToString());
                Session["totalrecords"] = Convert.ToString(totals);
                Session["paging_size"] = Convert.ToString(pagesize);
                ViewData["totalrecords"] = totals;
                ViewData["paging_size"] = pagesize;
                StringBuilder strHTML = new StringBuilder();
                if (lstAccessGroup.Count > 0)
                {
                    strHTML.Append("<table class='datatable-bordered datatable-head-custom datatable-table' id='kt_datatable'>");
                    strHTML.Append("<thead class='datatable-head'>");
                    strHTML.Append("<tr class='datatable-row'>");
                    strHTML.Append("<th class='datatable-cell'>Id</th>");
                    strHTML.Append("<th class='datatable-cell'>Name</th>");
                    strHTML.Append("<th class='datatable-cell'>EmailId</th>");
                    strHTML.Append("<th class='datatable-cell'>Phone Number</th>");
                    strHTML.Append("<th class='datatable-cell'>Status</th>");
                    strHTML.Append("<th class='datatable-cell'>Action</th>");
                    strHTML.Append("</tr>");
                    strHTML.Append("</thead>");
                    strHTML.Append("<tbody class='datatable-body custom-scroll'>");
                    foreach (var item in lstAccessGroup)
                    {
                        string ImgSrc = string.Empty;
                        string PhotoImgSrc = string.Empty;
                        string DeleteConfirmationEvent = "DeleteConfirmation('" + item.Id + "','Customer','Customer','Delete')";
                        string fileExtension = Path.GetExtension(item.ImagePath);

                        if (!string.IsNullOrEmpty(item.ImageBase64))
                            PhotoImgSrc = "data:image/" + fileExtension.TrimStart('.') + ";base64," + item.ImageBase64;

                        strHTML.Append("<tr>");
                        strHTML.Append("<td>" + item.Id + "</td>");
                        //strHTML.Append("<td>" + item.Name + "</td>");
                        strHTML.Append("<td style='width:250px;'><span><div class='d-flex align-items-center'><div class='symbol symbol-40 flex-shrink-0'><img src='" + PhotoImgSrc + "' style='height:40px;border-radius:100%;border:1px solid;' alt='photo'></div>" +
                             "<div class='ml-4'>" +
                             "<a href='#' class='font-size-sm text-dark-50 text-hover-primary'>" + item.Name + "</a></div></div></span></td>");
                        strHTML.Append("<td>" + item.EmailId + "</td>");
                        strHTML.Append("<td>" + item.PhoneNumber + "</td>");
                        if (item.Isactive)
                            strHTML.Append("<td><span><span class='label font-weight-bold label-lg label-light-primary label-inline'>Active</span></span></td>");
                        else
                            strHTML.Append("<td><span><span class='label font-weight-bold label-lg label-light-danger label-inline'>InActive</span></span></td>");

                        strHTML.Append("<td>");
                        strHTML.Append("<a class='btn btn-sm btn-icon btn-lg-light btn-text-primary btn-hover-light-primary mr-3' href= '/Customer/AddEditCustomer?ID=" + item.Id + "'><i class='flaticon-edit'></i></a>");
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


            //return Json(customer, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerListCount()
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
                _CustomerModel.ActionType = "Delete";
                _CustomerModel.Id = Convert.ToInt32(Id);
                var response = _customerRepository.AddEdit_Customer(_CustomerModel);

                return RedirectToAction("Customer", "Customer");
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
                _CustomerModel.ActionType = PhotoType;
                _CustomerModel.Id = Convert.ToInt32(Id);
                _CustomerModel.CompanyId = SessionManager.CompanyId;
                var response = _customerRepository.AddEdit_Customer(_CustomerModel);

                return RedirectToAction("Customer", "Customer");
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
            DataTable dt = new DataTable("Customer");
            try
            {
                dt = _customerRepository.GetCustomerData_Export(searchtxt, SessionManager.CompanyId.ToString());
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Customer.xlsx");
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