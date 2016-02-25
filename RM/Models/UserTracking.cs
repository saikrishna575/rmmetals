using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace RM.Models
{
    public class UserTracking
    {

        public int Id { get; set; }
        public string user_Id { get; set; }
        public string IPAddress { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string SearchedData { get; set; }
        [DataType(DataType.DateTime)]
        public string StartDate { get; set; }
        [DataType(DataType.DateTime)]
        public string EndDate
        {
            set;
            get;

        }
        public int StartDateHours { get; set; }
        public int StartDateMinutes { get; set; }
        public string StartDateAmPm { get; set; }
        public int EndDateHours { get; set; }
        public int EndDateMinutes { get; set; }
        public string EndDateAmPm { get; set; }
        public List<UserTracking> UserTrackingList { get; set; }

        public List<UserTracking> SearchedDataList { get; set; }
        public IPagedList<UserTracking> PagedUserTrackingList { get; set; }
        public string RadioButtonSelectedValue { get; set; }




        public IEnumerable<SelectListItem> GetAmPm()
        {
            List<SelectListItem> newlist = new List<SelectListItem>();
            newlist.Add(new SelectListItem { Text = "AM", Value = "AM" });
            newlist.Add(new SelectListItem { Text = "PM", Value = "PM" });
            return newlist;

        }
        public IEnumerable<SelectListItem> GetHours()
        {

            List<SelectListItem> newlist = new List<SelectListItem>();
            newlist.Add(new SelectListItem { Text = "1", Value = "1" });
            newlist.Add(new SelectListItem { Text = "2", Value = "2" });
            newlist.Add(new SelectListItem { Text = "3", Value = "3" });
            newlist.Add(new SelectListItem { Text = "4", Value = "4" });
            newlist.Add(new SelectListItem { Text = "5", Value = "5" });
            newlist.Add(new SelectListItem { Text = "6", Value = "6" });
            newlist.Add(new SelectListItem { Text = "7", Value = "7" });
            newlist.Add(new SelectListItem { Text = "8", Value = "8" });
            newlist.Add(new SelectListItem { Text = "9", Value = "9" });
            newlist.Add(new SelectListItem { Text = "10", Value = "10" });
            newlist.Add(new SelectListItem { Text = "11", Value = "11" });
            newlist.Add(new SelectListItem { Text = "12", Value = "12" });
            return newlist;

        }

        public IEnumerable<SelectListItem> GetMins()
        {
            List<SelectListItem> newlist = new List<SelectListItem>();
            newlist.Add(new SelectListItem { Text = "00", Value = "00" });
            newlist.Add(new SelectListItem { Text = "10", Value = "10" });
            newlist.Add(new SelectListItem { Text = "20", Value = "20" });
            newlist.Add(new SelectListItem { Text = "30", Value = "30" });
            newlist.Add(new SelectListItem { Text = "40", Value = "40" });
            newlist.Add(new SelectListItem { Text = "50", Value = "50" });

            return newlist;

        }


        public void insert(UserTracking user)
        {

            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;



            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("Sp_AddUserTracking", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@_User_Id", user_Id);
                cmd.Parameters.AddWithValue("@_IPAddress", IPAddress);
                cmd.Parameters.AddWithValue("@_TimeStamp", DateTime.Now);
                cmd.ExecuteNonQuery();

            }


        }



        public List<UserTracking> UserTrackingReport()
        {
            List<UserTracking> UTList = new List<UserTracking>();

            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_UserTrackingReport", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    UserTracking UT = new UserTracking();
                    UT.IPAddress = rdr["IPAddress"].ToString();
                    UT.TimeStamp = Convert.ToDateTime(rdr["TimeIn"]);
                    UT.user_Id = rdr["userid"].ToString();
                    UT.Name = rdr["Name"].ToString();
                    UT.CompanyName = rdr["CompanyName"].ToString();
                    UT.Email = rdr["Email"].ToString();
                    UT.PhoneNumber = rdr["PhoneNumber"].ToString();
                    UTList.Add(UT);
                }
            }
            return UTList;
        }


        public List<UserTracking> SearchDataReport()
        {
            List<UserTracking> UTList = new List<UserTracking>();

            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_InventorySearchDataReport", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    UserTracking UT = new UserTracking();
                    UT.IPAddress = rdr["IPAddress"].ToString();
                    UT.TimeStamp = Convert.ToDateTime(rdr["CurrentTime"]);
                    UT.user_Id = rdr["userid"].ToString();
                    UT.Name = rdr["Name"].ToString();
                    UT.CompanyName = rdr["CompanyName"].ToString();
                    UT.Email = rdr["Email"].ToString();
                    UT.PhoneNumber = rdr["PhoneNumber"].ToString();
                    UT.SearchedData = rdr["SearchData"].ToString();
                    UTList.Add(UT);
                }
            }
            return UTList;
        }
    }
}