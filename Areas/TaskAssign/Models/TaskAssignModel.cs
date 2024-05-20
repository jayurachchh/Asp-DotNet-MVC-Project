using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Project_Management2.Areas.TaskAssign.Models
{
    public class TaskAssignModel
    {
        public int? TaskAssignID { get; set; } = null;

        [Required(ErrorMessage = "TaskAssign StartDate is Required")]
        [DisplayName("TaskAssign StartDate")]
        public DateTime? TaskAssignStartDate { get; set; } = null;

        [Required(ErrorMessage = "TaskAssign CompletionDate is Required")]
        [DisplayName("TaskAssign CompletionDate")]
        public DateTime? TaskAssignCompletionDate { get; set; } = null;

        [Required(ErrorMessage = "TaskAssign Remarks is Required")]
        [DisplayName("TaskAssign Remarks")]
        public string Remarks { get; set; }

        [Required(ErrorMessage = "Project Name is Required")]
        [DisplayName("Project Name")]
        public int? ProjWiseTaskID { get; set; } = null;

        public string? ProjWiseTaskName { get; set; } = null;

        [Required(ErrorMessage = "Employee Name is Required")]
        [DisplayName("Employee Name")]
        public int? EmpID { get; set; } = null;
        public string? EmpName { get; set; } = null;

        [Required(ErrorMessage = "Status Name is Required")]
        [DisplayName("Status Name")]
        public int? StatusID { get; set; } = null;
        public string? StatusName { get; set; } = null;
        public string? StatusCssColor { get; set; } = null;
        public DateTime? Created { get; set; } = null;
        public DateTime? Modified { get; set; } = null;
    }
}
