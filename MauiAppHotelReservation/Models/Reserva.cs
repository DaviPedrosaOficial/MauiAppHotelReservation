using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppHotelReservation.Models
{
    public class Reserva
    {
        public Quarto QuartoSelecionado { get; set; }
        public int QuantidadeAdultos { get; set; }
        public int QuantidadeCriancas { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime DataSaida { get; set; }
        public double ValorTotal { get; set; }
        public string NomeHospede { get; set; }
    }
}
