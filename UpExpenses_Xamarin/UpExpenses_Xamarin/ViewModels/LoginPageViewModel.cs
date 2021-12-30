using Acr.UserDialogs;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpExpenses_Xamarin.Models;
using UpExpenses_Xamarin.Views;
using Xamarin.Forms;

namespace UpExpenses_Xamarin.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {

        public int Id { get; set; }
        public string NameTemp { get; set; }
        public bool SaveTemp { get; set; }
        public Command Login { get; }

        List<UsernameCredentials> credentials = new List<UsernameCredentials>();

        private string user;

        public string User
        {
            get => user;
            set => SetProperty(ref user, value);
        }

        private string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        private bool visible;
        public bool Visible
        {
            get => visible;
            set => SetProperty(ref visible, value);
        }

        private string messagge;
        public string Messagge
        {
            get => messagge;
            set => SetProperty(ref messagge, value);
        }

        private bool guardarNombre;
        public bool GuardarNombre
        {
            get => guardarNombre;
            set => SetProperty(ref guardarNombre, value);
        }

        private bool passwordController;
        public bool PasswordController
        {
            get => passwordController;
            set => SetProperty(ref passwordController, value);                                       
        }      

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public INavigation Navigation { get; set; }
       
        public LoginPageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            LoadName();

            PasswordController = false;

            Login = new Command(() =>
            {
                LogIn();                
            });

