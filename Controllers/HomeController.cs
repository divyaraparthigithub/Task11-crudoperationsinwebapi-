//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Task11_crud_.Models;

namespace Task11_crud_.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult CustomerList()
        {
            string Connectionstring = _configuration.GetConnectionString("DefaultConnection");
            List<CustomerList>list= new List<CustomerList>();
            SqlConnection connection = new SqlConnection(Connectionstring);
            using (SqlCommand command = new SqlCommand("sp_list", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader dr= command.ExecuteReader();
                while (dr.Read())
                {
                    CustomerList cs = new CustomerList
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = dr["Name"].ToString(),
                        Address = dr["Address"].ToString(),
                        Phone = dr["Phone"].ToString(),
                        Email = dr["Email"].ToString(),
                        ProductName = dr["ProductName"].ToString(),
                        GenderName = dr["GenderName"].ToString()
                    };
                    list.Add(cs);
                }
            connection.Close();
            }
           
        return View(list);
        }
        //product list
        public IList<SelectListItem> GetProductList()
        {
            List<SelectListItem> ProductList = new List<SelectListItem>();
            string Connectionstring = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(Connectionstring))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("sp_GetProduct", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ProductList.Add(new SelectListItem
                        {
                            Value = dr["Id"].ToString(),
                            Text = dr["ProductName"].ToString()
                        });
                    }
                }
            }
            return (ProductList);
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            Customer obj = new Customer();
            obj.ProductList = (List<SelectListItem>)GetProductList();
            return View(obj);
        }
        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                // ModelState.AddModelError("Email", "Email already exists.");
                customer.ProductList = (List<SelectListItem>)GetProductList();
                //return View(customer);
            
                InsertCustomer(customer);
                return RedirectToAction("Index");
            }
            customer.ProductList = (List<SelectListItem>)GetProductList();
            return View(customer);
        }
        private void InsertCustomer(Customer customer)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_InsertCustomer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Id", customer.Id);
                cmd.Parameters.AddWithValue("@Name", customer.Name);
                cmd.Parameters.AddWithValue("@ProductId", customer.ProductId);
                cmd.Parameters.AddWithValue("@GenderId", customer.GenderId);
                cmd.Parameters.AddWithValue("@Address", customer.Address);
                cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                cmd.Parameters.AddWithValue("@Email", customer.Email);

                cmd.ExecuteNonQuery();
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Contact()
        {
            //Contact c=new Contact();
            return View();
        }
        [HttpPost]
        public IActionResult Contact(Contact contact)
        {
            Contact c = new Contact();

            if (ModelState.IsValid)
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_contactInsert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fullName", contact.fullName);
                    cmd.Parameters.AddWithValue("@message", contact.message);
                    cmd.Parameters.AddWithValue("@agreeTerms", contact.agreeTerms);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("CustomerList");
        }
        [HttpGet]
        public IActionResult GetDetailsById(int id)
        {
            //Customer obj = new Customer();
            CustomerList li = new CustomerList();
            li.ProductId = li.ProductId;
            li.ProductList = (List<SelectListItem>)GetProductList();


            SqlConnection con = new SqlConnection("Data Source=DRAPARTH-L-5507\\SQLEXPRESS;Initial Catalog=Task11;User ID=sa;Password=Welcome2evoke@1234");
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_getbyid", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {

                    //li.Id = Convert.ToInt32(dr["Id"]);
                    li.Name = dr["Name"].ToString();

                    li.ProductName = dr["ProductName"].ToString();
                    //list.SelectedProductId = Convert.ToInt32(dr["ProductId"]);
                    //list.ProductList = GetProductList();
                    li.Address = dr["Address"].ToString();
                    li.Phone = dr["Phone"].ToString();
                    li.Email = dr["Email"].ToString();
                    li.GenderName = dr["GenderName"].ToString();

                }

            }
            return View("Edit",li);          
            //return PartialView("_Edit", li);

        }
        [HttpPost]
        public IActionResult Edit(CustomerList customer)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                CustomerList li = new CustomerList();

                customer.ProductList = (List<SelectListItem>)GetProductList();
                customer.ProductName = GetProductNameById(customer.ProductId);


                //li.ProductId = li.ProductId;
                //li.ProductList = (List<SelectListItem>)GetProductList();
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_edit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", customer.Id);
                cmd.Parameters.AddWithValue("@Name", customer.Name);
                cmd.Parameters.AddWithValue("@Address", customer.Address);
                cmd.Parameters.AddWithValue("@ProductName", customer.ProductName);
                cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@GenderName", customer.GenderName);
                cmd.ExecuteNonQuery();
            }
            return View(customer);
        }

        
        private string GetProductNameById(int ProductId)
        {

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_GetProductNameById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductId", ProductId);

                return cmd.ExecuteScalar()?.ToString();
            }
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {

            CustomerList list = new CustomerList();
            //string connectionString = _configuration.GetConnectionString("DefaultConnection");
            //SqlConnection con = new SqlConnection(connectionString);
            //con.Open();
            //SqlCommand cmd = new SqlCommand("sp_getbyid", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@Id", Id);
            //using (SqlDataReader dr = cmd.ExecuteReader())
            //{
            //    while (dr.Read())
            //    {

            //        list.Id = Convert.ToInt32(dr["Id"]);
            //        list.Name = dr["Name"].ToString();
            //        list.Address = dr["Address"].ToString();
            //        list.ProductName = dr["PName"].ToString();
            //        list.Phone = dr["Phone"].ToString();
            //        list.Email = dr["Email"].ToString();
            //        list.GenderName = dr["GenderName"].ToString();

            //    }
            //}
            return View(list);
        }
        public IActionResult Delete(CustomerList list, int Id)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_deletecustomer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.ExecuteNonQuery();


            }
            return View(list);
        }
        [Authorize]
        public IActionResult AboutUs()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}