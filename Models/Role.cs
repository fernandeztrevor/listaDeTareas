using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ListaDeTareas.Models
{
    public partial class Role
    {
        public Role()
        {
            Usuarios = new HashSet<Usuario>();
        }

        [Key]
        [Column("idRol")]
        public int IdRol { get; set; }
        [Required]
        [Column("nombre")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [InverseProperty(nameof(Usuario.IdRolNavigation))]
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
