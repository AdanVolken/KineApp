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
            try
            {
                var sintoma = _databaseService.GetItems<SintomaModel>()
                                              .FirstOrDefault(s => s.IdSintoma == _sintomaId);
                if (sintoma != null)
                {
                    NombreLabel.Text = sintoma.Nombre;
                    DescripcionLabel.Text = sintoma.Descri;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cargando datos: {ex.Message}");
            }
        }
    }
}