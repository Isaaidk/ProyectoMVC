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
            // Obtener la lista de alumnos con sus asignaciones
            var alumnos = await _context.Alumno.Include(a => a.Asignacion).ToListAsync();

            // Obtener la lista de asignaciones
            var asignaciones = await _context.Asignacion.ToListAsync();

            // Pasar la lista de asignaciones a la vista mediante ViewBag
            ViewBag.Asignaciones = asignaciones;

            return View(alumnos);
        }


        // GET: Alumnoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alumno = await _context.Alumno
                .Include(a => a.Asignacion)
                .FirstOrDefaultAsync(m => m.IdBanner == id);
            if (alumno == null)
            {
                return NotFound();
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            if (id == null)
            {
                return NotFound();
            }

            var alumno = await _context.Alumno.FindAsync(id);
            if (alumno == null)
            {
                return NotFound();
            }
            ViewData["IdAsignacion"] = new SelectList(_context.Set<Asignacion>(), "IdAsignacion", "Nombre", alumno.IdAsignacion);
            return View(alumno);
        }

        // POST: Alumnoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdBanner,nombre,Correo,Modalidad,Asistencia,IdAsignacion")] Alumno alumno)
        {
            if (id != alumno.IdBanner)
            {
                return NotFound();
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
                        return NotFound();
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
            if (id == null)
            {
                return NotFound();
            }

            var alumno = await _context.Alumno
                .Include(a => a.Asignacion)
                .FirstOrDefaultAsync(m => m.IdBanner == id);
            if (alumno == null)
            {
                return NotFound();
            }

            return View(alumno);
        }

        // POST: Alumnoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var alumno = await _context.Alumno.FindAsync(id);
            if (alumno != null)
            {
                _context.Alumno.Remove(alumno);
            }

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
                    // Actualizamos el valor de asistencia con lo recibido del formulario
                    alumno.Asistencia = asistencia;
                    _context.Update(alumno);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



    }




}
