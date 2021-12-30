using System;
using System.Collections.Generic;
using System.Text;
using UpExpenses_Xamarin.Models;
using Xamarin.Forms;

namespace UpExpenses_Xamarin.ViewModels
{
    public class PerfilPageViewModel : BaseViewModel
    {
        public IList<Details> UserDetails  { get; set; }
        public INavigation Navigation { get; set; }

        private string username;
        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        public PerfilPageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            LoadList();
        }

        private void LoadList()
        {
            //Cargar nombre de usuario.
            Username = UserData.nombre;
            //Cargo los datos del usuario
            UserDetails = new List<Details>();
            UserDetails.Add(new Details {Titulo = "Correo electrónico", Resultado = UserData.Emp_email});
            UserDetails.Add(new Details {Titulo = "Departamento", Resultado = UserData.Departamento});
            UserDetails.Add(new Details {Titulo = "Centro de costos", Resultado = Convert.ToString(UserData.Emp_CC)});
            UserDetails.Add(new Details {Titulo = "No. empleado", Resultado = Convert.ToString(UserData.Emp_Num)});
            UserDetails.Add(new Details {Titulo = "Nivel", Resultado = UserData.nivel});
            UserDetails.Add(new Details {Titulo = "Autorización 1", Resultado = UserData.Emp_Release1});
            UserDetails.Add(new Details {Titulo = "Autorización 2", Resultado = UserData.Emp_Finance});
        }
    }
}
