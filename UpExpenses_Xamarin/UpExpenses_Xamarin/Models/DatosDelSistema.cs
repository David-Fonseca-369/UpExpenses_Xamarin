using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpExpenses_Xamarin.Models
{
    public class DatosDelSistema
    {
        [PrimaryKey]
        public int Id { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
