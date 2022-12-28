﻿using ArepouertoMovil.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ArepouertoMovil.Services
{
    public class VueloService
    {
        HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://aerolinea.sistemas19.com/api/vuelo")
        };


        public event Action<List<string>> Error;

        public async Task<List<Vuelo>> Get()
        {
            List<Vuelo> vuelos = new List<Vuelo>();
            var response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                vuelos = JsonConvert.DeserializeObject<List<Vuelo>>(json);
            }

            if (vuelos == null)
                return new List<Vuelo>();
            else
                return vuelos;

        }

        public async Task<bool> Insert(Vuelo vuelo)
        {

            if (Validate(vuelo, out List<string> erroreslocales))
            {
                var json = JsonConvert.SerializeObject(vuelo);
                var response = client.PostAsync("", new StringContent(json, Encoding.UTF8,
                    "application/json"));
                response.Wait();
                if (response.Result.StatusCode == HttpStatusCode.BadRequest) //BadRequest
                {
                    var errores = await response.Result.Content.ReadAsStringAsync();
                    LanzarErrorJson(errores);
                    return false;
                }
                return true;
            }
            else
            {
                Error.Invoke(erroreslocales);
                return false;
            }
        }

        public async Task<bool> Udate(Vuelo vuelo)
        {

            if (Validate(vuelo, out List<string> erroreslocales))
            {
                var json = JsonConvert.SerializeObject(vuelo);
                var response = await client.PutAsync("", new StringContent(json, Encoding.UTF8,
                    "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) //BadRequest
                {
                    var errores = await response.Content.ReadAsStringAsync();
                    LanzarErrorJson(errores);
                    return false;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    erroreslocales.Add("No se encontro el vuelo");
                    Error?.Invoke(erroreslocales);
                    return false;
                }
                return true;
            }
            else
            {
                Error.Invoke(erroreslocales);
                return false;
            }
        }

        public async Task<bool> Delete(Vuelo vuelo)
        {
            List<string> erroreslocales = new List<string>();

            if (vuelo != null)
            {
                var response = await client.DeleteAsync("" + vuelo.Id);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) //BadRequest
                {
                    var errores = await response.Content.ReadAsStringAsync();
                    LanzarErrorJson(errores);
                    return false;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    erroreslocales.Add("No se encontro el vuelo");
                    Error?.Invoke(erroreslocales);
                    return false;

                }
                return true;
            }
            else
                return false;
            
        }

        void LanzarErrorJson(string json)
        {
            List<string> errores = JsonConvert.DeserializeObject<List<string>>(json);
            if (errores != null)
            {
                Error?.Invoke(errores);
            }
        }

        //Metodo para validar
        private bool Validate(Vuelo vuelo, out List<string> errors)
        {
            errors = new List<string>();

            if (vuelo == null)
                errors.Add("Ingrese un vuelo e intente hacer otra solicitud");

            if (string.IsNullOrWhiteSpace(vuelo.Destino))
                errors.Add("Ingrese un destino e intente hacer otra solicitud");

            if (string.IsNullOrWhiteSpace(vuelo.Aerolinea))
                errors.Add("Debe ingresar una aerolinea.");

            if (vuelo.Fecha < DateTime.Now)
                errors.Add("Fecha invalida. Debe escribir una fecha correcta para contiuar");

            return errors.Count == 0;

        }

    }
}
