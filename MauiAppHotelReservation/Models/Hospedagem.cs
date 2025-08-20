namespace MauiAppHotelReservation.Models
{
    public class Hospedagem
    {
        public Quarto QuartoSelecionado { get; set; }
        public int QuantidadeAdultos { get; set; }
        public int QuantidadeCriancas { get; set; }
        public DateTime DataCheckIn { get; set; }
        public DateTime DataCheckOut { get; set; }
        public int Estadia 
        {
            get => DataCheckOut.Subtract(DataCheckIn).Days;
        }
        public double ValorTotal
        {
            get
            {
                double valorAdultos = QuartoSelecionado.ValorDiariaAdulto * QuantidadeAdultos * Estadia;
                double valorCriancas = QuartoSelecionado.ValorDiariaCriança * QuantidadeCriancas * Estadia;
                return valorAdultos + valorCriancas;
            }
        }
    }
}
