using System;
using System.Collections.Generic;
using System.Text;

namespace UpExpenses_Xamarin.Models
{
    public static class FacturaCache
    {
        public static string NoFactura { get; set; }
        public static string Serie { get; set; }
        public static int Folio { get; set; }
        public static string RFC { get; set; }
        public static string RFCReceptor { get; set; }
        public static string RazonSocial { get; set; }
        public static string UUID { get; set; }
        public static string NoCertificadoSAT { get; set; }
        public static string FechaComprobante { get; set; }
        public static string FechaTimbrado { get; set; }
        public static string MetodoPago { get; set; }
        public static double Subtotal { get; set; }
        public static double IVA { get; set; }
        public static double Total { get; set; }
    }
}
