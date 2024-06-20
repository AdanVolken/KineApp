using Microsoft.Maui.Controls;
using KineApp.Model;
using KineApp.DataAcces;
using System;

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
            // Cargar imagen de la parte
            var punto = _databaseService.GetItems<MusculoModel>()
                                        .FirstOrDefault(p => p.IdMusculo == _musculoId);
            if (punto != null && punto.Punto != null)
            {
                ParteImage.Source = ImageSource.FromStream(() => new MemoryStream(punto.Punto));
            }


            // Saco las aprtes del musculo seleccinado
            var musculoParte =  _databaseService.GetItems<MusculoModel>().Where(m => m.IdMusculo == _musculoId).Select(m => m.IdParte).ToList();
            var parte = _databaseService.GetItems<ParteModel>().Where(p => musculoParte.Contains(p.IdParte)).ToList();
            MusculosCollectionView.ItemsSource = parte;

            //Sacar Sintomas relacionados a ese musculo
            var musculoSintoma = _databaseService.GetItems<MusculoSintomaModel>().Where(ms => ms.IdMusculo == _musculoId).Select(ms => ms.IdSintoma).ToList();  
            var sintoma = _databaseService.GetItems<SintomaModel>().Where(s => musculoSintoma.Contains(s.IdSintoma)).ToList();
            SintomasCollectionView.ItemsSource = sintoma;



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
            if (sender is Button button && button.BindingContext is SintomaModel selectedParte)
            {
                await Navigation.PushAsync(new SintomaIdPage(selectedParte.IdSintoma));
            }
        }
    }
}