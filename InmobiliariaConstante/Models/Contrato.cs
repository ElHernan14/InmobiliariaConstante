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

        [Display(Name = "Inquilino")]
        public int IdInquilino { get; set; }
        [ForeignKey(nameof(IdInquilino))]
        public Inquilino inquilino { get; set; }

        [Display(Name = "Inmueble")]
        public int IdInmueble { get; set; }
        [ForeignKey(nameof(IdInmueble))]
        public Inmueble inmueble { get; set; }

        [Display(Name = "Garante")]
        public int IdGarante { get; set; }
        [ForeignKey(nameof(IdGarante))]
        public Garante garante { get; set; }

        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
    }
}
