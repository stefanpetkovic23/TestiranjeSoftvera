namespace Models;
public class Deadline
{
    [Key]
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public bool IsCompleted { get; set; } 


    public int ProjectId { get; set; }
    //[JsonIgnore]
    public Project? Project { get; set; }
}