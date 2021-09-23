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
    public class ContratoController : Controller
    {
        private readonly IRepositorioInquilino repoInquilino;
        private readonly IRepositorioInmueble repoInmueble;
        private readonly IRepositorioContrato repoContrato;
        private readonly IRepositorioGarante repoGarante;
        private readonly IRepositorioPago repoPago;
        private readonly IConfiguration config;

        public ContratoController(IConfiguration config, IRepositorioPago repoPago,IRepositorioInquilino repoInquilino, IRepositorioInmueble repoInmueble, IRepositorioContrato repoContrato, IRepositorioGarante repoGarante)
        {
            this.repoInquilino = repoInquilino;
            this.repoInmueble = repoInmueble;
            this.repoContrato = repoContrato;
            this.repoGarante = repoGarante;
            this.repoPago = repoPago;
            this.config = config;
        }

        // GET: ContratoController
        public ActionResult Index(string malo)
        {
            var inquilino = TempData["Inquilino"];
            var cuotas = TempData["Cuotas"];
            if (inquilino != null)
            {
                ViewBag.Cuotas = cuotas;
                ViewBag.Inquilino = inquilino;
            }
            ViewBag.Malo = malo;
            var lista = repoContrato.ObtenerTodos();
            return View(lista);
        }

        // GET: ContratoController/Details/5
        public ActionResult Details(int id)
        {
            var entidad = repoContrato.ObtenerPorId(id);
            return View(entidad);
        }

        // GET: ContratoController/Create
        public ActionResult Create()
        {
            ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
            ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
            ViewBag.Garantes = repoGarante.ObtenerTodos();
            return View();
        }

        // POST: ContratoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    entidad.Estado = true;
                    repoContrato.Alta(entidad);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
                    ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
                    ViewBag.Garantes = repoGarante.ObtenerTodos();
                    return View(entidad);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
                ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
                ViewBag.Garantes = repoGarante.ObtenerTodos();
                return View(entidad);
            }
        }

        // GET: ContratoController/Edit/5
        public ActionResult Edit(int id)
        {
            var entidad = repoContrato.ObtenerPorId(id);
            ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
            ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
            ViewBag.Garantes = repoGarante.ObtenerTodos();
            return View(entidad);
        }

        // POST: ContratoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato entidad)
        {
            try
            {
                entidad.Id = id;
                repoContrato.Modificacion(entidad);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
                ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
                ViewBag.Garantes = repoGarante.ObtenerTodos();
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(entidad);
            }
        }

        // GET: ContratoController/Delete/5
        public ActionResult Delete(int id)
        {
            var entidad = repoContrato.ObtenerPorId(id);
            return View(entidad);
        }

        // POST: ContratoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Contrato entidad) {       
            try
            {
                var contrato = repoContrato.ObtenerPorId(id);
                DateTime fechaDesde = contrato.FechaDesde;
                DateTime fechaHasta = contrato.FechaHasta;
                var hoy = DateTime.Now;
                var diferenciaHoras = (fechaHasta - fechaDesde).TotalHours;
                var mitad = diferenciaHoras / 2;
                var diferenciaToDay = (hoy - fechaDesde).TotalHours;
                if (mitad > diferenciaToDay)
                {
                    TempData["Cuotas"] = "Contrato finalizado, debe 2 Meses de alquiler";
                    TempData["Inquilino"] = contrato.inquilino.Nombre + " " + contrato.inquilino.Apellido;
                }
                else
                {
                    TempData["Cuotas"] = "Contrato finalizado, debe 1 mes de alquiler";
                    TempData["Inquilino"] = contrato.inquilino.Nombre + " " + contrato.inquilino.Apellido;
                }
                contrato.FechaHasta = hoy;
                repoContrato.Modificacion(contrato);
                repoContrato.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(entidad);
            }
        }

        //POST: Inmuebles/Buscar/5
        [Route("[controller]/Buscar/{fechaDesde}/{FechaHasta}/{idActual}", Name = "Buscar")]
        public IActionResult Buscar(DateTime fechaDesde, DateTime fechaHasta, int idActual)
        {
            try
            {

                    var res = repoContrato.obtenerInmuebles(fechaDesde,fechaHasta,idActual);
                return Json(new { Datos = res });
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }
        }
    }
}
