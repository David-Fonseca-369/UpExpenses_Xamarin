using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using UpExpenses_Xamarin.Data;
using UpExpenses_Xamarin.Models;
using UpExpenses_Xamarin.Services;
using UpExpenses_Xamarin.Views;
using UpExpenses_Xamarin.WebService;
using Xamarin.Forms;

namespace UpExpenses_Xamarin.ViewModels
{
    public class NuevoReporteGastosPageViewModel : BaseViewModel
    {
        private bool resultAnticiposTuple { get; set; }
        private string messageAnticiposTuple { get; set; }

        private IList<CabeceraReporte> reportesPendientes;
        public IList<CabeceraReporte> ReportesPendientes
        {
            get => reportesPendientes;
            set => SetProperty(ref reportesPendientes, value);
        }
        private IList<AnticipoPorComprobar> listAnticipos;
        public IList<AnticipoPorComprobar> ListAnticipos
        {
            get => listAnticipos;
            set => SetProperty(ref listAnticipos, value);
        }

        private IList<string> listTiposGastos;
        public IList<string> ListTiposGastos
        {
            get => listTiposGastos;
            set => SetProperty(ref listTiposGastos, value);
        }

        private string selectedTipoGasto;
        public string SelectedTipoGasto
        {
            get => selectedTipoGasto;
            set => SetProperty(ref selectedTipoGasto, value);
        }

        private string selectedRazon;
        public string SelectedRazon
        {
            get => selectedRazon;
            set => SetProperty(ref selectedRazon, value);
        }

        private AnticipoPorComprobar selectedAnticipo;
        public AnticipoPorComprobar SelectedAnticipo
        {
            get => selectedAnticipo;
            set => SetProperty(ref selectedAnticipo, value);
        }

        private string selectedViaje;
        public string SelectedViaje
        {
            get => selectedViaje;
            set => SetProperty(ref selectedViaje, value);
        }

        private string cambioTipoGasto;
        public string CambioTipoGasto
        {
            get => cambioTipoGasto;
            set => SetProperty(ref cambioTipoGasto, value);
        }

        private bool visibleAnticipo;
        public bool VisibleAnticipo
        {
            get => visibleAnticipo;
            set => SetProperty(ref visibleAnticipo, value);
        }

        private bool visibleRazon;
        public bool VisibleRazon
        {
            get => visibleRazon;
            set => SetProperty(ref visibleRazon, value);
        }

        private DateTime selectDesde;
        public DateTime SelectDesde
        {
            get => selectDesde;
            set => SetProperty(ref selectDesde, value);
        }
        private DateTime selectHasta;
        public DateTime SelectHasta
        {
            get => selectHasta;
            set => SetProperty(ref selectHasta, value);
        }
        private string concepto;
        public string Concepto
        {
            get => concepto;
            set => SetProperty(ref concepto, value);
        }
        private string nombreCliente;
        public string NombreCliente
        {
            get => nombreCliente;
            set => SetProperty(ref nombreCliente, value);
        }

        private int noProject;
        public int NoProject
        {
            get => noProject;
            set => SetProperty(ref noProject, value);
        }

        private bool visibleProjectNumber;
        public bool VisibleProjectNumber
        {
            get => visibleProjectNumber;
            set => SetProperty(ref visibleProjectNumber, value);
        }

        private bool visibleCheckBox;
        public bool VisibleCheckBox
        {
            get => visibleCheckBox;
            set => SetProperty(ref visibleCheckBox, value);
        }

        private int selectedIndexAnticipo;
        public int SelectedIndexAnticipo
        {
            get => selectedIndexAnticipo;
            set => SetProperty(ref selectedIndexAnticipo, value);
        }

        //
        public Command TappedCommandTipoGasto { get; set; }
        public Command TappedCommandRazon { get; set; }
        public Command Continuar { get; }
        public ICommand TappedCommandDesde { get; set; }
        public ICommand TappedCommandHasta { get; set; }

        public IList<string> Viaje
        {
            get
            {
                return new List<string> { "Nacional", "Extranjero" };
            }
        }

