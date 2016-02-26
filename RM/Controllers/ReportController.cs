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
    [Authorize(Roles ="Admin")]
    public class ReportController : Controller
    {

        [HttpGet]
        public ActionResult Index(int? page, UserTracking U)
        {
            ModelState.Clear();
            if (!page.HasValue)
            {
                ModelState.SetModelValue("StartDate", new ValueProviderResult(DateTime.Today.ToString("MM-dd-yyyy"), null, CultureInfo.InvariantCulture));
                ModelState.SetModelValue("EndDate", new ValueProviderResult(DateTime.Today.ToString("MM-dd-yyyy"), null, CultureInfo.InvariantCulture));
                ModelState.SetModelValue("StartDateHours", new ValueProviderResult("7", null, CultureInfo.InvariantCulture));
                ModelState.SetModelValue("EndDateHours", new ValueProviderResult("6", null, CultureInfo.InvariantCulture));
                ModelState.SetModelValue("StartDateMinutes", new ValueProviderResult("00", null, CultureInfo.InvariantCulture));
                ModelState.SetModelValue("EndDateMinutes", new ValueProviderResult("50", null, CultureInfo.InvariantCulture));
                ModelState.SetModelValue("EndDateAmPm", new ValueProviderResult("Pm", null, CultureInfo.InvariantCulture));
            }
            ViewBag.GetHours = U.GetHours();
            ViewBag.GetMins = U.GetMins();
            ViewBag.GetAmPm = U.GetAmPm();
            if (page.HasValue)
            {
                TimeSpan ts = new TimeSpan(U.StartDateHours, U.StartDateMinutes, 0);
                TimeSpan ts1 = new TimeSpan(U.EndDateHours, U.EndDateMinutes, 0);
                string StartDate = string.Concat(U.StartDate + " " + ts + " " + U.StartDateAmPm);
                string Enddate = string.Concat(U.EndDate + " " + ts1 + " " + U.EndDateAmPm);
                DateTime startDate = Convert.ToDateTime(StartDate);
                DateTime EndDate = Convert.ToDateTime(Enddate);

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
                    U.PagedUserTrackingList = U.SearchedDataList.ToPagedList(page ?? 1, 50); //Default Paging is 50
                    return View(U);
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(UserTracking U, int? page)
        {
            if (ModelState.IsValid)
            {
                ModelState.SetModelValue("StartDateHours", new ValueProviderResult("12", null, CultureInfo.InvariantCulture));
                ModelState.SetModelValue("EndDateHours", new ValueProviderResult("11", null, CultureInfo.InvariantCulture));
                ModelState.SetModelValue("StartDateMinutes", new ValueProviderResult("00", null, CultureInfo.InvariantCulture));
                ModelState.SetModelValue("EndDateMinutes", new ValueProviderResult("50", null, CultureInfo.InvariantCulture));
                ModelState.SetModelValue("EndDateAmPm", new ValueProviderResult("Pm", null, CultureInfo.InvariantCulture));

                TimeSpan ts = new TimeSpan(U.StartDateHours, U.StartDateMinutes, 0);
                TimeSpan ts1 = new TimeSpan(U.EndDateHours, U.EndDateMinutes, 0);
                string StartDate = string.Concat(U.StartDate + " " + ts + " " + U.StartDateAmPm);
                string Enddate = string.Concat(U.EndDate + " " + ts1 + " " + U.EndDateAmPm);
                DateTime startDate = Convert.ToDateTime(StartDate);
                DateTime EndDate = Convert.ToDateTime(Enddate);

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
            else
            {
                return View(U);
            }
        }
    }
}