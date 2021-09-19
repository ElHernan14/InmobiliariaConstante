using System;
using System.Collections.Generic;

namespace InmobiliariaConstante.Models
{
    public interface IRepositorioContrato: IRepositorio<Contrato>
    {
        IList<Inmueble> obtenerInmuebles(DateTime fechaDesde, DateTime fechaHasta, int idActual);
    }
}