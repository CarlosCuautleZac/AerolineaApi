using SalidasAerolinea.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SalidasAerolinea.Services
{
    public class VueloService
    {
        HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://aerolinea.sistemas19.com/api/vuelo")
        };

        public Task<List<Vuelo>>
        
    }
}
