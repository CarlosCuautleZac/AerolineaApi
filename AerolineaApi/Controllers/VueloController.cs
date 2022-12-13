using AerolineaApi.DTOs;
using AerolineaApi.Models;
using AerolineaApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AerolineaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VueloController : ControllerBase
    {
        private readonly sistem21_aerolineaContext context;
        Repository<Vuelo> repository;

        public VueloController(sistem21_aerolineaContext context)
        {
            this.context = context;
            repository = new(context);
        }

        [HttpPost]
        public IActionResult Post(VueloDTO vuelo)
        {
        //     public int Id { get; set; }
        //public string Destino { get; set; } = null!;
        //public string Aerolinea { get; set; } = null!;
        //public DateTime Fecha { get; set; }
        //public int Puerta { get; set; }
        //public int IdObservacion { get; set; }

           if()
        }
    }
}
