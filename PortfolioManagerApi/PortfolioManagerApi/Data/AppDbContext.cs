using Microsoft.EntityFrameworkCore;
using PortfolioManagerApi.Models;

namespace PortfolioManagerApi.Data
{
    public class AppDbContext : DbContext  // DbContext jest odpowiedzialny za połączenie z bazą danych
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }  // Konstruktor przyjmujący opcje dla DbContext

        public DbSet<Project> Projects { get; set; }  // Reprezentuje tabelę Project w bazie danych
        public DbSet<Technology> Technologies { get; set; }  // Reprezentuje tabelę Technology w bazie danych
        public DbSet<ProjectTechnology> ProjectTechnologies { get; set; }  // Reprezentuje tabelę pośrednią między Project a Technology

        // Konfiguracja modelu bazy danych (relacje między tabelami)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectTechnology>()
                .HasKey(pt => new { pt.ProjectId, pt.TechnologyId });  // Definiuje klucz złożony z dwóch kolumn: ProjectId i TechnologyId

            modelBuilder.Entity<ProjectTechnology>()
                .HasOne(pt => pt.Project)  // Określa, że ProjectTechnology ma jedno powiązanie z Project
                .WithMany(p => p.Technologies)  // Project może mieć wiele powiązań z Technology
                .HasForeignKey(pt => pt.ProjectId);  // Określa klucz obcy ProjectId

            modelBuilder.Entity<ProjectTechnology>()
                .HasOne(pt => pt.Technology)  // Określa, że ProjectTechnology ma jedno powiązanie z Technology
                .WithMany(t => t.Projects)  // Technology może mieć wiele powiązań z Project
                .HasForeignKey(pt => pt.TechnologyId);  // Określa klucz obcy TechnologyId
        }
    }
}
