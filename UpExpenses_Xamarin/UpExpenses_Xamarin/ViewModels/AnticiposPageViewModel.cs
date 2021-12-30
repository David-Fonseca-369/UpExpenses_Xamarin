using System.Collections.Generic;
using UpExpenses_Xamarin.Models;

namespace UpExpenses_Xamarin.ViewModels
{    
    public class AnticiposPageViewModel
    {
        public IList<UsuariosAnticipos> Usuarios { get; set; }
       
        public AnticiposPageViewModel()
        {          
            LoadList();
        }     
        private void LoadList()
        {
            Usuarios = new List<UsuariosAnticipos>();

            Usuarios.Add(new UsuariosAnticipos
            {
                Usuario = UserData.Emp_Release1,
                Descripcion = "Usuario liberador",
                ImgIcon = "userBlack.png"
            });

            Usuarios.Add(new UsuariosAnticipos
            {
                Usuario = UserData.Emp_Finance,
                Descripcion = "Usuario finanzas",
                ImgIcon = "userBlack.png"
            });

            Usuarios.Add(new UsuariosAnticipos
            {
                Usuario = UserData.Emp_Finance,
                Descripcion = "Usuario finanzas",
                ImgIcon = "userBlack.png"
            });

            Usuarios.Add(new UsuariosAnticipos
            {
                Usuario = UserData.Emp_Finance,
                Descripcion = "Usuario finanzas",
                ImgIcon = "userBlack.png"
            });

            Usuarios.Add(new UsuariosAnticipos
            {
                Usuario = UserData.Emp_Finance,
                Descripcion = "Usuario finanzas",
                ImgIcon = "userBlack.png"
            });
        }
    }
}
