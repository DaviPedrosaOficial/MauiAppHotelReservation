using MauiAppHotelReservation.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;


namespace MauiAppHotelReservation.ViewModels
{
    public enum TipoAlteracao { Menu, Hospedes, Quarto, Datas }         // Definindo os tipos de alteração possíveis na reserva

    public class AlteraReservaViewModel : BindableObject
    {

        // <------------------------------------------------------ Propriedades para vinculação de dados (binding) e lógica de UI ------------------------------------------------------> //

        private TipoAlteracao estadoAtual = TipoAlteracao.Menu;         // Definindo o tipo de alteração como Menu por padrão (definido no enum TipoAlteracao)
        public TipoAlteracao EstadoAtual
        {
            get => estadoAtual;
            set
            {
                if (estadoAtual == value) return;                       // Previne loops infinitos de atualização, caso nada seja selecionado, o mesmo valor é atribuído, e set não fará nada

                estadoAtual = value;                                    // Caso o valor seja diferente, atribui o novo valor
                OnPropertyChanged();                                    // Avisa que a propriedade mudou          
                OnPropertyChanged(nameof(Subtitulo));                   // Avisa que o subtítulo mudou (para atualizar o subtítulo da página)
                AtualizarUI();                                          // Atualiza a UI (botão confirmar)
            }
        }


        // Opções do menu (Ficarão visíveis no subtítulo da página )
        public string Subtitulo => EstadoAtual switch                   // Usando switch expression para definir o subtítulo baseado no estado atual, e criando uma propriedade para que o mesmo possa ser vinculada (binded) na UI, a depender do estado atual
        {
            TipoAlteracao.Menu => "O que você deseja alterar?",
            TipoAlteracao.Hospedes => "Quantidade de hospedes?",
            TipoAlteracao.Quarto => "Qual tipo de quarto?",
            TipoAlteracao.Datas => "Quais as novas datas?",
            _ => ""
        };

        // Opções de nosso Picker de escolha do que alterar na página de reservas
        public ObservableCollection<string> OpcoesMenu { get; } =       // ObservableCollection para permitir atualizações dinâmicas em nossa UI
            new(new[] { "Hospedes", "Quarto", "Data da estadia" });

        private string menuSelecionado;
        public string MenuSelecionado                                   // Propriedade pública para vinculação de dados (binding em nossa UI
        {
            get => menuSelecionado;
            set { menuSelecionado = value;                              // Pegando o valor selecionado atraves do Picker
                OnPropertyChanged();                                    // Avisa que a propriedade mudou
                AtualizarUI();                                          // Atualiza a UI (botão confirmar)
            }
        }

        // Propriedades para vinculação de dados enquanto altera a reserva (Hóspedes adultos)

        private int qtdAdultos = 1;                                     // Estabelece um valor padrão 1
        public int NumeroAdultos                                        // Propriedade pública para vinculação de dados (binding em nossa UI)
        {
            get => qtdAdultos;
            set { qtdAdultos = Math.Clamp(value, 1, 10);                // Recebendo o valor passado pelo nosso cliente pela UI, e garante que o mesmo fique entre 1 e 10
                OnPropertyChanged();
                AtualizarUI();
            }
        }

        // Propriedades para vinculação de dados enquanto altera a reserva (Hóspedes crianças)

        private int qtdCriancas = 0;                                    // Estabelece um valor padrão 0
        public int NumeroCriancas                                       // Propriedade pública para vinculação de dados (binding em nossa UI)
        {
            get => qtdCriancas;
            set { qtdCriancas = Math.Clamp(value, 0, 10);
                OnPropertyChanged();
                AtualizarUI();
            }
        }

