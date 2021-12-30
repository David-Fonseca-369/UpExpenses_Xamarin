using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace UpExpenses_Xamarin.Droid
{
    /// <summary>
    /// MaintLauncher = indica que esta actividad será la que se inicie cuando arranque la aplicación.
    /// NoHistory = Para que la actividad no forme parte de la pila de navegación.
    /// ConfigurationChanges = Detecte los cambios de horientación del dispositivo.
    /// ScreenOrientation = Establezco la orientación como vertical.
    /// </summary>
    [Activity(Label = "Reporte de gastos", Theme = "@style/SplashTheme", MainLauncher = true, NoHistory = true, ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Despues de que se muestre el splash debe inciar la actividad principal
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            // Create your application here
        }
    }
}