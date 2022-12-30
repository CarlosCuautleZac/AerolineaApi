using ArepouertoMovil.Models;
using ArepouertoMovil.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArepouertoMovil.ViewModels
{
    public class VuelosViewModel : INotifyPropertyChanged
    {

        public Command EnviarVueloCommand { get; set; }

        public Vuelo Vuelo { get; set; }
        public Observacion Observacion { get; set; }
        public List<Observacion> Observaciones { get; set; }
        public string Error { get; set; } = "";
        public DateTime Fecha { get; set; } = DateTime.Now;
        public TimeSpan Hora { get; set; }

        VueloService vueloService;
        ObservacionService observacionService;

        public VuelosViewModel()
        {
            EnviarVueloCommand = new Command(EnviarVuelo);

            vueloService = new VueloService();
            vueloService.Error += VueloService_Error;
            observacionService = new ObservacionService();

            Vuelo = new Vuelo() { Fecha = DateTime.Now.Date };

            LlenarVuelos();
            LLenarObservaciones();
        }

        private void VueloService_Error(List<string> errores)
        {
            Error = "";

            foreach (var e in errores)
            {
                Error += e + Environment.NewLine;
            }
        }

        private async void EnviarVuelo()
        { 
            //Validar

             Vuelo.Fecha = Fecha.Date;
            Vuelo.Fecha = Vuelo.Fecha.Add(Hora);
            Vuelo.Observacion = Observacion.Observacion1;



            var enviado = vueloService.Insert(Vuelo).Result;

            if (enviado)
                await App.Current.MainPage.DisplayAlert("Alert", "Se ha enviado la info", "OK");


        }

        private void LLenarObservaciones()
        {
            Observaciones = new List<Observacion>();
            var obs = observacionService.Get().Result;

            foreach (var item in obs.OrderBy(x => x.Observacion1))
            {
                Observaciones.Add(item);
            }
        }

        private void LlenarVuelos()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
