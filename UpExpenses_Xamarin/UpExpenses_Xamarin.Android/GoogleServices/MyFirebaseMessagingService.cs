using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Messaging;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UpExpenses_Xamarin.Droid.GoogleServices
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        AndroidNotificationManager androidNotification = new AndroidNotificationManager();
        public override void OnMessageReceived(RemoteMessage message)
        {

            Log.Debug(TAG, "From: " + message.From);
            Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);
            androidNotification.CrearNotificacionLocal(message.GetNotification().Title, message.GetNotification().Body);

            //Publico el mensaje recibido.
            MessagingCenter.Send<string>(message.GetNotification().Body, "MensajeFirebase");
        }
        //Recibe token al instalarse.
        public override void OnNewToken(string token)
        {
            base.OnNewToken(token);

            Preferences.Set("TokenFirebase", token);
            sedRegisterToken(token);

        }
        public async void sedRegisterToken(string token)
        {
            //Tu código para registrar el token a tu servidor y base de datos
            //var tokenFirebase = new TokenFirebase { Token = token };
            //App.DataBase.SaveToken(tokenFirebase);

            await App.DataBase.SaveToken(token);
        }
    }
}