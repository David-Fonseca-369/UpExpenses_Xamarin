using System;
using System.Collections.Generic;
using System.Text;

namespace UpExpenses_Xamarin.Models
{
    public class Gasto
    {
        public int Gastos_Cve { get; set; }
        public string Gastos_Concepto { get; set; }
        public string deducible { get; set; }
        public string no_deducible { get; set; }
        public double MXP_1 { get; set; }
        public double MXP_2 { get; set; }
        public double MXP_3 { get; set; }
        public double MXP_4 { get; set; }
        public int emp_type { get; set; }
        public int? TERCEROS { get; set; }
    }
}
