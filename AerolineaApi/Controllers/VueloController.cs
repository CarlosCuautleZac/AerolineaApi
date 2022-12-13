using AerolineaApi.DTOs;
using AerolineaApi.Models;
using AerolineaApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AerolineaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VueloController : ControllerBase
    {
        private readonly sistem21_aerolineaContext context;
        Repository<Vuelo> repository;
        Repository<Observacion> repositoryobservacion;

        public VueloController(sistem21_aerolineaContext context)
        {
            this.context = context;
            repository = new(context);
            repositoryobservacion = new(context);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var vuelos = repository.Get().Include(x => x.IdobservacionNavigation).Select(x => new VueloDTO
            {
                Id = x.Id,
                Aerolinea = x.Aerolinea,
                Destino = x.Destino,
                Fecha = x.Fecha,
                Observacion = x.IdobservacionNavigation.Observacion1,
                Puerta = x.Puerta
            }).OrderBy(x => x.Fecha).Where(x => x.Fecha > DateTime.Now);

            return Ok(vuelos);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var vuelos = repository.Get().Include(x => x.IdobservacionNavigation).Where(x => x.Id == id).Select(x => new VueloDTO
            {
                Id=x.Id,
                Aerolinea = x.Aerolinea,
                Destino = x.Destino,
                Fecha = x.Fecha,
                Observacion = x.IdobservacionNavigation.Observacion1,
                Puerta = x.Puerta
            }).OrderBy(x => x.Fecha).FirstOrDefault();

            if (vuelos == null)
                return NotFound("No se encontro el vuelo");
            else
                return Ok(vuelos);
        }

        [HttpPost]
        public IActionResult Post(VueloDTO vuelo)
        {
            if (vuelo == null)
                return BadRequest("Debe proporcionar un vuelo");

            if (Validate(vuelo, out List<string> errores))
            {
                Vuelo v = new()
                {
                    Aerolinea = vuelo.Aerolinea.Trim().ToUpper(),
                    Destino = vuelo.Destino.Trim().ToUpper(),
                    Fecha = vuelo.Fecha,
                    Idobservacion = repositoryobservacion.Get().Where(x=>x.Observacion1==vuelo.Observacion)
                    .Select(x=>x.Id).FirstOrDefault(),
                    Puerta = vuelo.Puerta
                };

                repository.Insert(v);
                return Ok();
            }
            else
                return BadRequest(errores);

        }

        [HttpPut]
        public IActionResult Put(VueloDTO vuelo)
        {

            if (Validate(vuelo, out List<string> errores))
            {
                Vuelo v = new()
                {
                    Aerolinea = vuelo.Aerolinea.Trim().ToUpper(),
                    Destino = vuelo.Destino.Trim().ToUpper(),
                    Fecha = vuelo.Fecha,
                    Idobservacion = repositoryobservacion.Get().Where(x => x.Observacion1 == vuelo.Observacion)
                    .Select(x => x.Id).FirstOrDefault(),
                    Puerta = vuelo.Puerta
                };

                repository.Insert(v);
                return Ok();
            }
            else
                return BadRequest(errores);
        }


        private bool Validate(VueloDTO vuelo, out List<string> errors)
        {
            errors = new();

            if (string.IsNullOrWhiteSpace(vuelo.Destino))
                errors.Add("Ingrese un destino e intente hacer otra solicitud");

            if (string.IsNullOrWhiteSpace(vuelo.Aerolinea))
                errors.Add("Debe ingresar una aerolinea.");

            if (vuelo.Fecha < DateTime.Now)
                errors.Add("Fecha invalida. Debe escribir una fecha correcta para contiuar");

            if (vuelo.Puerta < 1 || vuelo.Puerta > 20)
                errors.Add("Escriba una puerta entre "); 

            return errors.Count == 0;

        }
    }
}
