using MauiAppHotelReservation.Models;
using System.Collections.ObjectModel;

namespace MauiAppHotelReservation.Views.Hotel;

public partial class ReservasHospedagem : ContentPage
{
    private ObservableCollection<Hospedagem> reservas_do_cliente;
    // Construtor da classe ReservasHospedagem

    public ReservasHospedagem()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        try
        {
            base.OnAppearing();

            var app = (App)Application.Current;
            var usuarioLogado = await SecureStorage.GetAsync("usuario_logado");
            var cliente = app.lista_clientes.FirstOrDefault(c => c.Usuario.Usuario == usuarioLogado);

            // Garante uma coleção não nula
            reservas_do_cliente = cliente?.Reservas ?? new ObservableCollection<Hospedagem>();

            // Conecta a lista do cliente à CollectionView
            cvReservas.ItemsSource = reservas_do_cliente;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro ao carregar as reservas: {ex.Message}", "OK");
        }
    }

    private void btn_cancela_reserva_Clicked(System.Object sender, System.EventArgs e)
    {
        // Remove a reserva da coleção vinculada à CollectionView
        if (sender is Button btn && btn.CommandParameter is Hospedagem reserva)
        {
            reservas_do_cliente.Remove(reserva);
        }
    }

    private async void btn_alterar_reserva_Clicked(System.Object sender, System.EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Hospedagem reserva)
        {
            // Aqui você pode navegar para uma tela de edição
            // ou abrir um popup para alterar datas/pessoas.
            await DisplayAlert("Alterar", "Abrir tela de alteração desta reserva.", "OK");
        }
    }

    private void btn_retorna_menu_Clicked(System.Object sender, System.EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }
}