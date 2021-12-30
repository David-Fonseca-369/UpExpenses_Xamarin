using System;
using System.Collections.Generic;
using System.Text;

namespace UpExpenses_Xamarin.Models
{
    public class Reporte
    {
        /**Tipos 
        1 = Factura con requisitos fiscales
        2 = Comprobante sin requisitos fiscales
        3 = Sin comprobante/Sin factura
        **/
        //Fechas
        public DateTime desde { get; set; }
        public DateTime hasta { get; set; }
        public string Concepto { get; set; }
        public string tipoGasto { get; set; }
        public string anticipo { get; set; }
        public string moneda { get; set; }
        public string razon { get; set; }
        public string nombreCliente { get; set; }
        public int tipo { get; set; }
        public string nameFilePDF { get; set; }
        public string nameFileXML { get; set; }
        public string base64PDF { get; set; }
        public string base64XML { get; set; }
        public string base64Image { get; set; }
        public string serie_comprobante { get; set; }
        public string FechaGeneroGasto { get; set; }
        public double subtotal { get; set; }
        public double iva { get; set; }
        public double propina { get; set; }
        public double total { get; set; }
        public string gasto { get; set; }
        public string noComensales { get; set; }
        public string detalles { get; set; }
    }
}
