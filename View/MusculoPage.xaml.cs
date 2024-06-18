using Microsoft.Maui.Controls;
using KineApp.Model;
using KineApp.DataAcces;
using System.Collections.Generic;

namespace KineApp.Views
{
    public partial class MusculoPage : ContentPage
    {
        private KineDBconexion _databaseService;

        public MusculoPage()
        {
            InitializeComponent();
            _databaseService = new KineDBconexion();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Obtener datos de la tabla Musculo
                var musculos = _databaseService.GetItems<MusculoModel>();
                Console.WriteLine($"N�mero de m�sculos obtenidos: {musculos.Count}");

                // Enlazar los datos al CollectionView
                MusculosCollectionView.ItemsSource = musculos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cargando datos: {ex.Message}");
            }
        }
    }
}