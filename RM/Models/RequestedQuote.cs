using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RM.Models
{
    public class RequestedQuote
    {
        public int id { get; set; }
        public string User_Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Inventory product { get; set; }


        public void insert(RequestedQuote quote)
        {
            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_AddRequestedQuote", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("_UserId", quote.User_Id);
                cmd.Parameters.AddWithValue("_Email", quote.Email);
                cmd.Parameters.AddWithValue("_PhoneNumber", quote.PhoneNumber);
                cmd.Parameters.AddWithValue("_Location", quote.product.Loc);
                cmd.Parameters.AddWithValue("_type", quote.product.Type);
                cmd.Parameters.AddWithValue("_finish", quote.product.Finish);
                cmd.Parameters.AddWithValue("_Gauge", quote.product.Gauge);
                cmd.Parameters.AddWithValue("_width", quote.product.Width);
                cmd.Parameters.AddWithValue("_wtnet", quote.product.WTNET);
                cmd.Parameters.AddWithValue("_NOOFPCS", quote.product.NOOFPCS);
                cmd.ExecuteNonQuery();
            }

        }
        public RequestedQuote GetData(string id)
        {
            RequestedQuote Data = new RequestedQuote();
            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_GetUserDetails", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@_id", id);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Data.PhoneNumber = rdr["PhoneNumber"].ToString();
                    Data.Email = rdr["Email"].ToString();
                }

                return Data;
            }
        }
    }
}