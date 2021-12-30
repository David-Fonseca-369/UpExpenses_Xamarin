using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UpExpenses_Xamarin.Models;
using UpExpenses_Xamarin.Services;
using UpExpenses_Xamarin.Views;
using Xamarin.Forms;

namespace UpExpenses_Xamarin.ViewModels
{
    public class DetallesReporteAnteriorPageViewModel : BaseViewModel
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
        public ICommand ItemTappedCommand { get; set; }
        public INavigation Navigation { get; set; }
        public ICommand RefreshCommand { get; set; }


        public DetallesReporteAnteriorPageViewModel(INavigation navigation)
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

            Expenses = new Command(() => navigation.PushAsync(new GastosEnviadosPage()));

            RefreshCommand = new Command(() =>
            {
                //Pull to refresh                
                //Registrar mi dependency en el MainActivity
                Task.Run(async () => await LoadHeadboardReport());
                IsRefreshing = false;
                DependencyService.Get<IMessage>().ShortMessage("¡Actualizado!");
            });

            ItemTappedCommand = new Command<Details>(async (item) =>
            {
                await App.Current.MainPage.DisplayAlert($"{item.Titulo}", $"{item.Resultado}", "OK");
            });
        }

        private async Task<bool> LoadHeadboardReport()
        {
            IList<CabeceraReporte> listHeaderboard = await App.DataBase.HeaderSearchById(CabeceraReporteCache.Id);

            if (listHeaderboard != null)
            {
                string strAnticipo;
                Reportes = new List<Details>();

                Reportes.Add(new Details { Titulo = "Concepto", Resultado = listHeaderboard[0].Concepto });
                Reportes.Add(new Details { Titulo = "Folio", Resultado = listHeaderboard[0].Folio.ToString() });
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

                Reportes.Add(new Details { Titulo = "N° gastos", Resultado = listHeaderboard[0].NoGastos.ToString() });
                Reportes.Add(new Details { Titulo = "Total", Resultado = "$" + listHeaderboard[0].Total.ToString("00.00") });

                //Muestro los releases

                //Validar las autorizaciones                          
                Reportes.Add(new Details { Titulo = "Autorización 1", Resultado = GetTypeOfAuthorization(listHeaderboard[0].Gasto_Release) });
                Reportes.Add(new Details { Titulo = "Autorización 2", Resultado = GetTypeOfAuthorization(listHeaderboard[0].Gasto_Owner) });


                return true;
            }
            return false;
        }
        private string GetTypeOfAuthorization(decimal option)
        {
            if (option == 0)
            {
                return "Por autorizar";
            }
            else if (option == 1)
            {
                return "Autorizado";
            }
            else if (option == 2)
            {
                return "Cancelado";
            }
            else
            {
                return "Desconocido";
            }
        }
    }
}
