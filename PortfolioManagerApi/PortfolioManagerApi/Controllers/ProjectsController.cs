using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioManagerApi.Data;
using PortfolioManagerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]  // Definiuje trasę do kontrolera
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context; // Inicjalizacja kontekstu bazy danych
        }

        // GET: api/Projects
        [HttpGet]  // Obsługuje żądanie GET do pobrania listy projektów
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();  // Zwraca wszystkie projekty
        }

        // GET: api/Projects/{id}
        [HttpGet("{id}")]  // Pobiera projekt na podstawie id
        public async Task<ActionResult<Project>> GetProject(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);  // Wyszukuje projekt po id

            if (project == null)  // Jeśli nie znaleziono projektu
            {
                return NotFound();  // Zwraca błąd 404
            }

            return project;  // Zwraca projekt
        }

        // POST: api/Projects
        [HttpPost]  // Dodaje nowy projekt
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            if (!ModelState.IsValid)  // Sprawdza, czy dane są prawidłowe
            {
                return BadRequest(ModelState);  // Jeśli nie, zwraca błąd 400
            }

            _context.Projects.Add(project);  // Dodaje projekt do bazy danych
            await _context.SaveChangesAsync();  // Zapisuje zmiany w bazie

            return CreatedAtAction("GetProject", new { id = project.Id }, project);  // Zwraca stworzony projekt
        }

        // PUT: api/Projects/{id}
        [HttpPut("{id}")]  // Aktualizuje projekt
        public async Task<IActionResult> PutProject(Guid id, Project project)
        {
            if (id != project.Id)  // Sprawdza, czy id w ścieżce jest zgodne z id projektu
            {
                return BadRequest();  // Jeśli nie, zwraca błąd 400
            }

            _context.Entry(project).State = EntityState.Modified;  // Oznacza projekt jako zmodyfikowany

            try
            {
                await _context.SaveChangesAsync();  // Zapisuje zmiany w bazie
            }
            catch (DbUpdateConcurrencyException)  // Obsługuje błąd konkurencji przy aktualizacji
            {
                if (!ProjectExists(id))  // Jeśli projekt nie istnieje
                {
                    return NotFound();  // Zwraca błąd 404
                }
                else
                {
                    throw;  // Inny błąd
                }
            }

            return NoContent();  // Zwraca status 204 (brak treści)
        }

        // DELETE: api/Projects/{id}
        [HttpDelete("{id}")]  // Usuwa projekt
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);  // Wyszukuje projekt po id
            if (project == null)  // Jeśli nie znaleziono
            {
                return NotFound();  // Zwraca błąd 404
            }

            _context.Projects.Remove(project);  // Usuwa projekt z bazy
            await _context.SaveChangesAsync();  // Zapisuje zmiany w bazie

            return NoContent();  // Zwraca status 204 (brak treści)
        }

        private bool ProjectExists(Guid id)
        {
            return _context.Projects.Any(e => e.Id == id);  // Sprawdza, czy projekt istnieje
        }

        // Dodaj technologię do projektu
        [HttpPost("{projectId}/technologies/{technologyId}")]  // Dodaje technologię do projektu
        public async Task<IActionResult> AddTechnologyToProject(Guid projectId, Guid technologyId)
        {
            var project = await _context.Projects.FindAsync(projectId);  // Wyszukuje projekt
            var technology = await _context.Technologies.FindAsync(technologyId);  // Wyszukuje technologię

            if (project == null || technology == null)  // Jeśli nie znaleziono
            {
                return NotFound();  // Zwraca błąd 404
            }

            project.Technologies.Add(new ProjectTechnology
            {
                ProjectId = projectId,
                TechnologyId = technologyId
            });  // Dodaje technologię do projektu

            await _context.SaveChangesAsync();  // Zapisuje zmiany w bazie
            return NoContent();  // Zwraca status 204
        }

        // Usuń technologię z projektu
        [HttpDelete("{projectId}/technologies/{technologyId}")]  // Usuwa technologię z projektu
        public async Task<IActionResult> RemoveTechnologyFromProject(Guid projectId, Guid technologyId)
        {
            var project = await _context.Projects
                .Include(p => p.Technologies)  // Dołącza technologie do projektu
                .FirstOrDefaultAsync(p => p.Id == projectId);  // Wyszukuje projekt

            if (project == null)  // Jeśli projekt nie istnieje
            {
                return NotFound();  // Zwraca błąd 404
            }

            var technologyToRemove = project.Technologies
                .FirstOrDefault(pt => pt.TechnologyId == technologyId);  // Wyszukuje technologię do usunięcia

            if (technologyToRemove == null)  // Jeśli technologia nie istnieje
            {
                return NotFound();  // Zwraca błąd 404
            }

            project.Technologies.Remove(technologyToRemove);  // Usuwa technologię z projektu
            await _context.SaveChangesAsync();  // Zapisuje zmiany w bazie
            return NoContent();  // Zwraca status 204
        }

        // GET: api/Projects?technology=angular
        [HttpGet]  // Pobiera projekty na podstawie technologii
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects([FromQuery] string technology)
        {
            var query = _context.Projects.AsQueryable();  // Tworzy zapytanie

            if (!string.IsNullOrEmpty(technology))  // Jeśli podano nazwę technologii
            {
                query = query
                    .Include(p => p.Technologies)  // Dołącza technologie do projektów
                    .ThenInclude(pt => pt.Technology)  // Dołącza szczegóły technologii
                    .Where(p => p.Technologies.Any(pt => pt.Technology.Name.Contains(technology)));  // Filtruje projekty
            }

            return await query.ToListAsync();  // Zwraca listę projektów
        }
    }
}
