using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using UpExpenses_Xamarin.Data;
using UpExpenses_Xamarin.Models;
using UpExpenses_Xamarin.Services;
using UpExpenses_Xamarin.Views;
using Xamarin.Forms;

namespace UpExpenses_Xamarin.ViewModels
{
    public class GastosEnviadosPageViewModel : BaseViewModel
    {
        private IList<CardGasto> gastosEnviados;
        public IList<CardGasto> GastosEnviados
        {
            get => gastosEnviados;
            set => SetProperty(ref gastosEnviados, value);
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }

        private string gastoTotal;
        public string GastoTotal
        {
            get => gastoTotal;
            set => SetProperty(ref gastoTotal, value);
        }

        public ICommand ItemTappedCommand { get; set; }
        public ICommand AddExpense { get; set; }
        public ICommand Delete { get; set; }
        public ICommand Update { get; set; }
        public INavigation Navigation { get; set; }
        public ICommand RefreshCommand { get; set; }

        public GastosEnviadosPageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;


            //Inicio timer, paras que escuche en caso de que haya cambio en la lista de gastos.
            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                if (TimerData.UpdateExpensesList)
                {
                    Task.Run(async () =>
                    {
                        if (await LoadList())
                        {
                            TimerData.UpdateExpensesList = false;
                        }
                    });
                    return true;
                }
                return true;
            });

            //Cargar lista de manera asincrona 
            UpdateList();

            //Enviar de manera dinamica, modificar
            ItemTappedCommand = new Command<CardGasto>(async (item) =>
            {
                DetalleReporteCache.Id = item.Id;

                //Mando el id, para que consulte el tipo de viaje. Impuesto adicional
                DetalleReporteCache.IdCabecera = CabeceraReporteCache.Id;


                DetalleReporteCache.Tipo = item.Tipo;
                DetalleReporteCache.TipoComprobante = item.TipoComprobante;
                DetalleReporteCache.NombrePDF = item.NombrePDF;
                DetalleReporteCache.NombreXML = item.NombreXML;
                DetalleReporteCache.ImagenBase64 = item.ImagenBase64;
                DetalleReporteCache.SerieComprobante = item.SerieComprobante;
                DetalleReporteCache.FechaGeneroGasto = item.FechaGeneroGasto;
                DetalleReporteCache.Subtotal = item.Subtotal;
                DetalleReporteCache.IVA = item.IVA;
                DetalleReporteCache.Propina = item.Propina;
                DetalleReporteCache.Total = item.Total;
                DetalleReporteCache.Gasto = item.Gasto;
                DetalleReporteCache.ImagenKilometros64 = item.ImagenKilometros64;
                DetalleReporteCache.NoComensales = item.NoComensales;
                DetalleReporteCache.Detalles = item.Detalles;
                DetalleReporteCache.Excede = item.Excede;

                //Si es factura
                if (item.Tipo == 1)
                {
                    DetalleReporteCache.NoFactura = item.NoFactura;
                    DetalleReporteCache.RFC = item.RFC;
                    DetalleReporteCache.RFCReceptor = item.RFCReceptor;
                    DetalleReporteCache.RazonSocial = item.RazonSocial;
                    DetalleReporteCache.UUID = item.UUID;
                    DetalleReporteCache.NoCertificadoSAT = item.NoCertificadoSAT;
                    DetalleReporteCache.FechaComprobante = item.FechaComprobante;
                    DetalleReporteCache.FechaTimbrado = item.FechaTimbrado;
                    DetalleReporteCache.MetodoPago = item.MetodoPago;
                }
                await navigation.PushAsync(new GastoPopupPage());
            });

            AddExpense = new Command(async () =>
            {
                SystemDataCache.firstSelection = true;

                //No desea guradar con hijo, ya que solo guradará el gasto.
                SystemDataCache.insertWithChildren = false;

                //Indico que el tipo de gasto a guardar será como solo gasto.
                SystemDataCache.kindOfSpendindToSave = 2;
                SystemDataCache.idHeadBoard = CabeceraReporteCache.Id;

                //Consulto el viaje, si es nacional o extranjero, para el impuesto extra.

                CabeceraReporteCache.Viaje = await App.DataBase.HeaderColumnData(CabeceraReporteCache.Id, "Viaje");

                //Ya no envío los gastos así, si no que los consulto con el Id, ya que estos se mantienen
                //con información vieja.


                await navigation.PushAsync(new NuevoReporteGastosPage2());
            });

            Delete = new Command(async (id) =>
            {
                bool option = await App.Current.MainPage.DisplayAlert("Sistema de gastos", "¿Estás seguro que deseas eliminar este gasto?", "Si", "No");
                if (option)
                {
                    bool isDelete = await DeleteChildren((int)id);
                    if (isDelete)
                    {
                        UpdateList();
                        TimerData.UpdateReportDetails = true;
                    }
                }
            });

            Update = new Command(() => Task.Run(async () => await LoadList()));

            RefreshCommand = new Command(() =>
            {
                Task.Run(async () => await LoadList());
                IsRefreshing = false;
                DependencyService.Get<IMessage>().ShortMessage("¡Actualizado!");
            });
        }

        private async Task<bool> LoadList()
        {
            try
            {
                IList<CardGasto> result = await App.DataBase.GetChildrenCard(CabeceraReporteCache.Id);

                if (result.Count > 0)
                {
                    GastosEnviados = new List<CardGasto>();
                    foreach (var item in result)
                    {
                        item.StrTotal = $"${item.Total.ToString("00.00", CultureInfo.InvariantCulture)}";
                        GastosEnviados.Add(item);
                    }
                }
                else
                {
                    //Limpiar lista, al eliminar que da un último dato y no lo limpia.
                    GastosEnviados = new List<CardGasto>();
                    GastosEnviados = null;
                }
                return true;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

                //solo es para verififcar le timer, si pongo false entra en bucle y actualizará cada 100 milisegundos.
                return true;
            }
        }

        private async Task<bool> DeleteChildren(int idChildren)
        {
            try
            {
                //Consulto el total del gasto 
                var resultChildren = await App.DataBase.ChildrenById(idChildren);
                int idReceived = resultChildren.Item1;
                double totalExpense = resultChildren.Item2;

                //Traer Id de cabecera, para restar el gasto y el total de la cabecera.
                var resultSearch = await App.DataBase.HeadboardById(CabeceraReporteCache.Id);
                int idHeadboard = resultSearch[0].Id;
                int numberExpense = resultSearch[0].NoGastos;
                double total = resultSearch[0].Total;

                //Elimino el hijo.
                await App.DataBase.DeleteChildren(idChildren);

                //Resto el número de gastos y el total.
                if (CabeceraReporteCache.Id == idHeadboard)
                {
                    await App.DataBase.UpdateReport(numberExpense - 1, total - totalExpense, idHeadboard);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }
        private void UpdateList()
        {
            Task.Run(async () => await LoadList());
        }
    }
}
