using Microsoft.Maui.Controls;
using KineApp.Model;
using KineApp.DataAcces;
using System;
using System.Collections.Generic;

namespace KineApp.Views
{
    public partial class SintomaPage : ContentPage
    {
        private KineDBconexion _databaseService;

        public SintomaPage()
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
                // Obtener datos de la tabla Sintoma
                var sintomas = _databaseService.GetItems<SintomaModel>();
                Console.WriteLine($"Número de síntomas obtenidos: {sintomas.Count}");

                // Verificar que se obtienen datos
                foreach (var sintoma in sintomas)
                {
                    Console.WriteLine($"Sintoma: {sintoma.Nombre} - {sintoma.Descri}");
                }

                // Enlazar los datos al CollectionView
                SintomasCollectionView.ItemsSource = sintomas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cargando datos: {ex.Message}");
            }
        }
    }
}
