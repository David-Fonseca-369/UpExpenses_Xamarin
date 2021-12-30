using System;
using System.Collections.Generic;
using System.Text;

namespace UpExpenses_Xamarin.Models
{
    public static class DetalleReporteCache
    {
        public static int Id { get; set; }
        public static int  IdCabecera { get; set; }
        public static int Tipo { get; set; }
        public static string TipoComprobante { get; set; }
        public static string NombrePDF { get; set; }
        public static string NombreXML { get; set; }
        public static string PDFBase64 { get; set; }
        public static string XMLBase64 { get; set; }
        public static string ImagenBase64 { get; set; }
        public static string SerieComprobante { get; set; }
        //public DateTime FechaGeneroGasto { get; set; }
        public static string FechaGeneroGasto { get; set; }
        public static double Subtotal { get; set; }
        public static double IVA { get; set; }
        public static double Propina { get; set; }
        public static double Total { get; set; }
        public static string Gasto { get; set; }
        //Imagen de kilometros 
        public static string ImagenKilometros64 { get; set; }
        public static int NoComensales { get; set; }
        public static string Detalles { get; set; }

        //Si es factura
        public static string Serie { get; set; }
        public static int  Folio { get; set; }
        public static string NoFactura { get; set; }
        public static string RFC { get; set; }
        public static string RFCReceptor { get; set; }
        public static string RazonSocial { get; set; }
        public static string UUID { get; set; }
        public static string NoCertificadoSAT { get; set; }
        public static string FechaComprobante { get; set; }
        public static string FechaTimbrado { get; set; }
        public static string MetodoPago { get; set; }
        public static bool  Excede { get; set; }
    }
}