        // Definindo os quanto de hóspedes
        public ObservableCollection<Quarto> TiposDeQuarto { get; } =    // ObservableCollection que também nos permitirá atualizar dinâmicamente em nossa UI, possibilitando escolhar os tipos de quartos baseado em nossa classe Quarto
            new(new[]
            {
                new Quarto { Descrição = "Suíte Presidencial", ValorDiariaAdulto = 600, ValorDiariaCriança = 300 },
                new Quarto { Descrição = "Suíte de Luxo",      ValorDiariaAdulto = 400, ValorDiariaCriança = 200 },
                new Quarto { Descrição = "Flat",               ValorDiariaAdulto = 300, ValorDiariaCriança = 150 },
                new Quarto { Descrição = "Suíte Single",       ValorDiariaAdulto = 220, ValorDiariaCriança = 110 },
            });

        private Quarto quartoSelecionado;
        public Quarto TipoDeQuartoSelecionado                           // Propriedade pública para vinculação de dados (binding em nossa UI)
        {
            get => quartoSelecionado;
            set { 
                if(value == null) return;
                
                quartoSelecionado = value;
                OnPropertyChanged();
                AtualizarUI();
            }
        }

        // Propriedades para vinculação de dados enquanto altera a reserva (Datas)

        private DateTime checkIn = DateTime.Today;                      // Previne que reservas sejam feitas para datas passadas
        public DateTime CheckIn                                         // Propriedade pública para vinculação de dados (binding em nossa UI)
        {
            get => checkIn;
            set { checkIn = value;
                OnPropertyChanged();
                AtualizarUI();
            }
        }
        private DateTime checkOut = DateTime.Today.AddDays(1);          // Previne que reservas sejam feitas para datas passadas, e garante que o check-out seja no mínimo 1 dia após o check-in
        public DateTime CheckOut                                        // Propriedade pública para vinculação de dados (binding em nossa UI)
        {
            get => checkOut;
            set { checkOut = value; OnPropertyChanged(); AtualizarUI(); }
        }

        //Botão de confirmação

        private string btn_confirmar = "Confirmar";                     // Texto padrão do botão
        public string Btn_Confirmar                                     // Propriedade pública para vinculação de dados (binding em nossa UI)
        {
            get => btn_confirmar;
            set { btn_confirmar = value;
                OnPropertyChanged();
            }
        }

        // Habilita ou desabilita o botão de confirmação
        private bool btn_pode_confirmar;
        public bool Btn_Pode_Confirmar                                  // Propriedade pública para vinculação de dados (binding em nossa UI)
        {
            get => btn_pode_confirmar;
            set { btn_pode_confirmar = value;
                OnPropertyChanged();
            }
        }

        
        public ICommand ConfirmCommand { get; }                         // Criando o comando para o botão Confirmar/Salvar Alterações, atraves da interface ICommand que possibilita a criação de comandos para botões em MVVM


        private readonly object reservaDoCliente;                       // Criando um objeto para armazenar a reserva do cliente que será alterada


        // <------------------------------------------------------ Fim das propriedades para vinculação de dados (binding) e lógica de UI ------------------------------------------------------> //

        // <------------------------------------------------------------------------------ Construtor ------------------------------------------------------------------------------------------> //

