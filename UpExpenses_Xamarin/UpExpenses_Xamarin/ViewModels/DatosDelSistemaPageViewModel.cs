using Acr.UserDialogs;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UpExpenses_Xamarin.Models;
using UpExpenses_Xamarin.WebService;
using Xamarin.Forms;

namespace UpExpenses_Xamarin.ViewModels
{
    public class DatosDelSistemaPageViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        public ICommand Update { get; set; }

        List<string> listTiposGasto = new List<string>();

        private string lastUpdate;
        public string LastUpdate
        {
            get => lastUpdate;
            set => SetProperty(ref lastUpdate, value);
        }
        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public DatosDelSistemaPageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            Task loadLastUpdateAsync = new Task(async () => await LoadLastUpdate());
            loadLastUpdateAsync.Start();

            Update = new Command(async () =>
            {
                await UpdateLastUpdate();
            });
        }

        private async Task UpdateLastUpdate()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Actualizando...");

                IList<DatosDelSistema> resultLastUpdate = await App.DataBase.LoadLastUpdate();
                if (resultLastUpdate.Count > 0) //Existe, modifico
                {
                    //Esperar una tupla, para el mensaje de error
                    var resultUpdateGastos = await RestApiService.UpdateGastosAsync();
                    bool isCorrect = resultUpdateGastos.Item1;
                    string message = resultUpdateGastos.Item2;

                    //si se guarda correctamente 'tipo_gastos', actualizo la fecha de la última modificación.
                    if (await RestApiService.UpdateTipoGastos() && isCorrect)
                    {
                        await App.DataBase.UpdateLastUpdate();

                        resultLastUpdate = await App.DataBase.LoadLastUpdate();
                        foreach (var item in resultLastUpdate)
                        {
                            if (item.Id == 1)
                            {
                                LastUpdate = "Última comprobación: " + item.LastUpdate.ToLongDateString() + " a las " + item.LastUpdate.ToString("hh:mm:ss tt");
                                UserDialogs.Instance.HideLoading();
                                break;
                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                                await App.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar la fecha.", "OK");
                            }
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();

                        await App.Current.MainPage.DisplayAlert("Error", $"{message}\nNo se pudo actualizar la fecha.", "OK");
                    }
                }
                else //No existe tabla, agrego
                {
                    //si se guarda correctamente 'tipo_gastos', agrego la fecha de la última modificación.
                    if (await  RestApiService.UpdateTipoGastos())
                    {
                        await App.DataBase.InsertLastUpdate();

                        resultLastUpdate = await App.DataBase.LoadLastUpdate();

                        foreach (var item in resultLastUpdate)
                        {
                            if (item.Id == 1)
                            {
                                LastUpdate = "Última comprobación: " + item.LastUpdate.ToLongDateString() + " a las " + item.LastUpdate.ToString("hh:mm:ss tt");
                                UserDialogs.Instance.HideLoading();
                                break;
                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                                await App.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar la fecha.", "OK");
                            }
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await App.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar la fecha.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private async Task LoadLastUpdate()
        {
            IList<DatosDelSistema> result = await App.DataBase.LoadLastUpdate();
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    if (item.Id == 1)
                    {
                        LastUpdate = "Última comprobación: " + item.LastUpdate.ToLongDateString() + " a las " + item.LastUpdate.ToString("hh:mm:ss tt");
                        break;
                    }
                }
            }
        }     
    }
}
