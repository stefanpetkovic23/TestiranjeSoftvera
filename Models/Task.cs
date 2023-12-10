namespace Models;
public class ProjectTask
{
    [Key]
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int Priority { get; set; }
    public string? Status { get; set; }
    public int? Deadline { get; set; }

    // Dodaj relacije prema drugim entitetima
    public int ProjectId { get; set; }
    //[JsonIgnore]
    public Project? Project { get; set; }

    // Dodaj relaciju prema Kategoriji zadatka
    public int TaskCategoryId { get; set; }
    //[JsonIgnore]
    public TaskCategory? TaskCategory { get; set; }

    public int TeamMemberId { get; set; }
    //[JsonIgnore]
    public TeamMember? TeamMember { get; set; }
}