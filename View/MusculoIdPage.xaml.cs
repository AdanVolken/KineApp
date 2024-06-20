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
            // Saco las aprtes del musculo seleccinado
            var musculoParte =  _databaseService.GetItems<MusculoModel>().Where(m => m.IdMusculo == _musculoId).Select(m => m.IdParte).ToList();
            var parte = _databaseService.GetItems<ParteModel>().Where(p => musculoParte.Contains(p.IdParte)).ToList();
            MusculosCollectionView.ItemsSource = parte;

            //Sacar Sintomas relacionados a ese musculo
            var musculoSintoma = _databaseService.GetItems<MusculoSintomaModel>().Where(ms => ms.IdMusculo == _musculoId).Select(ms => ms.IdSintoma).ToList();  
            var sintoma = _databaseService.GetItems<SintomaModel>().Where(s => musculoSintoma.Contains(s.IdSintoma)).ToList();
            SintomasCollectionView.ItemsSource = sintoma;



        }
    }
}