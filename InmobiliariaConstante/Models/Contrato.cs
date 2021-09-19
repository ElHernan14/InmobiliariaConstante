using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaConstante.Models
{
    public class Contrato
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Inquilino")]
        public int IdInquilino { get; set; }
        [ForeignKey(nameof(IdInquilino))]
        public Inquilino inquilino { get; set; }
        [Required]
        [Display(Name = "Inmueble")]
        public int IdInmueble { get; set; }
        [ForeignKey(nameof(IdInmueble))]
        public Inmueble inmueble { get; set; }
        [Required]
        [Display(Name = "Garante")]
        public int IdGarante { get; set; }
        [ForeignKey(nameof(IdGarante))]
        public Garante garante { get; set; }
        [Required]
        public DateTime FechaDesde { get; set; }
        [Required]
        public DateTime FechaHasta { get; set; }
        [Display(Name = "Cuotas Contrato")]
        [Required]
        public int Cuotas { get; set; }
        public bool Estado { get; set; }
    }
}
