using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace RM.Models
{
    public class SearchedData
    {
        public int Id { get; set; }
        public int FilterDataId { get; set; }
        public int SearchedType { get; set; }
        public string SearchedItem { get; set; }
        public FilterData Filerdata { get; set; }
        public Dictionary<int, string> Details { get; set; }
        public void insert(SearchedData user)
        {
            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_AddInventorySearch", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@_User_Id", user.Filerdata.user_Id);
                cmd.Parameters.AddWithValue("@_IPAddress", user.Filerdata.IPAddress);
                cmd.Parameters.AddWithValue("@_Name", user.Filerdata.Name);
                cmd.Parameters.AddWithValue("@_CompanyName", user.Filerdata.CompanyName);
                cmd.Parameters.AddWithValue("@_phonenumber", user.Filerdata.CompanyName);
                cmd.Parameters.AddWithValue("@_email", user.Filerdata.Email);
                cmd.Parameters.AddWithValue("@_TimeStamp", DateTime.Now);
                cmd.Parameters.Add("@id", MySqlDbType.Int16).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                user.FilterDataId = Convert.ToInt32(cmd.Parameters["@id"].Value);
            }
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                foreach (KeyValuePair<int, string> pair in Details)
                {
                    MySqlCommand cmd1 = new MySqlCommand("Sp_AddInventorySearchdata", con);
                    cmd1.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@_fid", user.FilterDataId);
                    cmd1.Parameters.AddWithValue("@_searchedType", pair.Key.ToString());
                    cmd1.Parameters.AddWithValue("@_searchedItem", pair.Value.ToString());

                    cmd1.ExecuteNonQuery();
                }
            }
        }

       
    }
}