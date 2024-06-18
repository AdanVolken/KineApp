using Microsoft.Maui.Controls;
using KineApp.Model;
using KineApp.DataAcces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KineApp.Views
{
    public partial class ParteDetallePage : ContentPage
    {
        private KineDBconexion _databaseService;
        private int _parteId;

        public ParteDetallePage(int parteId)
        {
            InitializeComponent();
            _databaseService = new KineDBconexion();
            _parteId = parteId;
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
                // Obtener músculos relacionados con la parte
                var musculos = _databaseService.GetItems<MusculoModel>()
                                               .Where(m => m.IdParte == _parteId)
                                               .ToList();
                MusculosCollectionView.ItemsSource = musculos;

                // Obtener síntomas relacionados con la parte
                var sintomasPartes = _databaseService.GetItems<SintomaParte>()
                                                     .Where(sp => sp.IdParte == _parteId)
                                                     .Select(sp => sp.IdSintoma)
                                                     .ToList();
                var sintomas = _databaseService.GetItems<SintomaModel>()
                                               .Where(s => sintomasPartes.Contains(s.IdSintoma))
                                               .ToList();
                SintomasCollectionView.ItemsSource = sintomas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cargando datos: {ex.Message}");
            }
        }
    }
}