using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Project_Management2.Areas.Employee.Models
{


    public class Employee_Main
    {
        [JsonProperty("employeemodel")]
        public IEnumerable<EmployeeModel> Employee { get; set; }
       
        public Emp_Info_By_ID Emp_Info_By_ID { get; set; }

    }
    
    public class EmployeeModel
    {
        public int? EmpID { get; set; } = null;

        [Required(ErrorMessage = "Employee Name is Required")]
        [DisplayName("Employee Name")]
        public string EmpName { get; set; }

        [Required(ErrorMessage = "Employee Code is Required")]
        [DisplayName("Employee Code")]
        public string EmpCode { get; set; }

        [Required(ErrorMessage = "Employee Position is Required")]
        [DisplayName("Employee Position")]
        public string EmpPosition { get; set; }

        [Required(ErrorMessage = "Employee Contact is Required")]
        [DisplayName("Employee Contact")]
        public string EmpContact { get; set; }

        [Required(ErrorMessage = "Employee Email Address is Required")]
        [DisplayName("Employee Email Address")]
        public string EmpEmail { get; set; }

        [Required(ErrorMessage = "Employee Department is Required")]
        [DisplayName("Employee Department")]
        public string EmpDepartment { get; set; }

        [Required(ErrorMessage = "Employee DOB is Required")]
        [DisplayName("Employee DOB")]
        public DateTime? EmpDateOfBirth { get; set; } = null;

        [Required(ErrorMessage = "Employee Profile Image is Required")]
        [DisplayName("Employee Profile Image")]
        public string? EmpProfileImage { get; set; }="abc.jpg";


        [Required(ErrorMessage = "Employee Profile Image is Required")]
        [DisplayName("Employee Profile Image")]
        public IFormFile? EmpProfileImagefile { get; set; } = null;
        [Required(ErrorMessage = "Employee Proof Image is Required")]
        [DisplayName("Employee Proof Image")]
        public string? EmpProofImage { get; set; } = "abc.jpg";
        [Required(ErrorMessage = "Employee Proof Image is Required")]
        [DisplayName("Employee Proof Image")]
        public IFormFile? EmpProofImagefile { get; set; } = null;

        [Required(ErrorMessage = "Employee Proof Name is Required")]
        [DisplayName("Employee Proof Name")]
        public string EmpProofName { get; set; }

        [Required(ErrorMessage = "Employee IS Manager ? is Required")]
        [DisplayName("Employee IS Manager ?")]
        public string? EmpManagerId { get; set; } = null;

        [Required(ErrorMessage = "Employee PerHourCharge is Required")]
        [DisplayName("Employee PerHourCharge")]
        public string EmpPerHourCharge { get; set; }

        [Required(ErrorMessage = "Employee GitLink is Required")]
        [DisplayName("Employee GitLink")]
        public string? EmpGitLink { get; set; } = null;
        public DateTime? Created { get; set; } = null;
        public DateTime? Modified { get; set; } = null;
    }

    public class Emp_Info_By_ID
    {
        public int EmpID { get; set; }

        [Required(ErrorMessage = "Employee Name is Required")]
        [DisplayName("Employee Name")]
        public string EmpName { get; set; }

        [Required(ErrorMessage = "Employee Code is Required")]
        [DisplayName("Employee Code")]
        public string EmpCode { get; set; }

        [Required(ErrorMessage = "Employee Position is Required")]
        [DisplayName("Employee Position")]
        public string EmpPosition { get; set; }

        [Required(ErrorMessage = "Employee Contact is Required")]
        [DisplayName("Employee Contact")]
        public string EmpContact { get; set; }

        [Required(ErrorMessage = "Employee Email Address is Required")]
        [DisplayName("Employee Email Address")]
        public string EmpEmail { get; set; }

        [Required(ErrorMessage = "Employee Department is Required")]
        [DisplayName("Employee Department")]
        public string EmpDepartment { get; set; }

        [Required(ErrorMessage = "Employee DOB is Required")]
        [DisplayName("Employee DOB")]
        public DateTime EmpDateOfBirth { get; set; }

        [Required(ErrorMessage = "Employee Profile Image is Required")]
        [DisplayName("Employee Profile Image")]
        public string EmpProfileImage { get; set; }

        [Required(ErrorMessage = "Employee Proof Image is Required")]
        [DisplayName("Employee Proof Image")]
        public string EmpProofImage { get; set; }

        [Required(ErrorMessage = "Employee Proof Name is Required")]
        [DisplayName("Employee Proof Name")]
        public string EmpProofName { get; set; }

        [Required(ErrorMessage = "Employee IS Manager ? is Required")]
        [DisplayName("Employee IS Manager ?")]
        public string? EmpManagerId { get; set; } = null;

        [Required(ErrorMessage = "Employee PerHourCharge is Required")]
        [DisplayName("Employee PerHourCharge")]
        public string EmpPerHourCharge { get; set; }

        [Required(ErrorMessage = "Employee GitLink is Required")]
        [DisplayName("Employee GitLink")]
        public string? EmpGitLink { get; set; } = null;
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    public class EmployeeDropDownModel 
    {

        [Required(ErrorMessage = "Employee Name is Required")]
        [DisplayName("Employee Name")]
        public int? EmpID { get; set; } = null;
        public string? EmpName { get; set; }
    }
    public class ApiResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<EmployeeModel> data { get; set; }
    }
}