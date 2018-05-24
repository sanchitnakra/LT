using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sampleMVC.Models;
using System.Web.Security;

namespace sampleMVC.Controllers
{
    public class SuperAdminController : Controller
    {
        // GET: SuperAdmin
        public ActionResult GetPagViewBySuperAdmin()
        {
            SuperAdmin GetPagViewBySuperAdmin = new SuperAdmin();
            GetPagViewBySuperAdmin.PAGwiseViewForSuperAdmin = GetPagViewBySuperAdmin.GetPagViewBySuperAdmin();
            return View(GetPagViewBySuperAdmin);
        }


        public ActionResult GetPagViewBySuperAdminByPAGName(string PAGName)
        {
            Session["PAGName"] = PAGName;
            SuperAdmin GetPagViewBySuperAdminByPAGNamobj = new SuperAdmin();
            GetPagViewBySuperAdminByPAGNamobj.PAGWiseViewForSuperAdminByPAGNameList = GetPagViewBySuperAdminByPAGNamobj.PAGWiseViewForSuperAdminBYPAGName(PAGName);
            return View(GetPagViewBySuperAdminByPAGNamobj);
        }



        public ActionResult PAGWiseViewForSuperAdminByProjName(string PAGName, string ProjName)
        {
            Session["PAGName"] = PAGName;
            Session["ProjName"] = ProjName;
            SuperAdmin PAGWiseViewForSuperAdminByProjNameobj = new SuperAdmin();
            PAGWiseViewForSuperAdminByProjNameobj.PAGWiseViewForSuperAdminByProjNameList = PAGWiseViewForSuperAdminByProjNameobj.PAGWiseViewForSuperAdminByProjName(PAGName, ProjName);
            //  SuperAdmin PAGWiseViewForSuperAdminByProjNameobj = new SuperAdmin();
            var list = PAGWiseViewForSuperAdminByProjNameobj.getSelectMonth();

            PAGWiseViewForSuperAdminByProjNameobj.selectMonthIM = PAGWiseViewForSuperAdminByProjNameobj.GetSelectListItem(list);
            return View(PAGWiseViewForSuperAdminByProjNameobj);
        }

        public ActionResult PAGWiseViewForSuperAdminByProjNameByTOL(string PAGName, string ProjName, string TOL)
        {
            Session["PAGName"] = PAGName;
            Session["ProjName"] = ProjName;
            Session["TOL"] = TOL;
            SuperAdmin PAGWiseViewForSuperAdminByProjNameByTOLobj = new SuperAdmin();
            PAGWiseViewForSuperAdminByProjNameByTOLobj.PAGWiseViewForSuperAdminByProjNameByTOLList = PAGWiseViewForSuperAdminByProjNameByTOLobj.PAGWiseViewForSuperAdminByProjNameByTOL(ProjName, TOL);
            return View(PAGWiseViewForSuperAdminByProjNameByTOLobj);
        }

        public ActionResult GetPagViewBySuperAdminByPAGNameDetails(string PAGName)
        {
            string dummp = Request.HttpMethod;
            SuperAdmin GetPagViewBySuperAdminByPAGNamobj = new SuperAdmin();
            GetPagViewBySuperAdminByPAGNamobj.PAGWiseViewForSuperAdminByPAGNameList = GetPagViewBySuperAdminByPAGNamobj.PAGWiseViewForSuperAdminBYPAGNameDetailed(PAGName);
            return View(GetPagViewBySuperAdminByPAGNamobj);
        }


        // View to list all TypeOfLeaves when he click on Data in Table
        public ActionResult GetPagViewBySuperAdminByPAGNameByTOL(string PAGName, string TOL)
        {
            SuperAdmin GetPagViewBySuperAdminByPAGNamByTOLobj = new SuperAdmin();
            GetPagViewBySuperAdminByPAGNamByTOLobj.PAGWiseViewForSuperAdminByPAGNameByTOLList = GetPagViewBySuperAdminByPAGNamByTOLobj.PAGWiseViewForSuperAdminBYPAGNameByTOL(PAGName, TOL);
            return View(GetPagViewBySuperAdminByPAGNamByTOLobj);
        }


        public ActionResult ExportToExcelForSuperAdmin()
        {
            // string ProjName = Session["ProjName"].ToString();
            SuperAdmin ExportToExcelForSuperAdminobj = new SuperAdmin();
            ExportToExcelForSuperAdminobj.ExportToExcelForSuperAdminList = ExportToExcelForSuperAdminobj.ExportToExcelForSuperAdmin();
            //Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            //string filename = string.Empty;
            //string PresentFilename = string.Empty;
            //filename = DateTime.Today.ToString();
            //PresentFilename = filename.Substring(0,5) + " Leave Tracker Report";
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Today.ToString().Substring(0, 5) + " Leave Tracker Report" + "");
            Response.ContentType = "application/excel";

