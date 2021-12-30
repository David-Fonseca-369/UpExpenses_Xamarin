using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public partial class AcercaDe : ContentPage
    {
        public AcercaDe()
        {
            InitializeComponent();

            //Para la recepción de mensajes de Firebase 
            MessagingCenter.Subscribe<string>(this, "MensajeFirebase", (value) =>
            {

                //Invocamos el hilo principal
                Device.BeginInvokeOnMainThread(() =>
                {
                    lblMensajeFirebase.Text = "Mensaje: " + value;
                });
            });
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            List<DetalleReporte> ListaDetalle = await App.DataBase.GetAllChildren();
            List<CabeceraReporte> ListaCabeceraconHijos = await App.DataBase.GetAllReporteWithChildren();
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            int idReporte = Convert.ToInt32(lblIdReporte.Text);
            IList<DetalleReporte> all = await App.DataBase.GetAllChildren();

            IList<DetalleReporte> result = await App.DataBase.GetChildren(idReporte);
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            IList<TipoGastos> result = await App.DataBase.LoadTipoGastos();
        }

        private async void Button_Clicked_3(object sender, EventArgs e)
        {
            await App.DataBase.DeleteTipoGastos();
            await App.DataBase.LoadMaster();
        }

        private async void Button_Clicked_4(object sender, EventArgs e)
        {
            await RestApiService.UpdateGastos();
        }

        private async void Button_Clicked_5(object sender, EventArgs e)
        {
            IList<Gasto> resultGastos = await App.DataBase.LoadGastos((int)UserData.emp_type);
        }

        private async void Button_Clicked_6(object sender, EventArgs e)
        {
            int edad = 0;
            string result = await DisplayPromptAsync("Sistema de gastos", "¿Deseas ingresar tu edad?", "Si", "No", "Ingresa tu edad", keyboard: Keyboard.Numeric);
            if (!string.IsNullOrEmpty(result))
            {
                edad = Convert.ToInt32(result);
            }
            else
            {
                edad = 0;
            }

        }

        private async void Button_Clicked_7(object sender, EventArgs e)
        {
            await RestApiService.AdvanceList(67414);
            //Vacio 67414
            //contenido 67804
        }

        private async void Button_Clicked_8(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("¿Qué deseas hacer?", "Cancelar", null, "Guardar y salir", "Guardar y agregar otro gasto.");
        }

        private async void Button_Clicked_9(object sender, EventArgs e)
        {
            //Traer último ID
            int id = await App.DataBase.LatestId();
        }

        private async void Button_Clicked_10(object sender, EventArgs e)
        {
            await App.DataBase.HeaderSearchById(Convert.ToInt32(txtId.Text.Trim()));
        }

        private async void Button_Clicked_11(object sender, EventArgs e)
        {
            //Retorna -1 en caso de no encontrar datos.
            var result = await App.DataBase.ChildrenById(Convert.ToInt32(txtIdChildren.Text));
        }

        private void Button_Clicked_12(object sender, EventArgs e)
        {
            if (imgSample.Scale > MinimumHeightRequest)
            {
                RestoreScaleValues();
            }
            else
            {
                imgSample.AnchorX = imgSample.AnchorX = 0.5;
                imgSample.ScaleTo(2, 250, Easing.CubicInOut);
            }
        }
        void RestoreScaleValues()
        {
            Content.ScaleTo(Scale, 50, Easing.CubicInOut);
            Content.TranslateTo(0.5, 0.5, 250, Easing.CubicInOut);


            Content.TranslationX = 0.5;
            Content.TranslationY = 0.5;

            //xOffset = Content.TranslationX;
            //yOffset = Content.TranslationY;
        }

        private async void Button_Clicked_13(object sender, EventArgs e)
        {
            IList<CardReportePendiente> result = await App.DataBase.GetAllPendindReportsCard(UserData.Emp_Num);
        }

        private async void Button_Clicked_14(object sender, EventArgs e)
        {
            string travel = await App.DataBase.HeaderColumnData(Convert.ToInt32(txtIdViaje.Text), "Viaje");
        }

        private void Button_Clicked_15(object sender, EventArgs e)
        {
        }

        private async void Button_Clicked_16(object sender, EventArgs e)
        {
            double canSpend = await App.DataBase.CanSpend(Convert.ToInt32(txtNivel.Text), Convert.ToInt32(UserData.emp_type), txtConceptoGasto.Text);
        }

        private async void Button_Clicked_17(object sender, EventArgs e)
        {
            int id = await App.DataBase.Gasto_Cve(txtConceptoGasto.Text, Convert.ToInt32(txtNivel.Text));
        }

        private async void Button_Clicked_18(object sender, EventArgs e)
        {
            TokenDB result = await App.DataBase.GetToken();
        }

        private async void Button_Clicked_19(object sender, EventArgs e)
        {
            bool result = await FolioExists(Convert.ToDecimal(txtFolioAnticipo.Text));
        }
        private async Task<bool> FolioExists(decimal folioAnticipo)
        {
            IList<CabeceraReporte> reportsList = await App.DataBase.GetAllPendingReports(UserData.Emp_Num);

            bool result = false;

            if (reportsList.Count > 0)
            {
                foreach (var item in reportsList)
                {
                    if (item.Folio_Anticipo == folioAnticipo)
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        private async void Button_Clicked_20(object sender, EventArgs e)
        {
            //IList<decimal> folios_pendientes =  await App.DataBase.GetAllPendindReportsFolios(UserData.Emp_Num);
            IList<decimal> folios_anteriores = await App.DataBase.GetAllPreviousReportsFolios(UserData.Emp_Num);
            //bool isCorrect = await WebServiceFunctions.SendPendingsReports();        
        }

        private async void ModifyReport()
        {
            if (await App.DataBase.UpdateFolio(5, 33))
            {
                await App.Current.MainPage.DisplayAlert("Guardado", "Modificado exitosamente 5", "OK");
            }
            if (await App.DataBase.UpdateFolio(11, 55))
            {
                await App.Current.MainPage.DisplayAlert("Guardado", "Modificado exitosamente 11", "OK");
            }


        }

        private void Button_Clicked_21(object sender, EventArgs e)
        {
            ModifyReport();
        }
        private async void UpdateReportSent()
        {            
            if (await App.DataBase.UpdateReportSent(55, 1, 1))
            {
                await App.Current.MainPage.DisplayAlert("Guardado", "Modificado exitosamente 55", "OK");
            }
        }

        private void Button_Clicked_22(object sender, EventArgs e)
        {
            UpdateReportSent();
        }
    }
}
