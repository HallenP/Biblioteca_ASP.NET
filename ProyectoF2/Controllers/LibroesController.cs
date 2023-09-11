using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using ProyectoF2.Models;

namespace ProyectoF2.Controllers
{
    //[Authorize]
    public class LibroesController : Controller
    {
        private readonly BibliotecaMarcos3Context _context;
        private readonly IConverter _converter;

        public LibroesController(BibliotecaMarcos3Context context, IConverter converter)
        {
            _context = context;
            _converter = converter;
        }

        // GET: Libroes
        [Authorize(Roles ="Administrador,Lector")]
        public async Task<IActionResult> Index(string buscar)
        {

            var libros = from libro in _context.Libros select libro;

            if (!String.IsNullOrEmpty(buscar))
            {
                libros = libros.Where(s => s.Titulo!.Contains(buscar));
            }

            return View(await libros.ToListAsync());
            
        }
        public async Task<IActionResult> Index2(string buscar)
        {
            var libros = from libro in _context.Libros select libro;

            if (!String.IsNullOrEmpty(buscar))
            {
                libros = libros.Where(s => s.Titulo!.Contains(buscar));
            }

            return View(await libros.ToListAsync());

        }

        // GET: Libroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .FirstOrDefaultAsync(m => m.IdLibro == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libroes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Libroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLibro,Titulo,NombrePortada,Autor,GeneroLiterario,Editorial,Ubicacion,Ejemplares")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        // GET: Libroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            return View(libro);
        }

        // POST: Libroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdLibro,Titulo,NombrePortada,Autor,GeneroLiterario,Editorial,Ubicacion,Ejemplares")] Libro libro)
        {
            if (id != libro.IdLibro)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.IdLibro))
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
            return View(libro);
        }

        // GET: Libroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .FirstOrDefaultAsync(m => m.IdLibro == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Libros == null)
            {
                return Problem("Entity set 'BibliotecaMarcos3Context.Libros'  is null.");
            }
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibroExists(int id)
        {
          return _context.Libros.Any(e => e.IdLibro == id);
        }

        

        //public LibroesController(IConverter converter)
        //{
        //    _converter = converter;
        //}

        private async Task<string> RenderViewToStringAsync(string viewName, object model)
        {
            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                var viewEngine = HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

                var viewResult = viewEngine.FindView(ControllerContext, viewName, false);

                if (viewResult.Success == false)
                {
                    throw new Exception($"La vista '{viewName}' no pudo ser encontrada.");
                }

                var view = viewResult.View;

                var viewContext = new ViewContext(
                    ControllerContext,
                    view,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await view.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }

        public async Task<IActionResult> DescargarDetalle(int? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .FirstOrDefaultAsync(m => m.IdLibro == id);

            if (libro == null)
            {
                return NotFound();
            }
            

            // Renderizar la vista de detalles en HTML
            string htmlContent = await RenderViewToStringAsync("Details", libro);

            // Convertir el HTML a PDF
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = {
            PaperSize = PaperKind.A4,
            Orientation = Orientation.Portrait
        },
                Objects = {
            new ObjectSettings {
                PagesCount = true,
                HtmlContent = htmlContent,
            }
        }
            };

            var pdfBytes = _converter.Convert(pdf);

            // Devolver el archivo PDF para descarga
            return File(pdfBytes, "application/pdf", $"detalle_libro_{id}.pdf");
        }

        


    }
}
