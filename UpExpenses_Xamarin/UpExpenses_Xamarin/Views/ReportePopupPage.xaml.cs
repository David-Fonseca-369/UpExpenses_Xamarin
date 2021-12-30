using Rg.Plugins.Popup.Pages;
using UpExpenses_Xamarin.ViewModels;
using Xamarin.Forms.Xaml;

namespace UpExpenses_Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportePopupPage : PopupPage
    {
        public ReportePopupPage()
        {
            InitializeComponent();
            BindingContext = new ReportePopupPageViewModel(Navigation);

        }
    }
}