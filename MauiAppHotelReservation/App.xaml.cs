using MauiAppHotelReservation.Models;

namespace MauiAppHotelReservation
{
    public partial class App : Application
    {
        //Simulando um banco de dados com uma lista de quartos (Regra de negócio)
        public List<Quarto> lista_quartos = new List<Quarto>
        {
            new Quarto()
            {
                Descrição = "Suíte Presidencial",
                ValorDiariaAdulto = 1250.0,
                ValorDiariaCriança = 800.0
            },
            new Quarto()
            {
                Descrição = "Suíte de Luxo",
                ValorDiariaAdulto = 850.0,
                ValorDiariaCriança = 450
            },
            new Quarto()
            {
                Descrição = "Flat",
                ValorDiariaAdulto = 350.0,
                ValorDiariaCriança = 120
            },
            new Quarto()
            {
                Descrição = "Suíte Single",
                ValorDiariaAdulto = 150.0
            }
        };

        public List<Reserva> lista_reservas = new List<Reserva>();

        public App()
        {
            // Inicializando os componentes da aplicação
            InitializeComponent();

            // Inicializando o aplicativo e verificando se o usuário está logado
            InitializeAppAsync();
        }

        // Método assíncrono para inicializar o aplicativo e verificar o status de login do usuário
        private async void InitializeAppAsync()
        {
            var usuarioLogado = await SecureStorage.GetAsync("usuario_logado");

            if (!string.IsNullOrEmpty(usuarioLogado))
            {
                // Se o usuário estiver logado, define a página principal como MainPage
                MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                // Se o usuário não estiver logado, define a página principal como a página de login
                MainPage = new NavigationPage(new Views.Login.Login());
            }
        }

        //Preparando a janela de nosso projeto
        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

            window.Width = 400;
            window.Height = 600;

            return window;
        }
    }
}