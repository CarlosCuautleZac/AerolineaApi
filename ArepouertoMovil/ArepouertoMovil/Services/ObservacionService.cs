using ArepouertoMovil.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ArepouertoMovil.Services
{
    public class ObservacionService
    {
        HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://c5ae-2806-108e-26-cf91-d191-d48d-36ea-8b92.ngrok.io")
        };

        public async Task<List<Observacion>> Get()
        {
            List<Observacion> observaciones = new List<Observacion>();
            var response =  client.GetAsync("/api/observacion");
            response.Wait();
            if (response.Result.IsSuccessStatusCode)
            {
                var json = await response.Result.Content.ReadAsStringAsync();
                observaciones = JsonConvert.DeserializeObject<List<Observacion>>(json);
            }

            if (observaciones == null)
                return new List<Observacion>();
            else
                return observaciones;
        } 
    }
}
