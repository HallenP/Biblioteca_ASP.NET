using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoF2.Models;

namespace ProyectoF2.Controllers
{
    [Authorize]
    public class PrestamoesController : Controller
    {
        private readonly BibliotecaMarcos3Context _context;

        public PrestamoesController(BibliotecaMarcos3Context context)
        {
            _context = context;
        }

        // GET: Prestamoes
        public async Task<IActionResult> Index(int buscar)
        {

            var bibliotecaMarcos3Context = _context.Prestamos.Include(p => p.IdLibroNavigation).Include(p => p.IdentificacionNavigation);
            var prestamo = from Prestamo in _context.Prestamos select Prestamo;

            if (buscar == 0)
            {
                //var bibliotecaMarcos3Context = _context.Prestamos.Include(p => p.IdLibroNavigation).Include(p => p.IdentificacionNavigation);
                return View(await bibliotecaMarcos3Context.ToListAsync());


            }else
                if (buscar!=0)

                    {
                        prestamo = bibliotecaMarcos3Context.Where(s => s.Identificacion!.Equals(buscar));
                        return View(await prestamo.ToListAsync());

                    }
                                    

            return View(await bibliotecaMarcos3Context.ToListAsync());
           
        }

        // GET: Prestamoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Prestamos == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.IdLibroNavigation)
                .Include(p => p.IdentificacionNavigation)
                .FirstOrDefaultAsync(m => m.IdPrestamo == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // GET: Prestamoes/Create
        public IActionResult Create()
        {
            ViewData["IdLibro"] = new SelectList(_context.Libros, "IdLibro", "Titulo");
            ViewData["Identificacion"] = new SelectList(_context.Usuarios, "Identificacion", "Identificacion");
            return View();
        }

        // POST: Prestamoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPrestamo,Identificacion,IdLibro,FechaDevolucion,FechaConfirmacionDevolucion,Estado,FechaCreacion")] Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prestamo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdLibro"] = new SelectList(_context.Libros, "IdLibro", "IdLibro", prestamo.IdLibro);
            ViewData["Identificacion"] = new SelectList(_context.Usuarios, "Identificacion", "Identificacion", prestamo.Identificacion);
            return View(prestamo);
        }

        // GET: Prestamoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Prestamos == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }
            ViewData["IdLibro"] = new SelectList(_context.Libros, "IdLibro", "IdLibro", prestamo.IdLibro);
            ViewData["Identificacion"] = new SelectList(_context.Usuarios, "Identificacion", "Identificacion", prestamo.Identificacion);
            return View(prestamo);
        }

        // POST: Prestamoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPrestamo,Identificacion,IdLibro,FechaDevolucion,FechaConfirmacionDevolucion,Estado,FechaCreacion")] Prestamo prestamo)
        {
            if (id != prestamo.IdPrestamo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prestamo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrestamoExists(prestamo.IdPrestamo))
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
            ViewData["IdLibro"] = new SelectList(_context.Libros, "IdLibro", "IdLibro", prestamo.IdLibro);
            ViewData["Identificacion"] = new SelectList(_context.Usuarios, "Identificacion", "Identificacion", prestamo.Identificacion);
            return View(prestamo);
        }

        // GET: Prestamoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Prestamos == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.IdLibroNavigation)
                .Include(p => p.IdentificacionNavigation)
                .FirstOrDefaultAsync(m => m.IdPrestamo == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // POST: Prestamoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Prestamos == null)
            {
                return Problem("Entity set 'BibliotecaMarcos3Context.Prestamos'  is null.");
            }
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo != null)
            {
                _context.Prestamos.Remove(prestamo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrestamoExists(int id)
        {
          return _context.Prestamos.Any(e => e.IdPrestamo == id);
        }
    }
}
