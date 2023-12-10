namespace Models;
public class TaskCategory
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }

    // Dodaj relacije prema zadacima
    //[JsonIgnore]
    public List<ProjectTask>? Tasks { get; set; }
}