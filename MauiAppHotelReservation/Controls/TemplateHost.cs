using System.ComponentModel; 

namespace MauiAppHotelReservation.Controls
{
    // Uma View que pode aplicar um DataTemplateSelector ao seu BindingContext
    public class TemplateHost : ContentView
    {
        // Definição da propriedade vinculada para o DataTemplateSelector
        public static readonly BindableProperty PropriedadeSeletoraDeTemplate =
            BindableProperty.Create(
                nameof(SeletorDeTemplate),
                typeof(DataTemplateSelector),
                typeof(TemplateHost),
                propertyChanged: OnTemplateSelectorChanged);



        // Propriedade que define o DataTemplateSelector a ser usado
        public DataTemplateSelector SeletorDeTemplate
        {
            // Get recebe o valor da propriedade vinculada (PropriedadeSeletoraDeTemplate) e Set define o valor
            get => (DataTemplateSelector)GetValue(PropriedadeSeletoraDeTemplate);
            set => SetValue(PropriedadeSeletoraDeTemplate, value);
        }



        // Chamado quando a propriedade SeletorDeTemplate muda e binda o novo template
        static void OnTemplateSelectorChanged(BindableObject bindable, object antigoValor, object novoValor)
            => ((TemplateHost)bindable).ApplyTemplate();



        // Mantendo uma referência ao BindingContext atual (caso implemente INotifyPropertyChanged) para acompanhar as mudanças da propriedade (EstadoAtual) e reaplicar o template.
        INotifyPropertyChanged _notificadorAtual;



        // Chamado quando o BindingContext muda e binda o novo template
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            // Condicional para se desinscrever do contexto anterior (evita múltiplos handlers/memória)
            if (_notificadorAtual != null)
            {
                _notificadorAtual.PropertyChanged -= OnContextPropertyChanged;
                _notificadorAtual = null;
            }

            // Inscrevendo no novo contexto (se notificar mudanças)
            _notificadorAtual = BindingContext as INotifyPropertyChanged;
            if (_notificadorAtual != null)
                _notificadorAtual.PropertyChanged += OnContextPropertyChanged;

            ApplyTemplate();
        }



        // Chamado quando uma propriedade do BindingContext muda
        void OnContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == "EstadoAtual") //Filtra pela prop relevante
                ApplyTemplate(); 
        }



        // Aplica o template selecionado ao conteúdo da View
        void ApplyTemplate()
        {
            // Se o seletor de template ou o BindingContext forem nulos, não faz nada
            if (SeletorDeTemplate == null || BindingContext == null) return;

            // Seleciona o template apropriado com base no BindingContext
            var template = SeletorDeTemplate.SelectTemplate(BindingContext, this);
            if (template == null) return;

            // Cria o conteúdo do template e o define como o conteúdo da View
            var content = template.CreateContent();
            if (content is View view)
                Content = view;

            // Se o conteúdo for um ViewCell, define a propriedade View como o conteúdo da View
            else if (content is ViewCell cell)
                Content = cell.View;
        }
    }
}
