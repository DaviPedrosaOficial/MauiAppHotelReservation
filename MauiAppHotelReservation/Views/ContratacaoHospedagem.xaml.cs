namespace MauiAppHotelReservation.Views;

public partial class ContratacaoHospedagem : ContentPage
{
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
    private void btn_avancar_Clicked(System.Object sender, System.EventArgs e)
    {
		try
		{
			//
			Navigation.PushAsync(new Views.HospedagemContratada());
        
		} 
		catch (Exception ex) 
		{
            // Exibe uma mensagem de erro caso ocorra uma exceção
            DisplayAlert("Erro", ex.Message, "OK");
        
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