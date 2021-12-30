using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using UpExpenses_Xamarin.Models;
using UpExpenses_Xamarin.Views;
using Xamarin.Forms;

namespace UpExpenses_Xamarin.ViewModels
{
    public class MenuNuevoPageViewModel
    {
        public ObservableCollection<Card> ListDetails { get; set; }

        public ICommand ItemTappedCommand { get; set; }

        public MenuNuevoPageViewModel()
        {
            LoadList();

            ItemTappedCommand = new Command<Card>((item) =>
            {
                if (item.Name == "Gasto")
                {
                    OpenNuevoGastoPage();
                }
                else if (item.Name == "Anticipo")
                {
                   
                }
            });
        }


        private void LoadList()
        {
            ListDetails = new ObservableCollection<Card>
            {
                new Card{ImgIcon = "gastos.png", Name ="Gasto"},
                new Card{ImgIcon = "anticipos.png", Name ="Anticipo"}
            };
        }

        private async void OpenNuevoGastoPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new NuevoGastoPage());
        }

        
    }
}
