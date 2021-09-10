using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaConstante.Models
{
    public class Pago
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Display(Name = "Numero de Pago")]
        public decimal NumeroDePago { get; set; }
        [Display(Name = "Fecha De Pago")]
        public DateTime FechaDePago { get; set; }
        public decimal Monto { get; set; }
        [Display(Name = "Contrato")]
        public int IdContrato { get; set; }
        [ForeignKey(nameof(IdContrato))]
        public Contrato contrato { get; set; }
    }
}
