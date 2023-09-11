using System;
using System.Collections.Generic;

namespace ProyectoF2.Models;

public partial class Libro
{
    public int IdLibro { get; set; }

    public string? Titulo { get; set; }

    public string? NombrePortada { get; set; }

    public string? Autor { get; set; }

    public string? GeneroLiterario { get; set; }

    public string? Editorial { get; set; }

    public string? Ubicacion { get; set; }

    public int? Ejemplares { get; set; }

    public virtual ICollection<Prestamo> Prestamos { get; } = new List<Prestamo>();
}
