using DeepfakeWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;


namespace DeepfakeWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly string connectionString = "Server=DESKTOP-VQLGMRS;Database=DeppFakeAuthDb;Trusted_Connection=True;TrustServerCertificate=True;";


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            user.Password = HashPassword(user.Password);

            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                ViewBag.Message = "Kullanıcı adı ve şifre zorunludur.";
                return View();
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Kullanıcı adı zaten var mı?
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Username", user.Username);

                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    ViewBag.Message = "Bu kullanıcı adı zaten kayıtlı.";
                    return View();
                }

                // Kayıt işlemi
                string insertQuery = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@Username", user.Username);
                insertCmd.Parameters.AddWithValue("@Password", user.Password);
                insertCmd.ExecuteNonQuery();
            }

            ViewBag.Message = "Kayıt başarılı!";
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            user.Password = HashPassword(user.Password);

            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                ViewBag.Message = "Kullanıcı adı ve şifre zorunludur.";
                return View();
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password); // ileride hash ile karşılaştıracağız

                int count = (int)cmd.ExecuteScalar();

                if (count == 1)
                {
                    HttpContext.Session.SetString("Username", user.Username);
                    return RedirectToAction("Upload", "Video"); // girişten sonra yönlenecek sayfa
                }
                else
                {
                    ViewBag.Message = "Kullanıcı adı veya şifre yanlış.";
                    return View();
                }
            }
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }



    }
}

