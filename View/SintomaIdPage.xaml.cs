using Microsoft.Maui.Controls;
using KineApp.Model;
using KineApp.DataAcces;
using System;
using System.Linq;

namespace KineApp.Views
{
    public partial class SintomaIdPage : ContentPage
    {
        private KineDBconexion _databaseService;
        private int _sintomaId;

        public SintomaIdPage(int sintomaId)
        {
            InitializeComponent();
            _databaseService = new KineDBconexion();
            _sintomaId = sintomaId;
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
                // Obtener el síntoma seleccionado
                var sintoma = _databaseService.GetItems<SintomaModel>().FirstOrDefault(s => s.IdSintoma == _sintomaId);

                if (sintoma != null)
                {
                    // Establecer el título de la página y el nombre del síntoma
                    Title = sintoma.Nombre;
                    SintomaNombreLabel.Text = sintoma.Nombre;

                    // Cargar partes relacionadas con el síntoma
                    var sintomaParte = _databaseService.GetItems<SintomaParte>().Where(sp => sp.IdSintoma == _sintomaId).Select(sp => sp.IdParte).ToList();
                    var parte = _databaseService.GetItems<ParteModel>().Where(p => sintomaParte.Contains(p.IdParte)).ToList();
                    MusculosCollectionView.ItemsSource = parte;

                    // Cargar músculos relacionados con el síntoma
                    var sintomaMusculo = _databaseService.GetItems<MusculoSintomaModel>().Where(sm => sm.IdSintoma == _sintomaId).Select(sm => sm.IdMusculo).ToList();
                    var musculo = _databaseService.GetItems<MusculoModel>().Where(m => sintomaMusculo.Contains(m.IdMusculo)).ToList();
                    SintomasCollectionView.ItemsSource = musculo;
                }
                else
                {
                    DisplayAlert("Error", "Síntoma no encontrado", "OK");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Hubo un error al cargar la información del síntoma: {ex.Message}", "OK");
            }
        }

        private async void Button_Clicked_Parte(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is ParteModel selectedParte)
            {
                await Navigation.PushAsync(new ParteDetallePage(selectedParte.IdParte));
            }
        }

        private async void Button_Clicked_Musculo(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is MusculoModel selectedMusculo)
            {
                await Navigation.PushAsync(new MusculoIdPage(selectedMusculo.IdMusculo));
            }
        }
    }
}