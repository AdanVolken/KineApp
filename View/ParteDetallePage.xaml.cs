using Microsoft.Maui.Controls;
using KineApp.Model;
using KineApp.DataAcces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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
                // Cargar imagen de la parte
                var parte = _databaseService.GetItems<ParteModel>()
                                            .FirstOrDefault(p => p.IdParte == _parteId);
                if (parte != null && parte.ImgParte != null)
                {
                    Console.WriteLine($"Imagen de parte: {parte.ImgParte.Length} bytes");
                    ParteImage.Source = ImageSource.FromStream(() => new MemoryStream(parte.ImgParte));
                }

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