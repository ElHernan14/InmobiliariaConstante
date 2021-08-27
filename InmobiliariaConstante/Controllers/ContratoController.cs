using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InmobiliariaConstante.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace InmobiliariaConstante.Controllers
{
    public class ContratoController : Controller
    {
        private readonly RepositorioInquilino repoInquilino;
        private readonly RepositorioInmueble repoInmueble;
        private readonly RepositorioGarante repoGarante;
        private readonly RepositorioContrato repoContrato;
        private readonly IConfiguration config;

        public ContratoController(IConfiguration config)
        {
            this.repoInquilino = new RepositorioInquilino(config);
            this.repoInmueble = new RepositorioInmueble(config);
            this.repoContrato = new RepositorioContrato(config);
            this.repoGarante = new RepositorioGarante(config);
        }

        // GET: ContratoController
        public ActionResult Index()
        {
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
        public ActionResult Delete(int id, Contrato entidad)
        {
            try
            {
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
    }
}
