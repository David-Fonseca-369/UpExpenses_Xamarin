using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UpExpenses_Xamarin.Models;
using UpExpenses_Xamarin.Services.Sqlite;

namespace UpExpenses_Xamarin.Data
{
    public class DataBase
    {
        readonly SQLiteAsyncConnection _databaseAsync;

        public DataBase(string dbpath)
        {
            _databaseAsync = new SQLiteAsyncConnection(dbpath);

            //Crear tablas
            try
            {
                _databaseAsync.CreateTableAsync<UsernameCredentials>().Wait();
                _databaseAsync.CreateTableAsync<CabeceraReporte>().Wait();
                _databaseAsync.CreateTableAsync<DetalleReporte>().Wait();
                _databaseAsync.CreateTableAsync<DatosDelSistema>().Wait();
                _databaseAsync.CreateTableAsync<TipoGastos>().Wait();
                _databaseAsync.CreateTableAsync<Gasto>().Wait();
                _databaseAsync.CreateTableAsync<TokenDB>().Wait();

            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }
        }

        ///////////////Tareas tabla usernameCredentials////////////////
        public Task<int> SaveNameAsync(UsernameCredentials userCredentials)
        {
            return _databaseAsync.InsertAsync(userCredentials);
        }
        public Task<int> UpdateNameAsync(UsernameCredentials usernameCredentials)
        {
            return _databaseAsync.UpdateAsync(usernameCredentials);
        }
        public async Task<IList<UsernameCredentials>> UserSavedAsync(int id)
        {
            return await _databaseAsync.QueryAsync<UsernameCredentials>("SELECT * FROM UsernameCredentials WHERE Id = ?", id);
        }
        public async Task<int> DropTableUserNameCredentials()
        {
            return await _databaseAsync.DropTableAsync<UsernameCredentials>();
        }
        //////////////////////Reportes pendientes////////////////////////////////

        public Task InsertReportWithChild(CabeceraReporte cabeceraReporte)
        {
            return _databaseAsync.InsertWithChildrenAsync(cabeceraReporte);
        }
        public Task<List<CabeceraReporte>> GetAllReporteWithChildren()
        {
            return _databaseAsync.GetAllWithChildrenAsync<CabeceraReporte>();
        }
        public async Task<CabeceraReporte> GetListReporteWithChildren(int idHeader)
        {
            try
            {
                return await _databaseAsync.GetWithChildrenAsync<CabeceraReporte>(idHeader);
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return null;
            }
        }
        public async Task<IList<CabeceraReporte>> GetAllPendingReports(decimal emp_num)
        {
            return await _databaseAsync.Table<CabeceraReporte>().Where(x => x.Emp_Num == emp_num).ToListAsync();
        }

        //Lista de reporte pendiente, que no han sido enviados, ni autorizados 
        public async Task<IList<CabeceraReporte>> GetAllOnlyPendingsAsync(decimal emp_num)
        {
            return await _databaseAsync.Table<CabeceraReporte>().Where(x => x.Emp_Num == emp_num && x.Folio == 0 && x.Gasto_Release == 0 & x.Gasto_Owner == 0).ToListAsync();
        }
             

