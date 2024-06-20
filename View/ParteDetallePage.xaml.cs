using Microsoft.Maui.Controls;
using KineApp.Model;
using KineApp.DataAcces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using KineApp.Views;

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

                var sintomasPartes = _databaseService.GetItems<SintomaParte>()
                                                     .Where(sp => sp.IdParte == _parteId)
                                                     .Select(sp => sp.IdSintoma)
                                                     .ToList();
                var sintomas = _databaseService.GetItems<SintomaModel>()
                                              .Where(s => sintomasPartes.Contains(s.IdSintoma))
                                              .ToList();
                SintomasCollectionView.ItemsSource = sintomas;

                // Agregar eventos SelectionChanged
                SintomasCollectionView.SelectionChanged += SintomasCollectionView_SelectionChanged;
                MusculosCollectionView.SelectionChanged += MusculosCollectionView_SelectionChanged;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cargando datos: {ex.Message}");
            }
        }

        private void SintomasCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is SintomaModel selectedSintoma)
            {
                Navigation.PushAsync(new SintomaIdPage(selectedSintoma.IdSintoma));
                Console.WriteLine("Seleccionaste un síntoma.");
            }
        }

        private void MusculosCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is MusculoModel selectedMusculo)
            {
                Navigation.PushAsync(new MusculoIdPage(selectedMusculo.IdMusculo));
                Console.WriteLine("Seleccionaste un músculo.");
            }
        }

        // Lo que hace es agrandar la imagen en pantalla completa  
        private void OnImageTapped(object sender, EventArgs e)
        {
            if (ParteImage.Source != null)
            {
                Navigation.PushAsync(new FullScreenImagePage(ParteImage.Source));
            }
        }
    }
}