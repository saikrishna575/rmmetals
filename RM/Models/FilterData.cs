using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RM.Models
{
    public class FilterData
    {
        public int Id { get; set; }
        public string user_Id { get; set; }
        public string IPAddress { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }


        public FilterData GetData(string id)
        {
            FilterData Data = new FilterData();
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
                    Data.Name = rdr["Name"].ToString();
                    Data.PhoneNumber = rdr["PhoneNumber"].ToString();
                    Data.CompanyName = rdr["CompanyName"].ToString();
                    Data.Email = rdr["Email"].ToString();
                }

                return Data;
            }
        }
    }
}