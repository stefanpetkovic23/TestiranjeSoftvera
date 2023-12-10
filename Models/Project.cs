namespace Models;
public class Project
{
    [Key]
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }

    public Deadline? Deadline { get; set; }
    //[JsonIgnore]
    public List<ProjectTask>? Tasks { get; set; }
    //[JsonIgnore]
    public List<TeamMember>? TeamMembers { get; set; }
}