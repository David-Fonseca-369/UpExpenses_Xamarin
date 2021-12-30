using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UpExpenses_Xamarin.Data;
using UpExpenses_Xamarin.Models;
using UpExpenses_Xamarin.Views;
using Xamarin.Forms;

namespace UpExpenses_Xamarin.ViewModels
{
    public class GastoPopupPageViewModel : BaseViewModel
    {

        public ICommand ClosePopup { get; set; }
        public ICommand OpenImage { get; set; }
        public ICommand ItemTappedCommand { get; set; }
        public INavigation Navigation { get; set; }
        public IList<Details> DetailsReport { get; set; }

        private ImageSource resultImage;
        public ImageSource ResultImage
        {
            get => resultImage;
            set => SetProperty(ref resultImage, value);
        }

        private bool visibleImage;
        public bool VisibleImage
        {
            get => visibleImage;
            set => SetProperty(ref visibleImage, value);
        }
        public GastoPopupPageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            ClosePopup = new Command(() =>
            {
                navigation.PopAsync();
            });

            //OpenImage = new Command(async () => await ZoomImage());

            //LoadImage();
            LoadDetailsList();

            ItemTappedCommand = new Command<Details>(async (item) =>
            {
                //Si da clic en alguna fila
                if (item.Titulo == "Comprobante")
                {
                   await ZoomImage(DetalleReporteCache.ImagenBase64);
                }
                else if (item.Titulo == "Kilómetros")
                {
                    await ZoomImage(DetalleReporteCache.ImagenKilometros64);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(item.Titulo, item.Resultado, "OK");
                }
            });
        }

        private void LoadDetailsList()
        {
            DetailsReport = new List<Details>();
            DetailsReport.Add(new Details { Titulo = "Tipo", Resultado = DetalleReporteCache.TipoComprobante });
            //Agregar condición aquí
            if (DetalleReporteCache.TipoComprobante == "Comprobante sin requisitos fiscales")
            {
                DetailsReport.Add(new Details { Titulo = "Comprobante", Resultado = "Clic para ver imagen." });
            }
            
            //
            //Si es factura 
            if (DetalleReporteCache.Tipo == 1)
            {
                DetailsReport.Add(new Details { Titulo = "N° Factura/Comprobante", Resultado = DetalleReporteCache.NoFactura });

                //Nombres de archivos, es el mismo ya que se comprueba antes.
                DetailsReport.Add(new Details { Titulo = "Nombre de archivos", Resultado = DetalleReporteCache.NombrePDF });

                DetailsReport.Add(new Details { Titulo = "RFC", Resultado = DetalleReporteCache.RFC });
                DetailsReport.Add(new Details { Titulo = "Razón social", Resultado = DetalleReporteCache.RazonSocial });
                DetailsReport.Add(new Details { Titulo = "UUID", Resultado = DetalleReporteCache.UUID });
                DetailsReport.Add(new Details { Titulo = "N° Certificado SAT", Resultado = DetalleReporteCache.NoCertificadoSAT });
                DetailsReport.Add(new Details { Titulo = "Fecha comprobante", Resultado = DetalleReporteCache.FechaComprobante });
                DetailsReport.Add(new Details { Titulo = "Fecha timbrado", Resultado = DetalleReporteCache.FechaTimbrado });
                DetailsReport.Add(new Details { Titulo = "Método pago", Resultado = DetalleReporteCache.MetodoPago });

                //Detalles              
            }
            if (DetalleReporteCache.Tipo == 2) //Imagen
            {
                DetailsReport.Add(new Details { Titulo = "N° Serie/comprobante", Resultado = DetalleReporteCache.SerieComprobante });
            }



            //Detalles generales
            DetailsReport.Add(new Details { Titulo = "Generó gasto", Resultado = DetalleReporteCache.FechaGeneroGasto });
            DetailsReport.Add(new Details { Titulo = "Subtotal", Resultado = "$" + DetalleReporteCache.Subtotal.ToString("00.00") });

            if (DetalleReporteCache.Tipo == 1)
            {
                DetailsReport.Add(new Details { Titulo = "IVA", Resultado = "$" + DetalleReporteCache.IVA.ToString("00.00") });
            }

            DetailsReport.Add(new Details { Titulo = "Propina", Resultado = "$" + DetalleReporteCache.Propina.ToString("00.00") });

            //Agrego impuesto extra, en caso de que sea comprobante sin requsistos y sin factura.
            if (DetalleReporteCache.Tipo == 3 && CabeceraReporteCache.Viaje == "Nacional")
            {
                double impuestoExtra = DetalleReporteCache.Total * 0.46;
                DetailsReport.Add(new Details { Titulo = "Impuesto extra", Resultado = $"${ impuestoExtra.ToString("00.00")}" });
            }

            DetailsReport.Add(new Details { Titulo = "Total", Resultado = "$" + DetalleReporteCache.Total.ToString("00.00") });

            DetailsReport.Add(new Details { Titulo = "Gasto", Resultado = DetalleReporteCache.Gasto });
            
            if (DetalleReporteCache.NoComensales > 0)
            {
                string title = DetalleReporteCache.Gasto == "HOSPEDAJE" ? "No. de Noches" : "No. de Comensales";

                DetailsReport.Add(new Details { Titulo = title, Resultado = DetalleReporteCache.NoComensales.ToString() });
            }

            if (DetalleReporteCache.Gasto == "COMBUSTIBLE")
            {
                DetailsReport.Add(new Details { Titulo = "Kilómetros", Resultado = "Clic para ver imagen."});
            }
           
            
            DetailsReport.Add(new Details { Titulo = "Detalles", Resultado = DetalleReporteCache.Detalles });


            //Ponerlo de manera provisional y checaer si se agregará o no a una columna

            //Copnsulto el gasto
            //double mount = await App.DataBase.CanSpend(Convert.ToInt32(UserData.nivel), Convert.ToInt32(UserData.emp_type), DetalleReporteCache.Gasto);

            //if (DetalleReporteCache.Total > mount)
            //{
            //    DetailsReport.Add(new Details { Titulo = "Excede", Resultado = "Si" });
            //}
            //else
            //{
            //    DetailsReport.Add(new Details { Titulo = "Excede", Resultado = "No" });
            //}            

            if (DetalleReporteCache.Excede)
            {
                DetailsReport.Add(new Details { Titulo = "Excede", Resultado = "Si" });
            }
            else
            {
                DetailsReport.Add(new Details { Titulo = "Excede", Resultado = "No" });
            }
        }

        private void LoadImage()
        {
            if (DetalleReporteCache.Tipo == 2)
            {
                VisibleImage = true;
                ResultImage = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(DetalleReporteCache.ImagenBase64)));
            }
        }

        private async Task ZoomImage(string imgBase64)
        {
            SystemDataCache.resultImage = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(imgBase64)));
            await App.Current.MainPage.Navigation.PushAsync(new ImagePopupPage());
        }
    }
}
