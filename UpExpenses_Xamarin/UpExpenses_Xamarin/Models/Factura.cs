using System;
using System.Collections.Generic;
using System.Text;

namespace UpExpenses_Xamarin.Models
{
    public class Factura
    {
        public string Serie { get; set; }
        public  int Folio { get; set; }
        public  string RFC { get; set; }
        public  string RFCReceptor { get; set; }
        public  string RazonSocial { get; set; }
        public  string UUID { get; set; }
        public  string NoCertificadoSAT { get; set; }
        public  string FechaComprobante { get; set; }
        public  string FechaTimbrado { get; set; }
        public string MetodoPago { get; set; }
        public double Subtotal { get; set; }
        public  double IVA { get; set; }
        public double Total { get; set; }
    }
}
