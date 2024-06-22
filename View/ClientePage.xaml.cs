using Microsoft.Maui.Controls;
using KineApp.Model;
using KineApp.DataAcces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace KineApp.Views
{
    public partial class ClientePage : ContentPage
    {
        private KineDBconexion _databaseService;
        private ObservableCollection<ClienteModel> _clientes;
        private ObservableCollection<ClienteModel> _clientesFiltrados;
        private ClienteModel _clienteEditando;

        public ClientePage()
        {
            InitializeComponent();
            _databaseService = new KineDBconexion();
            LoadMusculos();
            LoadClientes();
        }

        private void LoadMusculos()
        {
            try
            {
                var musculos = _databaseService.GetItems<MusculoModel>().ToList();
                MusculoPicker.ItemsSource = musculos;
                MusculoPicker.ItemDisplayBinding = new Binding("Nombre");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cargando músculos: {ex.Message}");
            }
        }

        private void LoadClientes()
        {
            try
            {
                var clientes = _databaseService.GetItems<ClienteModel>().ToList();
                _clientes = new ObservableCollection<ClienteModel>(clientes);
                _clientesFiltrados = new ObservableCollection<ClienteModel>(clientes);
                ClientesCollectionView.ItemsSource = _clientesFiltrados;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cargando clientes: {ex.Message}");
            }
        }

        private async void OnAgregarClienteClicked(object sender, EventArgs e)
        {
            try
            {
                if (_clienteEditando == null)
                {
                    // Check if there is an existing client with the same DNI
                    var existingClient = _clientes.FirstOrDefault(c => c.Dni == DniEntry.Text);
                    if (existingClient != null)
                    {
                        await DisplayAlert("Error", "Ya existe un cliente con el mismo DNI.", "OK");
                        return;
                    }

                    var nuevoCliente = new ClienteModel
                    {
                        Nombre = NombreEntry.Text,
                        Apellido = ApellidoEntry.Text,
                        Dni = DniEntry.Text,
                        Telefono = TelefonoEntry.Text,
                        Correo = CorreoEntry.Text,
                        ObraSocial = ObraSocialEntry.Text,
                        Sesiones = int.Parse(SesionesEntry.Text),
                        Dolor = DolorEditor.Text,
                        IdMusculo = (MusculoPicker.SelectedItem as MusculoModel)?.IdMusculo ?? 0,
                        Estampillado = EstampilladoPicker.SelectedItem.ToString() == "Sí"
                    };

                    _databaseService.InsertItem(nuevoCliente);
                    _clientes.Add(nuevoCliente);
                    _clientesFiltrados.Add(nuevoCliente);
                    await DisplayAlert("Éxito", "Cliente agregado correctamente", "OK");
                }
                else
                {
                    // Check if there is an existing client with the same DNI but different ID
                    var existingClient = _clientes.FirstOrDefault(c => c.Dni == DniEntry.Text && c.IdCliente != _clienteEditando.IdCliente);
                    if (existingClient != null)
                    {
                        await DisplayAlert("Error", "Ya existe otro cliente con el mismo DNI.", "OK");
                        return;
                    }

                    _clienteEditando.Nombre = NombreEntry.Text;
                    _clienteEditando.Apellido = ApellidoEntry.Text;
                    _clienteEditando.Dni = DniEntry.Text;
                    _clienteEditando.Telefono = TelefonoEntry.Text;
                    _clienteEditando.Correo = CorreoEntry.Text;
                    _clienteEditando.ObraSocial = ObraSocialEntry.Text;
                    _clienteEditando.Sesiones = int.Parse(SesionesEntry.Text);
                    _clienteEditando.Dolor = DolorEditor.Text;
                    _clienteEditando.IdMusculo = (MusculoPicker.SelectedItem as MusculoModel)?.IdMusculo ?? 0;
                    _clienteEditando.Estampillado = EstampilladoPicker.SelectedItem.ToString() == "Sí";

                    _databaseService.UpdateItem(_clienteEditando);

                    // Actualizar las colecciones
                    var clienteIndex = _clientes.IndexOf(_clienteEditando);
                    _clientes[clienteIndex] = _clienteEditando;
                    _clientesFiltrados[clienteIndex] = _clienteEditando;

                    await DisplayAlert("Éxito", "Cliente actualizado correctamente", "OK");
                    _clienteEditando = null;
                }

                ClientesCollectionView.ItemsSource = _clientesFiltrados.OrderBy(c => c.Nombre).ThenBy(c => c.Apellido);
                ClearForm();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Hubo un error al agregar/actualizar el cliente: {ex.Message}", "OK");
            }
        }

        private void OnEditarClienteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var cliente = button?.CommandParameter as ClienteModel;

            if (cliente != null)
            {
                _clienteEditando = cliente;
                NombreEntry.Text = cliente.Nombre;
                ApellidoEntry.Text = cliente.Apellido;
                DniEntry.Text = cliente.Dni;
                TelefonoEntry.Text = cliente.Telefono;
                CorreoEntry.Text = cliente.Correo;
                ObraSocialEntry.Text = cliente.ObraSocial;
                SesionesEntry.Text = cliente.Sesiones.ToString();
                DolorEditor.Text = cliente.Dolor;
                MusculoPicker.SelectedItem = _databaseService.GetItems<MusculoModel>().FirstOrDefault(m => m.IdMusculo == cliente.IdMusculo);
                EstampilladoPicker.SelectedItem = cliente.Estampillado ? "Sí" : "No";
            }
        }

        private async void OnInfoClienteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var cliente = button?.CommandParameter as ClienteModel;

            if (cliente != null)
            {
                await Navigation.PushAsync(new ClienteIdInfoPage(cliente.IdCliente));
            }
        }

        private void ClearForm()
        {
            NombreEntry.Text = string.Empty;
            ApellidoEntry.Text = string.Empty;
            DniEntry.Text = string.Empty;
            TelefonoEntry.Text = string.Empty;
            CorreoEntry.Text = string.Empty;
            ObraSocialEntry.Text = string.Empty;
            SesionesEntry.Text = string.Empty;
            DolorEditor.Text = string.Empty;
            MusculoPicker.SelectedItem = null;
            EstampilladoPicker.SelectedItem = null;
        }

        private void OnClienteSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = e.NewTextValue.ToLower();
            _clientesFiltrados.Clear();
            foreach (var cliente in _clientes)
            {
                if (cliente.Nombre.ToLower().Contains(searchText) ||
                    cliente.Apellido.ToLower().Contains(searchText) ||
                    cliente.Dni.ToLower().Contains(searchText))
                {
                    _clientesFiltrados.Add(cliente);
                }
            }
        }
    }
}