
namespace UpExpenses_Xamarin.Models
{
   public class CardGasto
    {
        public int Id { get; set; }
        public int Tipo { get; set; }
        public string TipoComprobante { get; set; }
        public string NombrePDF { get; set; }
        public string NombreXML { get; set; }
        public string PDFBase64 { get; set; }
        public string XMLBase64 { get; set; }
        public string ImagenBase64 { get; set; }
        public string SerieComprobante { get; set; }       
        public string FechaGeneroGasto { get; set; }
        public double Subtotal { get; set; }
        public double IVA { get; set; }
        public double Propina { get; set; }
        public double Total { get; set; }
        public string Gasto { get; set; }
        public string ImagenKilometros64 { get; set; }
        public int NoComensales { get; set; }
        public string Detalles { get; set; }
        //Si es factura
        public string Serie { get; set; }
        public int Folio { get; set; }
        public string NoFactura { get; set; }
        public string RFC { get; set; }
        public string RFCReceptor { get; set; }
        public string RazonSocial { get; set; }
        public string UUID { get; set; }
        public string NoCertificadoSAT { get; set; }
        public string FechaComprobante { get; set; }
        public string FechaTimbrado { get; set; }
        public string MetodoPago { get; set; }
        public bool Excede { get; set; }
       //Gasto en string
        public string StrTotal  { get; set; }   
    }
}
