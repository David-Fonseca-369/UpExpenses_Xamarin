﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpExpenses_Xamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpExpenses_Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GastosPendientesPage : ContentPage
    {
        public GastosPendientesPage()
        {
            InitializeComponent();
            BindingContext = new GastosPendientesPageViewModel(Navigation);
        }
    }
}