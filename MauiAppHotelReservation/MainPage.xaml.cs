namespace MauiAppHotelReservation
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void btn_reservas_Clicked(System.Object sender, System.EventArgs e)
        {
            var nomeCliente = await SecureStorage.GetAsync("usuario_logado");

            await Navigation.PushAsync(new Views.Hotel.ReservasHospedagem
            {
                BindingContext = nomeCliente
            });

            
        }

        private void btn_reservar_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new Views.Hotel.ContratacaoHospedagem());
        }

        private void btn_deslogar_Clicked(System.Object sender, System.EventArgs e)
        {
            // Limpa o armazenamento seguro do usuário logado
            SecureStorage.Remove("usuario_logado");
            
            // Redireciona para a página de login
            App.Current.MainPage = new NavigationPage(new Views.Login.Login());
        }
    }
}
