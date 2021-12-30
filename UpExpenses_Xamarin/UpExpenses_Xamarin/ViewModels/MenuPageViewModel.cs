using System.Collections.ObjectModel;
using System.Windows.Input;
using UpExpenses_Xamarin.Models;
using UpExpenses_Xamarin.Views;
using Xamarin.Forms;

namespace UpExpenses_Xamarin.ViewModels
{
    public class MenuPageViewModel : BaseViewModel
    {
        public ObservableCollection<Card> ListDetails { get; set; }

        public ICommand ItemTappedCommand { get; set; }

        private string user;
        public string User
        {
            get => user;
            set => SetProperty(ref user, value);
        }

        public MenuPageViewModel()
        {
            LoadName();
            LoadList();

            ItemTappedCommand = new Command<Card>(async (item) =>
            {

                if (item.Name == "Anticipos")
                {
                    await App.Current.MainPage.DisplayAlert("No disponible", "Esta función no esta disponible por el momento.", "OK");
                    //Application.Current.MainPage.Navigation.PushAsync(new MenuLiberacionPage());
                    //Ancticipos

                }
                else if (item.Name == "Reportes")
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new MenuReportesPage());
                    //Reportes
                }
            });


        }

        private void LoadList()
        {
            ListDetails = new ObservableCollection<Card>
            {
                new Card{ImgIcon = "anticipo.png", Name = "Anticipos"}, //Anticipos
                new Card{ImgIcon = "reportes.png", Name = "Reportes"}

            };
        }

        private void LoadName()
        {
            User = UserData.nombre;
        }

    }
}
