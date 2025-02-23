namespace PortfolioManagerApi.Models
{
    public class ProjectTechnology
    {
        // Identyfikator projektu
        public Guid ProjectId { get; set; }

        // Projekt powiązany z technologią
        public Project Project { get; set; }

        // Identyfikator technologii
        public Guid TechnologyId { get; set; }

        // Technologia powiązana z projektem
        public Technology Technology { get; set; }
    }
}
