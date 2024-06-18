using Microsoft.Maui;
using Microsoft.Maui.Controls;
using KineApp.Views;

namespace KineApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new PaginaPrincipal());
        }
    }
}