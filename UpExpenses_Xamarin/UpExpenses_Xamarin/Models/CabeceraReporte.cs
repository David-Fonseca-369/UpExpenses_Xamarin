using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace UpExpenses_Xamarin.Models
{
    public class CabeceraReporte
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        //Este tiene un valor cuando ya se ha enviado.
        public decimal Folio { get; set; }
        public decimal Emp_Num { get; set; }        
        public string Concepto { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public string PeriodoReporte { get; set; }
        public string TipoGasto { get; set; }
        public double Anticipo { get; set; }
        public decimal Folio_Anticipo { get; set; }
        public string Viaje { get; set; }
        public string Razon { get; set; }
        public string NombreCliente { get; set; }
        public int NoProyecto { get; set; }
        public int NoGastos { get; set; }
        public double Total { get; set; }
        
        //Liberaciones 
        public decimal Gasto_Release { get; set; }
        public decimal Gasto_Owner { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeInsert | CascadeOperation.CascadeRead)]        
        public List<DetalleReporte> DetallesReporte { get; set; }
    }
}
