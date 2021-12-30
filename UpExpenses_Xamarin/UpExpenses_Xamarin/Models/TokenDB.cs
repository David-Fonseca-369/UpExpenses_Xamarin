using SQLite;

namespace UpExpenses_Xamarin.Models
{
    public class TokenDB
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Token { get; set; }
        public bool Enviado { get; set; }
    }
}
