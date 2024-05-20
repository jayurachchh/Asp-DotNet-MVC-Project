namespace Project_Management2.Models;


#region Model : RecentAction
public class RecentAction
{
    public int? Rec_Act_Id { get; set; } = null;
    public string? Rec_Act_Table_Name { get; set; } = null;
    public string? Rec_Act_Info { get; set; }
    public DateTime Rec_Act_Create { get; set; }
}
#endregion

