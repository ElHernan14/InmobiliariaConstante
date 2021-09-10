using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InmobiliariaConstante.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace InmobiliariaConstante.Controllers
{
    [Authorize]
    public class PagoController : Controller
    {
        private readonly IRepositorioContrato repoContrato;
        private readonly IRepositorioPago repoPago;
        private readonly IConfiguration config;
        public PagoController(IConfiguration config, IRepositorioContrato repoContrato, IRepositorioPago repoPago)
        {
            this.repoContrato = repoContrato;
            this.repoPago = repoPago;
            this.config = config;
        }

        // GET: PagoController
        public ActionResult Index(int id)
        {
            var entidad = repoPago.ObtenerTodos(id);
            var total = repoPago.ObtenerTotal(id);
            var contrato = repoContrato.ObtenerPorId(id);
            var valorInmueble = contrato.inmueble.Precio;
            var cuotas = contrato.Cuotas;
            if(total > 0)
            {
                if (total >= valorInmueble)
                {
                    ViewBag.Mensaje = "Este Contrato ha finalizado";
                }
            }
            ViewBag.Id = id;
            ViewBag.ValorInmueble = valorInmueble;
            ViewBag.Cuotas = cuotas;
            
            return View(entidad);
        }

        public ActionResult Delete(int id)
        {
            var entidad = repoPago.ObtenerPorId(id);
            return View(entidad);
        }

        // POST: UsuariosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                repoPago.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PagoController/Details/5
        public ActionResult Details(int id)
        {
            var entidad = repoPago.ObtenerPorId(id);
            return View(entidad);
        }

        // GET: PagoController/Create
        public ActionResult Create(int id, decimal valorInmueble, int cuotas)
        {
            var total = repoPago.ObtenerTotal(id);
            var valor = valorInmueble / cuotas;
            var diferencia = valorInmueble - total;
            if(diferencia < valor)
            {
                ViewBag.MontoSugerido = diferencia;
            }
            else
            {
                ViewBag.MontoSugerido = valor;
            }
                ViewBag.MontoRestante = diferencia;
            ViewBag.Id = id;
            return View();
        }

        // POST: PagoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago pago)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int id = pago.IdContrato;
                    var contrato = repoContrato.ObtenerPorId(id);
                    var entidad = repoPago.ObtenerTotal(id);
                    if (entidad > 0)
                    {
                        var diferencia = contrato.inmueble.Precio - entidad;
                        if (pago.Monto > diferencia)
                        {
                            ModelState.AddModelError("", "El valor supera al monto restante de pago");
                            return View();
                        }
                    }
                    Random r = new Random();
                    var hoy = DateTime.Now;
                    var numero = 0;
                    do
                    {
                        numero = r.Next(1, 1000);
                    } while (repoPago.ObtenerPorNumeroPago(numero) != null);
                    pago.FechaDePago = hoy;
                    pago.NumeroDePago = numero;
                    repoPago.Alta(pago);
                    return RedirectToAction("index", new { id });
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View(pago);
            }
        }

        public ActionResult Edit(int id)
        {
            var entidad = repoContrato.ObtenerPorId(id);
            var total = repoPago.ObtenerTotal(id);
            var valorInmueble = entidad.inmueble.Precio;
            var cuotas = entidad.Cuotas;
            var valor = valorInmueble / cuotas;
            var diferencia = valorInmueble - total;
            if (diferencia < valor)
            {
                ViewBag.MontoSugerido = diferencia;
            }
            else
            {
                ViewBag.MontoSugerido = valor;
            }
            ViewBag.MontoRestante = diferencia;
            ViewBag.Id = id;
            return View(entidad);
        }

        // POST: ContratoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Pago pago)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int id = pago.IdContrato;
                    var contrato = repoContrato.ObtenerPorId(id);
                    var entidad = repoPago.ObtenerTotal(id);
                    var pagoViejo = repoPago.ObtenerPorId(pago.Id);
                    if (entidad > 0)
                    {
                        var diferencia = contrato.inmueble.Precio - entidad;
                        if (pago.Monto > diferencia)
                        {
                            ModelState.AddModelError("", "El valor supera al monto restante de pago");
                            return View();
                        }
                    }
                    pago.NumeroDePago = pagoViejo.NumeroDePago;
                    pago.FechaDePago = pagoViejo.FechaDePago;
                    repoPago.Modificacion(pago);
                    return RedirectToAction("index", new { id });
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View(pago);
            }
        }
    }
}
