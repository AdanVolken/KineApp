using Microsoft.Maui.Controls;
using KineApp.Model;
using KineApp.DataAcces;
using System;

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
            // Se sacra las partes a las que pertenece el Sintoma
            var sintomaParte = _databaseService.GetItems<SintomaParte>().Where(sp => sp.IdSintoma == _sintomaId).Select(sp => sp.IdParte).ToList();
            var parte = _databaseService.GetItems<ParteModel>().Where(p => sintomaParte.Contains(p.IdParte)).ToList();

            // Sacamos los Muscuos que pueden tener ese sintoma
            var sintomaMusculo = _databaseService.GetItems<MusculoSintomaModel>().Where(sm => sm.IdSintoma == _sintomaId).Select(sm => sm.IdMusculo).ToList();  
            var musculo =_databaseService.GetItems<MusculoModel>().Where(m => sintomaMusculo.Contains(m.IdMusculo)).ToList();

        }
    }
}