﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RM.Models;
using System.IO;
using System.Data;
using System.Xml;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.OleDb;
using MySql.Data.MySqlClient;
using PagedList.Mvc;
using PagedList;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.Configuration;

namespace RM.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        // GET: Product
        public ActionResult Index(int? page, int? Id, string sortOrder, string Pages, Inventory pro, int? Export)
        {
            ModelState.Clear();
            string submit = Request["submit"];
            ViewBag.Pages = Pages;
            ViewBag.LocSortParm = String.IsNullOrEmpty(sortOrder) ? "Loc_desc" : "";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "Type_desc" : "Type";
            ViewBag.FinishSortParm = sortOrder == "Finish" ? "Finish_desc" : "Finish";
            ViewBag.GaugeSortParm = sortOrder == "Gauge" ? "Gauge_desc" : "Gauge";
            ViewBag.WidthSortParm = sortOrder == "Width" ? "Width_desc" : "Width";
            ViewBag.WTNETSortParm = sortOrder == "WTNET" ? "WTNET_desc" : "WTNET";
            ViewBag.NOOFPCSSortParm = sortOrder == "NOOFPCS" ? "NOOFPCS_desc" : "NOOFPCS";
            if (string.IsNullOrEmpty(pro.Loc) && string.IsNullOrEmpty(pro.Type) && string.IsNullOrEmpty(pro.Finish) && string.IsNullOrEmpty(pro.Gauge) && string.IsNullOrEmpty(pro.Width) && string.IsNullOrEmpty(pro.WTNET))
            {
                pro.ProductList = new List<Inventory>();

            }
            else
            {
                pro.ProductList = pro.ProductsList().ToList();
                pro.ProductList = pro.ProductList.Where(a => a.Loc.ToUpper().Contains(string.IsNullOrEmpty(pro.Loc) ? a.Loc.ToUpper() : pro.Loc.ToUpper())).
                    Where(a => a.Type.ToUpper().Contains(string.IsNullOrEmpty(pro.Type) ? a.Type.ToUpper() : pro.Type.ToUpper())).
                    Where(a => a.Finish.ToUpper().Contains(string.IsNullOrEmpty(pro.Finish) ? a.Finish.ToUpper() : pro.Finish.ToUpper())).
                    Where(a => a.Gauge.ToUpper().Contains(string.IsNullOrEmpty(pro.Gauge) ? a.Gauge.ToUpper() : pro.Gauge.ToUpper())).
                    Where(a => a.Width.ToUpper().Contains(string.IsNullOrEmpty(pro.Width) ? a.Width.ToUpper() : (pro.Width.ToUpper()))).
                    Where(a => a.WTNET.ToUpper().Contains(string.IsNullOrEmpty(pro.WTNET) ? a.WTNET.ToUpper() : (pro.WTNET.ToUpper()))).ToList();
            }

            //List Displayed only after searched in filters

            if (Id.HasValue)
            {
                RequestedQuote quote = new RequestedQuote();
                quote = quote.GetData(User.Identity.GetUserId());
                quote.User_Id = User.Identity.GetUserId();
                quote.product = new Inventory();
                quote.product = quote.product.productDetails(Id.Value);
                quote.insert(quote);

                var smtpClient = new SmtpClient();
                var message = new MailMessage("no-reply@suteki.co.uk", "admin@gmail.com")
                {
                    Subject = "Requested Quote",
                     Body = "User Name " + User.Identity.GetUserName() + Environment.NewLine + "Phone Number :" + quote.PhoneNumber + Environment.NewLine + "IP Address:" + Request.ServerVariables["REMOTE_ADDR"] 
                     + Environment.NewLine + "Location : " + quote.product.Loc + "Type : " + quote.product.Type + "Finish : " +quote.product.Finish + quote.product.Gauge
                     + quote.product.Width + quote.product.WTNET + quote.product.NOOFPCS
                };
                smtpClient.Send(message);

            }

            if (Export.HasValue)
            {
                var grid = new GridView();
                grid.DataSource = from data in pro.ProductList
                                  select new
                                  {
                                      Location = data.Loc,
                                      Type = data.Type,
                                      Finish = data.Finish,
                                      Gauge = data.Gauge,
                                      Width = data.Width,
                                      WTNET = data.WTNET
                                  };
                grid.DataBind();
                Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());

                Response.End();

            }

            switch (sortOrder)
            {
                case "Loc_desc":
                    pro.ProductList = pro.ProductList.OrderByDescending(s => s.Loc).ToList();
                    break;
                case "Finish":
                    pro.ProductList = pro.ProductList.OrderBy(s => s.Finish).ToList();
                    break;
                case "Finish_desc":
                    pro.ProductList = pro.ProductList.OrderByDescending(s => s.Finish).ToList();
                    break;
                case "Gauge":
                    pro.ProductList = pro.ProductList.OrderBy(s => s.Gauge).ToList();
                    break;
                case "Gauge_desc":
                    pro.ProductList = pro.ProductList.OrderByDescending(s => s.Gauge).ToList();
                    break;
                case "Type":
                    pro.ProductList = pro.ProductList.OrderBy(s => s.Type).ToList();
                    break;
                case "Type_desc":
                    pro.ProductList = pro.ProductList.OrderByDescending(s => s.Type).ToList();
                    break;
                case "Width":
                    pro.ProductList = pro.ProductList.OrderBy(s => s.Width).ToList();
                    break;
                case "Width_desc":
                    pro.ProductList = pro.ProductList.OrderByDescending(s => s.Width).ToList();
                    break;

                case "WTNET":
                    pro.ProductList = pro.ProductList.OrderBy(s => s.WTNET).ToList();
                    break;
                case "WTNET_desc":
                    pro.ProductList = pro.ProductList.OrderByDescending(s => s.WTNET).ToList();
                    break;

                case "NOOFPCS":
                    pro.ProductList = pro.ProductList.OrderBy(s => s.NOOFPCS).ToList();
                    break;
                case "NOOFPCS_desc":
                    pro.ProductList = pro.ProductList.OrderByDescending(s => s.NOOFPCS).ToList();
                    break;
                default:
                    pro.ProductList = pro.ProductList.OrderBy(s => s.Loc).ToList();
                    break;
            }

            List<SelectListItem> listitems = new List<SelectListItem>();

            listitems.Add(new SelectListItem { Text = "50 items ", Value = "50" });
            listitems.Add(new SelectListItem { Text = "100 items ", Value = "100" });
            listitems.Add(new SelectListItem { Text = "150 items ", Value = "150" });
            ViewBag.ListItems = listitems;



            if (string.IsNullOrEmpty(Pages))
            {
                pro.IPagedProductsList = pro.ProductList.ToPagedList(page ?? 1, 50);     //Default Paging is 50
                return View(pro);
            }
            else
            {
                pro.IPagedProductsList = pro.ProductList.ToPagedList(page ?? 1, Convert.ToInt32(Pages));
                return View(pro);
            }

        }
        [HttpPost]
        public ActionResult Index(int? page, string Pages, Inventory pro)
        {
            List<SelectListItem> listitems = new List<SelectListItem>();
            listitems.Add(new SelectListItem { Text = "50 items ", Value = "50" });
            listitems.Add(new SelectListItem { Text = "100 items ", Value = "100" });
            listitems.Add(new SelectListItem { Text = "150 items ", Value = "150" });
            ViewBag.ListItems = listitems;


            if (ModelState.IsValid)
            {
                  string submit = Request["submit"];
                Dictionary<int, string> searchData = new Dictionary<int, string>();

                if (!(string.IsNullOrEmpty(pro.Loc)))
                {
                    searchData.Add(1, pro.Loc);
                }
                if (!(string.IsNullOrEmpty(pro.Type)))
                {
                    searchData.Add(2, pro.Type);
                }
                if (!(string.IsNullOrEmpty(pro.Finish)))
                {
                    searchData.Add(3, pro.Finish);
                }
                if (!(string.IsNullOrEmpty(pro.Gauge)))
                {
                    searchData.Add(4, pro.Gauge);
                }
                if (!(string.IsNullOrEmpty(pro.Width)))
                {
                    searchData.Add(5, pro.Width);
                }
                if (!(string.IsNullOrEmpty(pro.WTNET)))
                {
                    searchData.Add(6, pro.WTNET);
                }
                if (!(string.IsNullOrEmpty(submit)))
                {
                    SearchedData search = new SearchedData();
                    search.Filerdata = new FilterData();
                    search.Filerdata = search.Filerdata.GetData(User.Identity.GetUserId());
                    search.Filerdata.IPAddress = Request.ServerVariables["REMOTE_ADDR"];
                    search.Details = searchData;
                    search.Filerdata.user_Id = User.Identity.GetUserId();
                    search.insert(search);
                }

                if (string.IsNullOrEmpty(pro.Loc) && string.IsNullOrEmpty(pro.Type) && string.IsNullOrEmpty(pro.Finish) && string.IsNullOrEmpty(pro.Gauge) && string.IsNullOrEmpty(pro.Width) && string.IsNullOrEmpty(pro.WTNET))
                {
                    pro.ProductList = new List<Inventory>();

                }
                else
                {
                    pro.ProductList = pro.ProductsList().ToList();
                    pro.ProductList = pro.ProductList.Where(a => a.Loc.ToUpper().Contains(string.IsNullOrEmpty(pro.Loc) ? a.Loc.ToUpper() : pro.Loc.ToUpper())).
                        Where(a => a.Type.ToUpper().Contains(string.IsNullOrEmpty(pro.Type) ? a.Type.ToUpper() : pro.Type.ToUpper())).
                        Where(a => a.Finish.ToUpper().Contains(string.IsNullOrEmpty(pro.Finish) ? a.Finish.ToUpper() : pro.Finish.ToUpper())).
                        Where(a => a.Gauge.ToUpper().Contains(string.IsNullOrEmpty(pro.Gauge) ? a.Gauge.ToUpper() : pro.Gauge.ToUpper())).
                        Where(a => a.Width.ToUpper().Contains(string.IsNullOrEmpty(pro.Width) ? a.Width.ToUpper() : (pro.Width.ToUpper()))).
                        Where(a => a.WTNET.ToUpper().Contains(string.IsNullOrEmpty(pro.WTNET) ? a.WTNET.ToUpper() : (pro.WTNET.ToUpper()))).ToList();
                }

                if (string.IsNullOrEmpty(Pages))
                {

                    pro.IPagedProductsList = pro.ProductList.ToPagedList(page ?? 1, 50);   //Default Paging is 50
                    return View(pro);
                }
                else
                {
                    pro.IPagedProductsList = pro.ProductList.ToPagedList(page ?? 1, Convert.ToInt32(Pages));
                    return View(pro);
                }
            }
            else
            {
                return View(pro);
            }
        }


        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file)
        {
            string MinimumExcelDataValues = WebConfigurationManager.AppSettings["MinimumExcelDataValues"];
            DataSet ds = new DataSet();
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension =
                                     System.IO.Path.GetExtension(Request.Files["file"].FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/Content/") + Request.Files["file"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {

                        System.IO.File.Delete(fileLocation);
                    }
                    Request.Files["file"].SaveAs(fileLocation);
                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }

                    //                    Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }

                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }

                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                    string query = string.Format("Select * from [{0}] ", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                }
                if (fileExtension.ToString().ToLower().Equals(".xml"))
                {
                    string fileLocation = Server.MapPath("~/Content/") + Request.Files["FileUpload"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }

                    Request.Files["FileUpload"].SaveAs(fileLocation);
                    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    // DataSet ds = new DataSet();
                    ds.ReadXml(xmlreader);
                    xmlreader.Close();
                }
                string conn1 = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                MySqlConnection con1 = new MySqlConnection(conn1);
                try
                {
                    if (ds.Tables[0].Rows.Count > Convert.ToInt32(MinimumExcelDataValues))
                    {
                        string query1 = "truncate table products";
                        MySqlCommand cmd1 = new MySqlCommand(query1, con1);
                        con1.Open();
                        cmd1.ExecuteNonQuery();

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                            MySqlConnection con = new MySqlConnection(conn);

                            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i][0].ToString()) &&
                                !string.IsNullOrEmpty(ds.Tables[0].Rows[i][1].ToString()) &&
                                !string.IsNullOrEmpty(ds.Tables[0].Rows[i][2].ToString()) &&
                                !string.IsNullOrEmpty(ds.Tables[0].Rows[i][3].ToString()) &&
                                !string.IsNullOrEmpty(ds.Tables[0].Rows[i][4].ToString()) &&
                                !string.IsNullOrEmpty(ds.Tables[0].Rows[i][5].ToString()) &&
                                !string.IsNullOrEmpty(ds.Tables[0].Rows[i][6].ToString()))
                            {
                                string query = "Insert into products(Loc,Type,Finish,Gauge,Width,WTNET,NOOFPCS) Values('" +
                                 ds.Tables[0].Rows[i][0].ToString() + "','" + ds.Tables[0].Rows[i][1].ToString() +
                                 "','" + ds.Tables[0].Rows[i][2].ToString() + "','" + ds.Tables[0].Rows[i][3].ToString() + "','"
                                 + ds.Tables[0].Rows[i][4].ToString() + "','" +
                                 ds.Tables[0].Rows[i][5].ToString() + "','" + ds.Tables[0].Rows[i][6].ToString() + "')";
                                con.Open();
                                MySqlCommand cmd = new MySqlCommand(query, con);
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }


                        }

                        ViewBag.ShowData = ds.Tables[0].Rows.Count;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return View();
        }

        public JsonResult GetLocation(string term)
        {

            Inventory pro = new Inventory();
            List<string> Location;

            pro.ProductList = pro.ProductsList();

            Location = pro.ProductList.Where(a => a.Loc.StartsWith(term.ToUpper())).Select(b => b.Loc).Distinct().ToList();


            return Json(Location, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetType(string term)
        {

            Inventory pro = new Inventory();
            List<string> Type;

            pro.ProductList = pro.ProductsList();

            Type = pro.ProductList.Where(a => a.Type.StartsWith(term.ToUpper())).Select(b => b.Type).Distinct().ToList();


            return Json(Type, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFinish(string term)
        {

            Inventory pro = new Inventory();
            List<string> Finish;

            pro.ProductList = pro.ProductsList();

            Finish = pro.ProductList.Where(a => a.Finish.StartsWith(term.ToUpper())).Select(b => b.Finish).Distinct().ToList();


            return Json(Finish, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetGauge(string term)
        {

            Inventory pro = new Inventory();
            List<string> Gauge;

            pro.ProductList = pro.ProductsList();

            Gauge = pro.ProductList.Where(a => a.Gauge.StartsWith(term.ToUpper())).Select(b => b.Gauge).Distinct().ToList();


            return Json(Gauge, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetWidth(string term)
        {

            Inventory pro = new Inventory();
            List<string> Width;

            pro.ProductList = pro.ProductsList();

            Width = pro.ProductList.Where(a => a.Width.StartsWith(term.ToUpper())).Select(b => b.Width).Distinct().ToList();


            return Json(Width, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetWTNET(string term)
        {

            Inventory pro = new Inventory();
            List<string> WTNET;

            pro.ProductList = pro.ProductsList();

            WTNET = pro.ProductList.Where(a => a.WTNET.StartsWith(term.ToUpper())).Select(b => b.WTNET).Distinct().ToList();


            return Json(WTNET, JsonRequestBehavior.AllowGet);
        }




    }
}
