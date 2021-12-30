using UpExpenses_Xamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpExpenses_Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetallesReportePage : ContentPage
    {
        public DetallesReportePage()
        {
            InitializeComponent();
            BindingContext = new DetallesReportePageViewModel(Navigation);                 
        }
    }
}