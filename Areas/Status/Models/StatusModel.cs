namespace Project_Management2.Areas.Status.Models
{
    public class StatusModel
    {
        public int? StatusID { get; set; } = null;
        public string? StatusName { get; set; }
        public string? StatusCssColor { get; set; }

    }
    public class StatusDropDownModel
    {
        public int? StatusID { get; set; } = null;
        public string? StatusName { get; set; }
    }
}
