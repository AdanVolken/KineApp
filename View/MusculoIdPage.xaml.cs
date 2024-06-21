using Microsoft.Maui.Controls;
using KineApp.Model;
using KineApp.DataAcces;
using System;
using System.Linq;
using System.IO;

namespace KineApp.Views
{
    public partial class MusculoIdPage : ContentPage
    {
        private KineDBconexion _databaseService;
        private int _musculoId;

        public MusculoIdPage(int musculoId)
        {
            InitializeComponent();
            _databaseService = new KineDBconexion();
            _musculoId = musculoId;
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
                // Obtener el m�sculo seleccionado
                var musculo = _databaseService.GetItems<MusculoModel>().FirstOrDefault(m => m.IdMusculo == _musculoId);

                if (musculo != null)
                {
                    // Establecer el t�tulo de la p�gina y el nombre del m�sculo
                    Title = musculo.Nombre;
                    MusculoNombreLabel.Text = musculo.Nombre;

                    // Cargar imagen de la parte
                    if (musculo.Punto != null)
                    {
                        ParteImage.Source = ImageSource.FromStream(() => new MemoryStream(musculo.Punto));
                    }

                    // Cargar partes relacionadas con el m�sculo
                    var musculoParte = _databaseService.GetItems<MusculoModel>().Where(m => m.IdMusculo == _musculoId).Select(m => m.IdParte).ToList();
                    var parte = _databaseService.GetItems<ParteModel>().Where(p => musculoParte.Contains(p.IdParte)).ToList();
                    MusculosCollectionView.ItemsSource = parte;

                    // Cargar s�ntomas relacionados con el m�sculo
                    var musculoSintoma = _databaseService.GetItems<MusculoSintomaModel>().Where(ms => ms.IdMusculo == _musculoId).Select(ms => ms.IdSintoma).ToList();
                    var sintoma = _databaseService.GetItems<SintomaModel>().Where(s => musculoSintoma.Contains(s.IdSintoma)).ToList();
                    SintomasCollectionView.ItemsSource = sintoma;
                }
                else
                {
                    DisplayAlert("Error", "M�sculo no encontrado", "OK");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Hubo un error al cargar la informaci�n del m�sculo: {ex.Message}", "OK");
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

        private async void Button_Clicked_Parte(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is ParteModel selectedParte)
            {
                await Navigation.PushAsync(new ParteDetallePage(selectedParte.IdParte));
            }
        }

        private async void Button_Clicked_Sintomas(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is SintomaModel selectedSintoma)
            {
                await Navigation.PushAsync(new SintomaIdPage(selectedSintoma.IdSintoma));
            }
        }
    }
}