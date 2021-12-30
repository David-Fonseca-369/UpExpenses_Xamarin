using SQLite;

namespace UpExpenses_Xamarin.Models
{
   public class TipoGastos
    {
        [PrimaryKey]
        public int Tipo_Id { get; set; }
        public string Tipo_Gasto { get; set; }
        //public decimal Mostrar { get; set; }
    }
}
