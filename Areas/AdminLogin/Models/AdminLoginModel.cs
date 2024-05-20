using System.ComponentModel.DataAnnotations;

namespace Project_Management2.Areas.AdminLogin.Models
{
    public class AdminLoginModel
    {
        public int AdminID { get; set; }

        [Required(ErrorMessage = "Please Enter Username !")]
        public string? AdminName { get; set; }

        [Required(ErrorMessage = "Please Enter Password !")]
        public string? AdminPassword { get; set; }

        public string? AdminEmail { get; set; }


        public int AdminContactNo { get; set; }

        public DateTime AdminLastLogin { get; set; }
    }
}
