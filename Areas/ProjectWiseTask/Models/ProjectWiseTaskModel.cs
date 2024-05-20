using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Project_Management2.Areas.ProjectWiseTask.Models
{
    public class ProjectWiseTaskModel
    {
        public int? ProjWiseTaskID { get; set; } = null;

        [Required(ErrorMessage = "Project Name is Required")]
        [DisplayName("Project Name")]
        public int? ProjID { get; set; } = null;

        public string? ProjName { get; set; } = null;

        [Required(ErrorMessage = "ProjectWiseTask Name is Required")]
        [DisplayName("ProjectWiseTask Name")]
        public string ProjWiseTaskName { get; set; }

        [Required(ErrorMessage = "ProjectWiseTask Number is Required")]
        [DisplayName("ProjectWiseTask Number")]
        public string? ProjWiseTaskNumber { get; set; } 

        [Required(ErrorMessage = "PrProjectWiseTaskoject StartDate is Required")]
        [DisplayName("ProjectWiseTask StartDate")]
        public DateTime? ProjWiseTaskStartDate { get; set; } = null;

        [Required(ErrorMessage = "PrProjectWiseTaskoject EndDate is Required")]
        [DisplayName("ProjectWiseTask EndDate")]
        public DateTime? ProjWiseTaskEndDate { get; set; } = null;

        /*        [Required(ErrorMessage = "ProjectWiseTask Employees is Required")]
                [DisplayName("ProjectWiseTask Employees")]
                public int? ProjWiseTaskEmployees { get; set; } = null;*/

        [Required(ErrorMessage = "ProjectWiseTask Employees is Required")]
        [DisplayName("ProjectWiseTask Employees")]
        public int? ProjectWiseTaskEmployeeNumber { get; set; } = null;

        public DateTime? Created { get; set; } = null;
        public DateTime? Modified { get; set; } = null;
    }
     
    public class ProjectWiseTaskDropDownModel 
    {
        public int? ProjWiseTaskID { get; set; } = null; 
        public string? ProjWiseTaskName { get; set; }
    }
}
