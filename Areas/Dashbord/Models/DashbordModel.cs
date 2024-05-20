namespace Project_Management2.Areas.Dashbord.Models
{
    public class DashbordModel
    {
        public int? TotalEmployee { get; set; }

        public int? TotalEmployeesWithoutTasks { get; set; }

        public int? TotalEmployeeAssignedtask { get; set; }

        public int? TotalProject { get; set; }

        public int? TotalUpcomingProject { get; set; }

        public int? TotalCurrentProject { get; set; }

        public int? TotalCompleteProject { get; set; }
    }
}
