using Microsoft.Maui.Controls;
using KineApp.Model;
using KineApp.DataAcces;
using System;
using System.Linq;

namespace KineApp.Views
{
    public partial class ClienteIdInfoPage : ContentPage
    {
        private KineDBconexion _databaseService;
        private int _clienteId;

        public ClienteIdInfoPage(int clienteId)
        {
            InitializeComponent();
            _databaseService = new KineDBconexion();
            _clienteId = clienteId;
            LoadClienteInfo();
        }

        private void LoadClienteInfo()
        {
            try
            {
                var cliente = _databaseService.GetItems<ClienteModel>().FirstOrDefault(c => c.IdCliente == _clienteId);
                var musculo = _databaseService.GetItems<MusculoModel>().FirstOrDefault(m => m.IdMusculo == cliente.IdMusculo);

                if (cliente != null)
                {
                    Title = $"{cliente.Nombre} {cliente.Apellido}";
                    NombreLabel.Text = $"Nombre: {cliente.Nombre}";
                    ApellidoLabel.Text = $"Apellido: {cliente.Apellido}";
                    DniLabel.Text = $"DNI: {cliente.Dni}";
                    TelefonoLabel.Text = $"Teléfono: {cliente.Telefono}";
                    CorreoLabel.Text = $"Correo: {cliente.Correo}";
                    ObraSocialLabel.Text = $"Obra Social: {cliente.ObraSocial}";
                    SesionesLabel.Text = $"Sesiones: {cliente.Sesiones}";
                    DolorLabel.Text = $"Dolor: {cliente.Dolor}";
                    EstampilladoLabel.Text = $"Estampillado: {(cliente.Estampillado ? "Sí" : "No")}";
                    MusculoLabel.Text = $"Músculo: {musculo?.Nombre ?? "No especificado"}";
                }
                else
                {
                    DisplayAlert("Error", "Cliente no encontrado", "OK");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Hubo un error al cargar la información del cliente: {ex.Message}", "OK");
            }
        }
    }
}