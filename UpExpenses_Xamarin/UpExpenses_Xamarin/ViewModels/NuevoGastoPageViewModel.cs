using Acr.UserDialogs;
using Plugin.FilePicker;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UpExpenses_Xamarin.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UpExpenses_Xamarin.ViewModels
{
    public class NuevoGastoPageViewModel : BaseViewModel
    {
        public ICommand SelectPDF { get; set; }
        public ICommand SelectXML { get; set; }
        public ICommand Load { get; set; }



        private string nameFilePDF;

        public string NameFilePDF
        {
            get => nameFilePDF;
            set => SetProperty(ref nameFilePDF, value);
        }

        private string nameFileXML;

        public string NameFileXML
        {
            get => nameFileXML;
            set => SetProperty(ref nameFileXML, value);
        }

        public NuevoGastoPageViewModel()
        {
            SelectPDF = new Command(() =>
            {
                //LoadPDF();
                SelectFilePDF();

            });

            SelectXML = new Command(() =>
            {
                //LoadXML();
                SelectFileXML();
            });

            Load = new Command(() =>
            {
                //Cargar factura
                LoadBill();
            });
        }
        private async Task<bool> LoadPDF()
        {
            try
            {
                var client = new RestClient("https://eis-latam.info/WebService/api/archivos/cargarArchivoPDF");
                var request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(new
                {
                    user = UserData.usuario,
                    fileBase64 = BillData.PDFBase64
                });

                IRestResponse response = await client.ExecuteAsync(request);
                var content = response.Content;
                var statusCode = response.StatusDescription;

                if (statusCode == "OK")
                {                    
                    return true;
                }
                else
                {                   
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Error al cargar archivo o su tamaño supera 1MB.", "Ok");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");
                return false;
            }           
        }

        private async Task<bool> LoadXML()
        {
            try
            {
                var client = new RestClient("https://eis-latam.info/WebService/api/archivos/cargarArchivoXML");
                var request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(new
                {
                    user = UserData.usuario,
                    fileBase64 = BillData.XMLBase64
                });

                IRestResponse response = await client.ExecuteAsync(request);
                var content = response.Content;
                var statusCode = response.StatusDescription;

                if (statusCode == "OK")
                {                  
                    return true;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Error al cargar el XML o su tamaño supera 1MB.", "Ok");
                    return false;
                }

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");
                return false;
            }

        }
        private async void SelectFilePDF()
        {
            try
            {
                var file = await CrossFilePicker.Current.PickFile();

                if (file != null)
                {
                    bool verify = file.FileName.Contains(".pdf");
                    if (verify)
                    {
                        try
                        {
                            byte[] array = file.DataArray;
                            string fileToBase64 = Convert.ToBase64String(array);

                            BillData.nameFilePDF = file.FileName.Replace(".pdf", "");
                            BillData.PDFBase64 = fileToBase64;

                            NameFilePDF = BillData.nameFilePDF;


                        }
                        catch (Exception ex)
                        {
                            await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debes seleccionar un archivo PDF.", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");
            }
        }
        private async void SelectFileXML()
        {
            try
            {
                var file = await CrossFilePicker.Current.PickFile();

                if (file != null)
                {
                    bool verify = file.FileName.Contains(".xml");
                    if (verify)
                    {
                        try
                        {
                            byte[] array = file.DataArray;
                            string fileToBase64 = Convert.ToBase64String(array);

                            BillData.nameFileXML = file.FileName.Replace(".xml", "");
                            BillData.XMLBase64 = fileToBase64;

                            NameFileXML = BillData.nameFileXML;
                        }
                        catch (Exception ex)
                        {
                            await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debes seleccionar un archivo XML.", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");
            }
        }
        private async void LoadBill()
        {
            //Verifico que haya agregado archivos
            if (Verify())
            {
                //Comparar nombres
                if (BillData.nameFilePDF == BillData.nameFileXML)
                {
                    //Continua con la carga de archivos                    
                    bool verifyPDF = await LoadPDF();
                    bool verifyXML = await LoadXML();

                    if(verifyPDF && verifyPDF)
                    {
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Archivos subidos con éxito.", "Ok");

                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Los archivos no coinciden.", "Ok");
                }
            }
        }

        private bool Verify()
        {
            if (string.IsNullOrWhiteSpace(NameFilePDF) && string.IsNullOrWhiteSpace(NameFileXML))
            {
                App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe seleccionar un archivo PDF y un XML.", "Ok");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(NameFilePDF))
            {
                App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe seleccionar un archivo PDF.", "Ok");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(NameFileXML))
            {
                App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe seleccionar un archiov XML.", "Ok");
                return false;
            }
            else
            {
                return true;
            }
        }


    }
}
