using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Project_Management2.Areas.Project.Models
{
    public class ProjectModel
    {
        public int? ProjID { get; set; } = null;

        [Required(ErrorMessage = "Project Name is Required")]
        [DisplayName("Project Name")]
        public string ProjName { get; set; }

        [Required(ErrorMessage = "Project Description is Required")]
        [DisplayName("Project Description")]
        public string ProjDescription { get; set; }

        [Required(ErrorMessage = "Project StartDate is Required")]
        [DisplayName("Project StartDate")]
        public DateTime? ProjStartDate { get; set; } = null;

        [Required(ErrorMessage = "Project EndDate is Required")]
        [DisplayName("Project EndDate")]
        public DateTime? ProjEndDate { get; set; } = null;

        [Required(ErrorMessage = "Project TotalCost is Required")]
        [DisplayName("Project TotalCost")]
        public string ProjTotalCost { get; set; }

/*        [Required(ErrorMessage = "Project Manager is Required")]
        [DisplayName("Project Manager")]
        public string ProjManager { get; set; }*/

        [Required(ErrorMessage = "Employee Name is Required")]
        [DisplayName("Employee Name")]
        public int? EmpID { get; set; } = null;
        public string? EmpName { get; set; } = null;

        [Required(ErrorMessage = "Project Source is Required")]
        [DisplayName("Project Source")]
        public string ProjSource { get; set; }

        [Required(ErrorMessage = "Project Documentation is Required")]
        [DisplayName("Project Documentation")]
        public string ProjDocumentation { get; set; }

        [Required(ErrorMessage = "Project Status is Required")]
        [DisplayName("Project Status")]


        public int? StatusID { get; set; }= null;
        public string? StatusName { get; set; } = null;
        public string? StatusCssColor { get; set; } = null;
        public DateTime? Created { get; set; } = null;
        public DateTime? Modified { get; set; } = null;
    }
    public class ProjectDropDownModel
    {
        [Required(ErrorMessage = "Project Status is Required")]
        [DisplayName("Project Status")]
        public int? ProjID { get; set; } = null;
        public string? ProjName { get; set; }
    }
}
