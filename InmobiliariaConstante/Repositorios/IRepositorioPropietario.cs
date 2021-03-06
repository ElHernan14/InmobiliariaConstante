using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InmobiliariaConstante.Models;

namespace InmobiliariaConstante.Models
{
    public interface IRepositorioPropietario : IRepositorio<Propietario>
	{
		Propietario ObtenerPorEmail(string email);
        IList<Propietario> BuscarPorNombre(string nombre);
        IList<Inmueble> ObtenerInmueblesXPropietario(int id);
    }
}
