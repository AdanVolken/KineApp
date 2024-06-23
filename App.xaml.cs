using Microsoft.Maui;
using Microsoft.Maui.Controls;
using KineApp.Views;
using KineApp.DataAcces;

namespace KineApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Limpiar datos de Sesion y Cliente
            //var dbService = new KineDBconexion();
            //dbService.ClearTables();
            MainPage = new NavigationPage(new PaginaPrincipal());
        }
    }
}