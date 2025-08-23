namespace MauiAppHotelReservation.Views.Hotel;

public partial class AlteraReserva : ContentPage
{
	public AlteraReserva(object reserva)
	{
		InitializeComponent();



		BindingContext = new ViewModels.AlteraReservaViewModel(reserva);
    }
}