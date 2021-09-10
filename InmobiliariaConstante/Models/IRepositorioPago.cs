using System.Collections.Generic;

namespace InmobiliariaConstante.Models
{
    public interface IRepositorioPago : IRepositorio<Pago>
    {
        Pago ObtenerPorNumeroPago(int id);
        IList<Pago> ObtenerTodos(int id);
        decimal ObtenerTotal(int id);
    }
}