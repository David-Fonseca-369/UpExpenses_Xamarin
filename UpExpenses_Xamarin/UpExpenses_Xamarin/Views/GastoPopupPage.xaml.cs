using Rg.Plugins.Popup.Pages;
using UpExpenses_Xamarin.ViewModels;
using Xamarin.Forms.Xaml;

namespace UpExpenses_Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GastoPopupPage : PopupPage
    {
        public GastoPopupPage()
        {
            InitializeComponent();
            BindingContext = new GastoPopupPageViewModel(Navigation);
        }
    }
}