using MauiAppHotelReservation.Models;

namespace MauiAppHotelReservation.Views;

public partial class ContratacaoHospedagem : ContentPage
{
    // Declara��o de uma vari�vel para acessar as propriedades do App
    App PropriedadesDoApp;

    public ContratacaoHospedagem()
    {
        InitializeComponent();

        // Obt�m a inst�ncia do App para acessar as propriedades
        PropriedadesDoApp = (App)Application.Current;

        // Transfere a lista de quartos do App para o Picker (HospedagemContratada.xaml, linha 82)
        pck_quarto.ItemsSource = PropriedadesDoApp.lista_quartos;
        
        // Define a data m�nima de check-in como a data presente
        dtpck_check_in.MinimumDate = DateTime.Now;

        // Define a data m�xima de check-in para at� 2 mes ap�s a data presente
        dtpck_check_in.MaximumDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month +2, DateTime.Now.Day);

        // Define a data m�nima de check-out como a data de check-in + 1 dia
        dtpck_check_out.MinimumDate = dtpck_check_in.Date.AddDays(1);

        // Define a data m�xima de check-out para at� 6 meses ap�s a data de check-in
        dtpck_check_out.MaximumDate = dtpck_check_in.Date.AddMonths(6);
    }


    private async void btn_avancar_Clicked(System.Object sender, System.EventArgs e)
    {
		try
		{
            Hospedagem hospedagem = new Hospedagem 
            {
                // Obt�m o quarto selecionado no Picker e converte para o tipo Quarto
                QuartoSelecionado = (Quarto)pck_quarto.SelectedItem,

                // Obt�m a quantidade de adultos no stepper e converte para inteiro (Contrata��oDeHospedagem.xaml Linha 54)
                QuantidadeAdultos = Convert.ToInt32(stp_adultos.Value),

                // Obt�m a quantidade de crian�as no stepper e converte para inteiro (Contrata��oDeHospedagem.xaml Linha 67)
                QuantidadeCriancas = Convert.ToInt32(stp_criancas.Value),

                // Obt�m a data de check-in selecionada no DatePicker
                DataCheckIn = dtpck_check_in.Date,

                // Obt�m a data de check-out selecionada no DatePicker
                DataCheckOut = dtpck_check_out.Date
            };

            // Cria uma nova inst�ncia do modelo Hospedagem e preenche com os dados do formul�rio
           await Navigation.PushAsync(new Views.HospedagemContratada() 
           {
               // Passa a hospedagem criada para a pr�xima p�gina (HospedagemContratada.xaml)
               BindingContext = hospedagem
           });
        
		} 
		catch (Exception ex) 
		{
            // Exibe uma mensagem de erro caso ocorra uma exce��o
            await DisplayAlert("Erro", ex.Message, "OK");
        
		}
    }



    // Evento disparado quando a data de check-in � selecionada (Contrata��oDeHospedagem.xaml Linha 100)
    private void dtpck_check_in_DateSelected(System.Object sender, Microsoft.Maui.Controls.DateChangedEventArgs e)
    {

        // Envolvendo o c�digo em um bloco try-catch para capturar poss�veis exce��es
        try
        {
            //Converte o sender para DatePicker para que seja poss�vel acessar a data selecionada no DatePicker da dtpck_check_in
            DatePicker elemento = sender as DatePicker;

            // Obt�m a data selecionada no DatePicker de check-in
            DateTime data_selecionada_checkin = elemento.Date;

            // Atualiza a data m�nima de check-out para ser 1 dia ap�s a data de check-in selecionada
            dtpck_check_out.MinimumDate = data_selecionada_checkin.AddDays(1);

            // Atualiza a data m�xima de check-out para ser 6 meses ap�s a data de check-in selecionada
            dtpck_check_out.MaximumDate = data_selecionada_checkin.AddMonths(6);
        }
        catch (Exception ex) 
        {
            // Exibe uma mensagem de erro caso ocorra uma exce��o
            DisplayAlert("Erro", ex.Message, "OK");
        }

    }
}