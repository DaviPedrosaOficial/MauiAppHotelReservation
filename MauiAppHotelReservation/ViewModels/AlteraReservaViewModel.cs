using MauiAppHotelReservation.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiAppHotelReservation.ViewModels
{
    public enum TipoAlteracao { Menu, Hospedes, Quarto, Datas}

    public class AlteraReservaViewModel : BindableObject
    {
        // Definindo o tipo de alteração como Menu por padrão
        private TipoAlteracao estadoAtual = TipoAlteracao.Menu;

        public TipoAlteracao EstadoAtual
        {
            get { return estadoAtual; }
            set { estadoAtual = value; OnPropertyChanged(); AtualizarUI(); }
        }


        // Opções do menu
        public string Subtitulo => EstadoAtual switch
        {
            TipoAlteracao.Menu => "O que você deseja alterar?",
            TipoAlteracao.Hospedes => "Quantidade de hospedes?",
            TipoAlteracao.Quarto => "Qual tipo de quarto?",
            TipoAlteracao.Datas => "Quais as novas datas?",
            _ => ""
        };



        // Propriedades para vinculação de dados enquanto altera a reserva (Menu)
        public ObservableCollection<string> OpcoesMenu { get; } = 
            new(new[] { "Hospedes", "Quarto", "Data da estadia" });

        private string menuSelecionado;
        public string MenuSelecionado
        {
            get => menuSelecionado;
            set { menuSelecionado = value; OnPropertyChanged(); AtualizarUI(); }
        }




        // Propriedades para vinculação de dados enquanto altera a reserva (Hóspedes)
        private int qtdAdultos = 1;
        public int NumeroAdultos
        {
            get => qtdAdultos;
            set { qtdAdultos = Math.Clamp(value, 1, 10); OnPropertyChanged(); AtualizarUI(); }
        }

        private int qtdCriancas = 0;
        public int NumeroCriancas
        {
            get => qtdCriancas;
            set { qtdCriancas = Math.Clamp(value, 0, 10); OnPropertyChanged(); AtualizarUI(); }
        }

        // Definindo os quanto de hóspedes
        public ObservableCollection<Quarto> TiposDeQuarto { get; } =
            new(new[]
            {
                new Quarto { Descrição = "Suíte Presidencial", ValorDiariaAdulto = 600, ValorDiariaCriança = 300 },
                new Quarto { Descrição = "Suíte de Luxo",      ValorDiariaAdulto = 400, ValorDiariaCriança = 200 },
                new Quarto { Descrição = "Flat",               ValorDiariaAdulto = 300, ValorDiariaCriança = 150 },
                new Quarto { Descrição = "Suíte Single",       ValorDiariaAdulto = 220, ValorDiariaCriança = 110 },
            });

        private Quarto quartoSelecionado;
        public Quarto TipoDeQuartoSelecionado
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
        private DateTime checkIn = DateTime.Today;
        public DateTime CheckIn
        {
            get => checkIn;
            set { checkIn = value; OnPropertyChanged(); AtualizarUI(); }
        }
        private DateTime checkOut = DateTime.Today.AddDays(1);
        public DateTime CheckOut
        {
            get => checkOut;
            set { checkOut = value; OnPropertyChanged(); AtualizarUI(); }
        }




        //Botão de confirmação
        private string btn_confirmar = "Confirmar";
        public string Btn_Confirmar
        {
            get => btn_confirmar;
            set { btn_confirmar = value; OnPropertyChanged(); }
        }

        // Habilita ou desabilita o botão de confirmação
        private bool btn_pode_confirmar;
        public bool Btn_Pode_Confirmar
        {
            get => btn_pode_confirmar;
            set { btn_pode_confirmar = value; OnPropertyChanged(); }
        }


        // Comandos para os botões
        public ICommand ConfirmCommand { get; }

        // Referência à reserva do cliente que está sendo alterada
        private readonly object reservaDoCliente;

        public AlteraReservaViewModel(object reserva)
        {
            reservaDoCliente = reserva;
            quartoSelecionado = TiposDeQuarto.First();
            AtualizarUI();


            // Comando para o botão Confirmar/Salvar Alterações
            ConfirmCommand = new Command(async () =>
            {
                switch (estadoAtual)
                {
                    case TipoAlteracao.Menu:
                        if (MenuSelecionado == "Hospedes")
                            EstadoAtual = TipoAlteracao.Hospedes;
                        else if (MenuSelecionado == "Quarto")
                            EstadoAtual = TipoAlteracao.Quarto;
                        else if (MenuSelecionado == "Data da estadia")
                            EstadoAtual = TipoAlteracao.Datas;
                        MenuSelecionado = null; // Reseta a seleção do menu
                        break;


                    case TipoAlteracao.Hospedes:
                        if (reservaDoCliente is Models.Hospedagem reservaH)
                        {
                            reservaH.QuantidadeAdultos = NumeroAdultos;
                            reservaH.QuantidadeCriancas = NumeroCriancas;
                        }
                        await App.Current.MainPage.DisplayAlert("Sucesso", "Número de hóspedes alterado com sucesso!", "OK");
                        EstadoAtual = TipoAlteracao.Menu;

                        await Task.Delay(500); // Pequeno delay para melhor experiência do usuário
                        await App.Current.MainPage.Navigation.PopAsync(); // Volta para a página anterior
                        break;


                    case TipoAlteracao.Quarto:
                        if (reservaDoCliente is Models.Hospedagem reservaQ)
                        {
                            reservaQ.QuartoSelecionado = TipoDeQuartoSelecionado;
                        }
                        await App.Current.MainPage.DisplayAlert("Sucesso", "Tipo de quarto alterado com sucesso!", "OK");
                        EstadoAtual = TipoAlteracao.Menu;
                        break;


                    case TipoAlteracao.Datas:
                        if (reservaDoCliente is Models.Hospedagem reservaD)
                        {
                            reservaD.DataCheckIn = CheckIn;
                            reservaD.DataCheckOut = CheckOut;
                        }
                        await App.Current.MainPage.DisplayAlert("Sucesso", "Datas alteradas com sucesso!", "OK");
                        EstadoAtual = TipoAlteracao.Menu;
                        break;
                }
            });
        }


        // Propriedades para vinculação de dados enquanto altera a reserva
        private void AtualizarUI()
        {
            // Texto do botão por passo
            Btn_Confirmar = estadoAtual == TipoAlteracao.Menu ? "Confirmar" : "Salvar alterações";

            // Validações por passo
            Btn_Pode_Confirmar = estadoAtual switch
            {
                TipoAlteracao.Menu => !string.IsNullOrWhiteSpace(MenuSelecionado),
                TipoAlteracao.Hospedes => NumeroAdultos >=  1 && NumeroCriancas >= 0,
                TipoAlteracao.Quarto => quartoSelecionado != null,
                TipoAlteracao.Datas => CheckOut > CheckIn,
                _ => false
            };
        }
    }
}
