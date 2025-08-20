namespace MauiAppHotelReservation.Services
{
    public class UsuarioService
    {
        //•	Criando uma variável que armazenará a única instância da classe UsuarioService.
        private static UsuarioService? _instance;
        public static UsuarioService Instance => _instance ??= new UsuarioService();
        /* 	Usando o operador ??= (null-coalescing assignment):
            •	Se _instance for null, cria uma nova instância de UsuarioService e atribui a _instance.
            •	Se já existir, apenas retorna a instância existente. */

        public List<DadosUsuario> Usuarios { get; } = new()
    {
        new DadosUsuario() { Usuario = "admin", Senha = "123" },
        new DadosUsuario() { Usuario = "user", Senha = "123" },
        new DadosUsuario() { Usuario = "guest", Senha = "123" }
    };
    }
}