        public AlteraReservaViewModel(object reserva)                   // Construtor que recebe a reserva a ser alterada
        {
            reservaDoCliente = reserva;                                 // Armazena a reserva selecionada pelo cliente em nosso objeto reservaDoCliente, para que futuras alterações possam ser aplicadas a ela 
            quartoSelecionado = TiposDeQuarto.First();                  // Define um quarto padrão para evitar problemas de null reference
            AtualizarUI();                                              // Atualiza a UI inicialmente, para garantir que o botão esteja no estado correto



            ConfirmCommand = new Command(async () =>                    // Definindo a ação do comando ConfirmCommand, que é assíncrona para evitar travamentos na UI
            {
                switch (estadoAtual)                                    // Verifica o estado atual para determinar a ação a ser tomada, baseado no estado atual (Menu, Hóspedes, Quarto, Datas)
                {
                    case TipoAlteracao.Menu:                            // Se estiver no menu, verifica o que foi selecionado e muda o estado atual para o próximo passo
                        
                        if (MenuSelecionado == "Hospedes")
                            EstadoAtual = TipoAlteracao.Hospedes;
                        
                        else if (MenuSelecionado == "Quarto")
                            EstadoAtual = TipoAlteracao.Quarto;
                        
                        else if (MenuSelecionado == "Data da estadia")
                            EstadoAtual = TipoAlteracao.Datas;
                        
                        break;


                    case TipoAlteracao.Hospedes:                                                                                    // Se estiver alterando hóspedes, aplica as alterações e volta para a página de reservas
                        
                        if (reservaDoCliente is Models.Hospedagem reservaH)                                                         // Verifica se o objeto reservaDoCliente é do tipo Hospedagem antes de aplicar as alterações
                        {
                            reservaH.QuantidadeAdultos = NumeroAdultos;                                                             // Aplica a quantidade de adultos selecionada para alteração em nossa UI, na reserva do cliente
                            reservaH.QuantidadeCriancas = NumeroCriancas;                                                           // Aplica a quantidade de crianças selecionada para alteração em nossa UI, na reserva do cliente
                        }
                        
                        await App.Current.MainPage.DisplayAlert("Sucesso", "Número de hóspedes alterado com sucesso!", "OK");       // Exibe uma mensagem de sucesso para o usuário
                        EstadoAtual = TipoAlteracao.Menu;                                                                           // Volta para o menu principal após a alteração

                        await App.Current.MainPage.Navigation.PushAsync(new Views.Hotel.ReservasHospedagem());                      // Volta para a página de reservas
                        break;


                    case TipoAlteracao.Quarto:                                                                                      // Se estiver alterando o tipo de quarto, aplica as alterações e volta para a página de reservas
                        if (reservaDoCliente is Models.Hospedagem reservaQ)
                        {
                            reservaQ.QuartoSelecionado = TipoDeQuartoSelecionado;
                        }
                        await App.Current.MainPage.DisplayAlert("Sucesso", "Tipo de quarto alterado com sucesso!", "OK");
                        EstadoAtual = TipoAlteracao.Menu;

                        await App.Current.MainPage.Navigation.PushAsync(new Views.Hotel.ReservasHospedagem());
                        break;


                    case TipoAlteracao.Datas:                                                                                       // Se estiver alterando as datas, aplica as alterações e volta para a página de reservas
                        if (reservaDoCliente is Models.Hospedagem reservaD)
                        {
                            reservaD.DataCheckIn = CheckIn;
                            reservaD.DataCheckOut = CheckOut;
                        }
                        await App.Current.MainPage.DisplayAlert("Sucesso", "Datas alteradas com sucesso!", "OK");
                        EstadoAtual = TipoAlteracao.Menu;

                        await App.Current.MainPage.Navigation.PushAsync(new Views.Hotel.ReservasHospedagem());
                        break;
                }
            });
        }

        // <------------------------------------------------------ Fim do Construtor ------------------------------------------------------> //

        // <------------------------------------------------------ Método auxiliar ------------------------------------------------------> //


        // Propriedades para vinculação de dados enquanto altera a reserva
        private void AtualizarUI()
        {
            // Texto do botão por passo (se == Menu, "Confirmar", senão "Salvar alterações")
            Btn_Confirmar = estadoAtual == TipoAlteracao.Menu ? "Confirmar" : "Salvar alterações";

            // Validações por passo
            Btn_Pode_Confirmar = estadoAtual switch                                             // Usando switch expression para definir se o botão pode ser clicado ou não, baseado no estado atual e nas validações necessárias
            {
                TipoAlteracao.Menu => !string.IsNullOrWhiteSpace(MenuSelecionado),              // Verifica se algo foi selecionado no menu
                TipoAlteracao.Hospedes => NumeroAdultos >=  1 && NumeroCriancas >= 0,           // Verifica se a quantidade de adultos é pelo menos 1 e a de crianças é 0 ou mais
                TipoAlteracao.Quarto => quartoSelecionado != null,                              // Verifica se um quarto foi selecionado
                TipoAlteracao.Datas => CheckOut > CheckIn,                                      // Verifica se a data de check-out é maior que a de check-in
                _ => false                                                                      // Caso nenhum dos casos anteriores seja atendido, desabilita o botão
            };
        }
    }
}
