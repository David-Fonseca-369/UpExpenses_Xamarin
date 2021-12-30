using System.IO;
using UpExpenses_Xamarin.Services;

namespace UpExpenses_Xamarin.Droid.Services
{
    public class LocalFileProviderAnroid : ILocalFileProvider
    {
        public byte[] GetFileBytes(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}