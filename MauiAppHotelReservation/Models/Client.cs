using MauiAppHotelReservation.Services;
using System.Collections.ObjectModel;

namespace MauiAppHotelReservation.Models
{
    public class Client
    {
        public DadosUsuario Usuario { get; set; }
        public ObservableCollection<Hospedagem> Reservas { get; set; }

        public Client(DadosUsuario usuario)
        {
            Usuario = usuario;
            Reservas = new ObservableCollection<Hospedagem>();
        }

    }
}
