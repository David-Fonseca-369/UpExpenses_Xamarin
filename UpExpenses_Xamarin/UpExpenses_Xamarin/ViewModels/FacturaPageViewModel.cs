using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UpExpenses_Xamarin.Models;
using Xamarin.Forms;

namespace UpExpenses_Xamarin.ViewModels
{
    public class FacturaPageViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        public ICommand ClosePopup { get; set; }
        
        
        private string noFactura;
        public string NoFactura
        {
            get => noFactura;
            set => SetProperty(ref noFactura, value);
        }

        private string rfc;
        public string RFC
        {
            get => rfc;
            set => SetProperty(ref rfc, value);
        }

        private string razonSocial;
        public string RazonSocial
        {
            get => razonSocial;
            set => SetProperty(ref razonSocial, value);
        }

        private string uuid;
        public string UUID
        {
            get => uuid;
            set => SetProperty(ref uuid, value);
        }

        private string noCertificadoSAT;
        public string NoCertificadoSAT
        {
            get => noCertificadoSAT;
            set => SetProperty(ref noCertificadoSAT, value);
        }

        //Verificar tipo de variable

        private string fechaComprobante;
        public string FechaComprobante
        {
            get => fechaComprobante;
            set => SetProperty(ref fechaComprobante, value);
        }
        private string fechaTimbrado;
        public string FechaTimbrado
        {
            get => fechaTimbrado;
            set => SetProperty(ref fechaTimbrado, value);
        }

        //

        private string metodoPago;
        public string MetodoPago
        {
            get => metodoPago;
            set => SetProperty(ref metodoPago, value);
        }
        private double subtotal;
        public double Subtotal
        {
            get => subtotal;
            set => SetProperty(ref subtotal, value);
        }
        private double iva;
        public double IVA
        {
            get => iva;
            set => SetProperty(ref iva, value);
        }

        private double total;
        public double Total
        {
            get => total;
            set => SetProperty(ref total, value);
        }


        //public FacturaPageViewModel( INavigation navigation)
        //{
        //    Navigation = navigation;
        //}

        public FacturaPageViewModel()
        {
            LoadBill();

            ClosePopup = new Command(() =>
            {
                Navigation.PopPopupAsync();
            });
        }

        private void LoadBill()
        {
            NoFactura = FacturaCache.NoFactura;
            RFC = FacturaCache.RFC;
            RazonSocial = FacturaCache.RazonSocial;
            UUID = FacturaCache.UUID;
            NoCertificadoSAT = FacturaCache.NoCertificadoSAT;
            FechaComprobante = FacturaCache.FechaComprobante;
            FechaTimbrado = FacturaCache.FechaTimbrado;
            MetodoPago = FacturaCache.MetodoPago;
            Subtotal = FacturaCache.Subtotal;
            IVA = FacturaCache.IVA;
            Total = FacturaCache.Total;
        }
    }
}