            return View(ExportToExcelForSuperAdminobj);
        }

        public ActionResult ExportToExcelForSuperAdminByPAGNameByProj()
        {
            string PAGName = Session["PAGName"].ToString();
            string ProjName = Session["ProjName"].ToString();
            
            SuperAdmin ExportToExcelForSuperAdminobj = new SuperAdmin();
           
                ExportToExcelForSuperAdminobj.ExportToExcelByPAGNameForSuperAdminList = ExportToExcelForSuperAdminobj.ExportToExcelForSuperAdminByPAGNameByProj(PAGName,ProjName,string.Empty);
            
            //Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            //string filename = string.Empty;
            //string PresentFilename = string.Empty;
            //filename = DateTime.Today.ToString();
            //PresentFilename = filename.Substring(0,5) + " Leave Tracker Report";
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Today.ToString().Substring(0, 5) + " Leave Tracker Report" + "");
            Response.ContentType = "application/excel";

            return View(ExportToExcelForSuperAdminobj);
        }

        //to change
        public ActionResult ExportToExcelForSuperAdminByPAGNameByProjByTOL()
        {
            string PAGName = Session["PAGName"].ToString();
            string ProjName = Session["ProjName"].ToString();
            string TypeOfLeave = Session["TOL"].ToString();
            SuperAdmin ExportToExcelForSuperAdminobj = new SuperAdmin();

            ExportToExcelForSuperAdminobj.ExportToExcelByPAGNameForSuperAdminList = ExportToExcelForSuperAdminobj.ExportToExcelForSuperAdminByPAGNameByProj(PAGName, ProjName, TypeOfLeave);

            //Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            //string filename = string.Empty;
            //string PresentFilename = string.Empty;
            //filename = DateTime.Today.ToString();
            //PresentFilename = filename.Substring(0,5) + " Leave Tracker Report";
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Today.ToString().Substring(0, 5) + " Leave Tracker Report" + "");
            Response.ContentType = "application/excel";

            return View(ExportToExcelForSuperAdminobj);
        }



        public ActionResult ExportToExcelForSuperAdminByPAGName()
        {
            string PAGName = Session["PAGName"].ToString();
          

            SuperAdmin ExportToExcelForSuperAdminobj = new SuperAdmin();
           
                ExportToExcelForSuperAdminobj.ExportToExcelByPAGNameForSuperAdminList = ExportToExcelForSuperAdminobj.ExportToExcelForSuperAdminByPAGName(PAGName);
            
            //Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            //string filename = string.Empty;
            //string PresentFilename = string.Empty;
            //filename = DateTime.Today.ToString();
            //PresentFilename = filename.Substring(0,5) + " Leave Tracker Report";
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Today.ToString().Substring(0, 5) + " Leave Tracker Report" + "");
            Response.ContentType = "application/excel";

            return View(ExportToExcelForSuperAdminobj);
        }

        public ActionResult ExportToExcelForSuperAdminByDate()
        {
            string ProjName = Session["ProjName"].ToString();
            string sDstring = Session["sDforExportToExcel"].ToString();
            string eDstring = Session["eDforExportToExcel"].ToString();
            SuperAdmin ExportToExcelByDateForSuperAdminobj = new SuperAdmin();
            ExportToExcelByDateForSuperAdminobj.ExportToExcelForSuperAdminListByDate = ExportToExcelByDateForSuperAdminobj.ExportToExcelForSuperAdminByDate(ProjName, sDstring, eDstring);
            //Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            //string filename = string.Empty;
            //string PresentFilename = string.Empty;
            //filename = DateTime.Today.ToString();
            //PresentFilename = filename.Substring(0,5) + " Leave Tracker Report";
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Today.ToString().Substring(0, 5) + " Leave Tracker Report" + "");
            Response.ContentType = "application/excel";

            return View(ExportToExcelByDateForSuperAdminobj);
        }


        public ActionResult Logout()
        {
            //Session.Remove("MSID");
            //Session.Clear();
            //Session.Abandon();

            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "LTLogin");
        }

        [HttpPost]
        public ActionResult RefineResult(SuperAdmin superAdmin)
        {

            SuperAdmin PAGWiseViewForSuperAdminByProjNameRefinedByDate = new SuperAdmin();
            string sDstring = superAdmin.StartDateBySuperAdmin.ToString("yyyy-MM-dd");

            string eDstring = superAdmin.EndDateBySuperAdmin.ToString("yyyy-MM-dd");
            Session["sDforExportToExcel"] = sDstring;
            Session["eDforExportToExcel"] = eDstring;
            PAGWiseViewForSuperAdminByProjNameRefinedByDate.PAGWiseViewForSuperAdminByProjNameList =
                PAGWiseViewForSuperAdminByProjNameRefinedByDate.PAGWiseViewForSuperAdminByProjNameRefinedByDate(superAdmin, sDstring, eDstring);
            return View(PAGWiseViewForSuperAdminByProjNameRefinedByDate);
        }
        


        public ActionResult inputByCalender(SuperAdmin superAdmin)
        {
            DateTime todayDate = DateTime.Now;
            string fetchingYear = Convert.ToString(todayDate.Year);

            SuperAdmin inputByCalender = new SuperAdmin();
            var list = inputByCalender.getSelectMonth();

            inputByCalender.selectMonthIM = inputByCalender.GetSelectListItem(list);
            string dummp = superAdmin.selectMonth;
            string sDstring = string.Empty;
            string eDstring = string.Empty;
            if (dummp == "Jan")
            {
                sDstring = fetchingYear + "-01-01";
                eDstring = fetchingYear + "-01-31";
            }
            if (dummp == "Feb")
            {
                sDstring = fetchingYear + "-02-01";
                eDstring = fetchingYear + "-02-28";
            }
            if (dummp == "March")
            {
                sDstring = fetchingYear + "-03-01";
                eDstring = fetchingYear + "-03-31";
            }
            if (dummp == "April")
            {
                sDstring = fetchingYear + "-04-01";
                eDstring = fetchingYear + "-04-30";
            }
            if (dummp == "May")
            {
                sDstring = fetchingYear + "-05-01";
                eDstring = fetchingYear + "-05-31";
            }
            if (dummp == "June")
            {
                sDstring = fetchingYear + "-06-01";
                eDstring = fetchingYear + "-06-30";
            }
            if (dummp == "July")
            {
                sDstring = fetchingYear + "-08-01";
                eDstring = fetchingYear + "-07-31";
            }
            if (dummp == "August")
            {
                sDstring = fetchingYear + "-08-01";
                eDstring = fetchingYear + "-08-31";
            }
            if (dummp == "Sep")
            {
                sDstring = fetchingYear + "-09-01";
                eDstring = fetchingYear + "-09-30";
            }
            if (dummp == "Oct")
            {
                sDstring = fetchingYear + "-10-01";
                eDstring = fetchingYear + "-10-31";
            }
            if (dummp == "Nov")
            {
                sDstring = fetchingYear + "-11-01";
                eDstring = fetchingYear + "-11-30";
            }
            if (dummp == "Dec")
            {
                sDstring = fetchingYear + "-12-01";
                eDstring = fetchingYear + "-12-31";
            }


            Session["sDforExportToExcel"] = sDstring;

            Session["eDforExportToExcel"] = eDstring;
            inputByCalender.PAGWiseViewForSuperAdminByProjNameList =
               inputByCalender.PAGWiseViewForSuperAdminByProjNameRefinedByMonth(superAdmin, sDstring, eDstring,fetchingYear);
            return View(inputByCalender);

        }


        public ActionResult ScreenShotViewBySuperAdmin(int Id)
        {
            ApplyLeave obj = new ApplyLeave();
            obj = ApplyLeave.getLeaveInfo(Id);
            return View("ScreenShotViewBySuperAdmin", obj);
        }

        [HttpPost]
        public ActionResult SearchByNameForSuperAdmin(SuperAdmin superAdmin)
        {
            //  Session["superAdmin"] = superAdmin;
            string Name = superAdmin.searchTextBox;
            SuperAdmin SearchByNameForSuperAdmin = new SuperAdmin();
            SearchByNameForSuperAdmin.SearchByNameForSuperAdminList = SearchByNameForSuperAdmin.SearchByNameForSuperAdminDetails(Name);
            return View(SearchByNameForSuperAdmin);
        }

        [HttpPost]
        public ActionResult SearchByNMByLocByTOLForSuperAdmin(SuperAdmin superAdmin)
        {
           
            string PAGName = Session["PAGName"].ToString();
            string ProjName = Session["ProjName"].ToString();
            string dummyVariable = superAdmin.ByNameByLocByTOL;
            SuperAdmin SearchByNameForSuperAdmin = new SuperAdmin();
            SearchByNameForSuperAdmin.SearchByNameForSuperAdminList = SearchByNameForSuperAdmin.SearchByNMByLocByTOLForSuperAdmin(dummyVariable, PAGName, ProjName);
            return View(SearchByNameForSuperAdmin);
        }

        public ActionResult ScreenShotViewBySuperAdminByAllDetails(int Id)
        {
            SuperAdmin SearchByNameForSuperAdmin = new SuperAdmin();
            SearchByNameForSuperAdmin = SearchByNameForSuperAdmin.getLeaveInfo(Id);
            return View("ScreenShotViewBySuperAdminByAllDetails", SearchByNameForSuperAdmin);
        }

    }

}