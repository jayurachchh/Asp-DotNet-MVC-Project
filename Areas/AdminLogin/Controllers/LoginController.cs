using Microsoft.AspNetCore.Mvc;
using Project_Management2.Areas.AdminLogin.Models;
using System.Data;
using System.Data.SqlClient;

namespace Project_Management2.Areas.AdminLogin.Controllers
{
    [Area("AdminLogin")]
    [Route("~/[controller]/[action]")]
    public class LoginController : Controller
    {
        #region Database Configuration

        // IConfiguration instance to access configuration settings
        public IConfiguration Configuration;

        // Constructor for LoginController, accepting IConfiguration for dependency injection
        public LoginController(IConfiguration configuration)
        {
            // Initialize the Configuration property with the injected IConfiguration
            Configuration = configuration;
        }

        #endregion Database Configuration
        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(AdminLoginModel adminLoginModel)
        {

            string connection = this.Configuration.GetConnectionString("PersonConnectionString");

            if (ModelState.IsValid)
            {
                SqlConnection sqlConnection = new SqlConnection(connection);

                sqlConnection.Open();

                SqlCommand command = sqlConnection.CreateCommand();

                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.CommandText = "PR_ADMIN_LOGIN";

                command.Parameters.AddWithValue("@Admin_Username", SqlDbType.VarChar).Value = adminLoginModel.AdminName;

                command.Parameters.AddWithValue("@Password", SqlDbType.VarChar).Value = adminLoginModel.AdminPassword;

                SqlDataReader dataReader = command.ExecuteReader();

                DataTable admininfo = new DataTable();

                admininfo.Load(dataReader);

                if (admininfo.Rows.Count > 0)
                {
                    foreach (DataRow row in admininfo.Rows)
                    {
                        HttpContext.Session.SetString("AdminID", row["AdminID"].ToString());
                        HttpContext.Session.SetString("AdminName", row["AdminName"].ToString());
                        HttpContext.Session.SetString("AdminPassword", row["AdminPassword"].ToString());
                        HttpContext.Session.SetString("AdminPhoneNo", row["AdminContactNo"].ToString());
                        HttpContext.Session.SetString("AdminEmail", row["AdminEmail"].ToString());
                        HttpContext.Session.SetString("LastLogin", row["AdminLastLogin"].ToString());
                        break;
                    }
                }
                else
                {
                    TempData["ErrorMsg"] = "Invalid Username or Password.";

                    return RedirectToAction("LoginPage");
                }

                if (HttpContext.Session.GetString("AdminName") != null && HttpContext.Session.GetString("AdminPassword") != null)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("LoginPage");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LoginPage");
        }
    }
}
