using PortfolioManagerApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PortfolioManagerApi.Models
{
    public class Project
    {
        // Unikalny identyfikator projektu
        public Guid Id { get; set; }

        // Tytuł projektu (wymagane, maksymalna długość 200 znaków)
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        // Opis projektu, który może zawierać HTML
        [DataType(DataType.Html)]
        public string Description { get; set; }

        // Data rozpoczęcia projektu
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        // Data zakończenia projektu, po dacie rozpoczęcia
        [DateAfter("StartDate")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        // Status projektu
        public string Status { get; set; }

        // URL do repozytorium projektu
        public string RepositoryUrl { get; set; }

        // URL do demonstracji projektu
        public string DemoUrl { get; set; }

        // Lista technologii powiązanych z projektem
        public List<ProjectTechnology> Technologies { get; set; } = new();
    }
}
