using Microsoft.EntityFrameworkCore;
namespace Models;
public class TaskMenagmentDbContext : DbContext
{
    public DbSet<ProjectTask> Tasks { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Deadline> Deadlines { get; set; }
    public DbSet<TaskCategory> TaskCategories { get; set; }
    public DbSet<TeamMember> TeamMembers { get; set; }

    public TaskMenagmentDbContext(DbContextOptions options) : base(options)
    {

    }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Konfiguracija relacija između entiteta ako je potrebno

        modelBuilder.Entity<TeamMember>()
            .HasOne(tm => tm.Team)
            .WithMany(t => t.Members)
            .HasForeignKey(tm => tm.TeamId);

        modelBuilder.Entity<Deadline>()
                .HasOne(p => p.Project)
                .WithOne(d => d.Deadline)
                .HasForeignKey<Deadline>(p => p.ProjectId);

        modelBuilder.Entity<ProjectTask>()
        .HasOne(pt => pt.TeamMember)
        .WithMany(tm => tm.AssignedTasks)
        .HasForeignKey(pt => pt.TeamMemberId)
        .OnDelete(DeleteBehavior.NoAction);


        // Dodajte slične konfiguracije za ostale relacije između entiteta
    }
}

