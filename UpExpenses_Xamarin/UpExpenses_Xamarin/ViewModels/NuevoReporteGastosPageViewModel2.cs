using Acr.UserDialogs;
using Newtonsoft.Json;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Plugin.Media.Abstractions;
using RestSharp;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using UpExpenses_Xamarin.Data;
using UpExpenses_Xamarin.Models;
using UpExpenses_Xamarin.Services;
using UpExpenses_Xamarin.Views;
using UpExpenses_Xamarin.WebService;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace UpExpenses_Xamarin.ViewModels
{
    public class NuevoReporteGastosPageViewModel2 : BaseViewModel
    {
        //Cantidad de gastos guardadados
        public static int noSpendings = 1;

        //Declaro mi lista para los gastos pendientes, temporales
        List<DetalleReporte> listPendingDetails = new List<DetalleReporte>();

        //Opción en la que se encuentra
        private static string selectionActualTipoComprobante { get; set; }
        public ICommand SelectPDF { get; set; }
        public ICommand SelectXML { get; set; }
        public ICommand UploadBill { get; set; }
        public ICommand SeeBill { get; set; }
        public INavigation Navigation { get; set; }
        public ICommand SelectImage { get; set; }
        public ICommand TakePhoto { get; set; }
        public ICommand SelectImageKilometres { get; set; }
        public ICommand TakePhotoKilometres { get; set; }
        public ICommand OpenImage { get; set; }
        public ICommand OpenImageKilometres { get; set; }
        public Command TappedCommandTipoComprobante { get; set; }
        public Command TappedCommandTipoGasto { get; set; }
        public Command Save { get; set; }
        public Command Exit { get; set; }

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

        private string selectedTipoComprobante;
        public string SelectedTipoComprobante
        {
            get => selectedTipoComprobante;
            set => SetProperty(ref selectedTipoComprobante, value);
        }
        private bool visibleConFactura;
        public bool VisibleConFactura
        {
            get => visibleConFactura;
            set => SetProperty(ref visibleConFactura, value);
        }

        private bool visibleImage;
        public bool VisibleImage
        {
            get => visibleImage;
            set => SetProperty(ref visibleImage, value);
        }

        private bool visibleSuccessfulBill;
        public bool VisibleSuccessfulBill
        {
            get => visibleSuccessfulBill;
            set => SetProperty(ref visibleSuccessfulBill, value);
        }
        private bool visibleDetalleGasto;
        public bool VisibleDetalleGasto
        {
            get => visibleDetalleGasto;
            set => SetProperty(ref visibleDetalleGasto, value);
        }

        private bool visibleImageKilometres;
        public bool VisibleImageKilometres
        {
            get => visibleImageKilometres;
            set => SetProperty(ref visibleImageKilometres, value);
        }

        ///Detalles de gasto///
        ///
        private string serie_Comprobante;
        public string Serie_Comprobante
        {
            get => serie_Comprobante;
            set => SetProperty(ref serie_Comprobante, value);
        }

        private bool serie_ComprobanteVisible;
        public bool Serie_ComprobanteVisible
        {
            get => serie_ComprobanteVisible;
            set => SetProperty(ref serie_ComprobanteVisible, value);
        }

        //Editar detalles
        private bool editarDetalles;
        public bool EditarDetalles
        {
            get => editarDetalles;
            set => SetProperty(ref editarDetalles, value);
        }


        private double subtotal;
        public double Subtotal
        {
            get => subtotal;
            set
            {
                SetProperty(ref subtotal, value);
                TextChangedCommand.Execute(subtotal);
            }
        }
        private double iva;
        public double IVA
        {
            get => iva;
            set
            {
                SetProperty(ref iva, value);
                TextChangedCommand.Execute(iva);
            }
        }

        private double propina;
        public double Propina
        {
            get => propina;
            set
            {
                SetProperty(ref propina, value);
                TextChangedCommand.Execute(propina);
            }
        }
        private double impuestoExtra;
        public double ImpuestoExtra
        {
            get => impuestoExtra;
            set
            {
                SetProperty(ref impuestoExtra, value);
                // TextChangedCommand.Execute(impuestoExtra);
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        private string titleComensales;
        public string TitleComensales
        {
            get => titleComensales;
            set => SetProperty(ref titleComensales, value);
        }



        //Calcular total
        public Command TextChangedCommand => new Command(() => TextChanged());
        private void TextChanged()
        {
            try
            {
                Total = Subtotal + IVA + Propina;
                ImpuestoExtra = (Subtotal + Propina) * 0.46;
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Sistema gastos", ex.Message, "OK");
            }
        }

        private double total;
        public double Total
        {
            get => total;
            set => SetProperty(ref total, value);
        }




        private DateTime fechaGeneroGasto;
        public DateTime FechaGeneroGasto
        {
            get => fechaGeneroGasto;
            set => SetProperty(ref fechaGeneroGasto, value);
        }

        private string detalles;
        public string Detalles
        {
            get => detalles;
            set => SetProperty(ref detalles, value);
        }

        private bool visibleNoComensales;
        public bool VisibleNoComensales
        {
            get => visibleNoComensales;
            set => SetProperty(ref visibleNoComensales, value);
        }
        private bool visibleImpuestoExtra;
        public bool VisibleImpuestoExtra
        {
            get => visibleImpuestoExtra;
            set => SetProperty(ref visibleImpuestoExtra, value);
        }

        private string selectedTipoGasto;
        public string SelectedTipoGasto
        {
            get => selectedTipoGasto;
            set => SetProperty(ref selectedTipoGasto, value);
        }

        private string selectedNoComensales;
        public string SelectedNoComensales
        {
            get => selectedNoComensales;
            set => SetProperty(ref selectedNoComensales, value);
        }

        private ImageSource resultImage;
        public ImageSource ResultImage
        {
            get => resultImage;
            set => SetProperty(ref resultImage, value);
        }
        private ImageSource resultImageKilometres;
        public ImageSource ResultImageKilometres
        {
            get => resultImageKilometres;
            set => SetProperty(ref resultImageKilometres, value);
        }
        private bool visibleIVA;
        public bool VisibleIVA
        {
            get => visibleIVA;
            set => SetProperty(ref visibleIVA, value);
        }
        private IList<string> gastosList;
        public IList<string> GastosList
        {
            get => gastosList;
            set => SetProperty(ref gastosList, value);
        }

        public IList<string> TipoComprobante
        {
            get
            {
                return new List<string> { "Factura con requisitos fiscales", "Comprobante sin requisitos fiscales", "Sin comprobante/Sin factura" };
            }
        }
        //public IList<string> TipoGasto
        //{
        //    get
        //    {
        //        //Pendiente, será con el API
        //        return new List<string> { "ALIMENTOS", "AUTOBUS", "COMBUSTIBLE", "EQUIPAJE", "ESTACIONAMIENTO", "HERRAMIENTAS",
        //            "HOSPEDAJE", "MATERIAL DE INSTALACION", "PAQUETERIA Y ENVIOS", "PEAJES", "TAXI/UBER", "TRANSPORTACION AEREO", "TRANSPORTE PUBLICO SIN COMPROBANTE"};
        //    }
        //}
        public IList<string> NoComensales
        {
            get
            {
                //Pendiente, será por API (Verififcar)
                return new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };
            }
        }

        public NuevoReporteGastosPageViewModel2(INavigation navigation)
        {
            this.Navigation = navigation;

            CurrentDate();

            Task _loadPickerGastos = new Task(async () => await LoadPickerGastos());
            _loadPickerGastos.Start();

            //Guardar
            Save = new Command(async () =>
           {
               if (!string.IsNullOrWhiteSpace(selectionActualTipoComprobante))
               {
                   if (await VerifySave(selectionActualTipoComprobante))
                   {
                       //Se insertará con hijo
                       if (SystemDataCache.insertWithChildren)
                       {
                           //Despliego dialogo de opciones 
                           //async Task<string> Option() => await App.Current.MainPage.DisplayActionSheet("¿Qué desea hacer?", "Cancelar", null, "Sólo guardar", "Guardar y salir");                           

                           if (!await ExceedsTip(Subtotal, Propina, SelectedTipoGasto, Convert.ToInt32(UserData.nivel), Convert.ToInt32(SelectedNoComensales), CabeceraReporteCache.Viaje, DocumentType(selectionActualTipoComprobante)))
                           {
                               if (await SaveReport(selectionActualTipoComprobante))
                               {
                                   //Indico que ya se ha guardado la cabecera con su hijo.
                                   SystemDataCache.insertWithChildren = false;

                                   //Indico que la opcion sera uno, para poder guardar otro desde esta ventana.
                                   SystemDataCache.kindOfSpendindToSave = 1;


                                   //EL await y el async son importantes para que no se salte a la otra pantalla, antes debes aceptar el Display Alert
                                   await App.Current.MainPage.DisplayAlert("Sistema de gastos", "¡Reporte guardado!", "OK");
                                   //App.Current.MainPage = new NavigationPage(new NavigationMasterPage());
                               }
                               else
                               {
                                   await App.Current.MainPage.DisplayAlert("Error", "Error al guardar reporte", "OK");
                               }
                           }
                           else
                           {
                               await App.Current.MainPage.DisplayAlert("Sistema de gastos", "La propina excede la política.", "OK");
                           }
                       }
                       else //Solo se guardara el gasto
                       {
                           //Guardará obteniendo el último Id.
                           if (SystemDataCache.kindOfSpendindToSave == 1)
                           {
                               var saveSpending = await OptionSpending(1);
                               bool result = saveSpending.Item1;
                               string message = saveSpending.Item2;

                               if (result)
                               {
                                   await App.Current.MainPage.DisplayAlert("Sistema de gastos", message, "OK");
                               }
                               else
                               {
                                   await App.Current.MainPage.DisplayAlert("Error", message, "OK");
                               }
                           }
                           //Guardará obteniendo el id seleccionado en "gastos".
                           else if (SystemDataCache.kindOfSpendindToSave == 2)
                           {
                               var saveSpending = await OptionSpending(2);
                               bool result = saveSpending.Item1;
                               string message = saveSpending.Item2;

                               if (result)
                               {
                                   await App.Current.MainPage.DisplayAlert("Sistema de gastos", message, "OK");
                               }
                               else
                               {
                                   await App.Current.MainPage.DisplayAlert("Sistema de gastos", message, "OK");
                               }
                           }
                           else
                           {
                               await App.Current.MainPage.DisplayAlert("Error", "No se pudo obtener la opción del gasto a guardar.", "OK");
                           }
                       }
                   }
               }
               else
               {
                   await App.Current.MainPage.DisplayAlert("Debe seleccionar un tipo de comprobante.", "Éxito", "OK");
               }
           });

            SelectPDF = new Command(() =>
            {
                //SelectFilePDF();
                SelectFilePDFFilterAsync();
            });

            SelectXML = new Command(() =>
            {
                //SelectFileXML();
                SelectFileXMLFilterAsync();
            });

            TappedCommandTipoComprobante = new Command(() =>
            {
                SelectionType(SelectedTipoComprobante);
            });

            TappedCommandTipoGasto = new Command(() =>
            {
                if (SelectedTipoGasto == "ALIMENTOS" || SelectedTipoGasto == "TAXI / UBER")
                {
                    TitleComensales = "No. de comensales";
                    VisibleNoComensales = true;
                    VisibleImageKilometres = false;
                }
                else if (SelectedTipoGasto == "HOSPEDAJE")
                {
                    TitleComensales = "No. de noches";
                    VisibleNoComensales = true;
                    VisibleImageKilometres = false;
                }
                else if (SelectedTipoGasto == "COMBUSTIBLE")
                {
                    VisibleImageKilometres = true;
                    VisibleNoComensales = false;
                }
                else
                {
                    VisibleImageKilometres = false;
                    VisibleNoComensales = false;
                    CleanNumberDiners();
                }
            });

            UploadBill = new Command(async () =>
            {
                await UploadBillAsync();
            });

            SeeBill = new Command(() =>
            {
                Navigation.PushPopupAsync(new FacturaPage());
            });

            //1 = gallery
            //2 = photo    

            SelectImage = new Command(async () =>
            {
                //Seleccionar Imagen
                //PickImageAsync();
                await LoadImage(1);
            });

            TakePhoto = new Command(async () =>
            {
                //Tomar foto
                //TakePhotoAsync();

                await LoadImage(2);

            });

            SelectImageKilometres = new Command(async () =>
            {
                await LoadImageKilometres(1);
            });

            TakePhotoKilometres = new Command(async () =>
            {
                await LoadImageKilometres(2);
            });

            ///////////// 

            OpenImage = new Command(async () => await OpenImageZoom(1));
            OpenImageKilometres = new Command(async () => await OpenImageZoom(2));



            Exit = new Command(() =>
            {
                ExitPage(SystemDataCache.kindOfSpendindToSave);
            });

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
                            await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "OK");
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debes seleccionar un archivo PDF.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "OK");
            }
        }
        private async void SelectFilePDFFilterAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Cargando archivos PDF...");

                var options = new PickOptions
                {
                    PickerTitle = "Please select a PDF file",
                    FileTypes = FilePickerFileType.Pdf
                };

                var result = await FilePicker.PickAsync(options);
                if (result != null)
                {
                    bool verify = result.FileName.Contains(".pdf");
                    if (verify)
                    {
                        try
                        {
                            string path = result.FullPath;
                            var bytes = DependencyService.Get<ILocalFileProvider>().GetFileBytes(path);
                            string base64 = Convert.ToBase64String(bytes);

                            BillData.nameFilePDF = result.FileName.Replace(".pdf", "");
                            BillData.PDFBase64 = base64;

                            NameFilePDF = BillData.nameFilePDF;
                            UserDialogs.Instance.HideLoading();
                        }
                        catch (Exception ex)
                        {
                            UserDialogs.Instance.HideLoading();
                            await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "OK");
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debes seleccionar un archivo PDF.", "OK");
                    }
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debes seleccionar un archivo PDF.", "OK");
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "OK");
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
                            await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "OK");
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debes seleccionar un archivo XML.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", $"{ex.Message}, {ex.StackTrace}", "OK");
            }
        }
        private async void SelectFileXMLFilterAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Cargando archivos XML...");

                //Especififcar archivo XML
                //var customFileType =
                //    new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>> {
                //        { DevicePlatform.Android, new[] {"xml", ".xml", ".XML", "XML"} }                        
                //    });

                var options = new PickOptions
                {
                    PickerTitle = "Please select a XML file",
                    //FileTypes = customFileType
                };

                var result = await FilePicker.PickAsync(options);
                if (result != null)
                {
                    bool verify = result.FileName.Contains(".xml");
                    if (verify)
                    {
                        try
                        {
                            string path = result.FullPath;
                            var bytes = DependencyService.Get<ILocalFileProvider>().GetFileBytes(path);
                            string base64 = Convert.ToBase64String(bytes);

                            BillData.nameFileXML = result.FileName.Replace(".xml", "");
                            BillData.XMLBase64 = base64;

                            NameFileXML = BillData.nameFileXML;
                            UserDialogs.Instance.HideLoading();
                        }
                        catch (Exception ex)
                        {
                            UserDialogs.Instance.HideLoading();
                            await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "OK");
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debes seleccionar un archivo XML.", "OK");
                    }
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debes seleccionar un archivo XML.", "OK");
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message + " " + ex.StackTrace, "OK");
            }
        }
        private async Task UploadBillAsync()
        {
            if (Verify())
            {
                //Cargar archivos al servidor
                try
                {
                    UserDialogs.Instance.ShowLoading("Conectando...");

                    var client = new RestClient("https://eis-latam.info/WebService/api/archivos/cargarFactura");
                    client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", UserData.token));
                    var request = new RestRequest(Method.POST);
                    request.RequestFormat = DataFormat.Json;
                    request.AddJsonBody(new
                    {
                        user = UserData.usuario,
                        PDFBase64 = BillData.PDFBase64,
                        XMLBase64 = BillData.XMLBase64
                    });

                    IRestResponse response = await client.ExecuteAsync(request);
                    var statusCode = response.StatusDescription;

                    if (statusCode == "OK")
                    {
                        var result = JsonConvert.DeserializeObject<Factura>(response.Content);

                        //Cargar datos de factura a clase estática 
                        FacturaCache.Serie = result.Serie;
                        FacturaCache.Folio = result.Folio;
                        FacturaCache.NoFactura = result.Serie + result.Folio;
                        FacturaCache.RFC = result.RFC;
                        FacturaCache.RFCReceptor = result.RFCReceptor;
                        FacturaCache.RazonSocial = result.RazonSocial;
                        FacturaCache.UUID = result.UUID;
                        FacturaCache.NoCertificadoSAT = result.NoCertificadoSAT;
                        FacturaCache.FechaComprobante = result.FechaComprobante;
                        FacturaCache.FechaTimbrado = result.FechaTimbrado;
                        FacturaCache.MetodoPago = result.MetodoPago;
                        FacturaCache.Subtotal = result.Subtotal;
                        FacturaCache.IVA = result.IVA;
                        FacturaCache.Total = result.Total;

                        //Activar boton ver factura
                        VisibleSuccessfulBill = true;

                        //Activar contenedor de detalles
                        DetailLoad();
                        VisibleDetalleGasto = true;


                        UserDialogs.Instance.HideLoading();
                    }
                    else if (statusCode == "Bad Request")
                    {
                        var result = JsonConvert.DeserializeObject<Response_HTTP>(response.Content);
                        UserDialogs.Instance.HideLoading();

                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", result.Resultado, "OK");
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Error al cargar archivo.", "OK");
                        //return false;
                    }
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "OK");
                }
            }
            //return false;
        }
        private bool Verify()
        {
            if (!string.IsNullOrWhiteSpace(SelectedTipoComprobante))
            {
                //Si es con requisitos fiscales
                if (SelectedTipoComprobante == "Factura con requisitos fiscales")
                {
                    if (string.IsNullOrWhiteSpace(NameFilePDF) && string.IsNullOrWhiteSpace(NameFileXML))
                    {
                        App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe seleccionar un archivo PDF y un XML.", "OK");
                        return false;
                    }
                    else if (string.IsNullOrWhiteSpace(NameFilePDF))
                    {
                        App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe seleccionar un archivo PDF.", "OK");
                        return false;
                    }
                    else if (string.IsNullOrWhiteSpace(NameFileXML))
                    {
                        App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe seleccionar un archiov XML.", "OK");
                        return false;
                    }
                    else if (BillData.nameFilePDF != BillData.nameFileXML)
                    {
                        App.Current.MainPage.DisplayAlert("Sistema de gastos", "Los archivos no coinciden.", "OK");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe seleccionar un tipo de comprobante.", "OK");
                return false;
            }

            return false;
        }
        private async Task<string> PickImageAsync()
        {
            try
            {
                FileData fileData = await CrossFilePicker.Current.PickFile();

                if (fileData != null)
                {
                    if (fileData.FileName.Contains(".png") || fileData.FileName.Contains(".jpg") || fileData.FileName.Contains(".jpeg"))
                    {
                        //ResultImage = ImageSource.FromStream(() => fileData.GetStream());
                        byte[] array = fileData.DataArray;
                        string imageBase64 = Convert.ToBase64String(array);

                        return imageBase64;
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debes seleccionar una imagen (PNG, JPG o JPEG).", "OK");
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "OK");
                return null;
            }
        }
        private async Task<string> PickImageFilterAsync()
        {
            try
            {
                //Options FilePicker 
                var options = new PickOptions
                {
                    PickerTitle = "Please select a image file",
                    FileTypes = FilePickerFileType.Images
                };

                var resultImage = await FilePicker.PickAsync(options);
                if (resultImage != null)
                {
                    if (resultImage.FileName.Contains(".png") || resultImage.FileName.Contains(".jpg") || resultImage.FileName.Contains(".jpeg"))
                    {
                        //Convierto a base64
                        string path = resultImage.FullPath;
                        var base64Bytes = DependencyService.Get<ILocalFileProvider>().GetFileBytes(path);

                        string fileBase64 = Convert.ToBase64String(base64Bytes);

                        return fileBase64;
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debes seleccionar una imagen (PNG, JPG o JPEG).", "OK");
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "OK");
                return null;
            }
        }
        private async Task<string> PickImageAsync2()
        {
            try
            {
                FileData fileData = await CrossFilePicker.Current.PickFile();

                if (fileData != null)
                {
                    if (fileData.FileName.Contains(".png") || fileData.FileName.Contains(".jpg") || fileData.FileName.Contains(".jpeg"))
                    {
                        //ResultImage = ImageSource.FromStream(() => fileData.GetStream());
                        byte[] array = fileData.DataArray;
                        string imageBase64 = Convert.ToBase64String(array);

                        return imageBase64;
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debes seleccionar una imagen (PNG, JPG o JPEG).", "OK");
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "OK");
                return null;
            }
        }
        private async Task<string> TakePhotoAsync()
        {
            try
            {
                var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());

                if (photo != null)
                {
                    //ResultImage = ImageSource.FromStream(() =>
                    //{
                    //    return photo.GetStream();
                    //});

                    using (var memoryStream = new MemoryStream())
                    {
                        photo.GetStream().CopyTo(memoryStream);
                        photo.Dispose();
                        byte[] array = memoryStream.ToArray();
                        string photoBase64 = Convert.ToBase64String(array);

                        return photoBase64;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "OK");
                return null;
            }
        }
        private void DetailLoad()
        {
            Subtotal = FacturaCache.Subtotal;
            IVA = FacturaCache.IVA;
            Total = FacturaCache.Total;
        }
        private void DetailClean()
        {
            Subtotal = 0;
            IVA = 0;
            Total = 0;
            Propina = 0;
            Detalles = "";
            ResultImage = null;

            //Ver como eliminar temporales, ya sea en el servidor o aquí
            NameFilePDF = "";
            NameFileXML = "";
            Serie_Comprobante = "";
        }
        private bool IsEmpty()
        {
            if (string.IsNullOrWhiteSpace(NameFilePDF) &&
                string.IsNullOrWhiteSpace(NameFileXML) &&
                ResultImage == null &&
                string.IsNullOrWhiteSpace(Serie_Comprobante) &&
                Subtotal == 0 &&
                ImpuestoExtra == 0 &&
                IVA == 0 &&
                Total == 0 &&
                Propina == 0 &&
                string.IsNullOrWhiteSpace(Detalles) &&
                string.IsNullOrWhiteSpace(SelectedTipoGasto))
            {
                return true;
            }
            return false;
        }

        private async void SelectionType(string tipoComprobante)
        {
            if ((selectionActualTipoComprobante != null) && (selectionActualTipoComprobante != SelectedTipoComprobante) && (SystemDataCache.firstSelection == false))
            {
                //checar si estan limpios o no
                if (IsEmpty())
                {
                    DetailClean();
                    if (tipoComprobante == "Factura con requisitos fiscales")
                    {
                        //Quito el entry de No. Serie/Comprobante 
                        Serie_ComprobanteVisible = false;

                        VisibleConFactura = true;
                        VisibleImage = false;
                        VisibleDetalleGasto = false;

                        //Que el IVA sea visible 
                        VisibleIVA = true;
                        VisibleImpuestoExtra = false;
                        //Le indico que la seleccion actual sera esta.
                        selectionActualTipoComprobante = SelectedTipoComprobante;

                        //Como es factura la factura ya trae el importe, IVA y total, por ello dejo los campos como solo lectura.
                        EditarDetalles = true;
                    }
                    else if (tipoComprobante == "Comprobante sin requisitos fiscales")
                    {
                        //Que el IVA no sea visible 
                        VisibleIVA = false;

                        if (CabeceraReporteCache.Viaje == "Nacional")
                        {
                            VisibleImpuestoExtra = true;
                        }
                        else
                        {
                            VisibleImpuestoExtra = false;
                        }

                        //Hago visible el entry de No. Serie/Comprobante
                        Serie_ComprobanteVisible = true;

                        VisibleImage = true;
                        VisibleConFactura = false;
                        VisibleSuccessfulBill = false;
                        VisibleDetalleGasto = false;

                        //Poder editar detalles
                        EditarDetalles = false;

                        //Le indico que la seleccion actual sera esta.
                        selectionActualTipoComprobante = SelectedTipoComprobante;
                    }
                    else
                    {
                        //Que el IVA no sea visible 
                        VisibleIVA = false;

                        if (CabeceraReporteCache.Viaje == "Nacional")
                        {
                            VisibleImpuestoExtra = true;
                        }
                        else
                        {
                            VisibleImpuestoExtra = false;
                        }
                        //Quito el entry de No. Serie/Comprobante 
                        Serie_ComprobanteVisible = false;

                        //Poder editar detalles
                        EditarDetalles = false;

                        VisibleImage = false;
                        VisibleConFactura = false;
                        VisibleSuccessfulBill = false;
                        VisibleDetalleGasto = true;

                        //Le indico que la seleccion actual sera esta.
                        selectionActualTipoComprobante = SelectedTipoComprobante;
                    }
                }
                else
                {
                    bool option = await App.Current.MainPage.DisplayAlert("Cambiar de opción", "¿Estás seguro de que deseas salir?" +
                       "\n\nLas modificaciones realizadas se perderán.", "Si", "No");
                    if (option)
                    {
                        DetailClean();
                        if (tipoComprobante == "Factura con requisitos fiscales")
                        {
                            //Quito el entry de No. Serie/Comprobante 
                            Serie_ComprobanteVisible = false;

                            VisibleConFactura = true;
                            VisibleImage = false;
                            VisibleDetalleGasto = false;

                            //Que el IVA sea visible 
                            VisibleIVA = true;
                            VisibleImpuestoExtra = false;
                            //Le indico que la seleccion actual sera esta.
                            selectionActualTipoComprobante = SelectedTipoComprobante;

                            //Como es factura la factura ya trae el importe, IVA y total, por ello dejo los campos como solo lectura.
                            EditarDetalles = true;
                        }
                        else if (tipoComprobante == "Comprobante sin requisitos fiscales")
                        {
                            //Que el IVA no sea visible 
                            VisibleIVA = false;

                            if (CabeceraReporteCache.Viaje == "Nacional")
                            {
                                VisibleImpuestoExtra = true;
                            }
                            else
                            {
                                VisibleImpuestoExtra = false;
                            }

                            //Hago visible el entry de No. Serie/Comprobante
                            Serie_ComprobanteVisible = true;

                            VisibleImage = true;
                            VisibleConFactura = false;
                            VisibleSuccessfulBill = false;
                            VisibleDetalleGasto = false;

                            //Poder editar detalles
                            EditarDetalles = false;

                            //Le indico que la seleccion actual sera esta.
                            selectionActualTipoComprobante = SelectedTipoComprobante;
                        }
                        else
                        {
                            //Que el IVA no sea visible 
                            VisibleIVA = false;

                            if (CabeceraReporteCache.Viaje == "Nacional")
                            {
                                VisibleImpuestoExtra = true;
                            }
                            else
                            {
                                VisibleImpuestoExtra = false;
                            }
                            //Quito el entry de No. Serie/Comprobante 
                            Serie_ComprobanteVisible = false;

                            //Poder editar detalles
                            EditarDetalles = false;

                            VisibleImage = false;
                            VisibleConFactura = false;
                            VisibleSuccessfulBill = false;
                            VisibleDetalleGasto = true;

                            //Le indico que la seleccion actual sera esta.
                            selectionActualTipoComprobante = SelectedTipoComprobante;
                        }
                    }
                    else
                    {
                        //No acepta

                    }
                }
            }
            else if (SystemDataCache.firstSelection) //Aún no ha seleccionado nada.
            {

                if (tipoComprobante == "Factura con requisitos fiscales")
                {
                    //Como es factura la factura ya trae el importe, IVA y total, por ello dejo los campos como solo lectura.
                    EditarDetalles = true;
                    //Que el IVA sea visible 
                    VisibleIVA = true;
                    VisibleImpuestoExtra = false;

                    VisibleConFactura = true;
                    //Le indico que la seleccion actual sera esta.
                    selectionActualTipoComprobante = SelectedTipoComprobante;

                    //Le indico que ya se ha seleccionado algo
                    SystemDataCache.firstSelection = false;
                }
                else if (tipoComprobante == "Comprobante sin requisitos fiscales")
                {
                    //Hago visible el entry de No. Serie/Comprobante
                    Serie_ComprobanteVisible = true;


                    //Que el IVA no sea visible 
                    VisibleIVA = false;
                    if (CabeceraReporteCache.Viaje == "Nacional")
                    {
                        VisibleImpuestoExtra = true;
                    }
                    else
                    {
                        VisibleImpuestoExtra = false;
                    }

                    VisibleImage = true;
                    //Le indico que la seleccion actual sera esta.
                    selectionActualTipoComprobante = SelectedTipoComprobante;

                    //Poder editar detalles 
                    EditarDetalles = false;

                    //Le indico que ya se ha seleccionado algo
                    SystemDataCache.firstSelection = false;
                }
                else
                {
                    //Que el IVA no sea visible 
                    VisibleIVA = false;
                    if (CabeceraReporteCache.Viaje == "Nacional")
                    {
                        VisibleImpuestoExtra = true;
                    }
                    else
                    {
                        VisibleImpuestoExtra = false;
                    }

                    //Poder editar detalles 
                    EditarDetalles = false;

                    VisibleDetalleGasto = true;
                    //Le indico que la seleccion actual sera esta.
                    selectionActualTipoComprobante = SelectedTipoComprobante;

                    //Le indico que ya se ha seleccionado algo
                    SystemDataCache.firstSelection = false;
                }
            }
        }

        private async Task<bool> VerifySave(string option)
        {
            if (option == "Factura con requisitos fiscales")
            {
                if (string.IsNullOrWhiteSpace(NameFilePDF))
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe agregar un archivo PDF.", "OK");
                    return false;
                }
                else if (string.IsNullOrWhiteSpace(NameFileXML))
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe agregar un archivo XML.", "OK");
                    return false;
                }
                //Comprobar si es tipo factura, si lo es, comprobar que la 'fecha genero gasto' debe ser menor o igual a la fecha comporbante de la factura.
                //Cada que se carga una factura, se almacena como dato estatico, por lo que no es necesario comprobar desde que ventan se guarda el gasto.
                if (!string.IsNullOrWhiteSpace(NameFilePDF) && !string.IsNullOrWhiteSpace(NameFileXML) && FechaGeneroGasto > Convert.ToDateTime(FacturaCache.FechaComprobante))
                {
                    string date = Convert.ToDateTime(FacturaCache.FechaComprobante).ToString("dd/MM/yyyy");
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", $"La fecha del gasto debe ser menor o igual a la fecha comprobante de la factura ({date}).", "OK");
                    return false;
                }


                else if (await VerifyDetails())
                {
                    return true;
                }
            }
            else if (option == "Comprobante sin requisitos fiscales")
            {
                if (ResultImage != null && VisibleImage)
                {
                    if (string.IsNullOrWhiteSpace(Serie_Comprobante))
                    {
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe agregar un número de serie o comprobante.", "OK");
                        return false;
                    }
                    else if (ResultImage == null)
                    {
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe seleccionar una imagen.", "OK");
                        return false;
                    }
                    else
                    {
                        if (await VerifyDetails())
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe agregar una imagen o tomar una foto.", "OK");
                    return false;
                }
            }
            else //Sin requisitos fiscales
            {
                if (await VerifyDetails())
                {
                    return true;
                }
            }

            return false;
        }
        private async Task<bool> VerifyDetails()
        {
            //Verificar la fecha en la que se generó el gasto
            //Si apenas esta registrando el gasto, verifico la fecha que esta almacenada en las variables estáticas de la cabecera.
            if ((SystemDataCache.insertWithChildren && FechaGeneroGasto < CabeceraReporteCache.Desde) || (SystemDataCache.insertWithChildren && FechaGeneroGasto > CabeceraReporteCache.Hasta))
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", $"La fecha esta fuera del periodo del reporte {CabeceraReporteCache.Desde.ToString("dd/MM/yyyy")} - {CabeceraReporteCache.Hasta.ToString("dd/MM/yyyy")}.", "OK");
                return false;
            }

            //Si ya se agregó y deseo agregar uno desde la misma ventana, puedo seguir comparando con las variables alamacenadas de la cabecera.
            if ((SystemDataCache.kindOfSpendindToSave == 1 && FechaGeneroGasto < CabeceraReporteCache.Desde) || (SystemDataCache.kindOfSpendindToSave == 1 && FechaGeneroGasto > CabeceraReporteCache.Hasta))
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", $"La fecha esta fuera del periodo del reporte {CabeceraReporteCache.Desde.ToString("dd/MM/yyyy")} - {CabeceraReporteCache.Hasta.ToString("dd/MM/yyyy")}.", "OK");
                return false;
            }
            //Si deseo agregar desde la ventana de gastos, entonces debo consultar las fechas de la cabecera por medio del id que se obtuvo al seleccionar el gasto.
            //SystemDataCache.idHeadBoard
            if ((SystemDataCache.kindOfSpendindToSave == 2 && FechaGeneroGasto < Convert.ToDateTime(await App.DataBase.HeaderColumnData(SystemDataCache.idHeadBoard, "Desde")))
                || (SystemDataCache.kindOfSpendindToSave == 2 && fechaGeneroGasto > Convert.ToDateTime(await App.DataBase.HeaderColumnData(SystemDataCache.idHeadBoard, "Hasta"))))
            {
                string desde = Convert.ToDateTime(await App.DataBase.HeaderColumnData(SystemDataCache.idHeadBoard, "Desde")).ToString("dd/MM/yyyy");
                string hasta = Convert.ToDateTime(await App.DataBase.HeaderColumnData(SystemDataCache.idHeadBoard, "Hasta")).ToString("dd/MM/yyyy");

                await App.Current.MainPage.DisplayAlert("Sistema de gastos", $"La fecha esta fuera del periodo del reporte {desde} - {hasta}.", "OK");
                return false;
            }
            if (Subtotal <= 0)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", "El importe debe ser mayor o igual a cero.", "OK");
                return false;
            }
            else if (Propina < 0)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", "La propina debe ser mayor o igual a cero.", "OK");
                return false;
            }
            else if (Total <= 0)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", "El total debe ser mayor que cero.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(SelectedTipoGasto))
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe seleccionar un tipo de gasto.", "OK");
                return false;
            }
            else if ((!string.IsNullOrWhiteSpace(SelectedTipoGasto) && SelectedTipoGasto == "ALIMENTOS" && string.IsNullOrWhiteSpace(SelectedNoComensales))
                || (!string.IsNullOrWhiteSpace(SelectedTipoGasto) && SelectedTipoGasto == "TAXI / UBER" && string.IsNullOrWhiteSpace(SelectedNoComensales)))
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe seleccionar un número de comensales.", "OK");
                return false;
            }
            else if (!string.IsNullOrWhiteSpace(SelectedTipoGasto) && SelectedTipoGasto == "HOSPEDAJE" && string.IsNullOrWhiteSpace(SelectedNoComensales))
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe  seleccionar un número de noches.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(Detalles))
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe ingresar detalles.", "OK");
                return false;
            }
            //Cuando COMBUSTIBLE este seleccionado
            else if (!string.IsNullOrWhiteSpace(SelectedTipoGasto) && SelectedTipoGasto == "COMBUSTIBLE" && ResultImageKilometres == null)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe comprobar los kilómetros.", "OK");
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> SaveReport(string option)
        {
            if (option == "Factura con requisitos fiscales")
            {
                try
                {
                    CabeceraReporte cabeceraReporte = new CabeceraReporte();
                    cabeceraReporte = ReportHeader();

                    DetalleReporte detalleReporte = new DetalleReporte();
                    detalleReporte = await ReportDetails(1);

                    cabeceraReporte.DetallesReporte = new List<DetalleReporte> { detalleReporte };

                    await App.DataBase.InsertReportWithChild(cabeceraReporte);

                    CleanPage(1);

                    return true;
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error al agregar reporte", ex.Message, "OK");
                    return false;
                }
            }
            else if (option == "Comprobante sin requisitos fiscales")
            {
                try
                {
                    CabeceraReporte cabeceraReporte = new CabeceraReporte();
                    cabeceraReporte = ReportHeader();

                    DetalleReporte detalleReporte = new DetalleReporte();
                    detalleReporte = await ReportDetails(2);

                    cabeceraReporte.DetallesReporte = new List<DetalleReporte> { detalleReporte };

                    await App.DataBase.InsertReportWithChild(cabeceraReporte);

                    CleanPage(2);
                    return true;
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error al agregar reporte", ex.Message, "OK");
                    return false;
                }
            }
            else
            {
                try
                {
                    CabeceraReporte cabeceraReporte = new CabeceraReporte();
                    cabeceraReporte = ReportHeader();

                    DetalleReporte detalleReporte = new DetalleReporte();
                    detalleReporte = await ReportDetails(3);

                    cabeceraReporte.DetallesReporte = new List<DetalleReporte> { detalleReporte };

                    await App.DataBase.InsertReportWithChild(cabeceraReporte);

                    CleanPage(3);
                    return true;
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error al agregar reporte", ex.Message, "OK");
                    return false;
                }
            }
        }
        public async Task<bool> SaveSpending(string option, int idHeadBoard, int expenseNumber, double totalExpenses)
        {

            if (option == "Factura con requisitos fiscales")
            {
                try
                {
                    DetalleReporte detalleReporte = new DetalleReporte();
                    detalleReporte = new DetalleReporte
                    {
                        Tipo = 1,
                        TipoComprobante = selectionActualTipoComprobante,
                        NombrePDF = NameFilePDF,
                        NombreXML = NameFileXML,
                        PDFBase64 = BillData.PDFBase64,
                        XMLBase64 = BillData.XMLBase64,
                        SerieComprobante = Serie_Comprobante,
                        FechaGeneroGasto = FechaGeneroGasto.ToString("dd/MM/yyyy"),
                        Subtotal = Subtotal,
                        IVA = IVA,
                        Propina = Propina,
                        Total = Total,
                        Gasto = SelectedTipoGasto,
                        //Opción guardar kilómetros 
                        ImagenKilometros64 = DetalleReporteCache.ImagenKilometros64,
                        NoComensales = Convert.ToInt32(SelectedNoComensales),
                        Detalles = Detalles,
                        Excede = await ExceedsExpense(),

                        //Factura
                        Serie = FacturaCache.Serie,
                        Folio = FacturaCache.Folio,
                        NoFactura = FacturaCache.NoFactura,
                        RFC = FacturaCache.RFC,
                        RFCReceptor = FacturaCache.RFCReceptor,
                        RazonSocial = FacturaCache.RazonSocial,
                        UUID = FacturaCache.UUID,
                        NoCertificadoSAT = FacturaCache.NoCertificadoSAT,
                        FechaComprobante = FacturaCache.FechaComprobante,
                        FechaTimbrado = FacturaCache.FechaTimbrado,
                        MetodoPago = FacturaCache.MetodoPago,

                        //Indico el id del reporte 
                        IdCabecera = idHeadBoard
                    };
                    //Inserto gasto
                    await App.DataBase.InsertChild(detalleReporte);

                    //Actualizo reporte, agrego un nuevo gasto y modifico el total
                    //Aumento el gasto y el total de gastos.
                    await App.DataBase.UpdateReport(expenseNumber + 1, totalExpenses + Total, idHeadBoard);


                    CleanPage(1);

                    return true;
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error al agregar reporte", ex.Message, "OK");
                    return false;
                }
            }
            else if (option == "Comprobante sin requisitos fiscales")
            {
                try
                {
                    DetalleReporte detalleReporte = new DetalleReporte();
                    detalleReporte = new DetalleReporte
                    {
                        Tipo = 2,
                        TipoComprobante = selectionActualTipoComprobante,
                        ImagenBase64 = DetalleReporteCache.ImagenBase64,
                        SerieComprobante = Serie_Comprobante,
                        FechaGeneroGasto = FechaGeneroGasto.ToString("dd/MM/yyyy"),
                        Subtotal = Subtotal,
                        Propina = Propina,
                        Total = Total,
                        Gasto = SelectedTipoGasto,
                        //Opción guardar kilómetros 
                        ImagenKilometros64 = DetalleReporteCache.ImagenKilometros64,
                        NoComensales = Convert.ToInt32(SelectedNoComensales),
                        Detalles = Detalles,
                        Excede = await ExceedsExpense(),

                        //Indico el id del reporte 
                        IdCabecera = idHeadBoard
                    };


                    await App.DataBase.InsertChild(detalleReporte);


                    //Actualizo reporte, agrego un nuevo gasto y modifico el total
                    //Aumento el gasto y el total de gastos.
                    await App.DataBase.UpdateReport(expenseNumber + 1, totalExpenses + Total, idHeadBoard);

                    CleanPage(2);
                    return true;
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error al agregar reporte", ex.Message, "OK");
                    return false;
                }
            }
            else
            {
                try
                {
                    DetalleReporte detalleReporte = new DetalleReporte();
                    detalleReporte = new DetalleReporte
                    {
                        Tipo = 3,
                        TipoComprobante = selectionActualTipoComprobante,
                        SerieComprobante = Serie_Comprobante,
                        FechaGeneroGasto = FechaGeneroGasto.ToString("dd/MM/yyyy"),
                        Subtotal = Subtotal,
                        Propina = Propina,
                        Total = Total,
                        Gasto = SelectedTipoGasto,
                        //Opción guardar kilómetros 
                        ImagenKilometros64 = DetalleReporteCache.ImagenKilometros64,
                        NoComensales = Convert.ToInt32(SelectedNoComensales),
                        Detalles = Detalles,
                        Excede = await ExceedsExpense(),

                        //Indico el id del reporte 
                        IdCabecera = idHeadBoard
                    };

                    await App.DataBase.InsertChild(detalleReporte);

                    //Actualizo reporte, agrego un nuevo gasto y modifico el total
                    //Aumento el gasto y el total de gastos.
                    await App.DataBase.UpdateReport(expenseNumber + 1, totalExpenses + Total, idHeadBoard);

                    CleanPage(3);
                    return true;
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error al agregar reporte", ex.Message, "OK");
                    return false;
                }
            }
        }

        public async Task<Tuple<bool, string>> OptionSpending(int option)
        {
            //En caso de que desee agregar un gasto después del gasto reciente.
            if (option == 1)
            {
                //Traigo el último registro, el número de gastos y el total.
                var resultLatestRecord = await App.DataBase.LatestRecord();

                //Se acaba de agregar un gasto, por lo que aún hay datos el la cache sobre la cabecera.
                if (!await ExceedsTip(Subtotal, Propina, SelectedTipoGasto, Convert.ToInt32(UserData.nivel), Convert.ToInt32(SelectedNoComensales), CabeceraReporteCache.Viaje, DocumentType(selectionActualTipoComprobante)))
                {
                    if (await SaveSpending(selectionActualTipoComprobante, resultLatestRecord.Item1, resultLatestRecord.Item2, resultLatestRecord.Item3))
                    {
                        return Tuple.Create<bool, string>(true, "¡Gasto guardado!");
                    }
                    else
                    {
                        return Tuple.Create<bool, string>(false, "No se pudo guardar el gasto.");
                    }
                }
                else
                {
                    //await App.Current.MainPage.DisplayAlert("Sistema de gastos", "El anticipo excede la política.", "OK");
                    return Tuple.Create<bool, string>(false, "La propina excede la política.");
                }
            }
            //En caso de que desee agregar un gasto desde "Gastos"
            else if (option == 2)
            {
                //Traigo el Id del gasto seleccionado, para la consultar la cabecea de ese gasto.
                var resultHeadboardById = await App.DataBase.HeadboardById(CabeceraReporteCache.Id);
                if (resultHeadboardById[0].Id == CabeceraReporteCache.Id)
                {
                    //Evaluar por consulta, ya que no se tiene en cache el viaje.
                    if (!await ExceedsTip(Subtotal, Propina, SelectedTipoGasto, Convert.ToInt32(UserData.nivel), Convert.ToInt32(SelectedNoComensales), resultHeadboardById[0].Viaje, DocumentType(selectionActualTipoComprobante)))
                    {
                        if (await SaveSpending(selectionActualTipoComprobante, SystemDataCache.idHeadBoard, resultHeadboardById[0].NoGastos, resultHeadboardById[0].Total))
                        {
                            //Indico que actualice la lista de gastos pendientes, ya que se acaba de agregar otro y también la lista de 
                            //detalles del reporte, para que actulice el contador de gastos y el total.
                            TimerData.UpdateReportDetails = true;
                            TimerData.UpdateExpensesList = true;

                            return Tuple.Create<bool, string>(true, "¡Gasto guardado!");
                        }
                        else
                        {
                            return Tuple.Create<bool, string>(false, "No se pudo guardar el gasto.");
                        }
                    }
                    else
                    {
                        //await App.Current.MainPage.DisplayAlert("Sistema de gastos", "El anticipo excede la política.", "OK");
                        return Tuple.Create<bool, string>(false, "La propina excede la politíca.");
                    }
                }
                else
                {
                    return Tuple.Create<bool, string>(false, "Error al consultar el gasto");
                }
            }
            else
            {
                return Tuple.Create<bool, string>(false, "No se encontró ningún tipo de gasto.");
            }
        }

        private void CleanPage(int option)
        {
            //Limpio los detalles
            Subtotal = 0;
            IVA = 0;
            Propina = 0;
            Total = 0;
            SelectedTipoGasto = null;
            Detalles = null;
            ResultImageKilometres = null;

            switch (option)
            {
                case 1: //Factura con requisitos fiscales.
                    NameFilePDF = null;
                    NameFileXML = null;
                    VisibleConFactura = true;
                    VisibleSuccessfulBill = false;

                    break;
                case 2: //comprobante sin requisitos fiscales.
                    Serie_Comprobante = null;
                    ResultImage = null;
                    VisibleImage = true;
                    break;
                default:
                    break;
            }
        }
        private async void ExitPage(int exitOption)
        {
            bool option = await App.Current.MainPage.DisplayAlert("Sistema de gastos", "¿Está seguro de que deseas salir?", "Si", "No");
            if (option)
            {
                switch (exitOption)
                {
                    case 0:
                        App.Current.MainPage = new NavigationPage(new NavigationMasterPage());
                        break;
                    case 1:
                        App.Current.MainPage = new NavigationPage(new NavigationMasterPage());
                        break;
                    case 2:
                        await App.Current.MainPage.Navigation.PopAsync();
                        break;
                    default:
                        break;
                }
            }
        }
        private async Task LoadPickerGastos()
        {
            try
            {
                IList<Gasto> resultGastos = await App.DataBase.LoadGastos((int)UserData.emp_type);
                //Si tiene datos, cargo la lista de gastos.
                if (resultGastos.Count > 0)
                {
                    GastosList = new List<string>();

                    foreach (var item in resultGastos)
                    {
                        GastosList.Add(item.Gastos_Concepto);
                    }
                }
                else
                {
                    //Esperarla por una tupla, para poder recibir el mensaje de error
                    var resultUpdateGastos = await RestApiService.UpdateGastosAsync();
                    bool isCorrect = resultUpdateGastos.Item1;
                    string message = resultUpdateGastos.Item2;

                    //Si no tiene, significa que apenas inició el programa, por lo que debo cargar los datos del API.
                    if (isCorrect)
                    {
                        IList<Gasto> resultGastos2 = await App.DataBase.LoadGastos((int)UserData.emp_type);

                        if (resultGastos2.Count > 0)
                        {
                            GastosList = new List<string>();

                            foreach (var item in resultGastos2)
                            {
                                GastosList.Add(item.Gastos_Concepto);
                            }
                        }
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await App.Current.MainPage.DisplayAlert("Error", message, "OK");
                        });

                        /*
                         Por alguna razón, no está llamando a esto desde el hilo de la interfaz de usuario.

                         Envuelva la llamada en el BeginInvokeOnMainThreadmétodo

                            Device.BeginInvokeOnMainThread (async () => {
                                await DisplayAlert("Alert", "You have been alerted", "OK");
                            });

                         Mientras está haciendo algo en un subproceso en segundo 
                         plano y desea interactuar con la interfaz de usuario, 
                         necesitará el BeginInvokeOnMainThreadmétodo para invocarlo 
                         en el subproceso principal que es responsable de la interfaz 
                         de usuario.
                         */

                    }
                }
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                });

                /*
                 Por alguna razón, no está llamando a esto desde el hilo de la interfaz de usuario.

                 Envuelva la llamada en el BeginInvokeOnMainThreadmétodo

                    Device.BeginInvokeOnMainThread (async () => {
                        await DisplayAlert("Alert", "You have been alerted", "OK");
                    });

                 Mientras está haciendo algo en un subproceso en segundo 
                 plano y desea interactuar con la interfaz de usuario, 
                 necesitará el BeginInvokeOnMainThreadmétodo para invocarlo 
                 en el subproceso principal que es responsable de la interfaz 
                 de usuario.
                 */

            }
        }

        private async Task<bool> LoadResizedImage(string imageBase64)
        {
            //recibir base64 de galeria o foto 
            try
            {
                if (!string.IsNullOrWhiteSpace(imageBase64))
                {
                    //enviarlo al serrvidor y esperar respuesta                
                    var client = new RestClient("https://eis-latam.info/WebService/api/imagenes/Redimensionar_imagen");
                    client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", UserData.token));
                    var request = new RestRequest(Method.POST);
                    request.RequestFormat = DataFormat.Json;
                    request.AddJsonBody(new { Emp_Num = UserData.Emp_Num, ImagenBase64 = imageBase64 });

                    IRestResponse response = await client.ExecuteAsync(request);

                    var statusCode = response.StatusDescription;

                    if (statusCode == "OK")
                    {
                        var result = JsonConvert.DeserializeObject<Response_HTTP>(response.Content);

                        if (result.Estado)
                        {
                            string redimensionadaBase64 = result.Resultado;
                            ResultImage = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(result.Resultado)));

                            //Almaceno imagen redireccionada para almacenarla en la BD Local.
                            DetalleReporteCache.ImagenBase64 = result.Resultado;

                            return true;
                        }
                        return false;
                    }
                    else if (statusCode == null)
                    {
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No hay conexión a Internet.", "OK");
                        return false;
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No se encontró ningún resultado.", "OK");
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "OK");
                return false;
            }
        }

        private async Task<bool> LoadResizedImageKilometres(string imageKilometresBase64)
        {
            //recibir base64 de galeria o foto 
            try
            {
                if (!string.IsNullOrWhiteSpace(imageKilometresBase64))
                {
                    //enviarlo al serrvidor y esperar respuesta                
                    var client = new RestClient("https://eis-latam.info/WebService/api/imagenes/Redimensionar_imagen");
                    client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", UserData.token));
                    var request = new RestRequest(Method.POST);
                    request.RequestFormat = DataFormat.Json;
                    request.AddJsonBody(new { Emp_Num = UserData.Emp_Num, ImagenBase64 = imageKilometresBase64 });

                    IRestResponse response = await client.ExecuteAsync(request);

                    var statusCode = response.StatusDescription;

                    if (statusCode == "OK")
                    {
                        var result = JsonConvert.DeserializeObject<Response_HTTP>(response.Content);

                        if (result.Estado)
                        {
                            //string redimensionadaBase64 = result.Resultado;
                            ResultImageKilometres = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(result.Resultado)));

                            //Almaceno imagen redireccionada para almacenarla en la BD Local.
                            DetalleReporteCache.ImagenKilometros64 = result.Resultado;

                            return true;
                        }
                        return false;
                    }
                    else if (statusCode == null)
                    {
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No hay conexión a Internet.", "OK");
                        return false;
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No se encontró ningún resultado.", "OK");
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "OK");
                return false;
            }
        }

        private async Task LoadImage(int option)
        {
            try
            {
                switch (option)
                {
                    case 1: //Cargar imagen de galería
                        UserDialogs.Instance.ShowLoading("Cargando imágenes...");
                        //string resultImage = await PickImageAsync();
                        string resultImage = await PickImageFilterAsync();

                        if (!string.IsNullOrWhiteSpace(resultImage))
                        {
                            //UserDialogs.Instance.ShowLoading("Cargando imagen...");
                            bool isCorrect = await LoadResizedImage(resultImage);
                            if (isCorrect)
                            {
                                //Activo contenedor detalles.
                                VisibleDetalleGasto = true;
                                //Oculto el Actvity indicator
                                UserDialogs.Instance.HideLoading();
                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                                await App.Current.MainPage.DisplayAlert("Error", "No se pudo procesar la imagen.", "OK");
                            }
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe seleccionar una imagen.", "OK");
                        }
                        break;
                    case 2: //Cargar foto      

                        UserDialogs.Instance.ShowLoading("Cargando cámara...");
                        string resultPhoto = await TakePhotoAsync();

                        if (!string.IsNullOrWhiteSpace(resultPhoto))
                        {
                            //UserDialogs.Instance.ShowLoading("Cargando imagen...");
                            bool isCorrect = await LoadResizedImage(resultPhoto);
                            if (isCorrect)
                            {
                                //Activo contenedor detalles.
                                VisibleDetalleGasto = true;
                                //Oculto el Actvity indicator
                                UserDialogs.Instance.HideLoading();
                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                                await App.Current.MainPage.DisplayAlert("Error", "No se pudo procesar la foto.", "OK");
                            }
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe tomar una fotografía.", "OK");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task LoadImageKilometres(int option)
        {
            try
            {
                switch (option)
                {
                    case 1: //Cargar imagen de galería
                        UserDialogs.Instance.ShowLoading("Cargando imágenes...");
                        //string resultImage = await PickImageAsync();
                        //Convierto a base 64
                        string resultImage = await PickImageFilterAsync();

                        if (!string.IsNullOrWhiteSpace(resultImage))
                        {
                            //Redimensiono la imagen
                            bool isCorrect = await LoadResizedImageKilometres(resultImage);
                            if (isCorrect)
                            {
                                //Oculto el Activity indicator
                                UserDialogs.Instance.HideLoading();
                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                                await App.Current.MainPage.DisplayAlert("Error", "No se pudo procesar la imagen.", "OK");
                            }
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe seleccionar una imagen.", "OK");
                        }
                        break;
                    case 2: //Cargar foto      

                        UserDialogs.Instance.ShowLoading("Cargando cámara...");

                        //Toma foto y la convierte a base 64
                        string resultPhoto = await TakePhotoAsync();

                        if (!string.IsNullOrWhiteSpace(resultPhoto))
                        {
                            //Redimensiono la foto.
                            bool isCorrect = await LoadResizedImageKilometres(resultPhoto);
                            if (isCorrect)
                            {
                                //Oculto el Actvity indicator
                                UserDialogs.Instance.HideLoading();
                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                                await App.Current.MainPage.DisplayAlert("Error", "No se pudo procesar la foto.", "OK");
                            }
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe tomar una fotografía.", "OK");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private CabeceraReporte ReportHeader()
        {
            CabeceraReporte cabeceraReporte = new CabeceraReporte();
            cabeceraReporte = new CabeceraReporte
            {
                Emp_Num = CabeceraReporteCache.Emp_Num,
                Concepto = CabeceraReporteCache.Concepto,
                Desde = CabeceraReporteCache.Desde,
                Hasta = CabeceraReporteCache.Hasta,
                PeriodoReporte = CabeceraReporteCache.Desde.ToString("dd/MM/yyyy") + " - " + CabeceraReporteCache.Hasta.ToString("dd/MM/yyyy"),
                TipoGasto = CabeceraReporteCache.TipoGasto,
                Anticipo = CabeceraReporteCache.Anticipo,
                Folio_Anticipo = CabeceraReporteCache.Folio_Anticipo,
                Viaje = CabeceraReporteCache.Viaje,
                Razon = CabeceraReporteCache.Razon,
                NombreCliente = CabeceraReporteCache.NombreCliente,
                NoProyecto = CabeceraReporteCache.NoProyecto,
                NoGastos = 1, //Es el primer gasto, ya que apenas insertará el reporte.
                Total = Total
            };

            return cabeceraReporte;
        }

        private async Task<DetalleReporte> ReportDetails(int option)
        {
            if (option == 1)
            {
                DetalleReporte detalleReporte = new DetalleReporte();
                detalleReporte = new DetalleReporte
                {
                    Tipo = option,
                    TipoComprobante = selectionActualTipoComprobante,
                    NombrePDF = NameFilePDF,
                    NombreXML = NameFileXML,
                    PDFBase64 = BillData.PDFBase64,
                    XMLBase64 = BillData.XMLBase64,
                    SerieComprobante = Serie_Comprobante,
                    FechaGeneroGasto = FechaGeneroGasto.ToString("dd/MM/yyyy"),
                    Subtotal = Subtotal,
                    IVA = IVA,
                    Propina = Propina,
                    Total = Total,
                    Gasto = SelectedTipoGasto,
                    //Opción guardar kilómetros 
                    ImagenKilometros64 = DetalleReporteCache.ImagenKilometros64,
                    NoComensales = Convert.ToInt32(SelectedNoComensales),
                    Detalles = Detalles,
                    Excede = await ExceedsExpense(),

                    //Factura+
                    Serie = FacturaCache.Serie,
                    Folio = FacturaCache.Folio,
                    NoFactura = FacturaCache.NoFactura,
                    RFC = FacturaCache.RFC,
                    RFCReceptor = FacturaCache.RFCReceptor,
                    RazonSocial = FacturaCache.RazonSocial,
                    UUID = FacturaCache.UUID,
                    NoCertificadoSAT = FacturaCache.NoCertificadoSAT,
                    FechaComprobante = FacturaCache.FechaComprobante,
                    FechaTimbrado = FacturaCache.FechaTimbrado,
                    MetodoPago = FacturaCache.MetodoPago
                };

                return detalleReporte;
            }
            else if (option == 2)
            {
                DetalleReporte detalleReporte = new DetalleReporte();
                detalleReporte = new DetalleReporte
                {
                    Tipo = option,
                    TipoComprobante = selectionActualTipoComprobante,
                    ImagenBase64 = DetalleReporteCache.ImagenBase64,
                    SerieComprobante = Serie_Comprobante,
                    FechaGeneroGasto = FechaGeneroGasto.ToString("dd/MM/yyyy"),
                    Subtotal = Subtotal,
                    Propina = Propina,
                    Total = Total,
                    Gasto = SelectedTipoGasto,
                    //Opción guardar kilómetros 
                    ImagenKilometros64 = DetalleReporteCache.ImagenKilometros64,
                    NoComensales = Convert.ToInt32(SelectedNoComensales),
                    Detalles = Detalles,
                    Excede = await ExceedsExpense()
                };
                return detalleReporte;
            }
            else
            {
                DetalleReporte detalleReporte = new DetalleReporte();
                detalleReporte = new DetalleReporte
                {
                    Tipo = option,
                    TipoComprobante = selectionActualTipoComprobante,
                    FechaGeneroGasto = FechaGeneroGasto.ToString("dd/MM/yyyy"),
                    Subtotal = Subtotal,
                    Propina = Propina,
                    Total = Total,
                    Gasto = SelectedTipoGasto,
                    //Opción guardar kilómetros 
                    ImagenKilometros64 = DetalleReporteCache.ImagenKilometros64,
                    NoComensales = Convert.ToInt32(SelectedNoComensales),
                    Detalles = Detalles,
                    Excede = await ExceedsExpense()
                };
                return detalleReporte;
            }
        }
        private void CurrentDate()
        {
            DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            FechaGeneroGasto = currentMonth;
        }
        private void CleanNumberDiners()
        {
            SelectedNoComensales = null;
        }
        private async Task OpenImageZoom(int option)
        {
            // 1 = Resultado del Comprobante sin requisitos fiscales
            // 2 = Resultado del comporbante de los kilometros
            switch (option)
            {
                case 1:
                    SystemDataCache.resultImage = ResultImage;
                    await App.Current.MainPage.Navigation.PushAsync(new ImagePopupPage());
                    break;
                case 2:
                    SystemDataCache.resultImage = ResultImageKilometres;
                    await App.Current.MainPage.Navigation.PushAsync(new ImagePopupPage());
                    break;
                default:
                    break;
            }
        }
        private async Task<bool> ExceedsTip(double amount, double tip, string spending, int level, int diners, string travel, int typeOfDocument)
        {
            //Debo calcular el Gastos_Cve para traer su id de Gastos_concepto
            int gasto_cve = await App.DataBase.Gasto_Cve(spending, (int)UserData.emp_type);
            double tipTemp = 0;

            //ALIMENTOS, HOSPEDAJE, TAXI/UBER
            if (gasto_cve == 1 || gasto_cve == 2 || gasto_cve == 4)
            {
                if (typeOfDocument == 1) //Si es con factura
                {
                    //Si es nivel 1
                    if (Convert.ToInt32(UserData.nivel) == 1)
                    {
                        tipTemp = (amount * 0.2) + 10;
                    }
                    else //Los demás niveles
                    {
                        if (travel == "EXTRANJERO")
                        {
                            tipTemp = (amount * 0.15) + 5;
                        }
                        else //Nacional
                        {
                            if (diners >= 6) //aqui si pido no comensales.
                            {
                                tipTemp = (amount * 0.15) + 5;
                            }
                            else
                            {
                                tipTemp = (amount * 0.12) + 5;
                            }
                        }
                    }
                }
                else if (typeOfDocument == 2 || typeOfDocument == 3) //Si es con comprobante sin requisitos fiscales o sin comprobante/sin factura
                {
                    //No importan los niveles
                    if (travel == "EXTRANJERO")
                    {
                        tipTemp = (amount * 0.2) + 20;
                    }
                    else //Nacional
                    {
                        tipTemp = (amount * 0.15) + 20;
                    }
                }
            }
            //PEAJES, TRANSPORTACION AEREO, KILOMETRAJE, AUTOBUS, TRASNPORTE PUBLICO SIN COMPROBANTE
            else if (gasto_cve == 3 || gasto_cve == 5 || gasto_cve == 7 || gasto_cve == 9)
            {
                if (level == 1)//Si es nivel 1
                {
                    tipTemp = (amount * 0.2) + 5;
                }
                else //El resto de niveles
                {
                    tipTemp = (amount * 0.15) + 5;
                }
            }
            //else if (gasto_cve == 6 || gasto_cve == 8 || gasto_cve == 10 || gasto_cve == 11 || gasto_cve == 12 || gasto_cve == 13)//Falta el 14, y los que se agregan
            else
            {
                tipTemp = (amount * 0.15) + 5;
            }

            return tip > tipTemp ? true : false;

            //Notas///
            //No tomas en cuenta si es factura o no.
            //6 ,8, 10, 11, 12, 13 //que aplicque el 15 %
            //Equipaje, Combutible, MTTO Eq de transporte, renta de automovil, herramientas, estacionamiento.
        }

        private int DocumentType(string type)
        {
            if (type == "Factura con requisitos fiscales")
            {
                return 1;
            }
            else if (type == "Comprobante sin requisitos fiscales")
            {
                return 2;
            }
            else if (type == "Sin comprobante/Sin factura")
            {
                return 3;
            }
            else
            {
                return -1;
            }
        }

        private async Task<bool> ExceedsExpense()
        {
            //Calculo el monto de lo que puede gastar de acuerdo al gasto seleccionado
            double mount = await App.DataBase.CanSpend(Convert.ToInt32(UserData.nivel), (int)UserData.emp_type, SelectedTipoGasto);

            //Lo comparo con el total del gasto y si excede es true.
            return Total > mount ? true : false;
        }
    }
}

