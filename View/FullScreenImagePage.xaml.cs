using Microsoft.Maui.Controls;

namespace KineApp.Views
{
    public partial class FullScreenImagePage : ContentPage
    {
        public FullScreenImagePage(ImageSource imageSource)
        {
            InitializeComponent();
            FullScreenImage.Source = imageSource;
        }
    }
}