            PasswordController = true;
        }

        private async void LoadName()
        {
            try
            {
                //Le indico el id '1', ya que solo se cargará un registro y este es el de default, en caso de que no desee recordar, truncará la tabla.
                IList<UsernameCredentials> result = await App.DataBase.UserSavedAsync(1);

                //Verificar que haya un registro
                if (result.Count > 0)
                {
                    //Si hay un registro, verificar que el usuario desea recordar su nombre.
                    if (result[0].Save == true)
                    {

                        //Temporal para comparar
                        NameTemp = result[0].Name;
                        SaveTemp = result[0].Save;

                        //Enlazadas
                        User = result[0].Name;
                        GuardarNombre = result[0].Save;
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");
            }
        }
        private async Task<bool> UserSaved()
        {
            try
            {
                //Le indico el id '1', ya que solo se cargará un registro y este es el de default, en caso de que no desee recordar, la truncará la tabla.
                IList<UsernameCredentials> result = await App.DataBase.UserSavedAsync(1);

                //Verificar que haya un registro
                if (result.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");

                return false;
            }
        }

        async void SaveUser()
        {
            if (!string.IsNullOrWhiteSpace(User))
            {
                try
                {
                    int verify = await App.DataBase.SaveNameAsync(new UsernameCredentials
                    {
                        Name = User,
                        Save = true
                    });
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");
                }
            }
        }
        public async void UpdateUser()
        {
            if (!string.IsNullOrWhiteSpace(User))
            {
                try
                {
                    int verify = await App.DataBase.UpdateNameAsync(new UsernameCredentials
                    {
                        Id = 1,
                        Name = User,
                        Save = true
                    });
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");
                }
            }
        }
        async void ValidateCheckBox()
        {
            try
            {
                //Verificar si existe el usuario con el id == 1
                //Crear consulta en la Database
                //Valido la propiedad enlazada al checkbox 'GuardarNombre' 
                if (GuardarNombre)
                {
                    //Verificar si hay un usuario guardado.
                    bool userSaved = await UserSaved();
                    //Si es true significa que desea recordar el nombre
                    //Valido si hay un registro 
                    //Verifico que si hay algún cambio con el nombre temporal                                
                    if (User != NameTemp)
                    {
                        //Verifico si existe un registro

                        if (userSaved)
                        {
                            //Modifico
                            UpdateUser();
                        }
                        else
                        {
                            //Guardo
                            SaveUser();
                        }
                    }
                }
                else
                {
                    //Verificar si hay un usuario guardado.
                    bool userSaved = await UserSaved();
                    //Si es false, significa que no desea recordar el nombre 
                    //Verifico si hay algun registro
                    if (userSaved)
                    {
                        //Mando a truncar tabla
                        int verify = await App.DataBase.DropTableUserNameCredentials();
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");
            }
        }

        private class Error
        {
            public string Title { get; set; }
            public string Result { get; set; }
        }

        public async void LogIn()
        {
            try
            {
                if (Validate())
                {
                    UserDialogs.Instance.ShowLoading("Conectando...");

                    ValidateCheckBox();

                    user = user.ToLower().Trim();
                    var client = new RestClient("https://eis-latam.info/WebService/api/login/token");
                    var request = new RestRequest(Method.POST);
                    request.RequestFormat = DataFormat.Json;
                    request.AddJsonBody(new { user, password });

                    IRestResponse response = await client.ExecuteAsync(request);

                    var content = response.Content;
                    var statusCode = response.StatusDescription;


                    if (statusCode == "OK")
                    {
                        var objDatosUsuarioList = new List<DatosEmpleado>();

                        //Recibe una lista en una arrray [{}]
                        objDatosUsuarioList = JsonConvert.DeserializeObject<List<DatosEmpleado>>(response.Content);
                        //Cuando recibe un onbjeto
                        //var result = JsonConvert.DeserializeObject<DatosEmpleado>(response.Content);

                        //Así recibe en objetos
                        //UserData.token = result.token;

                        //Así recibe en arreglo
                        UserData.token = objDatosUsuarioList[0].token;
                        UserData.nombre = objDatosUsuarioList[0].nombre;
                        UserData.Emp_email = objDatosUsuarioList[0].Emp_email;
                        UserData.usuario = user;

                        UserData.Emp_Num = objDatosUsuarioList[0].Emp_Num;
                        UserData.Departamento = objDatosUsuarioList[0].Departamento;
                        UserData.Emp_CC = objDatosUsuarioList[0].Emp_CC;
                        UserData.nivel = objDatosUsuarioList[0].nivel;
                        UserData.Emp_Release1 = objDatosUsuarioList[0].Emp_Release1;
                        UserData.Emp_Finance = objDatosUsuarioList[0].Emp_Finance;
                        UserData.agrega_pear = Convert.ToInt32(objDatosUsuarioList[0].agrega_pear);
                        UserData.emp_type = objDatosUsuarioList[0].emp_type;

                        Password = "";
                        Messagge = "";
                        Visible = false;

                        UserDialogs.Instance.HideLoading();
                        //Cierro todas la vistas y le dejo la pagina maestra al menú, para que no se pueda regresar al login.
                        App.Current.MainPage = new NavigationPage(new NavigationMasterPage());
                        //await Application.Current.MainPage.Navigation.PushAsync(new NavigationMasterPage());
                    }
                    else if (statusCode == "Not Found")
                    {
                        var result = JsonConvert.DeserializeObject<Error>(response.Content);

                        if (result.Title == "Error al autenticar usuario")
                        {
                            Messagge = "Usuario o password incorrectos o verifica que tu contraseña no haya caducado.";
                            Visible = true;
                            Password = "";
                        }
                        else if (result.Title == "El usuario no existe.")
                        {
                            Messagge = "Él usuario no existe en la base de datos del servidor.";
                            Visible = true;
                            Password = "";
                        }

                        UserDialogs.Instance.HideLoading();
                        
                    }
                    else if (statusCode == null)
                    {
                        UserDialogs.Instance.HideLoading();
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No se pudo obtener respuesta del servidor.\nPosibles errores:\n- No hay conexión a internet.\n- No se pudo establecer la conexión SSL.", "Ok");
                    }
                    //Capturar código de 401
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        Messagge = "Usuario o password incorrectos o verifica que tu contraseña no haya caducado.";
                        Visible = true;
                        Password = "";
                    }
                }

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");
            }

        }
        private bool Validate()
        {


            if (string.IsNullOrWhiteSpace(user) && string.IsNullOrWhiteSpace(password))
            {
                Messagge = "Debes ingresar un usuario y una contraseña.";
                Visible = true;

                return false;

            }
            else if (string.IsNullOrWhiteSpace(user))
            {
                Messagge = "Debes ingresar un usuario.";
                Visible = true;

                return false;

            }
            else if (string.IsNullOrWhiteSpace(password))
            {
                Messagge = "Debes ingresar una contraseña.";
                Visible = true;

                return false;
            }
            else
            {
                Messagge = "";
                Visible = false;

                return true;
            }
        }

        private void LoginSinCredeciales()
        {
            //await Application.Current.MainPage.Navigation.PushAsync(new MenuPage());
            Application.Current.MainPage.Navigation.PushAsync(new NavigationMasterPage());
        }
    }
}
