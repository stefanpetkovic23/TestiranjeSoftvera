namespace Models;
public class TeamMember
{
    [Key]
    public int Id { get; set; }

    // Dodaj informacije o članu tima
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }

    // Dodaj relacije prema drugim entitetima
    public int TeamId { get; set; }
    //[JsonIgnore]
    public Team? Team { get; set; }

    public int ProjectId { get; set; }
    public Project? Project { get; set; }

    //[JsonIgnore]
    public List<ProjectTask>? AssignedTasks { get; set; }
}