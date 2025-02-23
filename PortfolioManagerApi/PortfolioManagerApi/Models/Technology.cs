using System.ComponentModel.DataAnnotations;

namespace PortfolioManagerApi.Models
{
    public class Technology
    {
        // Identyfikator technologii
        public Guid Id { get; set; }

        // Nazwa technologii (tylko litery, cyfry i spacje)
        [Required]
        [RegularExpression("^[a-zA-Z0-9 ]+$")]
        public string Name { get; set; }

        // Lista powiązanych projektów z tą technologią
        public List<ProjectTechnology> Projects { get; set; } = new();
    }
}
