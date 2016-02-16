using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RM.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int csv_id { get; set; }
        public string location { get; set; }
        public string item { get; set; }
        public string whrs { get; set; }
        public string package { get; set; }
        public string types { get; set; }
        public string finish { get; set; }
        public string thickness { get; set; }
        public string width { get; set; }
        public string net_weight { get; set; }
        public string price { get; set; }



        public List<Product> ProductList()
        {
            List<Product> productListItems = new List<Product>();

            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("Sp_GetAllProduct", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Product product = new Product();

                    product.Id = Convert.ToInt32(rdr["id"]);
                    product.csv_id = Convert.ToInt32(rdr["csv_id"]);
                    product.location = rdr["location"].ToString();
                    product.item = rdr["item"].ToString();
                    product.whrs = rdr["whrs"].ToString();
                    product.package = rdr["packg"].ToString();
                    product.types = rdr["types"].ToString();
                    product.finish = rdr["finish"].ToString();
                    product.thickness = rdr["thickness"].ToString();
                    product.width = rdr["width"].ToString();
                    product.net_weight = rdr["net_weight"].ToString();
                    product.price = rdr["price"].ToString();
                    productListItems.Add(product);
                }


            }

            return productListItems;
        }


       
    }
}