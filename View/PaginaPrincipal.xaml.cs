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
    }
}