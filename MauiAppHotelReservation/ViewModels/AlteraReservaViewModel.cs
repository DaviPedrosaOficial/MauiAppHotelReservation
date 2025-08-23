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
            TipoAlteracao.Hospedes => "Quantos hóspedes?",
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
            set { qtdAdultos = Math.Max(1,10); OnPropertyChanged(); AtualizarUI(); }
        }

        private int qtdCriancas = 1;
        public int NumeroCriancas
        {
            get => qtdCriancas;
            set { qtdCriancas = Math.Max(0, 10); OnPropertyChanged(); AtualizarUI(); }
        }




        // Propriedades para vinculação de dados enquanto altera a reserva (Quarto)
        public ObservableCollection<string> TiposDeQuarto =
            new(new[] { "Suíte Presidencial", "Suíte de Luxo", "Flat", "Suíte Single" });

        private string quartoSelecionado;
        public string TipoDeQuartoSelecionado
        {
            get => quartoSelecionado;
            set { quartoSelecionado = value; OnPropertyChanged(); AtualizarUI(); }
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
                            reservaH.NumeroAdultos = NumeroAdultos;
                            reservaH.NumeroCriancas = NumeroCriancas;
                        }
                        await App.Current.MainPage.DisplayAlert("Sucesso", "Número de hóspedes alterado com sucesso!", "OK");
                        EstadoAtual = TipoAlteracao.Menu;
                        break;


                    case TipoAlteracao.Quarto:
                        if (reservaDoCliente is Models.Hospedagem reservaQ)
                        {
                            reservaQ.TipoDeQuarto = TipoDeQuartoSelecionado;
                        }
                        await App.Current.MainPage.DisplayAlert("Sucesso", "Tipo de quarto alterado com sucesso!", "OK");
                        EstadoAtual = TipoAlteracao.Menu;
                        break;


                    case TipoAlteracao.Datas:
                        if (reservaDoCliente is Models.Hospedagem reservaD)
                        {
                            reservaD.CheckIn = CheckIn;
                            reservaD.CheckOut = CheckOut;
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
            TextoBotaoConfirmar = estadoAtual == TipoAlteracao.Menu ? "Confirmar" : "Salvar alterações";

            // Validações por passo
            PodeConfirmar = estadoAtual switch
            {
                TipoAlteracao.Menu => !string.IsNullOrWhiteSpace(SelectedMenuOption),
                TipoAlteracao.Hospedes => NumeroHospedes >= 1,
                TipoAlteracao.Quarto => !string.IsNullOrWhiteSpace(TipoDeQuartoSelecionado),
                TipoAlteracao.Datas => CheckOut > CheckIn,
                _ => false
            };
        }
    }
}
