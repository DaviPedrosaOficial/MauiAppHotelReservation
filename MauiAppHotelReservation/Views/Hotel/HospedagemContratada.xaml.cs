using MauiAppHotelReservation.Models;

namespace MauiAppHotelReservation.Views.Hotel;

public partial class HospedagemContratada : ContentPage
{
	public HospedagemContratada()
	{
		InitializeComponent();
	}

    private void btn_retorna_menu_Clicked(System.Object sender, System.EventArgs e)
    {

		try 
		{
            // Retornando ao menu principal após a confirmação da reserva
            Navigation.PushAsync(new MainPage());

        } 
		catch (Exception ex) 
		{

			DisplayAlert("Erro", ex.Message, "OK");
        
		}

    }
}