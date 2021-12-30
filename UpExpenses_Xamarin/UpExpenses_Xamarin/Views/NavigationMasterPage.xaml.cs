using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpExpenses_Xamarin.Data;
using UpExpenses_Xamarin.Models;
using UpExpenses_Xamarin.WebService;
using UpExpenses_Xamarin.WebService.Functions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpExpenses_Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationMasterPage : MasterDetailPage
    {
        public List<MasterPageItem> menuList { get; set; }

        public NavigationMasterPage()
        {
            InitializeComponent();

            //Corro la consulta del token, por si ya se envío o no.
            VerifyToken();

            //Traigo la lista de anticipos y que esten estaticos., ya que si los cunsulto en el momento, no se actualizan.
            LoadListAdvances();

            //Verifico si hay actuzalizaciones en reportes pendientes...
            Task.Run(async () =>
            {
                bool isCorrect = await WebServiceFunctions.SendPendingsReports();
                if (!isCorrect)
                {
                    await DisplayAlert("Sistema de gastos", "Error al solicitar actualización de reportes.", "OK");
                }
            });
             
            //UpdateSentReports();

            lblName.Text = UserData.nombre;
            lblEmail.Text = UserData.Emp_email;

            menuList = new List<MasterPageItem>();

            var page1 = new MasterPageItem() { id = 1, Title = "Perfil", Icon = "details.png" };
            var page2 = new MasterPageItem() { id = 2, Title = "Datos del sistema", Icon = "api.png" };
            var page3 = new MasterPageItem() { id = 3, Title = "Pruebas", Icon = "details.png" };
            var page4 = new MasterPageItem() { id = 4, Title = "Cerrar sesión", Icon = "logout.png" };

            menuList.Add(page1);
            menuList.Add(page2);
            menuList.Add(page3);
            menuList.Add(page4);

            navigationDrawerList.ItemsSource = menuList;

            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MenuPage)));
        }

        async private void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var myselecteditem = e.Item as MasterPageItem;
            switch (myselecteditem.id)
            {
                case 1:
                    await Navigation.PushAsync(new PerfilPage());
                    break;
                case 2:
                    await Navigation.PushAsync(new DatosDelSistemaPage());
                    break;
                case 3:
                    await App.Current.MainPage.DisplayAlert("No disponible", "Esta función no esta disponible por el momento.", "OK");
                    //await Navigation.PushAsync(new AcercaDe());

                    break;
                case 4:
                    bool option = await DisplayAlert("Cerrar sesión", "¿Estás seguro de que deseas cerrar la sesión?", "Si", "No");

                    if (option)
                    {                        
                        App.Current.MainPage = new NavigationPage(new LoginPage());
                    }
                    break;

            }
            ((ListView)sender).SelectedItem = null;
            IsPresented = false;
        }

        private async void VerifyToken()
        {
            try
            {
                TokenDB tokenDB = await App.DataBase.GetToken();
                if (tokenDB != null)
                {
                    if (tokenDB.Enviado == false)
                    {
                        //Envio servicio y actualizo el token.
                        bool result = await RestApiService.SendToken(tokenDB.Token);

                        //Si se envío correctamente, actualizo.
                        if (result)
                        {
                            await App.DataBase.UpdateToken(tokenDB.Token);
                        }
                        else
                        {
                            await DisplayAlert("Sistema de gastos", "Error al enviar el token al servidor.", "OK");
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Sistema de gastos", "El token no ha sido guardado correctamente.\nPor favor notifícalo al área de TI.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Sistema de gastos", "Error: in method Verifytoken(), " + ex.Message, "OK");
            }
        }
        private async void UpdateSentReports()
        {
            bool isCorrect = await RestApiService.Pendientes();
            if (!isCorrect)
            {
                await DisplayAlert("Sistema de gastos", "Error al actulizar los reportes enviados.", "Ok");
            }
        }

        private async void LoadListAdvances()
        {
            try
            {
                var advances = await RestApiService.AdvanceList(UserData.Emp_Num);
                bool isCorrect = advances.Item1;
                IList<AnticipoPorComprobar> advancesList = advances.Item2;
                string message = advances.Item3;

                if (isCorrect)
                {
                    if (advancesList.Count > 0)
                    {
                        AdvanceCache.AnticiposList = advancesList;
                    }
                    else //Como no tiene ningún registro, le agrego uno.
                    {
                        AdvanceCache.AnticiposList.Add(new AnticipoPorComprobar { Folio = -1, Por_Comprobar = "0" });
                    }
                }
                else
                {
                    await DisplayAlert("Sistema de gastos", $"Error al consultar servicio de Anticipos: {message}.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Sistema de gastos", $"Anticipos:  {ex.Message}", "OK");
            }
        }
    }
}
