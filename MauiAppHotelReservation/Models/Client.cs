using MauiAppHotelReservation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppHotelReservation.Models
{
    public class Client
    {
        public DadosUsuario Usuario { get; set; }
        public List<Hospedagem>? Reservas { get; set; }

        public Client(DadosUsuario usuario)
        {
            Usuario = usuario;
            Reservas = new List<Hospedagem>();
        }

    }
}
