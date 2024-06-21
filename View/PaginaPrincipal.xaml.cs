using KineApp.Views;
using Microsoft.Maui.Controls;
using System;

namespace KineApp.Views
{
    public partial class PaginaPrincipal : ContentPage
    {
        public PaginaPrincipal()
        {
            InitializeComponent();
        }

        private async void btnMusculo_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MusculoPage());
        }

        private async void btnSintomas_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SintomaPage());
        }

        private async void ParteButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            int parteId = int.Parse(button.CommandParameter.ToString());
            await Navigation.PushAsync(new ParteDetallePage(parteId));
        }

        private async void btnCliente_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ClientePage());
        }
    }
}