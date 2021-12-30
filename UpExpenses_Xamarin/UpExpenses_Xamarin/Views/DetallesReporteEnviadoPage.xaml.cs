using UpExpenses_Xamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpExpenses_Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetallesReporteEnviadoPage : ContentPage
    {
        public DetallesReporteEnviadoPage()
        {
            InitializeComponent();
            BindingContext = new DetallesReporteEnviadoPageViewModel(Navigation);
        }
    }
}