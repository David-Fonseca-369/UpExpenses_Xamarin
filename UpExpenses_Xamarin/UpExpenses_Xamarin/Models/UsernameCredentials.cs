using SQLite;

namespace UpExpenses_Xamarin.Models
{

    //[Table("UserCredentials")]
    public class UsernameCredentials
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Save { get; set; }
    }
}
