﻿using System;
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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {   
               
            return View();
        }
        public ActionResult Policies()
        {

            return View();
        }

        public ActionResult DataTables()
        {

            return View();
        }


        public ActionResult LocalDistribution()
        {

            return View();
        }

        public ActionResult ExportSales()
        {

            return View();
        }
    }
}