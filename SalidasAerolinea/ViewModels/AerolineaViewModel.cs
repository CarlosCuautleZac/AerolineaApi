using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalidasAerolinea.ViewModels
{
    public class AerolineaViewModel : INotifyPropertyChanged
    {
        


        public void Actualizar(string nombre= "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
