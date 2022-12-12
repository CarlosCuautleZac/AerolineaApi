using System;
using System.Collections.Generic;

namespace AerolineaApi.Models
{
    public partial class Vuelo
    {
        public int Id { get; set; }
        public string Destino { get; set; } = null!;
        public string Aerolinea { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public int Puerta { get; set; }
        public int IdObservacion { get; set; }

        public virtual Observacion IdObservacionNavigation { get; set; } = null!;
    }
}
