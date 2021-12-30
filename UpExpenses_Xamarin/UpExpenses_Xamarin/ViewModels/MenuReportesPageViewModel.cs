using System.Collections.ObjectModel;
using System.Windows.Input;
using UpExpenses_Xamarin.Data;
using UpExpenses_Xamarin.Models;
using UpExpenses_Xamarin.Views;
using Xamarin.Forms;

namespace UpExpenses_Xamarin.ViewModels
{
    public class MenuReportesPageViewModel
    {
        
        public ObservableCollection<Card> ListDetails { get; set; }

        public ICommand ItemTappedCommand { get; set; }

        public MenuReportesPageViewModel()
        {
            LoadList();

            ItemTappedCommand = new Command<Card>((item) =>
            {
                if (item.Name == "Nuevo reporte")
                {
                    OpenNuevoReportePage();
                }
                else if (item.Name == "Reportes pendientes")
                {
                    OpenReportesPendientes();                    
                }
                else if (item.Name == "Reportes enviados")
                {
                    OpenReportesEnviados();
                }
                else if (item.Name == "Autorizados")
                {
                    App.Current.MainPage.DisplayAlert("No disponible", "Esta función no esta disponible por el momento.", "OK");
                }
                else if (item.Name == "Reportes anteriores")
                {
                    OpenReportesAnteriores();
                }
            });
        }
        private void LoadList()
        {
            ListDetails = new ObservableCollection<Card>
            {
                new Card{ImgIcon = "nuevo.png", Name = "Nuevo reporte"},                
                new Card{ImgIcon = "pendiente.png", Name = "Reportes pendientes"},                
                new Card{ImgIcon = "reporte_enviado.png", Name = "Reportes enviados"},
                //new Card{ImgIcon = "autorizado.png", Name = "Autorizados"},                
                new Card{ImgIcon = "anteriores.png", Name = "Reportes anteriores"},
            };
        }
        private void OpenNuevoReportePage()
        {
            //Guarde gasto con hijo
            SystemDataCache.insertWithChildren = true;
            SystemDataCache.kindOfSpendindToSave = 0;
            Application.Current.MainPage.Navigation.PushAsync(new NuevoReporteGastosPage());            
        }
        private void OpenPorAutorizarPage()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ReportesEnviadosPage());
        }
        private void OpenReportesPendientes()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ReportesPendientesPage());
        }

        private void OpenReportesEnviados()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ReportesEnviadosPage());
        }
        private void OpenReportesAnteriores()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ReportesAnterioresPage());
        }
    }
}
