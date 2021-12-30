using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using UpExpenses_Xamarin.Data;
using UpExpenses_Xamarin.Models;
using UpExpenses_Xamarin.Services;
using UpExpenses_Xamarin.Views;
using UpExpenses_Xamarin.WebService;
using Xamarin.Forms;

namespace UpExpenses_Xamarin.ViewModels
{
    public class DetallesReportePageViewModel : BaseViewModel
    {

        public IList<Details> reportes;
        public IList<Details> Reportes
        {
            get => reportes;
            set => SetProperty(ref reportes, value);
        }
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }
        private bool isVisible;
        public bool IsVisible
        {
            get => isVisible;
            set => SetProperty(ref isVisible, value);
        }

        public ICommand Expenses { get; set; }
        public INavigation Navigation { get; set; }
        public ICommand Delete { get; set; }
        public ICommand Update { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand Send { get; set; }
        public ICommand ItemTappedCommand { get; set; }


        public DetallesReportePageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            //Inicio timer, para escuchar en caso de que se elimine un gasto y los destalles se actualicen automáticamente.
            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                if (TimerData.UpdateReportDetails)
                {
                    Task.Run(async () =>
                    {

                        bool isCorrect = await LoadHeadboardReport();

                        if (isCorrect)
                        {
                            TimerData.UpdateReportDetails = false;
                        }
                    });
                    return true;
                }
                return true;
            });

            Task.Run(async () => await LoadHeadboardReport());

            Expenses = new Command(() => navigation.PushAsync(new GastosPendientesPage()));

            Delete = new Command(async () => await DeleteReport());

            Update = new Command(() => Task.Run(async () => await LoadHeadboardReport()));

            RefreshCommand = new Command(() =>
            {
                //Pull to refresh                
                //Registrar mi dependency en el MainActivity
                Task.Run(async () => await LoadHeadboardReport());
                IsRefreshing = false;
                DependencyService.Get<IMessage>().ShortMessage("¡Actualizado!");
            });

            //Enviar reporte
            Send = new Command(async () =>
            {
                bool option = await App.Current.MainPage.DisplayAlert("Enviar reporte", "El reporte se enviará al sistema, por lo que ya no podrás hacer cambios.", "OK", "Cancelar");

                if (option)
                {
                    UserDialogs.Instance.ShowLoading("Enviando reporte...");
                    var result = await RestApiService.SendReport(CabeceraReporteCache.Id);
                    UserDialogs.Instance.HideLoading();

                    if (result.Item1)
                    {
                        //Reporte enviado correctamente
                        //Le envío el folio
                        SystemDataCache.folio = result.Item2;
                        SystemDataCache.concepto = result.Item3;

                        //Actualizo el folio del reporte
                        await Navigation.PopAsync();
                        await App.Current.MainPage.Navigation.PushAsync(new ReporteEnviadoPage());

                        //Actulice lista de reportes pendientes.
                        TimerData.UpdateReportList = true;

                    }
                    else
                    {
                        //Traerr el mensaje de error de la tupla.
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Error al enviar reporte", "OK");
                    }
                }
            });

            ItemTappedCommand = new Command<Details>(async (item) =>
            {                
                await App.Current.MainPage.DisplayAlert(item.Titulo, item.Resultado, "Ok");
            });
        }

        private async Task DeleteReport()
        {
            bool option = await App.Current.MainPage.DisplayAlert("Sistema de gastos", "¿Estás seguro que deseas eliminar este reporte? ", "Si", "No");

            if (option)
            {
                bool isDelete = await App.DataBase.DeleteWithChildren(CabeceraReporteCache.Id);

                if (isDelete)
                {
                    TimerData.UpdateReportList = true;

                    await App.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "No se puedo eliminar el registro.", "OK");
                }
            }
        }
        private async Task<bool> LoadHeadboardReport()
        {
            IList<CabeceraReporte> listHeaderboard = await App.DataBase.HeaderSearchById(CabeceraReporteCache.Id);

            if (listHeaderboard != null)
            {                
                Reportes = new List<Details>();

                Reportes.Add(new Details { Titulo = "Concepto", Resultado = listHeaderboard[0].Concepto });
                Reportes.Add(new Details { Titulo = "Período", Resultado = listHeaderboard[0].PeriodoReporte });
                Reportes.Add(new Details { Titulo = "Tipo de gasto", Resultado = listHeaderboard[0].TipoGasto });

                //Si agrega anticipo
                if (listHeaderboard[0].TipoGasto == "ANTICIPO MXP")
                {
                    Reportes.Add(new Details { Titulo = "Anticipo", Resultado = "$" + listHeaderboard[0].Anticipo.ToString("00.00", CultureInfo.InvariantCulture) });
                }

                Reportes.Add(new Details { Titulo = "Viaje", Resultado = listHeaderboard[0].Viaje });

                //Guardo el viaje, para el impuesto adicional.
                CabeceraReporteCache.Viaje = listHeaderboard[0].Viaje;

                Reportes.Add(new Details { Titulo = "Razón", Resultado = listHeaderboard[0].Razon });

                //Si va con un cliente muestra el nombre
                if (listHeaderboard[0].Razon == "Cliente")
                {
                    Reportes.Add(new Details { Titulo = "Nombre del cliente", Resultado = listHeaderboard[0].NombreCliente });
                }

                //Si agrega número de proyecto
                if (listHeaderboard[0].NoProyecto > 0)
                {
                    Reportes.Add(new Details { Titulo = "N° Proyecto", Resultado = listHeaderboard[0].NoProyecto.ToString() });
                }
                              
                Reportes.Add(new Details { Titulo = "N° Gastos", Resultado = listHeaderboard[0].NoGastos.ToString() });
                Reportes.Add(new Details { Titulo = "Total", Resultado = "$" + listHeaderboard[0].Total.ToString("00.00") });

                return true;
            }
            return false;
        }
    }
}
