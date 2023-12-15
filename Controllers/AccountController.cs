using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using Task11_crud_.Models;
using ActiveUp.Net.Mail;
using System.Threading;

namespace Task11_crud_.Controllers
{
    public class AccountController : Controller
    {
        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly SignInManager<Registration> _signInManager;
        private static readonly Dictionary<string, string> ConfirmationTokenDict = new Dictionary<string, string>();
        private readonly IConfiguration _configuration;
        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
            //_userManager = userManager;
            //_signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            var model = new Registration
            {
                Countries = GetCountries(),
                States = new Dictionary<string, IEnumerable<string>>()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Register(Registration user)
        {
            if (!IsPasswordComplex(user.Password))
            {
                ModelState.AddModelError("Password", "Password must contain at least one uppercase letter, one special character, and one digit.");
            }
            string connectionstring = _configuration.GetConnectionString("DefaultConnection");
            user.Countries = GetCountries();
            if (ModelState.IsValid)
            {
                SqlConnection connection = new SqlConnection(connectionstring);
                connection.Open();
                using (SqlCommand command = new SqlCommand("InsertUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName",user.LastName);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("PhoneNumber", user.PhoneNumber);
                    command.Parameters.AddWithValue("@Address", user.Address);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Country", user.SelectedCountry);
                    command.Parameters.AddWithValue("@State", user.SelectedState);
                    command.ExecuteNonQuery();
                }
                   //ViewBag.Message = "Registration successful!";
                   string confirmationToken = Guid.NewGuid().ToString();
                   ConfirmationTokenDict[user.Email] = confirmationToken;
                   await SendConfirmationEmail(user.Email, confirmationToken);
            }
            else
            {
                user.Countries = GetCountries();
                return View("Register", user);
            }
            
           return RedirectToAction("welcome");
        }
        private bool IsPasswordComplex(string password)
        {
            return password.Any(char.IsUpper) && password.Any(char.IsDigit) && password.Any(IsSpecialCharacter);
        }

        private static bool IsSpecialCharacter(char c)
        {
            const string specialCharacters = @"!@#$%^&*()-_=+";
            return specialCharacters.Contains(c);
        }
        public IActionResult ConfirmEmail(string token)
        {
            if (IsValidToken(token))
            {
                SetEmailConfirmedStatus(token, true);
                return View("EmailConfirmed");
            }
            return View("Error");
        }
        public IActionResult EmailConfirmed()
        {
            return View();
        }
        //new code
        //private async Task SendConfirmationEmail(string email, string token)
        //{
        //    try
        //    {
        //        using (var smtpClient = SmtpClient("smtp.gmail.com"))
        //        {
        //            smtpClient.Port = 587;
        //            smtpClient.UseDefaultCredentials = false;
        //            smtpClient.Credentials = new NetworkCredential("divyaraparthi03@gmail.com", "fxnuersetmnrrilv");
        //            smtpClient.EnableSsl = true;

        //            var tk = GetConfirmationLink(token);

        //            using (var mailMessage = new MailMessage
        //            {
        //                From = new MailAddress("divyaraparthi03@gmail.com"),
        //                Subject = "Confirm Your Email",
        //                Body = $"Click the link to confirm your email: {tk}",
        //                // Body = $"<p>Click the link to confirm your email: <a href='{tk}'>{tk}</a></p>",
        //                IsBodyHtml = true, 
        //            })
        //            {
        //                mailMessage.To.Add(email);
        //                //smtpClient.SendMailAsync(mailMessage);

        //                await smtpClient.SendMailAsync(mailMessage);
        //                Console.WriteLine($"Confirmation email sent to {email}");
        //            }
        //        }
        //    }
        //    catch (System.Net.Mail.SmtpException ex)
        //    {
        //        Console.WriteLine($"SMTP Exception: {ex}");
        //        if (ex.InnerException != null)
        //        {
        //            Console.WriteLine($"Inner Exception: {ex.InnerException}");
        //        }
        //    }
        //}

        //private string GetConfirmationLink(string token)
        //{
        //    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { token = token }, Request.Scheme);
        //    return confirmationLink;
        //}

        private async Task SendConfirmationEmail(string email, string token)
        {
            try
            {
                using (var smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential("divyaraparthi03@gmail.com", "fxnuersetmnrrilv");
                    smtpClient.EnableSsl = true;
                    var tk = GetConfirmationLink(token);
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("divyaraparthi03@gmail.com"),
                        Subject = "Confirm Your Email",
                        Body = $"<p>Click the link to confirm your email: <a href='{tk}'>{tk}</a></p>",
                        IsBodyHtml = true,
                    };

                    mailMessage.To.Add(email);

                    await smtpClient.SendMailAsync(mailMessage);
                    Console.WriteLine($"Confirmation email sent to {email}");
                }
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                Console.WriteLine($"SMTP Exception: {ex.Message}");

            }
            catch (SystemException ex)
            {
                Console.WriteLine("exception");
            }
        }

