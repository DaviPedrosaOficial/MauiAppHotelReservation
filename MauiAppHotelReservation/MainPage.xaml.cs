namespace MauiAppHotelReservation
{
    public partial class MainPage : ContentPage
    {
        App PropriedadesDoApp;
        public MainPage()
        {
            InitializeComponent();
            PropriedadesDoApp = (App)Application.Current;
        }

        private async void btn_reservas_Clicked(System.Object sender, System.EventArgs e)
        {

            if (string.IsNullOrEmpty(await SecureStorage.GetAsync("usuario_logado")))
            {
                await DisplayAlert("Erro!", "Você precisa estar logado para acessar as reservas!", "OK");
                return;
            }

            for(int i = 0; i < PropriedadesDoApp.lista_clientes.Count; i++)
            {
                if (PropriedadesDoApp.lista_clientes[i].Usuario.Usuario == await SecureStorage.GetAsync("usuario_logado"))
                {
                    await Navigation.PushAsync(new Views.Hotel.ReservasHospedagem
                    {
                        BindingContext = PropriedadesDoApp.lista_clientes[i]
                    });
                    break;
                }
            }  
        }

        private void btn_reservar_Clicked(System.Object sender, System.EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new Views.Hotel.ContratacaoHospedagem());
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
