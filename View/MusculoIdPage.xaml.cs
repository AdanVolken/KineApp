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
            try
            {
                var musculo = _databaseService.GetItems<MusculoModel>()
                                              .FirstOrDefault(m => m.IdMusculo == _musculoId);
                if (musculo != null)
                {
                    NombreLabel.Text = musculo.Nombre;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cargando datos: {ex.Message}");
            }
        }
    }
}