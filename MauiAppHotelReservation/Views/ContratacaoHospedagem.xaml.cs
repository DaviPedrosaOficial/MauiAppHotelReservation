using MauiAppHotelReservation.Models;

namespace MauiAppHotelReservation.Views;

public partial class ContratacaoHospedagem : ContentPage
{
    // Declaração de uma variável para acessar as propriedades do App
    App PropriedadesDoApp;

    public ContratacaoHospedagem()
    {
        InitializeComponent();

        // Obtém a instância do App para acessar as propriedades
        PropriedadesDoApp = (App)Application.Current;

        // Transfere a lista de quartos do App para o Picker (HospedagemContratada.xaml, linha 82)
        pck_quarto.ItemsSource = PropriedadesDoApp.lista_quartos;
        
        // Define a data mínima de check-in como a data presente
        dtpck_check_in.MinimumDate = DateTime.Now;

        // Define a data máxima de check-in para até 2 mes após a data presente
        dtpck_check_in.MaximumDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month +2, DateTime.Now.Day);

        // Define a data mínima de check-out como a data de check-in + 1 dia
        dtpck_check_out.MinimumDate = dtpck_check_in.Date.AddDays(1);

        // Define a data máxima de check-out para até 6 meses após a data de check-in
        dtpck_check_out.MaximumDate = dtpck_check_in.Date.AddMonths(6);
    }


    private async void btn_avancar_Clicked(System.Object sender, System.EventArgs e)
    {
		try
		{
            Hospedagem hospedagem = new Hospedagem 
            {
                // Obtém o quarto selecionado no Picker e converte para o tipo Quarto
                QuartoSelecionado = (Quarto)pck_quarto.SelectedItem,

                // Obtém a quantidade de adultos no stepper e converte para inteiro (ContrataçãoDeHospedagem.xaml Linha 54)
                QuantidadeAdultos = Convert.ToInt32(stp_adultos.Value),

                // Obtém a quantidade de crianças no stepper e converte para inteiro (ContrataçãoDeHospedagem.xaml Linha 67)
                QuantidadeCriancas = Convert.ToInt32(stp_criancas.Value),

                // Obtém a data de check-in selecionada no DatePicker
                DataCheckIn = dtpck_check_in.Date,

                // Obtém a data de check-out selecionada no DatePicker
                DataCheckOut = dtpck_check_out.Date
            };

            // Cria uma nova instância do modelo Hospedagem e preenche com os dados do formulário
           await Navigation.PushAsync(new Views.HospedagemContratada() 
           {
               // Passa a hospedagem criada para a próxima página (HospedagemContratada.xaml)
               BindingContext = hospedagem
           });
        
		} 
		catch (Exception ex) 
		{
            // Exibe uma mensagem de erro caso ocorra uma exceção
            await DisplayAlert("Erro", ex.Message, "OK");
        
		}
    }



    // Evento disparado quando a data de check-in é selecionada (ContrataçãoDeHospedagem.xaml Linha 100)
    private void dtpck_check_in_DateSelected(System.Object sender, Microsoft.Maui.Controls.DateChangedEventArgs e)
    {

        // Envolvendo o código em um bloco try-catch para capturar possíveis exceções
        try
        {
            //Converte o sender para DatePicker para que seja possível acessar a data selecionada no DatePicker da dtpck_check_in
            DatePicker elemento = sender as DatePicker;

            // Obtém a data selecionada no DatePicker de check-in
            DateTime data_selecionada_checkin = elemento.Date;

            // Atualiza a data mínima de check-out para ser 1 dia após a data de check-in selecionada
            dtpck_check_out.MinimumDate = data_selecionada_checkin.AddDays(1);

            // Atualiza a data máxima de check-out para ser 6 meses após a data de check-in selecionada
            dtpck_check_out.MaximumDate = data_selecionada_checkin.AddMonths(6);
        }
        catch (Exception ex) 
        {
            // Exibe uma mensagem de erro caso ocorra uma exceção
            DisplayAlert("Erro", ex.Message, "OK");
        }

    }
}