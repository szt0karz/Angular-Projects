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
    public class TechnologiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TechnologiesController(AppDbContext context)
        {
            _context = context; // Inicjalizacja kontekstu bazy danych
        }

        // GET: api/Technologies
        [HttpGet]  // Obsługuje żądanie GET do pobrania listy technologii
        public async Task<ActionResult<IEnumerable<Technology>>> GetTechnologies()
        {
            return await _context.Technologies.ToListAsync();  // Zwraca wszystkie technologie
        }

        // GET: api/Technologies/{id}
        [HttpGet("{id}")]  // Pobiera technologię na podstawie id
        public async Task<ActionResult<Technology>> GetTechnology(Guid id)
        {
            var technology = await _context.Technologies.FindAsync(id);  // Wyszukuje technologię po id

            if (technology == null)  // Jeśli nie znaleziono technologii
            {
                return NotFound();  // Zwraca błąd 404
            }

            return technology;  // Zwraca technologię
        }

        // POST: api/Technologies
        [HttpPost]  // Dodaje nową technologię
        public async Task<ActionResult<Technology>> PostTechnology(Technology technology)
        {
            if (!ModelState.IsValid)  // Sprawdza, czy dane są prawidłowe
            {
                return BadRequest(ModelState);  // Jeśli nie, zwraca błąd 400
            }

            _context.Technologies.Add(technology);  // Dodaje technologię do bazy danych
            await _context.SaveChangesAsync();  // Zapisuje zmiany w bazie

            return CreatedAtAction("GetTechnology", new { id = technology.Id }, technology);  // Zwraca stworzony obiekt
        }

        // PUT: api/Technologies/{id}
        [HttpPut("{id}")]  // Aktualizuje technologię
        public async Task<IActionResult> PutTechnology(Guid id, Technology technology)
        {
            if (id != technology.Id)  // Sprawdza, czy id w ścieżce jest zgodne z id technologii
            {
                return BadRequest();  // Jeśli nie, zwraca błąd 400
            }

            _context.Entry(technology).State = EntityState.Modified;  // Oznacza technologię jako zmodyfikowaną

            try
            {
                await _context.SaveChangesAsync();  // Zapisuje zmiany w bazie
            }
            catch (DbUpdateConcurrencyException)  // Obsługuje błąd konkurencji przy aktualizacji
            {
                if (!TechnologyExists(id))  // Jeśli technologia nie istnieje
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

        // DELETE: api/Technologies/{id}
        [HttpDelete("{id}")]  // Usuwa technologię
        public async Task<IActionResult> DeleteTechnology(Guid id)
        {
            var technology = await _context.Technologies.FindAsync(id);  // Wyszukuje technologię po id
            if (technology == null)  // Jeśli nie znaleziono
            {
                return NotFound();  // Zwraca błąd 404
            }

            _context.Technologies.Remove(technology);  // Usuwa technologię z bazy
            await _context.SaveChangesAsync();  // Zapisuje zmiany w bazie

            return NoContent();  // Zwraca status 204 (brak treści)
        }

        private bool TechnologyExists(Guid id)
        {
            return _context.Technologies.Any(e => e.Id == id);  // Sprawdza, czy technologia istnieje
        }
    }
}
