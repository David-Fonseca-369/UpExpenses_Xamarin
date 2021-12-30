using System.Windows.Input;
using UpExpenses_Xamarin.Data;
using UpExpenses_Xamarin.Models;
using UpExpenses_Xamarin.Views;
using Xamarin.Forms;

namespace UpExpenses_Xamarin.ViewModels
{
    class ReporteEnviadoPageViewModel:BaseViewModel
    {
        public ICommand Salir { get; set; }

        private string concepto;
        public string Concepto
        {
            get => concepto;
            set => SetProperty(ref concepto, value);
        }

        private int folio;
        public int Folio
        {
            get => folio;
            set => SetProperty(ref folio, value);
        }

        private string emp_release;
        public string Emp_Release
        {
            get => emp_release;
            set => SetProperty(ref emp_release, value);
        }

        private string emp_finance;
        public string Emp_Finance
        {
            get => emp_finance;
            set => SetProperty(ref emp_finance, value);
        }

        public ReporteEnviadoPageViewModel()
        {
            Concepto = SystemDataCache.concepto; ///Previo
            Folio = SystemDataCache.folio;

            Emp_Release = UserData.Emp_Release1;
            Emp_Finance = UserData.Emp_Finance;

            Salir = new Command(() =>
            {
                //App.Current.MainPage = new NavigationPage(new NavigationMasterPage());                
                App.Current.MainPage.Navigation.PopAsync();
            });
        }
    }
}
