using System.ComponentModel; 

namespace MauiAppHotelReservation.Controls
{
    // Classe criada para hospedar templates dinâmicos com base no estado do ViewModel atrelado ao BindingContext
    public class TemplateHost : ContentView
    {
        // Definição da propriedade vinculada para o DataTemplateSelector
        public static readonly BindableProperty PropriedadeSeletoraDeTemplate =
            BindableProperty.Create(
                nameof(SeletorDeTemplate),
                typeof(DataTemplateSelector),
                typeof(TemplateHost),
                propertyChanged: OnTemplateSelectorChanged);



        // Propriedade que define o DataTemplateSelector a ser usado, Wrapper para a propriedade vinculada, expondo-a publicamente
        public DataTemplateSelector SeletorDeTemplate
        {
            // Get recebe o valor da propriedade vinculada (PropriedadeSeletoraDeTemplate) e Set define o valor
            get => (DataTemplateSelector)GetValue(PropriedadeSeletoraDeTemplate);
            set => SetValue(PropriedadeSeletoraDeTemplate, value);
        }



        // Chamado quando a propriedade SeletorDeTemplate muda e binda o novo template, aplicando o template imediatamente
        static void OnTemplateSelectorChanged(BindableObject bindable, object antigoValor, object novoValor)
            => ((TemplateHost)bindable).ApplyTemplate();



        // Mantendo uma referência ao BindingContext atual (caso implemente INotifyPropertyChanged) para acompanhar as mudanças da propriedade (EstadoAtual) e reaplicar o template.
        INotifyPropertyChanged notificadorAtual;



        // Chamado para que o TemplateHost saiba quando trocar de template ao mudar propriedades do ViewModel (EstadoAtual), evitando de ficar preso no template errado.
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged(); // Chama o método base para garantir que o comportamento padrão seja mantido (necessário para o funcionamento correto do BindingContext)

            // Condicional para se desinscrever do contexto anterior (evita múltiplos handlers/memória)
            if (notificadorAtual != null)                                        // Se houver um contexto anterior inscrito
            {
                notificadorAtual.PropertyChanged -= OnContextPropertyChanged;    // Remove o handler do evento PropertyChanged do contexto anterior
                notificadorAtual = null;                                         // Limpa a referência ao contexto anterior
            }

            // Inscrevendo no novo contexto (se notificar mudanças)
            notificadorAtual = BindingContext as INotifyPropertyChanged;          // Tenta converter o BindingContext atual para INotifyPropertyChanged
            if (notificadorAtual != null)                                         // Se a conversão for bem-sucedida (o BindingContext implementa INotifyPropertyChanged)
                notificadorAtual.PropertyChanged += OnContextPropertyChanged;     // Adiciona o handler do evento PropertyChanged para o novo contexto

            ApplyTemplate();                                                      // Aplica o template com base no novo BindingContext
        }



        // Chamado quando uma propriedade do BindingContext muda, replicando o template se a propriedade relevante (EstadoAtual) mudar
        void OnContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == "EstadoAtual") // Filtra pela propriedade relevante (EstadoAtual) ou todas (string.IsNullOrEmpty)
                ApplyTemplate(); 
        }



        // Aplica o template selecionado ao conteúdo da View, metódo central para atualizar o conteúdo com base no estado atual do ViewModel
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
