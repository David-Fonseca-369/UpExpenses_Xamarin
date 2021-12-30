using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;
using Xamarin.Forms;
using Plugin.CurrentActivity;
using UpExpenses_Xamarin.Services;
using UpExpenses_Xamarin.Droid.Services;
using Android.Gms.Common;
using Xamarin.Essentials;

namespace UpExpenses_Xamarin.Droid
{
    [Activity(Label = "UpExpenses_Xamarin", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            //Cargar validacion de servicios de Google 
            IsPlayServicesAvailable();
            //Inicializo RG Popup
            Rg.Plugins.Popup.Popup.Init(this);
            //Inicializo la biblioteca.
            UserDialogs.Init(this);

            //Iniciar camara
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            //Registrar mi Dependency service para los toast, interface y clase en Android.
            DependencyService.Register<IMessage, MessageAndroid>();

            //Registro el Dependency service del Serivcio 'Local ´Provider'
            DependencyService.Register<ILocalFileProvider, LocalFileProviderAnroid>();


            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            FormsMaterial.Init(this, savedInstanceState);
            LoadApplication(new App());            
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {                        

            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //Permisos camara
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            bool isGooglePlayService = resultCode != ConnectionResult.Success;
            Preferences.Set("isGooglePlayService", isGooglePlayService);
        }
    }
}