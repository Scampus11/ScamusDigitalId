using ScampusCloud.Repository.Reader;
using ScampusCloud.Repository.Visitor;
using ScampusCloud.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Controllers
{
    public class VisitorController : Controller
    {
        private readonly VisitorRepository _VisitorRepository;

        public VisitorController()
        {
                _VisitorRepository = new VisitorRepository();
        }
        // GET: Visitor
        public ActionResult Visitor()
        {
            ViewData["paging_size"] = 10;
            if (ViewData["currentPage"] == null)
                ViewData["currentPage"] = 1;
            string searchtxt = "NA";
            int totals = 0; //Convert.ToInt32(_ReaderRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
            ViewData["totalrecords"] = totals;
            return View();
        }
        public ActionResult VisitorList(int page = 1, int pagesize = 10, string searchtxt = "")
        {
            try
            {
                searchtxt = string.IsNullOrEmpty(searchtxt) ? "" : searchtxt;
                int totals = Convert.ToInt32(_VisitorRepository.GetAllCount(searchtxt, SessionManager.CompanyId.ToString()));
                var lstVisitor = _VisitorRepository.GetVisitorList(searchtxt, page, pagesize, SessionManager.CompanyId.ToString());
                Session["totalrecords"] = Convert.ToString(totals);
                Session["paging_size"] = Convert.ToString(pagesize);
                ViewData["totalrecords"] = totals;
                ViewData["paging_size"] = pagesize;
                StringBuilder strHTML = new StringBuilder();
                if (lstVisitor.Count > 0)
                {
                    strHTML.Append("<table class='datatable-bordered datatable-head-custom datatable-table' id='kt_datatable'>");
                    strHTML.Append("<thead class='datatable-head'>");
                    strHTML.Append("<tr class='datatable-row'>");
                    strHTML.Append("<th class='datatable-cell'>Photo</th>");
                    strHTML.Append("<th class='datatable-cell'>Visitor Reg No</th>");
                    strHTML.Append("<th class='datatable-cell'>Name</th>");
                    strHTML.Append("<th class='datatable-cell'>EmailId</th>");
                    strHTML.Append("<th class='datatable-cell'>PhoneNumber</th>");
                    strHTML.Append("<th class='datatable-cell'>Visitor Register Status</th>");
                    strHTML.Append("<th class='datatable-cell'>Action</th>");
                    strHTML.Append("</tr>");
                    strHTML.Append("</thead>");
                    strHTML.Append("<tbody class='datatable-body custom-scroll'>");
                    foreach (var item in lstVisitor)
                    {
                        string ImgSrc = string.Empty;
                        string PhotoImgSrc = string.Empty;
                        string DeleteConfirmationEvent = "DeleteConfirmation('" + item.Id + "','Visitor','Visitor','Delete')";
                        string fileExtension = Path.GetExtension(item.ImagePath);
                        if (!string.IsNullOrEmpty(item.ImageBase64))
                            PhotoImgSrc = "data:image/png;base64," + item.ImageBase64;
                        strHTML.Append("<tr>");
                        strHTML.Append("<td style='width:250px;'><span><div class='d-flex align-items-center'><div class='symbol symbol-40 flex-shrink-0'><img src='" + PhotoImgSrc + "' style='height:40px;border-radius:100%;border:1px solid;' alt='photo'></div>" +
                             "<div class='ml-4'>" +
                             "<a href='#' class='font-size-sm text-dark-50 text-hover-primary'>" + item.Fullname + "</a></div></div></span></td>");
                        strHTML.Append("<td>" + item.VisitorRegNumber + "</td>");
                        strHTML.Append("<td>" + item.Fullname + "</td>");
                        strHTML.Append("<td>" + item.EmailId + "</td>");
                        strHTML.Append("<td>" + item.PhoneNumber + "</td>");
                        strHTML.Append("<td>" + item.VisitorPreRegStatus + "</td>");
                        if (item.IsActive)
                            strHTML.Append("<td><span><span class='label font-weight-bold label-lg label-light-primary label-inline'>Active</span></span></td>");
                        else
                            strHTML.Append("<td><span><span class='label font-weight-bold label-lg label-light-danger label-inline'>InActive</span></span></td>");

                        strHTML.Append("<td>");
                        strHTML.Append("<a class='btn btn-sm btn-icon btn-lg-light btn-text-primary btn-hover-light-primary mr-3' href= '/Reader/AddEditReader?ID=" + item.Id + "'><i class='flaticon-edit'></i></a>");
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

        public ActionResult VisitorListCount()
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
    }
}