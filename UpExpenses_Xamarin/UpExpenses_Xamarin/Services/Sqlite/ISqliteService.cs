using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UpExpenses_Xamarin.Models;

namespace UpExpenses_Xamarin.Services.Sqlite
{
    interface ISqliteService
    {
        Task<IList<CabeceraReporte>> GetAllPendingReports();
    }
}
