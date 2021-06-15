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
        

        [Required(ErrorMessage = "Escriba el titulo de la tarea")]
        [Column("titulo")]
        [StringLength(100)]
        [MinLength(5, ErrorMessage = "Escriba al menos 5 caracteres")]
        [MaxLength(50, ErrorMessage = "Escriba un maximo de 50 caracteres")]
        public string Titulo { get; set; }

        

        [Required(ErrorMessage = "Escriba la descripcion de la tarea")]
        [Column("descripcion")]
        [StringLength(200)]
        [MinLength(5, ErrorMessage = "Escriba al menos 5 caracteres")]
        [MaxLength(200, ErrorMessage = "Escriba un maximo de 200 caracteres")]
        public string Descripcion { get; set; }
        [Column("idCreador")]
        public int IdCreador { get; set; }
        [Column("idAsignado")]
        public int? IdAsignado { get; set; }
        [Column("fecha", TypeName = "datetime")]
        public DateTime Fecha { get; set; }
        [Column("finalizada")]
        public bool? Finalizada { get; set; }
        [Column("bloqueada")]
        public bool Bloqueada { get; set; }

        [ForeignKey(nameof(IdAsignado))]
        [InverseProperty(nameof(Usuario.TareaIdAsignadoNavigations))]
        public virtual Usuario IdAsignadoNavigation { get; set; }
        [ForeignKey(nameof(IdCreador))]
        [InverseProperty(nameof(Usuario.TareaIdCreadorNavigations))]
        public virtual Usuario IdCreadorNavigation { get; set; }
    }
}
