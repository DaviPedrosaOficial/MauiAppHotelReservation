namespace MauiAppHotelReservation.Models
{
    public class AlteraReservaTemplateSelector : DataTemplateSelector                           // A classe herda de DataTemplateSelector que permite selecionar templates dinamicamente
    {
        // Definindo os DataTemplates para cada estado de nosso seletor
        public DataTemplate MenuTemplate { get; set; }
        public DataTemplate HospedesTemplate { get; set; }
        public DataTemplate QuartoTemplate { get; set; }
        public DataTemplate DatasTemplate { get; set; }



        // Selecionando o template baseado no estado atual do /ViewModel/AlteraReservaViewModel.cs
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container) // Sobrescreve o método OnSelectTemplate para fornecer a lógica de seleção de template, o que nos permitirá escolher o template correto com base no estado atual do ViewModel
        {
            if (item is ViewModels.AlteraReservaViewModel viewModel)                            // Verifica se o item é do tipo AlteraReservaViewModel
            {
                return viewModel.EstadoAtual switch                                             // Usa um switch expression para retornar o template correto com base no estado atual do ViewModel
                {
                    ViewModels.TipoAlteracao.Menu => MenuTemplate,
                    ViewModels.TipoAlteracao.Hospedes => HospedesTemplate,
                    ViewModels.TipoAlteracao.Quarto => QuartoTemplate,
                    ViewModels.TipoAlteracao.Datas => DatasTemplate,
                    _ => MenuTemplate,
                };
            }
            return MenuTemplate;                                                                // Retorna o template padrão se o item não for do tipo esperado
        }

    }
}
