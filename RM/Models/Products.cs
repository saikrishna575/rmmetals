using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
namespace RM.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Loc { get; set; }
        public string Type { get; set; }
        public string Finish { get; set; }
        public string Gauge { get; set; }
        public string Width { get; set; }
        public string WTNET { get; set; }
        public string NOOFPCS { get; set; }

        public string SelectedPage { get; set; }

        public List<Products>  ProductList { get; set; }

        public IPagedList<Products> IPagedProductsList { get; set; }
        public List<SelectListItem> PageList
        {
            get
            {
                return new List<SelectListItem>()
                {
                   new SelectListItem() {Text = "10", Value="10" },
                   new SelectListItem() {Text = "20", Value="20" },
                   new SelectListItem() {Text = "30", Value="30" },
                   new SelectListItem() {Text = "40", Value="40" }

                };
            }
        }


        public IEnumerable<SelectListItem> GetPages()
        {
            return PageList.Select(a => new SelectListItem()

            {
                Text= a.Text , Value = a.Value, Selected = (a.Value== SelectedPage)
            }
            );

        }


        public List<Products> ProductsList()
        {
            List<Products> productsListItems = new List<Products>();

            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_GetAllProducts", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Products products = new Products();
                    products.Id = Convert.ToInt32(rdr["Id"]);
                    products.Loc = rdr["Loc"].ToString();
                    products.Type = rdr["Type"].ToString();
                    products.Finish = rdr["Finish"].ToString();
                    products.Gauge = rdr["Gauge"].ToString();
                    products.Width = rdr["Width"].ToString();
                    products.WTNET = rdr["WTNET"].ToString();
                    products.NOOFPCS = rdr["NOOFPCS"].ToString();
                    productsListItems.Add(products);
                }
            }

            return productsListItems;
        }

        public Products productDetails(int id)
        {

            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("Sp_GetProductDetails", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@_id", id);
                MySqlDataReader rdr = cmd.ExecuteReader();
                Products products = new Products();
                while (rdr.Read())
                {
                    products.Loc = rdr["Loc"].ToString();
                    products.Type = rdr["Type"].ToString();
                    products.Finish = rdr["Finish"].ToString();
                    products.Gauge = rdr["Gauge"].ToString();
                    products.Width = rdr["Width"].ToString();
                    products.WTNET = rdr["WTNET"].ToString();
                    products.NOOFPCS = rdr["NOOFPCS"].ToString();
                }

                return products;
            }
        }
    }
}