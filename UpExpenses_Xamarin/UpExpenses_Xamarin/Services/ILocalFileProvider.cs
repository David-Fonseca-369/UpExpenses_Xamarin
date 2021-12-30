
namespace UpExpenses_Xamarin.Services
{
    public interface ILocalFileProvider
    {
        byte[] GetFileBytes(string path);
    }
    
    public static class FileUtility
    {
        public static ILocalFileProvider FileSystem { get; set; }

        public static void SetUp(ILocalFileProvider fs)
        {
            FileSystem = fs;
        }
    }
}
