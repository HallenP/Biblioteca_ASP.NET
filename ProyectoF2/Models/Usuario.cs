using System;
using System.Collections.Generic;

namespace ProyectoF2.Models;

public partial class Usuario
{
    public int Identificacion { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Correo { get; set; }

    public string? Clave { get; set; }

    public int? Edad { get; set; }

    public int? IdRol { get; set; }

    public DateTime? FechaIngreso { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }

    public virtual ICollection<Prestamo> Prestamos { get; } = new List<Prestamo>();
}
