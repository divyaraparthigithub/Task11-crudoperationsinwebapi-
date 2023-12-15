using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using Task11_crud_.Models;

namespace Task11_crud_.Controllers
{
    public class ContactController : Controller
    {
        private readonly IConfiguration _configuration;
        public ContactController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        //[Authorize]
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
            return View();
        }
    }
}