        public async Task<IList<CardReportePendiente>> GetAllPendindReportsCard(decimal emp_num)
        {
            try
            {
                //Sólo filtro los reportes cuyo folio es 0, ya que son los que estan pendientes.
                return await _databaseAsync.QueryAsync<CardReportePendiente>("SELECT Id, Concepto, PeriodoReporte, Total FROM CabeceraReporte WHERE Emp_Num = ? AND Folio = 0", emp_num);
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return null;
            }
        }
        public async Task<IList<decimal>> GetAllPendindReportsFolios(decimal emp_num)
        {
            try
            {
                var folios_pendientes = await _databaseAsync.QueryAsync<Datos_Reporte>($"SELECT Id, Folio " +
                    $"FROM CabeceraReporte " +
                    $"WHERE (Emp_Num = {emp_num} AND Gasto_Release = 0 AND Gasto_Owner = 0 AND Folio > 0) OR " +
                          $"(Emp_Num = {emp_num} AND Gasto_Release = 1 AND Gasto_Owner = 0 AND Folio > 0) OR " +
                          $"(Emp_Num = {emp_num} AND Gasto_Release = 0 AND Gasto_Owner = 1 AND Folio > 0)");
                List<decimal> folios_list = new List<decimal>();

                if (folios_pendientes.Count > 0)
                {
                    foreach (var item in folios_pendientes)
                    {
                        folios_list.Add(item.Folio);
                    }

                    return folios_list;
                }
                else
                {
                    return folios_list;
                }
                //Sólo filtro los reportes cuyo folio es 0, ya que son los que estan pendientes.             

            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return null;
            }
        }
        public async Task<IList<decimal>> GetAllPreviousReportsFolios(decimal emp_num)
        {
            try
            {
                var folios_pendientes = await _databaseAsync.QueryAsync<Datos_Reporte>($"SELECT Id, Folio FROM CabeceraReporte " +
                    $"WHERE (Emp_Num = {emp_num} AND Gasto_Release = 0 AND Gasto_Owner > 0 AND Folio > 0) OR" +
                    $"      (Emp_Num = {emp_num} AND Gasto_Release > 0 AND Gasto_Owner = 0 AND Folio > 0)");

                List<decimal> folios_list = new List<decimal>();

                if (folios_pendientes.Count > 0)
                {
                    foreach (var item in folios_pendientes)
                    {
                        folios_list.Add(item.Folio);
                    }
                    return folios_list;
                }
                else
                {
                    return folios_list;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return null;
            }
        }
        public async Task<IList<CardReportePendiente>> GetAllReportsSentCard(int emp_num)
        {
            try
            {
                return await _databaseAsync.QueryAsync<CardReportePendiente>($"SELECT Id, Concepto, PeriodoReporte, Total " +
                    $"FROM CabeceraReporte " +
                    $"WHERE (Emp_Num = {emp_num} AND Folio > 0 AND Gasto_Release = 0 AND Gasto_Owner = 0) OR " +
                    $"(Emp_Num = {emp_num} AND Folio > 0 AND Gasto_Release = 1 AND Gasto_Owner = 0) OR "+
                    $"(Emp_Num = {emp_num} AND Folio > 0 AND Gasto_Release = 0 AND Gasto_Owner = 1)");
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return null;
            }
        }
        public async Task<IList<CardReportePendiente>> GetAllReportsPreviousCard(int emp_num)
        {

            //La consulta debe traer reportes que ya esten autorizados por ambas partes o en dado caso
            //traerlo si al menos uno ya ha sido cancelado.
            try
            {
                //return await _databaseAsync.QueryAsync<CardReportePendiente>($"SELECT Id, Concepto, PeriodoReporte, Total FROM CabeceraReporte WHERE (Emp_Num = {emp_num} AND Folio > 0 AND Gasto_Release > 0)  OR (Emp_Num = {emp_num} AND Folio > 0 AND Gasto_Owner > 0)");
                return await _databaseAsync.QueryAsync<CardReportePendiente>($"" +
                    $"SELECT Id, Concepto, PeriodoReporte, Total " +
                    $"FROM CabeceraReporte " +
                    $"WHERE (Emp_Num = {emp_num} AND Folio > 0 AND Gasto_Release = 1 AND Gasto_Owner = 1)  " +
                    $"OR " +
                    $"(Emp_Num = {emp_num} AND Folio > 0 AND Gasto_Release = 2) " +
                    $"OR " +
                    $"(Emp_Num = {emp_num} AND Folio > 0 AND Gasto_Owner = 2)");
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return null;
            }
        }



        public async Task<IList<CabeceraReporte>> GetAllPendingReports2(int emp_num) => await _databaseAsync.Table<CabeceraReporte>().Where(x => x.Emp_Num == emp_num).ToListAsync();
        public Task<List<DetalleReporte>> GetAllChildren()
        {
            return _databaseAsync.Table<DetalleReporte>().ToListAsync();
        }
        public async Task<IList<DetalleReporte>> GetChildren(int idReporte)
        {
            return await _databaseAsync.QueryAsync<DetalleReporte>("SELECT * FROM DetalleReporte WHERE IdCabecera = ?", idReporte);
        }
        public async Task<IList<CardGasto>> GetChildrenCard(int idReporte)
        {
            return await _databaseAsync.QueryAsync<CardGasto>("SELECT * FROM DetalleReporte WHERE IdCabecera = ?", idReporte);
        }
        public Task<int> InsertChild(DetalleReporte detalleReporte)
        {
            return _databaseAsync.InsertAsync(detalleReporte);
        }
        public async Task UpdateReport(int noGastos, double total, int id)
        {
            try
            {
                await _databaseAsync.QueryAsync<CabeceraReporte>("UPDATE CabeceraReporte SET NoGastos = ?, Total = ? WHERE Id = ?", noGastos, total, id);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        public async Task<bool> UpdateFolio(int idHeader, decimal folio)
        {
            try
            {
                await _databaseAsync.QueryAsync<CabeceraReporte>("UPDATE CabeceraReporte SET Folio = ? WHERE Id = ?", folio, idHeader);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return false;
            }
        }
        public async Task<bool> UpdateReportSent(decimal folio, decimal gasto_release, decimal gasto_owner)
        {
            try
            {
                //Buscar el reporte 
                CabeceraReporte result = await _databaseAsync.Table<CabeceraReporte>().Where(x => x.Folio == folio).FirstOrDefaultAsync();
                if (result != null)
                {
                    //Actualizo 
                    await _databaseAsync.QueryAsync<CabeceraReporte>($"UPDATE CabeceraReporte SET Gasto_Release = {gasto_release}, Gasto_Owner = {gasto_owner} WHERE Id = {result.Id}");
                    return true;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Sistema de gastos", $"El registro con folio No.{folio}, no existe.", "OK");
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return false;
            }

        }


        public async Task<bool> DeleteWithChildren(int id)
        {
            try
            {
                //Elimino hijos
                await _databaseAsync.QueryAsync<DetalleReporte>("DELETE FROM DetalleReporte WHERE IdCabecera = ?", id);
                //Elimino cabecera
                await _databaseAsync.QueryAsync<CabeceraReporte>("DELETE FROM CabeceraReporte WHERE Id = ?", id);

                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return false;
            }
        }
        public async Task DeleteChildren(int id)
        {
            try
            {
                await _databaseAsync.QueryAsync<DetalleReporte>("DELETE FROM DetalleReporte WHERE Id = ?", id);
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }
        }
        public async Task<int> LatestId()
        {
            try
            {
                IList<CabeceraReporte> result = await _databaseAsync.QueryAsync<CabeceraReporte>("SELECT * FROM CabeceraReporte WHERE ID = (SELECT MAX(ID) FROM CabeceraReporte);");
                return result[0].Id;
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return -1;
            }
        }
        public async Task<Tuple<int, int, double>> LatestRecord()
        {
            try
            {
                IList<CabeceraReporte> result = await _databaseAsync.QueryAsync<CabeceraReporte>("SELECT * FROM CabeceraReporte WHERE ID = (SELECT MAX(ID) FROM CabeceraReporte);");
                return Tuple.Create<int, int, double>(result[0].Id, result[0].NoGastos, result[0].Total);
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return Tuple.Create<int, int, double>(-1, -1, -1);
            }
        }
        public async Task<IList<CabeceraReporte>> HeadboardById(int id)
        {
            try
            {
                IList<CabeceraReporte> result = await _databaseAsync.QueryAsync<CabeceraReporte>("SELECT * FROM CabeceraReporte WHERE Id = ?", id);

                if (result.Count > 0)
                {
                    return result;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("SQLite", "No se encontró ningún registro.", "OK");
                    return null;
                }
                //return Tuple.Create<int, int, double>(result[0].Id, result[0].NoGastos, result[0].Total);                
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return null;
            }
        }
        public async Task<Tuple<int, double>> ChildrenById(int idChildren)
        {
            try
            {
                IList<DetalleReporte> resultList = await _databaseAsync.QueryAsync<DetalleReporte>("SELECT * FROM DetalleReporte WHERE Id = ?", idChildren);
                return Tuple.Create<int, double>(resultList[0].Id, resultList[0].Total);
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return Tuple.Create<int, double>(-1, -1);
            }
        }
        public async Task<IList<CabeceraReporte>> HeaderSearchById(int idHeader)
        {
            try
            {
                return await _databaseAsync.QueryAsync<CabeceraReporte>("SELECT * FROM CabeceraReporte WHERE Id = ?", idHeader);
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return null;
            }
        }
        public async Task<string> HeaderColumnData(int idHeader, string columnName)
        {
            try
            {
                IList<CabeceraReporte> result = await _databaseAsync.QueryAsync<CabeceraReporte>($"SELECT {columnName} FROM CabeceraReporte WHERE Id = ?", idHeader);
                if (result.Count > 0)
                {
                    //Agregar los campos que quiera retornar

                    if (columnName == "Viaje")
                    {
                        return result[0].Viaje;
                    }
                    else if (columnName == "Desde")
                    {
                        return result[0].Desde.ToString();
                    }
                    else if (columnName == "Hasta")
                    {
                        return result[0].Hasta.ToString();
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Alerta", $"El campo {columnName} no esta agregado.", "OK");
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return null;
            }
        }

        //////Datos del sistema///////
        public async Task InsertLastUpdate()
        {
            try
            {
                await _databaseAsync.QueryAsync<DatosDelSistema>("INSERT INTO DatosDelSistema (Id, LastUpdate) VALUES (1,?)", DateTime.Now);
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }
        }
        public async Task UpdateLastUpdate()
        {
            try
            {
                await _databaseAsync.QueryAsync<DatosDelSistema>("UPDATE DatosDelSistema SET LastUpdate = ? WHERE Id = 1", DateTime.Now);
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }
        }
        public async Task<IList<DatosDelSistema>> LoadLastUpdate()
        {
            try
            {
                return await _databaseAsync.QueryAsync<DatosDelSistema>("SELECT * FROM DatosDelSistema WHERE Id = 1");
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);

                return null;
            }
        }


        //////Tipo Gastos/////
        public async Task InsertTipoGastos(int id, string tipo_gasto)
        {
            try
            {
                //Vuelvo a crear tabla ya que la trunque
                await _databaseAsync.CreateTableAsync<TipoGastos>();
                await _databaseAsync.QueryAsync<TipoGastos>("INSERT INTO TipoGastos (Tipo_Id, Tipo_Gasto) VALUES (?,?)", id, tipo_gasto);
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }
        }
        public async Task<bool> DeleteTipoGastos()
        {
            try
            {
                await _databaseAsync.DropTableAsync<TipoGastos>();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return false;
            }
        }
        public async Task<IList<TipoGastos>> LoadTipoGastos()
        {
            try
            {
                return await _databaseAsync.Table<TipoGastos>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return null;
            }
        }
        ///Base de datos
        public async Task LoadMaster()
        {
            var result = await _databaseAsync.QueryAsync<int>("SELECT * FROM sqlite_master");
        }


        //////Gastos////

        public async Task<bool> DeleteGasto()
        {
            try
            {
                await _databaseAsync.DropTableAsync<Gasto>();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return false;
            }
        }

        public async Task InsertGasto(Gasto gasto)
        {
            try
            {
                //Ingreso tabla, ya que se truncó.
                await _databaseAsync.CreateTableAsync<Gasto>();

                //await _databaseAsync.InsertAsync();
                await _databaseAsync.InsertAsync(gasto);
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }
        }
        public async Task<IList<Gasto>> LoadGastos(int emp_type)
        {
            try
            {
                return await _databaseAsync.QueryAsync<Gasto>("SELECT * FROM Gasto WHERE emp_type = ?", emp_type);
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return null;
            }
        }

        //Consultar de acuerdo al nivel la cantidad de lo que puede gastar por el gasto seleccionado
        public async Task<double> CanSpend(int level, int emp_type, string expenseType)
        {
            try
            {
                var result = await _databaseAsync.QueryAsync<Gasto>("SELECT * FROM Gasto WHERE Gastos_Concepto = ? AND emp_type = ?", expenseType, emp_type);

                if (result.Count > 0)
                {
                    if (level == 1)
                    {
                        return result[0].MXP_1;
                    }
                    else if (level == 2)
                    {
                        return result[0].MXP_2;
                    }
                    else if (level == 3)
                    {
                        return result[0].MXP_3;
                    }
                    else if (level == 4)
                    {
                        return result[0].MXP_4;
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Base de datos", $"No existe el nivel {level}", "OK");
                        return -1;
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Base de datos", "No se encontró ningún resultado.", "OK");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return -1;
            }
        }

        //Consultar el Gastos_Cve del Concepto
        public async Task<int> Gasto_Cve(string spending_concept, int emp_type)
        {
            try
            {
                var result = await _databaseAsync.QueryAsync<Gasto>("SELECT * FROM Gasto WHERE Gastos_Concepto = ? AND emp_type = ?", spending_concept, emp_type);

                if (result.Count > 0)
                {
                    return result[0].Gastos_Cve;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("SQLite", "No se encontró ningún resultado.", "OK");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return -1;
            }
        }

        //////////// Start Token DB//////////////

        ///Guardar registro
        public async Task<bool> SaveToken(string token)
        {
            try
            {
                TokenDB tokenDB = new TokenDB();
                tokenDB = new TokenDB
                {
                    Id = 1,
                    Token = token,
                    Enviado = false
                };

                int result = await _databaseAsync.InsertAsync(tokenDB);

                if (result == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return false;
            }

        }
        ///Actualizar registro
        public async Task<int> UpdateToken(string token)
        {
            try
            {
                TokenDB tokenDB = new TokenDB();
                tokenDB = new TokenDB
                {
                    Id = 1,
                    Token = token,
                    Enviado = true
                };

                return await _databaseAsync.UpdateAsync(tokenDB);
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return -1;
            }
        }


        ///Consultar token
        public async Task<TokenDB> GetToken()
        {
            try
            {
                TokenDB result = await _databaseAsync.GetAsync<TokenDB>(x => x.Id == 1);

                return result;
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);

                return null;
            }
        }


        /////////////End Toke DB///////////////

        private async void ErrorMessage(Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("SQLite", ex.Message, "OK");
        }



    }
}
