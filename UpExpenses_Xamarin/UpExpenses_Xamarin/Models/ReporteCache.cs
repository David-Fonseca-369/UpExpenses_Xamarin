using System;
using System.Collections.Generic;
using System.Text;

namespace UpExpenses_Xamarin.Models
{
    public class ReporteCache
    {
        public static DateTime desde { get; set; }
        public static DateTime hasta { get; set; }
        public static string concepto { get; set; }
        public static string tipoGasto { get; set; }
        public  static string anticipo { get; set; }
        public static string moneda { get; set; }
        public static string razon { get; set; }
        public static string nombreCliente { get; set; }
        //Parte 2       
        public static int tipo { get; set; }
        public static string tipoComprobante { get; set; }
        public static string nameFilePDF { get; set; }
        public static string nameFileXML { get; set; }
        public static string base64PDF { get; set; }
        public static string base64XML { get; set; }
        public static string base64Image { get; set; }
        public static string serie_comprobante { get; set; }
        public static DateTime FechaGeneroGasto { get; set; }
        public static double subtotal { get; set; }
        public static double iva { get; set; }
        public static double propina { get; set; }
        public static double total { get; set; }
        public static string gasto { get; set; }
        public static string noComensales { get; set; }
        public static string detalles { get; set; }

    }
}
