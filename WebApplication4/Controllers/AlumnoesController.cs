using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Data;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class AlumnoesController : Controller
    {
        private readonly WebApplication4Context _context;

        public AlumnoesController(WebApplication4Context context)
        {
            _context = context;
        }

        // GET: Alumnoes
        public async Task<IActionResult> Index()
        {
            var alumnos = await _context.Alumno.Include(a => a.Asignacion).ToListAsync();
            var asignaciones = await _context.Asignacion.ToListAsync();

            ViewBag.Asignaciones = asignaciones;
            return View(alumnos);
        }

        // GET: Alumnoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("El ID del alumno no puede ser nulo o vacío.");
            }

            var alumno = await _context.Alumno
                .Include(a => a.Asignacion)
                .FirstOrDefaultAsync(m => m.IdBanner == id);

            if (alumno == null)
            {
                return NotFound("No se encontró el alumno con el ID proporcionado.");
            }

            return View(alumno);
        }

        // GET: Alumnoes/Create
        public IActionResult Create()
        {
            ViewData["IdAsignacion"] = new SelectList(_context.Set<Asignacion>(), "IdAsignacion", "Nombre");
            return View();
        }

        // POST: Alumnoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdBanner,nombre,Correo,Modalidad,Asistencia,IdAsignacion")] Alumno alumno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(alumno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdAsignacion"] = new SelectList(_context.Set<Asignacion>(), "IdAsignacion", "Nombre", alumno.IdAsignacion);
            return View(alumno);
        }

        // GET: Alumnoes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("El ID del alumno no puede ser nulo o vacío.");
            }

            var alumno = await _context.Alumno.FindAsync(id);
            if (alumno == null)
            {
                return NotFound("No se encontró el alumno con el ID proporcionado.");
            }

            ViewData["IdAsignacion"] = new SelectList(_context.Set<Asignacion>(), "IdAsignacion", "Nombre", alumno.IdAsignacion);
            return View(alumno);
        }

        // POST: Alumnoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdBanner,nombre,Correo,Modalidad,Asistencia,IdAsignacion")] Alumno alumno)
        {
            if (id != alumno.IdBanner)
            {
                return BadRequest("El ID del alumno no coincide.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alumno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlumnoExists(alumno.IdBanner))
                    {
                        return NotFound("No se encontró el alumno con el ID proporcionado.");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdAsignacion"] = new SelectList(_context.Set<Asignacion>(), "IdAsignacion", "Nombre", alumno.IdAsignacion);
            return View(alumno);
        }

        // GET: Alumnoes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("El ID del alumno no puede ser nulo o vacío.");
            }

            var alumno = await _context.Alumno
                .Include(a => a.Asignacion)
                .FirstOrDefaultAsync(m => m.IdBanner == id);

            if (alumno == null)
            {
                return NotFound("No se encontró el alumno con el ID proporcionado.");
            }

            return View(alumno);
        }
        // POST: Alumnoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("El ID del alumno no puede ser nulo o vacío.");
            }

            var alumno = await _context.Alumno.FindAsync(id);
            if (alumno == null)
            {
                return NotFound("No se encontró el alumno con el ID proporcionado.");
            }

            _context.Alumno.Remove(alumno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlumnoExists(string id)
        {
            return _context.Alumno.Any(e => e.IdBanner == id);
        }

        public async Task<IActionResult> FiltrarPorAsignacion(int id)
        {
            var alumnosFiltrados = await _context.Alumno
                .Where(a => a.IdAsignacion == id)
                .Include(a => a.Asignacion)
                .ToListAsync();

            return View("Index", alumnosFiltrados);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarCambios(Dictionary<string, bool> asistencias)
        {
            if (asistencias == null || !asistencias.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            var alumnos = await _context.Alumno.ToListAsync();

            foreach (var alumno in alumnos)
            {
                if (asistencias.TryGetValue(alumno.IdBanner, out bool asistencia))
                {
                    alumno.Asistencia = asistencia;
                    _context.Update(alumno);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
