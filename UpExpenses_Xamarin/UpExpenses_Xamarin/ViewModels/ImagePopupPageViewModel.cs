using System.Threading.Tasks;
using System.Windows.Input;
using UpExpenses_Xamarin.Data;
using Xamarin.Forms;

namespace UpExpenses_Xamarin.ViewModels
{
    public class ImagePopupPageViewModel : BaseViewModel
    {
        private ImageSource resultImage;
        public ImageSource ResultImage
        {
            get => resultImage;
            set => SetProperty(ref resultImage, value);
        }
        public ICommand ClosePopup { get; set; }

        public ImagePopupPageViewModel()
        {
            LoadImage();
            
            ClosePopup = new Command(async () => await Close());
        }

        private void LoadImage()
        {         
            ResultImage = SystemDataCache.resultImage;
        }
        private async Task Close()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
