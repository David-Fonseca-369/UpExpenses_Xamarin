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
    public class ReportesPendientesPageViewModel : BaseViewModel
    {
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }

        public ICommand Delete { get; set; }
        public ICommand Update { get; set; }
        public ICommand Add { get; set; }
        public ICommand RefreshCommand { get; set; }

        private IList<CardReportePendiente> reportesPendientes;
        public IList<CardReportePendiente> ReportesPendientes
        {
            get => reportesPendientes;
            set => SetProperty(ref reportesPendientes, value);
        }

        private bool isVisibleNoContent;
        public bool IsVisibleNoContent
        {
            get => isVisibleNoContent;
            set => SetProperty(ref isVisibleNoContent, value);
        }

        public ICommand ItemTappedCommand { get; set; }
        public INavigation Navigation { get; set; }

        public ReportesPendientesPageViewModel(INavigation navigation)
        {

            //Inicio  temporizador que estará escuchando si se actualizan o no la lista de reportes, en caso de que se elimine uno.
            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                if (TimerData.UpdateReportList)
                {
                    Task.Run(async () =>
                    {
                        if (await LoadReports())
                        {
                            TimerData.UpdateReportList = false;
                        }
                    });
                    return true;
                }
                return true;
            });

            this.Navigation = navigation;

            Task task = new Task(async () => await LoadReports());
            task.Start();

            Delete = new Command(async (id) => await DeleteReport((int)id));

            ItemTappedCommand = new Command<CardReportePendiente>((item) =>
            {
                //Paso el id del reporte
                CabeceraReporteCache.Id = item.Id;

                navigation.PushAsync(new DetallesReportePage());
            });

            Update = new Command(async () =>
            {
                await UpdateReports();
            });

            Add = new Command(async () =>
            {
                //Guarde gasto con hijo
                SystemDataCache.insertWithChildren = true;
                SystemDataCache.kindOfSpendindToSave = 0;
                await App.Current.MainPage.Navigation.PushAsync(new NuevoReporteGastosPage());
            });

            RefreshCommand = new Command(() =>
            {
                Task.Run(async () => await LoadReports());
                IsRefreshing = false;
                DependencyService.Get<IMessage>().ShortMessage("¡Actualizado!");
            });

        }

        private async Task DeleteReport(int id)
        {
            try
            {
                bool option = await App.Current.MainPage.DisplayAlert("Sistema de gastos", "¿Estás seguro que deseas eliminar este reporte?", "Si", "No");
                if (option)
                {
                    await App.DataBase.DeleteWithChildren(id);

                    Task task2 = new Task(async () => await LoadReports());
                    task2.Start();
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        private async Task<bool> LoadReports()
        {
            try
            {
                IList<CardReportePendiente> result = await App.DataBase.GetAllPendindReportsCard(UserData.Emp_Num);

                if (result.Count > 0)
                {
                    ReportesPendientes = new List<CardReportePendiente>();
                    foreach (var item in result)
                    {
                        double total = Convert.ToDouble(item.Total);
                        item.Total = $"${total.ToString("00.00", CultureInfo.InvariantCulture)}";
                        ReportesPendientes.Add(item);
                    }

                    IsVisibleNoContent = false;                    
                }
                else
                {
                    IsVisibleNoContent = true;
                    ReportesPendientes = null;                    
                }

                return true;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error al cargar reportes", ex.Message, "OK");

                //Si no al actualizar entraría en un bucle, esto solo lo hago para que verifique que realmente
                //esta ejecutando esta tarea en el timer.
                return true;
            }
        }


        private async Task UpdateReports()
        {
            try
            {
                Task task3 = new Task(async () => await LoadReports());
                task3.Start();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error al cargar reportes", ex.Message, "OK");
            }
        }
    }
}
