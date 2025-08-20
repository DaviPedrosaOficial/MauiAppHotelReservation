using MauiAppHotelReservation.Services;

namespace MauiAppHotelReservation.Views.Login;

public partial class Cadastro : ContentPage
{
    private List<DadosUsuario> usuarios;

    public Cadastro()
    {
        InitializeComponent();
        usuarios = UsuarioService.Instance.Usuarios;
    }

    private async void btn_cadastro_cadastro_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (usuarios.Any(i => i.Usuario == txt_usuario_cadastro.Text))
            {
                throw new Exception("Usuário já cadastrado!");

            }
            else if (txt_senha_cadastro == null || txt_confirma_senha_cadastro == null)
            {
                throw new Exception("Senha &/ou confirmação de senha não inserida!");
            }
            else if (txt_senha_cadastro.Text != txt_confirma_senha_cadastro.Text)
            {
                txt_senha_cadastro.Text = string.Empty;
                txt_confirma_senha_cadastro.Text = string.Empty;
                throw new Exception("Senha e confirmação de senha não batem!");
            }
            else
            {

                // Criando o usuário com os dados digitados
                DadosUsuario novo_usuario = new DadosUsuario()
                {
                    Usuario = txt_usuario_cadastro.Text,
                    Senha = txt_senha_cadastro.Text
                };

                // Adicionando o usuário na lista de usuários
                usuarios.Add(novo_usuario);

                // Salvando o usuário no armazenamento seguro do dispositivo
                await SecureStorage.SetAsync("usuario_logado", novo_usuario.Usuario);

                // Redirecionando para a página protegida
                Application.Current.MainPage = new MainPage();
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "Ok!");
        }
    }
}