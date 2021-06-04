using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ListaDeTareas.Models
{
    public partial class Tarea
    {
        [Key]
        [Column("idTarea")]
        public int IdTarea { get; set; }
        [Required]
        [Column("titulo")]
        [StringLength(100)]
        public string Titulo { get; set; }
        [Required]
        [Column("descripcion")]
        [StringLength(200)]
        public string Descripcion { get; set; }
        [Column("idCreador")]
        public int? IdCreador { get; set; }
        [Column("idAsignado")]
        public int? IdAsignado { get; set; }
        [Column("fecha", TypeName = "datetime")]
        public DateTime Fecha { get; set; }
        [Column("finalizada")]
        public bool? Finalizada { get; set; }
        [Column("bloqueada")]
        public bool? Bloqueada { get; set; }

        [ForeignKey(nameof(IdAsignado))]
        [InverseProperty(nameof(Usuario.TareaIdAsignadoNavigations))]
        public virtual Usuario IdAsignadoNavigation { get; set; }
        [ForeignKey(nameof(IdCreador))]
        [InverseProperty(nameof(Usuario.TareaIdCreadorNavigations))]
        public virtual Usuario IdCreadorNavigation { get; set; }
    }
}