        public IList<string> Razon
        {
            get
            {
                return new List<string> { "Cliente", "Trabajo" };
            }
        }

        public NuevoReporteGastosPageViewModel()
        {
            CurrentDate();

            Task tiposGastosTask = new Task(async () => await LoadPickerTiposDeGastos());
            tiposGastosTask.Start();


            Task anticiposLoad = new Task(async () => await LoadPickerAnticipos());
            anticiposLoad.Start();


            LoadPEAR();

            visibleAnticipo = false;

            TappedCommandTipoGasto = new Command(() =>
            {
                string tipo = SelectedTipoGasto;
                if (selectedTipoGasto == "ANTICIPO MXP")
                {
                    VisibleAnticipo = true;
                }
                else
                {
                    VisibleAnticipo = false;
                    //Limpio el picker de anticipo para no llevar ese dato.
                    CleanAdvance();
                }
            });

            TappedCommandRazon = new Command(() =>
             {
                 if (SelectedRazon == "Cliente")
                 {
                     VisibleRazon = true;
                 }
                 else
                 {
                     VisibleRazon = false;
                     //Limpio el entry para no arrastrar su contenido.
                     CleanCustomerName();
                 }
             });

            Continuar = new Command(async () =>
            {
                if (await Validate())
                {
                    if (await VerifyDates())
                    {
                        if (await Save())
                        {
                            //Indicar que será la primera selección que hará el picker.
                            SystemDataCache.firstSelection = true;
                            //Insertar con cabecera
                            SystemDataCache.insertWithChildren = true;
                            await Application.Current.MainPage.Navigation.PushAsync(new NuevoReporteGastosPage2());
                        }
                    }
                }
            });
        }

