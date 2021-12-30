using System;

namespace UpExpenses_Xamarin.Models
{
    public class CabeceraReporteCache
    {
        //Solo debe hacer referencia a los 'gastos pendientes' ya que los datos deben ser persistentes.
        public static int Id { get; set; }
        public static decimal Folio { get; set; }
        public static decimal Emp_Num { get; set; }        
        public static string Concepto { get; set; }
        public static DateTime Desde { get; set; }
        public static DateTime Hasta { get; set; }
        public static string PeriodoReporte { get; set; }
        public static string TipoGasto { get; set; }
        public static double Anticipo { get; set; }
        public static decimal Folio_Anticipo { get; set; }
        public static string Viaje { get; set; }
        public static string Razon { get; set; }
        public static string NombreCliente { get; set; }
        public static int NoProyecto { get; set; }
        public static int NoGastos { get; set; }
        public static double Total { get; set; }
        
        //Liberaciones 
        public decimal Gasto_Release { get; set; }
        public decimal Gasto_Owner { get; set; }
    }
}
