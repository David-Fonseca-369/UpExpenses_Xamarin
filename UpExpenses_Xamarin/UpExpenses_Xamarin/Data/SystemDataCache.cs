using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace UpExpenses_Xamarin.Data
{
    public class SystemDataCache
    {
        public static bool firstSelection {get; set;}
        public static bool insertWithChildren { get; set; }
        public static int idHeadBoard { get; set; }       

        //En esta variable "kindOfSpendindToSave" almacena el tipo de gasto que se guardará
        // 1 = Se guardará después de haber insertado el padre e hijo anterior, por lo que se traerá el último Id.
        // 2 = Se guardará con el Id del gasto seleccionado desde "agregar nuevo gasto", en la ventana de Gastos.
        public static int  kindOfSpendindToSave { get; set; }     
        //Guardar imagen
        public static ImageSource resultImage { get; set; }
        //Respuesta folio del reporte enviado
        public static int folio { get; set; }
        //Respuesta concepto del reporte enviado
        public static string concepto { get; set; }
    
    }
}
