using System;
using System.IO;
using UpExpenses_Xamarin.Data;
using UpExpenses_Xamarin.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpExpenses_Xamarin
{
    public partial class App : Application
    {
        static DataBase database;

        public static DataBase DataBase
        {
            get
            {
                if(database == null)
                {
                    database = new DataBase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "UpExpenses.db3"));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            //Debo iniciarlo con NavigationPage si no, no responde a las tranciciones de pagina.
            MainPage = new NavigationPage(new LoginPage());
            //MainPage = new NavigationPage(new NuevoReporteGastosPage2());
            //MainPage = new NavigationPage(new NuevoReporteGastosPage2());


        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
