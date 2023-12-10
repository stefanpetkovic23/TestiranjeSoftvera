namespace Models;
public class Team
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }

    // Dodaj relacije prema drugim entitetima
    //[JsonIgnore]
    public List<TeamMember>? Members { get; set; }
}