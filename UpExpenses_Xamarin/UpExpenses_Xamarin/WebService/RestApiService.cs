using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpExpenses_Xamarin.Models;
using UpExpenses_Xamarin.ViewModels;

namespace UpExpenses_Xamarin.WebService
{
    public class RestApiService
    {
        public static async Task<bool> UpdateTipoGastos()
        {
            try
            {
                //UserDialogs.Instance.ShowLoading("Conectando...");
                var client = new RestClient("https://eis-latam.info/WebService/api/reportes/getTipoGastos");
                client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", UserData.token));
                var request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;

                IRestResponse response = await client.ExecuteAsync(request);

                var content = response.Content;
                var statusCode = response.StatusDescription;

                if (statusCode == "OK")
                {
                    //Mando a eliminar la tabla, ya que los datos se cargaron correctamente.
                    //Pues si no, podría haber perdida de datos al no cargar nada.                   
                    if (await App.DataBase.DeleteTipoGastos())
                    {
                        var objTipoGastoList = new List<TipoGastos>();
                        objTipoGastoList = JsonConvert.DeserializeObject<List<TipoGastos>>(response.Content);

                        foreach (TipoGastos tipoGastos in objTipoGastoList)
                        {
                            //listTiposGasto.Add(tipoGastos.Tipo_Gasto);
                            //Almacenamos en la base de datos.

                            //await App.DataBase.InsertTipoGastos((decimal)tipoGastos.Tipo_Id, tipoGastos.Tipo_Gasto);
                            await App.DataBase.InsertTipoGastos(tipoGastos.Tipo_Id, tipoGastos.Tipo_Gasto);
                        }
                        //UserDialogs.Instance.HideLoading();
                        //OpenAnticiposPage();
                        return true;
                    }

                    return false;
                }
                else if (statusCode == "Unauthorized")
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Tu token ha caducado.\nPor favor, vuelva a iniciar sesión.", "OK");
                    return false;
                }
                else if (statusCode == null)
                {
                    //UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No hay conexión a Internet.", "Ok");
                    return false;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No se encontró ningún resultado.", "Ok");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");
                return false;
            }
        }
        public static async Task<bool> UpdateGastos()
        {
            try
            {
                //UserDialogs.Instance.ShowLoading("Conectando...");
                var client = new RestClient("https://eis-latam.info/WebService/api/reportes/getGastos");
                //client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", UserData.token));
                client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", "Perrito"));
                var request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;

                IRestResponse response = await client.ExecuteAsync(request);

                var content = response.Content;
                var statusCode = response.StatusDescription;

                if (statusCode == "OK")
                {
                    //Mando a eliminar la tabla, ya que los datos se cargaron correctamente.
                    //Pues si no, podría haber perdida de datos al no cargar nada.                   
                    if (await App.DataBase.DeleteGasto())
                    {
                        var objGastosList = new List<Gasto>();
                        objGastosList = JsonConvert.DeserializeObject<List<Gasto>>(response.Content);

                        foreach (Gasto gasto in objGastosList)
                        {
                            await App.DataBase.InsertGasto(gasto);
                        }
                        //UserDialogs.Instance.HideLoading();
                        //OpenAnticiposPage();
                        return true;
                    }

                    return false;
                }
                else if (statusCode == "Unauthorized")
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Tu token ha caducado.\nPor favor, vuelva a iniciar sesión.", "OK");
                    return false;
                }
                else if (statusCode == null)
                {
                    //UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No hay conexión a Internet.", "Ok");
                    return false;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No se encontró ningún resultado.", "Ok");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");
                return false;
            }
        }

        public static async Task<Tuple<bool, string>> UpdateGastosAsync()
        {
            try
            {
                //UserDialogs.Instance.ShowLoading("Conectando...");
                var client = new RestClient("https://eis-latam.info/WebService/api/reportes/getGastos");
                //client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", UserData.token));
                client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", UserData.token));
                var request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;

                IRestResponse response = await client.ExecuteAsync(request);

                var content = response.Content;
                var statusCode = response.StatusDescription;

                if (statusCode == "OK")
                {
                    //Mando a eliminar la tabla, ya que los datos se cargaron correctamente.
                    //Pues si no, podría haber perdida de datos al no cargar nada.                   
                    if (await App.DataBase.DeleteGasto())
                    {
                        var objGastosList = new List<Gasto>();
                        objGastosList = JsonConvert.DeserializeObject<List<Gasto>>(response.Content);

                        foreach (Gasto gasto in objGastosList)
                        {
                            await App.DataBase.InsertGasto(gasto);
                        }
                        //UserDialogs.Instance.HideLoading();
                        //OpenAnticiposPage();
                        return Tuple.Create<bool, string>(true, null);
                    }

                    return Tuple.Create<bool, string>(false, "No se pudo eliminar la tabla gasto.");
                }
                else if (statusCode == "Unauthorized")
                {
                    return Tuple.Create(false, "Tu token ha caducado.\nPor favor, vuelva a iniciar sesión.");
                }
                else if (statusCode == null)
                {                    
                    return Tuple.Create<bool, string>(false, "No hay conexión a Internet.");
                }
                else
                {                    
                    return Tuple.Create<bool, string>(false, "No se encontró ningún resultado.");
                }
            }
            catch (Exception ex)
            {                
                return Tuple.Create<bool, string>(false, ex.Message);
            }
        }

        //public static async Task<IList<string>> AdvanceList(int emp_num)
        public static async Task<Tuple<bool, IList<AnticipoPorComprobar>, string>> AdvanceList(decimal emp_num)
        {
            try
            {
                //UserDialogs.Instance.ShowLoading("Conectando...");
                var client = new RestClient("https://eis-latam.info/WebService/api/reportes/getAnticipo/" + emp_num);
                client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", UserData.token));
                var request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;

                IRestResponse response = await client.ExecuteAsync(request);

                var statusCode = response.StatusDescription;

                var objAnticiposList = new List<AnticipoPorComprobar>();

                if (statusCode == "OK")
                {
                    //var objAnticiposList = new List<AnticipoPorComprobar>();
                    objAnticiposList = JsonConvert.DeserializeObject<List<AnticipoPorComprobar>>(response.Content);


                    return Tuple.Create<bool, IList<AnticipoPorComprobar>, string>(true, objAnticiposList, null);
                }
                else if (statusCode == "Not Found")
                {
                    objAnticiposList.Add(new AnticipoPorComprobar { Folio = -1, Por_Comprobar = 0.0.ToString() });
                    return Tuple.Create<bool, IList<AnticipoPorComprobar>, string>(true, objAnticiposList, null);
                }
                else if (statusCode == "Unauthorized")
                {
                    objAnticiposList.Add(new AnticipoPorComprobar { Folio = -1, Por_Comprobar = 0.0.ToString() });
                    return Tuple.Create<bool, IList<AnticipoPorComprobar>, string>(false, objAnticiposList, "Tu token ha caducado.\nPor favor, vuelva a iniciar sesión.");
                }
                else if (statusCode == null)
                {
                    objAnticiposList.Add(new AnticipoPorComprobar { Folio = -1, Por_Comprobar = 0.0.ToString() });
                    return Tuple.Create<bool, IList<AnticipoPorComprobar>, string>(false, objAnticiposList, "No hay conexión a internet.");
                }
                else
                {
                    objAnticiposList.Add(new AnticipoPorComprobar { Folio = -1, Por_Comprobar = 0.0.ToString() });
                    return Tuple.Create<bool, IList<AnticipoPorComprobar>, string>(false, objAnticiposList, "No se encontró ningún resultado.");
                }
            }
            catch (Exception ex)
            {
                var listNull = new List<AnticipoPorComprobar>();
                listNull.Add(new AnticipoPorComprobar { Folio = -1, Por_Comprobar = 0.0.ToString() });

                return Tuple.Create<bool, IList<AnticipoPorComprobar>, string>(false, listNull, ex.Message);
            }
        }
        public static async Task<Tuple<bool, int, string>> SendReport(int id)
        {
            try
            {
                if (id > 0)
                {
                    //enviarlo al serrvidor y esperar respuesta                
                    //var client = new RestClient($"https://eis-latam.info/WebService/api/Reportes/Gasto_Reporte"); //Prueba (url no valida)
                    var client = new RestClient($"https://eis-latam.info/WebService/api/Reportes/Gasto_Reportes"); //Producción 
                    client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", UserData.token));
                    var request = new RestRequest(Method.POST);
                    request.RequestFormat = DataFormat.Json;

                    //Consulto el reporte en la base de datoschchr
                    CabeceraReporte report = new CabeceraReporte();
                    report = await App.DataBase.GetListReporteWithChildren(id);

                    request.AddJsonBody(report);

                    IRestResponse response = await client.ExecuteAsync(request);

                    var statusCode = response.StatusDescription;

                    if (statusCode == "OK")
                    {
                        var result = JsonConvert.DeserializeObject<Response_HTTP>(response.Content);
                        //Agregar código que retorne 
                        //retorne el numero de folio

                        //Actualizo el folio del reporte
                        if (await App.DataBase.UpdateFolio(id, result.Folio))
                        {
                            return Tuple.Create<bool, int, string>(true, result.Folio, result.Concepto);
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Sistema de gastos", "El reporte se envió correctamente. Sin embargo, hubo un error al actualizar el folio.", "Ok");
                            return Tuple.Create<bool, int, string>(true, result.Folio, result.Concepto);
                        }
                    }
                    else if (statusCode == "Bad Request")
                    {
                        var result = JsonConvert.DeserializeObject<Response_HTTP>(response.Content);

                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", result.Resultado, "Ok");
                        return Tuple.Create<bool, int, string>(false, -1, result.Resultado);
                    }
                    else if (statusCode == "Unauthorized")
                    {
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Tu token ha caducado.\nPor favor, vuelva a iniciar sesión.", "OK");
                        return Tuple.Create<bool, int, string>(false, -1, "Token caducado.");
                    }
                    else if (statusCode == null)
                    {
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No hay conexión a Internet.", "Ok");
                        return Tuple.Create<bool, int, string>(false, -1, null);
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No se encontró ningún resultado.", "Ok");
                        return Tuple.Create<bool, int, string>(false, -1, null);
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Id de usuario local no valido.", "Ok");
                    return Tuple.Create<bool, int, string>(false, -1, null);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");
                return Tuple.Create<bool, int, string>(false, -1, null);
            }
        }

        //Servicio para enviar token, verificar si ha sido enviado.
        public static async Task<bool> SendToken(string token)
        {
            try
            {
                //enviarlo al serrvidor y esperar respuesta                
                var client = new RestClient($"https://eis-latam.info/WebService/api/Empleados/token_movil");
                client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", UserData.token));
                var request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;

                request.AddJsonBody(new
                {
                    Emp_num = UserData.Emp_Num,
                    Token = token
                });

                IRestResponse response = await client.ExecuteAsync(request);

                var statusCode = response.StatusDescription;

                if (statusCode == "OK")
                {
                    return true;
                }
                else if (statusCode == "Bad Request")
                {
                    var result = JsonConvert.DeserializeObject<Response_HTTP>(response.Content);

                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", result.Resultado, "Ok");
                    return false;
                }
                else if (statusCode == "Unauthorized")
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Tu token ha caducado.\nPor favor, vuelva a iniciar sesión.", "OK");
                    return false;
                }
                else if (statusCode == null)
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No hay conexión a Internet.", "Ok");
                    return false;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No se encontró ningún resultado.", "Ok");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");
                return false;
            }
        }
        public static async Task<bool> Pendientes()
        {
            try
            {
                //UserDialogs.Instance.ShowLoading("Conectando...");
                var client = new RestClient($"https://eis-latam.info/WebService/api/reportes/pendientes/{UserData.Emp_Num}");
                client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", UserData.token));
                var request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;

                IRestResponse response = await client.ExecuteAsync(request);

                var content = response.Content;
                var statusCode = response.StatusDescription;

                if (statusCode == "OK")
                {
                    var pendingReportsList = new List<ReportesActualizados>();

                    pendingReportsList = JsonConvert.DeserializeObject<List<ReportesActualizados>>(response.Content);

                    if (pendingReportsList.Count > 0)
                    {
                        foreach (ReportesActualizados item in pendingReportsList)
                        {
                            //Actualizar reportes
                            bool isCorrect = await App.DataBase.UpdateReportSent(item.Folio, item.Gasto_Release, item.Gasto_Owner);

                            if (!isCorrect)
                            {
                                return false;
                            }
                        }
                        return true;
                    }

                    return false;
                }
                else if (statusCode == "Not Found")
                {
                    //var result = JsonConvert.DeserializeObject<Response_HTTP>(response.Content);
                    //await App.Current.MainPage.DisplayAlert("Sistema de gastos", result.Resultado, "Ok");

                    //No hay cambios para actualizar.

                    return true;
                }
                else if (statusCode == "Bad Request")
                {
                    var result = JsonConvert.DeserializeObject<Response_HTTP>(response.Content);

                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", result.Resultado, "Ok");
                    return false;
                }
                else if (statusCode == "Unauthorized")
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Tu token ha caducado.\nPor favor, vuelva a iniciar sesión.", "OK");
                    return false;
                }
                else if (statusCode == null)
                {
                    //UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No hay conexión a Internet.", "Ok");
                    return false;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No se encontró ningún resultado.", "Ok");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");
                return false;
            }
        }
        public static async Task<bool> Folios_Pendientes(IList<decimal> folios_list)
        {
            try
            {
                var client = new RestClient($"https://eis-latam.info/WebService/api/reportes/folios_pendientes");
                client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", UserData.token));
                var request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;

                request.AddJsonBody(new
                {
                    Emp_Num = UserData.Emp_Num,
                    Folios = folios_list
                });

                IRestResponse response = await client.ExecuteAsync(request);

                var content = response.Content;
                var statusCode = response.StatusDescription;

                if (statusCode == "OK")
                {
                    //Revisar lista de reportes actulizados
                    var pendingReportsList = new List<ReportesActualizados>();

                    pendingReportsList = JsonConvert.DeserializeObject<List<ReportesActualizados>>(response.Content);

                    if (pendingReportsList.Count > 0)
                    {
                        foreach (ReportesActualizados item in pendingReportsList)
                        {
                            //Actualizar reportes
                            bool isCorrect = await App.DataBase.UpdateReportSent(item.Folio, item.Gasto_Release, item.Gasto_Owner);

                            if (!isCorrect)
                            {
                                return false;
                            }
                        }
                        return true;
                    }

                    return false;
                }
                else if (statusCode == "Not Found")
                {
                    //var result = JsonConvert.DeserializeObject<Response_HTTP>(response.Content);
                    //await App.Current.MainPage.DisplayAlert("Sistema de gastos", result.Resultado, "Ok");

                    //No hay cambios para actualizar.

                    return true;
                }
                else if (statusCode == "Bad Request")
                {
                    var result = JsonConvert.DeserializeObject<Response_HTTP>(response.Content);

                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", result.Resultado, "Ok");
                    return false;
                }
                else if (statusCode == "Unauthorized")
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Tu token ha caducado.\nPor favor, vuelva a iniciar sesión.", "OK");
                    return false;
                }
                else if (statusCode == null)
                {
                    //UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No hay conexión a Internet.", "Ok");
                    return false;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No se encontró ningún resultado.", "Ok");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");
                return false;
            }
        }
        public static async Task<bool> Folios_Anteriores(IList<decimal> folios_list)
        {
            try
            {
                var client = new RestClient($"https://eis-latam.info/WebService/api/reportes/Folios_Anteriores");
                client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", UserData.token));
                var request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;

                request.AddJsonBody(new
                {
                    Emp_Num = UserData.Emp_Num,
                    Folios = folios_list
                });

                IRestResponse response = await client.ExecuteAsync(request);

                var content = response.Content;
                var statusCode = response.StatusDescription;

                if (statusCode == "OK")
                {
                    //Revisar lista de reportes actulizados
                    var pendingReportsList = new List<ReportesActualizados>();

                    pendingReportsList = JsonConvert.DeserializeObject<List<ReportesActualizados>>(response.Content);

                    if (pendingReportsList.Count > 0)
                    {
                        foreach (ReportesActualizados item in pendingReportsList)
                        {
                            //Actualizar reportes
                            bool isCorrect = await App.DataBase.UpdateReportSent(item.Folio, item.Gasto_Release, item.Gasto_Owner);

                            if (!isCorrect)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                    return false;
                }
                else if (statusCode == "Not Found")
                {
                    //var result = JsonConvert.DeserializeObject<Response_HTTP>(response.Content);
                    //await App.Current.MainPage.DisplayAlert("Sistema de gastos", result.Resultado, "Ok");

                    //No hay cambios para actualizar.

                    return true;
                }
                else if (statusCode == "Bad Request")
                {
                    var result = JsonConvert.DeserializeObject<Response_HTTP>(response.Content);

                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", result.Resultado, "Ok");
                    return false;
                }
                else if (statusCode == "Unauthorized")
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "Tu token ha caducado.\nPor favor, vuelva a iniciar sesión.", "OK");
                    return false;
                }
                else if (statusCode == null)
                {
                    //UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No hay conexión a Internet.", "Ok");
                    return false;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", "No se encontró ningún resultado.", "Ok");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sistema de gastos", ex.Message, "Ok");
                return false;
            }
        }
    }
}
