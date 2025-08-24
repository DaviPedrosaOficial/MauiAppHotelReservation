using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppHotelReservation.Models
{
    public class AlteraReservaTemplateSelector : DataTemplateSelector
    {
        // Definindo os DataTemplates para cada estado de nosso seletor
        public DataTemplate MenuTemplate { get; set; }
        public DataTemplate HospedesTemplate { get; set; }
        public DataTemplate QuartoTemplate { get; set; }
        public DataTemplate DatasTemplate { get; set; }

        // Selecionando o template baseado no estado atual do ViewModel
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is ViewModels.AlteraReservaViewModel viewModel)
            {
                return viewModel.EstadoAtual switch
                {
                    ViewModels.TipoAlteracao.Menu => MenuTemplate,
                    ViewModels.TipoAlteracao.Hospedes => HospedesTemplate,
                    ViewModels.TipoAlteracao.Quarto => QuartoTemplate,
                    ViewModels.TipoAlteracao.Datas => DatasTemplate,
                    _ => MenuTemplate,
                };
            }
            return MenuTemplate; // Retorna o template padrão se o item não for do tipo esperado
        }

    }
}
