using ArepouertoMovil.Models;
using ArepouertoMovil.Services;
using ArepouertoMovil.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArepouertoMovil.ViewModels
{
    public class VuelosViewModel : INotifyPropertyChanged
    {

        #region Comandos
        public Command EnviarVueloCommand { get; set; }
        public Command VerDetallesVueloCommand { get; set; }
        public Command VerNuevoVueloCommand { get; set; }
        #endregion

        #region Propiedades
        public Vuelo Vuelo { get; set; }
        public Observacion Observacion { get; set; }
        public List<Observacion> Observaciones { get; set; }
        public string Error { get; set; } = "";
        public DateTime Fecha { get; set; } = DateTime.Now;
        public TimeSpan Hora { get; set; }
        public ObservableCollection<Vuelo> Vuelos { get; set; }
        #endregion

        #region Objetos
        VueloService vueloService;
        ObservacionService observacionService;
        DetallesView detallesView;
        AgregarVueloView agregarView;
        #endregion

        public VuelosViewModel()
        {
            //Comandos
            EnviarVueloCommand = new Command(EnviarVuelo);
            VerDetallesVueloCommand = new Command<Vuelo>(VerDetalleVuelo);
            VerNuevoVueloCommand = new Command(VerNuevoVuelo);

            //Services
            vueloService = new VueloService();        
            vueloService.Error += VueloService_Error;
            observacionService = new ObservacionService();

            

            //Listas
            Vuelo = new Vuelo() { Fecha = DateTime.Now.Date };
            Observacion = new Observacion();

            //Views
            detallesView = new DetallesView() { BindingContext = this };
            agregarView = new AgregarVueloView() { BindingContext = this };

            LlenarVuelos();
            LLenarObservaciones();
        }

        private async void VerNuevoVuelo()
        {
            Vuelo = new Vuelo();
            Actualizar("");
            await Application.Current.MainPage.Navigation.PushAsync(agregarView);
        }

        private async void VerDetalleVuelo(Vuelo vuelo)
        {
            try
            {
                Vuelo = vuelo;
                Hora = vuelo.Fecha.TimeOfDay;
                Actualizar("");
                await Application.Current.MainPage.Navigation.PushAsync(detallesView);
            }
            catch(Exception m)
            {
                var error = m.Message;
            }
        }

        private async void VueloService_Error(List<string> errores)
        {
            Error = "";

            foreach (var e in errores)
            {
                Error += e + Environment.NewLine;
            }
            await App.Current.MainPage.DisplayAlert("Alert", Error, "OK");
        }

        private async void EnviarVuelo()
        {
            //Validar

            Vuelo.Fecha = Fecha.Date;
            Vuelo.Fecha = Vuelo.Fecha.Add(Hora);
            Vuelo.Observacion = Observacion.Observacion1;



            var enviado = vueloService.Insert(Vuelo).Result;

            if (enviado)
            {
                LlenarVuelos();
                await App.Current.MainPage.Navigation.PopAsync();
            }


        }

        private void LLenarObservaciones()
        {
            Observaciones = new List<Observacion>(observacionService.Get().Result);
            Actualizar("");
        }

        private void LlenarVuelos()
        {
            Vuelos = new ObservableCollection<Vuelo>(vueloService.Get().Result);
            Actualizar("");
        }

        public void Actualizar(string nombre = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
