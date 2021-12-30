using UpExpenses_Xamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpExpenses_Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportesPendientesPage : ContentPage
    {
        public ReportesPendientesPage()
        {            
            InitializeComponent();
            
            BindingContext = new ReportesPendientesPageViewModel(Navigation);            
        }

     
    }
}