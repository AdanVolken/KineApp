using Microsoft.Maui.Controls;
using KineApp.DataAcces;
using KineApp.Model;
using System.Collections.Generic;
using System;

namespace KineApp
{
    public partial class MainPage : ContentPage
    {
        private KineDBconexion _databaseService;

        public MainPage()
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
                Console.WriteLine($"Número de músculos obtenidos: {musculos.Count}");

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