        private string GetConfirmationLink(string token)
        {
            var confirmationLink = Url.Action("ConfirmEmail", "Account", new { token = token }, Request.Scheme);
            return confirmationLink;
        }
        private bool IsValidToken(string token)
        {
            
            return ConfirmationTokenDict.ContainsValue(token);
        }
        private void SetEmailConfirmedStatus(string token, bool EmailConfirmed)
        {
            var user = ConfirmationTokenDict.FirstOrDefault(x => x.Value == token);
            if (user.Key != null)
            {
                ConfirmationTokenDict.Remove(user.Key);
                UpdateUserEmailConfirmedStatus(user.Key, EmailConfirmed);
                Console.WriteLine($"User {user.Key}'s email is confirmed!");
            }
        }
        private void UpdateUserEmailConfirmedStatus(string Email, bool EmailConfirmed)
        {
            string connectionstring = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new SqlConnection(connectionstring))
            {
                connection.Open();

                using (var command = new SqlCommand("EmailConfirm", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmailConfirmed", EmailConfirmed);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.ExecuteNonQuery();
                }
            }
        }
        //email


        public IActionResult Welcome()
        {
            return View();
        }
        private IEnumerable<string> GetCountries()
        {
            return new List<string> ();
        }

        //private Dictionary<string, IEnumerable<string>> GetStates()
        //{

        //    var states = new Dictionary<string, IEnumerable<string>>
        //{
        //    { "India", new List<string> { "Andhrapradesh", "Telangana", "Karnataka" } },
        //    { "Australia", new List<string> { "sydney", "New South whales", "Melbourne" } }

        //};

        //    return states;
        //}
        //[HttpGet]
        public JsonResult GetStates(string selectedCountry)
        {
            var states = GetStatesForCountry(selectedCountry);
            return Json(states);
        }
        private List<string> GetStatesForCountry(string selectedCountry)
        {
            switch (selectedCountry)

            {
                case "India":
                    return new List<string> { "Andhra Pradesh", "Telangana", "Karnataka" };
                case "Australia":
                    return new List<string> { "NewSothWhales", "Sydney", "Melbourne" };
                default:
                   return new List<string>();
            }
        }

        [HttpGet]
        //[Route("Login")]
        public IActionResult Login()
        {
            return View();  
        }
        [HttpPost]
        //[Route("Login")]
        public async Task<IActionResult> Login(Registration users)
        {
                if (ValidateUser(users))
                {

                //new code 
                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Name, users.Email),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity));

                //new code end

                return RedirectToAction("Index", "Home", new { Email = users.Email });

            }

            return View("Index");
            
        }
        private bool ValidateUser(Registration users)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            SqlConnection connection=new SqlConnection(connectionString);
            connection.Open();
            using (SqlCommand cmd = new SqlCommand("sp_login",connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", users.Email);
                cmd.Parameters.AddWithValue("@Password",users.Password);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;

            }
        }       
        public IActionResult Registration()
        {
            return View();
        }

        public async Task<IActionResult> signout(string returnUrl = null)
        {
            //to delete cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");

        }

    }
}
