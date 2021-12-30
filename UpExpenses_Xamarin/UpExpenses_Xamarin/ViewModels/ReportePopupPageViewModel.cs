using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UpExpenses_Xamarin.Models;
using Xamarin.Forms;

namespace UpExpenses_Xamarin.ViewModels
{
    public class ReportePopupPageViewModel
    {
        public ICommand ClosePopup { get; set; }
        public INavigation Navigation { get; set; }
        public IList<Details> DetailsReport { get; set; }

        public ReportePopupPageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            ClosePopup = new Command(() =>
            {
                navigation.PopAsync();
            });
            LoadDetailsList();
        }
        private void LoadDetailsList()
        {
            DetailsReport = new List<Details>();
            DetailsReport.Add(new Details {Titulo = "Tipo", Resultado = ReporteCache.tipoComprobante});
            
            //Si es factura 
            if(ReporteCache.tipo == 1)
            {
                DetailsReport.Add(new Details { Titulo = "N° Factura/Comprobante", Resultado = FacturaCache.NoFactura});

                //Nombres de archivos, es el mismo ya que se comprueba antes.
                DetailsReport.Add(new Details { Titulo = "Nombre de archivos", Resultado = ReporteCache.nameFilePDF});

                DetailsReport.Add(new Details { Titulo = "RFC", Resultado = FacturaCache.RFC});
                DetailsReport.Add(new Details { Titulo = "Razón social", Resultado = FacturaCache.RazonSocial});
                DetailsReport.Add(new Details { Titulo = "UUID", Resultado = FacturaCache.UUID});
                DetailsReport.Add(new Details { Titulo = "N° Certificado SAT", Resultado = FacturaCache.NoCertificadoSAT});
                DetailsReport.Add(new Details { Titulo = "Fecha comprobante", Resultado = FacturaCache.FechaComprobante});
                DetailsReport.Add(new Details { Titulo = "Fecha timbrado", Resultado = FacturaCache.FechaTimbrado});
                DetailsReport.Add(new Details { Titulo = "Método pago", Resultado = FacturaCache.MetodoPago});              

                //Detalles              
            }
            if (ReporteCache.tipo == 2) //Imagen
            {
                DetailsReport.Add(new Details { Titulo = "N° Serie/comprobante", Resultado = ReporteCache.serie_comprobante});
            }

            //Detalles generales
            DetailsReport.Add(new Details { Titulo = "Generó gasto", Resultado = ReporteCache.FechaGeneroGasto.ToString("dd/MM/yyyy") });
            DetailsReport.Add(new Details { Titulo = "Subtotal", Resultado = "$" + ReporteCache.subtotal.ToString()});
            DetailsReport.Add(new Details { Titulo = "IVA", Resultado = "$" + ReporteCache.iva.ToString()});
            DetailsReport.Add(new Details { Titulo = "Propina", Resultado = "$" + ReporteCache.propina.ToString()});
            DetailsReport.Add(new Details { Titulo = "Total", Resultado = "$" + ReporteCache.total.ToString()});
            DetailsReport.Add(new Details { Titulo = "Gasto", Resultado = ReporteCache.gasto});

            if (string.IsNullOrWhiteSpace(ReporteCache.noComensales))
            {
                ReporteCache.noComensales = "N/A";
            }
            DetailsReport.Add(new Details { Titulo = "N° Comensales", Resultado = ReporteCache.noComensales});
            DetailsReport.Add(new Details { Titulo = "Detalles", Resultado = ReporteCache.detalles});
        }
    }
}
