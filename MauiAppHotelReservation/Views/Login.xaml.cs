using MauiAppHotelReservation.Services;
using MauiAppLogin;
using MauiAppLogin.Services;

namespace MauiAppHotelReservation;

public partial class Login : ContentPage
{
    private List<DadosUsuario> usuarios = new List<DadosUsuario>();
    //•	Criando uma variável que armazenará a lista de usuários.
    //•	Instanciando a lista de usuários no construtor da classe Login.

    public Login()
	{
		InitializeComponent();
        usuarios = UsuarioService.Instance.Usuarios;
    }

    private async void btn_login_Clicked(object sender, EventArgs e)
    {
        try
        {
            DadosUsuario dados_digitados = new DadosUsuario()
            {
                Usuario = txt_usuario_login.Text,
                Senha = txt_senha_login.Text
            };

            if (usuarios.Any(i => i.Usuario == dados_digitados.Usuario && i.Senha == dados_digitados.Senha))
            { 
                await SecureStorage.SetAsync("usuario_logado", dados_digitados.Usuario);

                App.Current.MainPage = new MainPage(); // Navegando para a página principal após o login bem-sucedido
            }
            else
            {
                txt_senha_login.Text = string.Empty;
                throw new Exception("Usuário ou senha inválidos.");
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", $"Ocorreu um erro: {ex.Message}", "OK");

        };
    }

    private void btn_cadastro_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new Cadastro(); // Navegando para a página de cadastro de usuário
    }
}