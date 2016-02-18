using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net;
using RM.Models;
using System.Globalization;
using PagedList.Mvc;
using PagedList;

namespace RM.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {

        [HttpGet]
        public ActionResult Index(int? page, UserTracking U)
        {
            ModelState.SetModelValue("StartDate", new ValueProviderResult(DateTime.Now, null, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("EndDate", new ValueProviderResult(DateTime.Today, null, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("StartDateHours", new ValueProviderResult("12", null, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("EndDateHours", new ValueProviderResult("11", null, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("StartDateMinutes", new ValueProviderResult("00", null, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("EndDateMinutes", new ValueProviderResult("50", null, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("EndDateAmPm", new ValueProviderResult("Pm", null, CultureInfo.InvariantCulture));
            ViewBag.GetHours = U.GetHours();
            ViewBag.GetMins = U.GetMins();
            ViewBag.GetAmPm = U.GetAmPm();
            DateTime startDate = U.StartDate;
            if (page.HasValue)
            {
                if (U.StartDateAmPm.ToUpper() == "PM")
                {
                    U.StartDateHours = (U.StartDateHours % 12) + 12;
                }

                if (U.EndDateAmPm.ToUpper() == "PM")
                {
                    U.EndDateHours = (U.EndDateHours % 12) + 12;
                }
             
                DateTime StartDate = U.StartDate;
                TimeSpan ts = new TimeSpan(U.StartDateHours, U.StartDateMinutes, 0);
                startDate = startDate.Date + ts;
                DateTime EndDate = U.EndDate;
                TimeSpan ts1 = new TimeSpan(U.EndDateHours, U.EndDateMinutes, 0);
                EndDate = EndDate.Date + ts1;
               
                if (U.RadioButtonSelectedValue == "1")
                {
                    U.UserTrackingList = U.UserTrackingReport();
                    U.UserTrackingList = U.UserTrackingList.Where(a => a.TimeStamp >= startDate && a.TimeStamp <= EndDate).Where(a=>a.Name.Contains(string.IsNullOrEmpty(U.Name)? a.Name :U.Name)).ToList();
                    U.PagedUserTrackingList = U.UserTrackingList.ToPagedList(page ?? 1, 50); //Default Paging is 50
                    return View(U);
                }
                else
                {
                    U.SearchedDataList = U.SearchDataReport();
                    U.SearchedDataList = U.SearchedDataList.Where(a => a.TimeStamp >= startDate && a.TimeStamp <= EndDate).Where(a => a.Name.Contains(string.IsNullOrEmpty(U.Name) ? a.Name : U.Name)).ToList();
                    U.PagedUserTrackingList = U.SearchedDataList.ToPagedList(page ?? 1, 50); //Default Paging is 50
                    return View(U);
                }
            }

            return View();
        }
      
        [HttpPost]
        public ActionResult Index(UserTracking U, int? page)
        {
            DateTime startDate = U.StartDate;
            if (U.StartDateAmPm.ToUpper() == "PM")
            {
                U.StartDateHours = (U.StartDateHours % 12) + 12;
            }
            if (U.EndDateAmPm.ToUpper() == "PM")
            {
                U.EndDateHours = (U.EndDateHours % 12) + 12;
            }
            ModelState.SetModelValue("StartDateHours", new ValueProviderResult("12", null, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("EndDateHours", new ValueProviderResult("11", null, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("StartDateMinutes", new ValueProviderResult("00", null, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("EndDateMinutes", new ValueProviderResult("50", null, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("EndDateAmPm", new ValueProviderResult("Pm", null, CultureInfo.InvariantCulture));          
            DateTime StartDate = U.StartDate;
            TimeSpan ts = new TimeSpan(U.StartDateHours, U.StartDateMinutes, 0);
            startDate = startDate.Date + ts;
            DateTime EndDate = U.EndDate;
            TimeSpan ts1 = new TimeSpan(U.EndDateHours, U.EndDateMinutes, 0);
            EndDate = EndDate.Date + ts1;
            ViewBag.GetHours = U.GetHours();
            ViewBag.GetMins = U.GetMins();
            ViewBag.GetAmPm = U.GetAmPm();
            if (U.RadioButtonSelectedValue == "1")
            {
                U.UserTrackingList = U.UserTrackingReport();
                U.UserTrackingList = U.UserTrackingList.Where(a => a.TimeStamp >= startDate && a.TimeStamp <= EndDate).Where(a => a.Name.Contains(string.IsNullOrEmpty(U.Name) ? a.Name : U.Name)).ToList();
                U.PagedUserTrackingList = U.UserTrackingList.ToPagedList(page ?? 1, 50); //Default Paging is 50
                return View(U);
            }
            else
            {
                U.SearchedDataList = U.SearchDataReport();
                U.SearchedDataList = U.SearchedDataList.Where(a => a.TimeStamp >= startDate && a.TimeStamp <= EndDate).Where(a => a.Name.Contains(string.IsNullOrEmpty(U.Name) ? a.Name : U.Name)).ToList();
                U.PagedUserTrackingList = U.SearchedDataList.ToPagedList(page ?? 1, 50);  //Default Paging is 50
                return View(U);
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}