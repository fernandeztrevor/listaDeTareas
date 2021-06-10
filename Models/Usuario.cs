using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ListaDeTareas.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            TareaIdAsignadoNavigations = new HashSet<Tarea>();
            TareaIdCreadorNavigations = new HashSet<Tarea>();
        }

        [Key]
        [Column("idUsuario")]
        public int IdUsuario { get; set; }
        
        [Required(ErrorMessage = "Escriba el email del usuario")]
        [Column("email")]
        [StringLength(100)]
        [MinLength(5, ErrorMessage = "Escriba al menos 5 caracteres")]
        [MaxLength(50, ErrorMessage = "Escriba un maximo de 50 caracteres")]
        public string Email { get; set; }
        [Required]
        [Column("clave")]
        [StringLength(500)]
        public string Clave { get; set; }
        
        
        [Column("llave")]
        [StringLength(500)]
        public string Llave { get; set; }
        [Column("idRol")]
        public int? IdRol { get; set; }

        [ForeignKey(nameof(IdRol))]
        [InverseProperty(nameof(Role.Usuarios))]
        public virtual Role IdRolNavigation { get; set; }
        [InverseProperty(nameof(Tarea.IdAsignadoNavigation))]
        public virtual ICollection<Tarea> TareaIdAsignadoNavigations { get; set; }
        [InverseProperty(nameof(Tarea.IdCreadorNavigation))]
        public virtual ICollection<Tarea> TareaIdCreadorNavigations { get; set; }
    }
}
