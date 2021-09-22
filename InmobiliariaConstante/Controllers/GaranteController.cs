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
    public class GaranteController : Controller
    {
        private readonly IRepositorioGarante repositorio;
        private readonly IConfiguration config;

        public GaranteController(IConfiguration config, IRepositorioGarante repositorio)
        {
            this.repositorio = repositorio;
            this.config = config;
        }

        // GET: Inquilino
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: Inquilino/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {//poner breakpoints para detectar errores
                throw;
            }
        }

        // GET: Inquilino/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inquilino/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Garante propietario)
        {
            try
            {
                TempData["Nombre"] = propietario.Nombre;
                repositorio.Alta(propietario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Inquilino/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        // POST: Inquilino/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Garante i)
        {
            Garante garante = null;
            try
            {
                garante = repositorio.ObtenerPorId(id);
                garante.Nombre = i.Nombre;
                garante.Apellido = i.Apellido;
                garante.Dni = i.Dni;
                garante.Email = i.Email;
                garante.Telefono = i.Telefono;
                repositorio.Modificacion(garante);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Inquilino/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            Garante garante = repositorio.ObtenerPorId(id);
            return View(garante);
        }

        // POST: Inquilino/Delete/5
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                repositorio.Baja(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}
