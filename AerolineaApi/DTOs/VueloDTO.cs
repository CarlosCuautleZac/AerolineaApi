namespace AerolineaApi.DTOs
{
    public class VueloDTO
    {
        public string Destino { get; set; } = null!;
        public string Aerolinea { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public int Puerta { get; set; }
        public int IdObservacion { get; set; }
    }
}
