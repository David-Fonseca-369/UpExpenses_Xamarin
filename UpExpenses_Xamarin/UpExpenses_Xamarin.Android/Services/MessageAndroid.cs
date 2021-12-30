using Android.App;
using Android.Widget;
using UpExpenses_Xamarin.Services;

namespace UpExpenses_Xamarin.Droid.Services
{
    class MessageAndroid : IMessage
    {
        public void LongMessage(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortMessage(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}