        private async Task<bool> Validate()
        {
            int prueba = SelectedIndexAnticipo;

            if (string.IsNullOrWhiteSpace(Concepto))
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe ingresar un concepto.", "Ok");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(SelectedTipoGasto))
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe seleccionar un tipo de gasto.", "Ok");
                return false;
            }            
            else if (!string.IsNullOrWhiteSpace(SelectedTipoGasto) && (SelectedTipoGasto == "ANTICIPO MXP") && (SelectedAnticipo.Folio <= 0)) //Si ha seleccionado un tipo de gasto            
            {
                //Si ha seleccionado anticipo, debe agregar un anticipo.
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe ingresar un anticipo.", "Ok");
                return false;
            }
            else if (!string.IsNullOrWhiteSpace(SelectedTipoGasto) && (SelectedTipoGasto == "ANTICIPO MXP") && (SelectedAnticipo.Folio > 0) && (await FolioExists(SelectedAnticipo.Folio))) //Si ha seleccionado un tipo de gasto verifico el anticipo           
            {
                return false;
            }
            else if (string.IsNullOrWhiteSpace(SelectedViaje))
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe seleccionar un tipo de viaje.", "Ok");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(SelectedRazon))
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe seleccionar una razón.", "Ok");
                return false;
            }
            else if (!string.IsNullOrWhiteSpace(SelectedRazon) && (SelectedRazon == "Cliente") && (string.IsNullOrWhiteSpace(NombreCliente)))
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe ingresar el nombre de él cliente.", "Ok");
                return false;
            }
            else if (VisibleProjectNumber == true && NoProject <= 0)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Debe ingresar el número de proyecto.", "Ok");
                return false;
            }

            return true;
        }

        private async Task<bool> Save()
        {
            try
            {
                CabeceraReporteCache.Emp_Num = Convert.ToInt32(UserData.Emp_Num);
                CabeceraReporteCache.Desde = SelectDesde;
                CabeceraReporteCache.Hasta = SelectHasta;
                CabeceraReporteCache.Concepto = Concepto;
                CabeceraReporteCache.TipoGasto = SelectedTipoGasto;

                //Le quito el '$', en caso de que haya sido selecionado
                if (SelectedAnticipo != null)//En ocasiones lo trae nulo
                {
                    if (SelectedAnticipo.Folio > 0 && VisibleAnticipo == true)
                    {
                        string anticipoStr = SelectedAnticipo.Por_Comprobar.Replace("$", "");
                        //Envio el anticipo
                        CabeceraReporteCache.Anticipo = Convert.ToDouble(anticipoStr);
                        //Envio el folio del anticipo
                        CabeceraReporteCache.Folio_Anticipo = SelectedAnticipo.Folio;
                    }
                }

                CabeceraReporteCache.Viaje = SelectedViaje;
                CabeceraReporteCache.Razon = SelectedRazon;
                CabeceraReporteCache.NombreCliente = NombreCliente;
                CabeceraReporteCache.NoProyecto = NoProject;

                return true;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error al guadar cabecera", ex.Message, "OK");
                return false;
            }
        }
        public static int RangeOfDates(DateTime desdeTemp, DateTime hastaTemp)
        {
            TimeSpan range = hastaTemp - desdeTemp;
            int days = range.Days;

            //Si 'desde' es antes que 'hasta'
            if (desdeTemp > hastaTemp)
            {
                return 1; // = La fecha fnal, no puede ser menor que la inicial.
            }
            else if (days > 30)
            {
                return 2; // = EL periodo del reporte es mayor a 30 días.
            }

            return 3; //Continuar
        }

        public static Tuple<bool, int, string> CheckDates
           (int id, DateTime desdeTemp, DateTime hastaTemp, string tipoGastoTemp, DateTime desde, DateTime hasta, string tipoGasto, string concepto)
        {
            if (tipoGastoTemp == tipoGasto)
            {
                //Checo si las
                //no están dentro del rango.
                //if (desdeTemp >= desde && desdeTemp <= hasta)
                //{
                //    return Tuple.Create<bool, int, string>(false, id, "La fecha inicial esta dentro del rango de un periodo del reporte " + concepto + "."); // la fecha inicial esta dentro del rango de un reporte con el mismo tipo de gasto.
                //}
                //else if (hastaTemp >= desde && hastaTemp <= hasta)
                //{
                //    return Tuple.Create<bool, int, string>(false, id, "La fecha final esta dentro del rango de un periodo del reporte " + concepto + "."); //la fecha final del periodo se encuentra dentro del rango de un reporte de gasto pendiente.
                //}

                string message = $"Las fechas indicadas ya se encuentran ocupadas en {concepto} del periodo {desde.ToString("dd/MM/yyyy")} - {hasta.ToString("dd/MM/yyyy")}.";
                //comparo los  gastos, porque en caso de que sea enviado y autorizado un reporte del mismo periodo, pueda registrar otro que se encuentre dentro de este.
                if (desde >= desdeTemp && desde <= hastaTemp)
                {
                    return Tuple.Create<bool, int, string>(false, id, message);
                }
                else if (hasta >= desdeTemp && hasta <= hastaTemp)
                {
                    return Tuple.Create<bool, int, string>(false, id, message);
                }

                return Tuple.Create<bool, int, string>(true, 0, null);
            }
            return Tuple.Create<bool, int, string>(true, 0, null);
        }

        private async Task<bool> VerifyDates()
        {
            int result = RangeOfDates(SelectDesde, SelectHasta);
            if (result == 1)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", "La fecha final no puede ser menor que la inicial.", "OK");
                return false;
            }
            else if (result == 2)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", "El periodo del reporte no puede ser mayor a 30 días.", "OK");
                return false;
            }
            else if (result == 3) //Continuar 
            {
                //traigo solo los reportes pendientes 
                IList<CabeceraReporte> resultList = await App.DataBase.GetAllOnlyPendingsAsync(UserData.Emp_Num);

                bool isCorrect = false;
                int id = 0;
                string messagge = null;

                if (resultList.Count > 0)
                {
                    foreach (var item in resultList)
                    {
                        var resultTuple = CheckDates(item.Id, SelectDesde, SelectHasta, SelectedTipoGasto, item.Desde, item.Hasta, item.TipoGasto, item.Concepto);
                        isCorrect = resultTuple.Item1;
                        id = resultTuple.Item2;
                        messagge = resultTuple.Item3;

                        if (!isCorrect)
                        {
                            break;
                        }
                    }
                    if (!isCorrect)
                    {
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", messagge, "OK");
                        return false;
                    }
                    return true;
                }
            }
            return true;
        }
        private async Task<bool> FolioExists(decimal folioAnticipo)
        {
            IList<CabeceraReporte> reportsList = await App.DataBase.GetAllPendingReports(UserData.Emp_Num);

            if (reportsList.Count > 0)
            {
                foreach (var item in reportsList)
                {
                    if (item.Folio_Anticipo == folioAnticipo)
                    {
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", $"El anticipo ya ha sido seleccionado por el reporte '{item.Concepto}'", "OK");
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        private void LoadPEAR()
        {
            if (UserData.agrega_pear == 1)
            {
                VisibleCheckBox = true;
            }
            else
            {
                VisibleCheckBox = false;
            }
        }

        private async Task LoadPickerAnticipos()
        {
            try
            {
                //Ya no solicito el servicio, lo traigo de una variable estatica.

                //var resultAnticipos = await RestApiService.AdvanceList(emp_num);
                //bool isCorrect = resultAnticipos.Item1;
                //IList<AnticipoPorComprobar> anticipos = resultAnticipos.Item2;
                //string message = resultAnticipos.Item3;

                IList<AnticipoPorComprobar> anticipos = AdvanceCache.AnticiposList;
                ListAnticipos = new List<AnticipoPorComprobar>();

                //if (isCorrect)
                //{
                if (anticipos.Count > 0)
                {
                    foreach (var item in anticipos)
                    {
                        ListAnticipos.Add(new AnticipoPorComprobar { Folio = item.Folio, Por_Comprobar = $"${Convert.ToDouble(item.Por_Comprobar).ToString("00.00", CultureInfo.InvariantCulture)}" });
                    }
                }
                //else
                //{
                //    ListAnticipos.Add(new AnticipoPorComprobar { Folio = -1, Por_Comprobar = "$0.00" });
                //}
                //}
                //else
                //{
                //    //LLeno lista, pero de igual manera obtengo el error que retorna la
                //    //Tuple, ya que como estoy ejecutando un hilo, me retorna un error al mostrar
                //    //un DisplayAlert, por lo que un metodo sincrono compureba si hubo un error.

                //    ListAnticipos.Add(null);
                //    resultAnticiposTuple = isCorrect;
                //    messageAnticiposTuple = message;

                //    //VerifyResultTuple();
                //}
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private async Task LoadPickerTiposDeGastos()
        {
            try
            {
                IList<TipoGastos> resultList = await App.DataBase.LoadTipoGastos();
                //Si tiene resultados llena la lista, si no, significa que no a cargado las APIs a la base de datos, por lo que comenzará 
                //a cargar los datos de la API a la base de datos local.
                if (resultList.Count > 0)
                {
                    ListTiposGastos = new List<string>();

                    foreach (var item in resultList)
                    {
                        ListTiposGastos.Add(item.Tipo_Gasto);
                    }
                }
                else
                {
                    //cargar datos a base de datos local
                    if (await RestApiService.UpdateTipoGastos())
                    {
                        IList<TipoGastos> resultList2 = await App.DataBase.LoadTipoGastos();
                        //Si tiene resultados llena la lista, si no, significa que no a cargado las APIs a la base de datos, por lo que comenzará 
                        //a cargar los datos de la API a la base de datos local.
                        if (resultList2.Count > 0)
                        {
                            ListTiposGastos = new List<string>();

                            foreach (var item in resultList2)
                            {
                                ListTiposGastos.Add(item.Tipo_Gasto);
                            }
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar el picker 'Tipo de gasto'.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private void CurrentDate()
        {
            DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            SelectHasta = currentMonth;
            SelectDesde = currentMonth;
        }
        private void CleanAdvance()
        {
            //Instancio un SelectAticipo
            AnticipoPorComprobar anticipoTemp = new AnticipoPorComprobar
            {
                Folio = -1,
                Por_Comprobar = ""
            };

            SelectedAnticipo = anticipoTemp;
        }
        private void CleanCustomerName()
        {
            NombreCliente = null;
        }
    }
}
