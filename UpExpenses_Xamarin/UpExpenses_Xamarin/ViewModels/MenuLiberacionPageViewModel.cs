using Acr.UserDialogs;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using UpExpenses_Xamarin.Models;
using UpExpenses_Xamarin.Views;
using Xamarin.Forms;

namespace UpExpenses_Xamarin.ViewModels
{
    public class MenuLiberacionPageViewModel
    {
        public ObservableCollection<Card> ListDetails { get; set; }

        public ICommand ItemTappedCommand { get; set; }

        public MenuLiberacionPageViewModel()
        {
            LoadList();

            ItemTappedCommand = new Command<Card>((item) =>
            {
                if (item.Name == "Gastos")
                {
                    App.Current.MainPage.DisplayAlert("No disponible", "Esta función no esta disponible por el momento.", "OK");
                }
                else if (item.Name == "Anticipos")
                {
                    //LoadUserRelease();
                    App.Current.MainPage.DisplayAlert("No disponible", "Esta función no esta disponible por el momento.", "OK");
                }
            });
        }



        private void LoadList()
        {
            ListDetails = new ObservableCollection<Card>
            {
                new Card{ImgIcon = "gastos.png", Name ="Gastos"},
                new Card{ImgIcon = "anticipos.png", Name ="Anticipos"}
            };
        }

        private async void OpenAnticiposPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AnticiposPage());
        }

        private async void LoadUserRelease()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Consultando servicio REST");

                var client = new RestClient("https://eis-latam.info/WebService/api/empleados/getanticipos");
                client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", UserData.token));
                var request = new RestRequest($"/" + UserData.usuario + "/" + UserData.Emp_email, Method.GET);
                request.RequestFormat = DataFormat.Json;

                IRestResponse response = await client.ExecuteAsync(request);

                var content = response.Content;
                var statusCode = response.StatusDescription;

                if (statusCode == "OK")
                {
                    var objApiAnticiposList = new List<ApiAnticipos>();

                    objApiAnticiposList = JsonConvert.DeserializeObject<List<ApiAnticipos>>(response.Content);

                    UserData.Emp_Release1 = objApiAnticiposList[0].emp_Release1;
                    UserData.Emp_Finance = objApiAnticiposList[0].emp_Finance;

                    UserDialogs.Instance.HideLoading();

                    OpenAnticiposPage();

                }
                else if (statusCode == null)
                {
                    UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No hay conexión a Internet.", "Ok");
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No se encontró ningún usuario.", "Ok");
                }

                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");
            }
        }
    }
}
