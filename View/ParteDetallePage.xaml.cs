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

                    ParteImage.Source = ImageSource.FromStream(() => new MemoryStream(parte.ImgParte));
                }

                // Obtener músculos relacionados con la parte
                var musculos = _databaseService.GetItems<MusculoModel>()
                                               .Where(m => m.IdParte == _parteId)
                                               .ToList();
                MusculosCollectionView.ItemsSource = musculos;

                // Obtener Partes relacionadas con los sintomas
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

        // Lo que hace es agrandar la imagen en pantalla completa  
        private void OnImageTapped(object sender, EventArgs e)
        {
            if (ParteImage.Source != null)
            {
                Navigation.PushAsync(new FullScreenImagePage(ParteImage.Source));
            }
        }

        private async void Button_Clicked_Musculo(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is MusculoModel selectedMusculo)
            {
                Console.WriteLine($"Selected Musculo: {selectedMusculo.Nombre}");
                await Navigation.PushAsync(new MusculoIdPage(selectedMusculo.IdMusculo));
            }
        }

        private async void Button_Clicked_Sintoma(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is SintomaModel selectedSintoma)
            {
                Console.WriteLine($"Selected Sintoma: {selectedSintoma.Nombre}");
                await Navigation.PushAsync(new SintomaIdPage(selectedSintoma.IdSintoma));
            }
        }
    }
}