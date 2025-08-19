namespace MauiAppHotelReservation.Views;

public partial class HospedagemContratada : ContentPage
{
	public HospedagemContratada()
	{
		InitializeComponent();
	}

    private void btn_confirma_reserva_Clicked(System.Object sender, System.EventArgs e)
    {

		try 
		{
            // Recebe os dados do quarto selecionado
            Navigation.PopAsync();

        } 
		catch (Exception ex) 
		{

			DisplayAlert("Erro", ex.Message, "OK");
        
		}

    }
}