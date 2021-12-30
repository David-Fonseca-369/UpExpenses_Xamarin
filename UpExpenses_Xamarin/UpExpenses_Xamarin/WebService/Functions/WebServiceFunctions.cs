using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UpExpenses_Xamarin.Models;

namespace UpExpenses_Xamarin.WebService.Functions
{
    public static class WebServiceFunctions
    {
        //Función para verififcar que si existen folios pendientes
        //En casop de existir, recibe las modidifcaciones y las actuliza en la base de datos
        public static async Task<bool> SendPendingsReports()
        {
            //Consulta para traer lista de reportes con folios pendientes 
            IList<decimal> folios_pendientes = await App.DataBase.GetAllPendindReportsFolios(UserData.Emp_Num);

            bool isCorrect = false;

            if (folios_pendientes.Count > 0)
            {
                isCorrect = await RestApiService.Folios_Pendientes(folios_pendientes);
            }
            else
            {
                //No revisa nada porque no hay datos, por eso retorno un true.
                isCorrect = true;
            }

            return isCorrect;
        }
        public static async Task<bool> SendAnterioresReports()
        {
            //Consulta para traer lista de reportes con folios pendientes 
            IList<decimal> folios_pendientes = await App.DataBase.GetAllPendindReportsFolios(UserData.Emp_Num);

            bool isCorrect = false;

            if (folios_pendientes.Count > 0)
            {
                isCorrect = await RestApiService.Folios_Pendientes(folios_pendientes);
            }
            else
            {
                //No revisa nada porque no hay datos, por eso retorno un true.
                isCorrect = true;
            }

            return isCorrect;
        }

        //Crear metodo para actulizar los datos del sistema 

        //Actualizar fecha
        //Consumir rest APi
        //Mandare a llamar la función, justamente cuando mando el token.




    }
}
