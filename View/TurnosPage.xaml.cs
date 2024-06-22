using Microsoft.Maui.Controls;
using KineApp.Model;
using KineApp.DataAcces;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace KineApp.Views
{
    public partial class TurnosPage : ContentPage
    {
        private KineDBconexion _databaseService;
        private SesionModel _turnoEditando;
        public ObservableCollection<SesionModel> Turnos { get; set; }
        public ObservableCollection<SesionModel> CompletedTurnos { get; set; }
        public ObservableCollection<ClienteModel> Clientes { get; set; }

        public TurnosPage()
        {
            InitializeComponent();
            _databaseService = new KineDBconexion();
            LoadData();
            BindingContext = this;
        }

        private void LoadData()
        {
            Clientes = new ObservableCollection<ClienteModel>(_databaseService.GetItems<ClienteModel>());
            var turnosList = _databaseService.GetItems<SesionModel>().OrderBy(t => t.Fecha).ToList();

            foreach (var turno in turnosList)
            {
                turno.Cliente = Clientes.FirstOrDefault(c => c.IdCliente == turno.IdCliente);
            }

            Turnos = new ObservableCollection<SesionModel>(turnosList.Where(t => !t.Completado));
            CompletedTurnos = new ObservableCollection<SesionModel>(turnosList.Where(t => t.Completado && t.Fecha.Month == DateTime.Now.Month && t.Fecha.Year == DateTime.Now.Year));

            TurnosCollectionView.ItemsSource = Turnos;
            CompletedSessionsCollectionView.ItemsSource = CompletedTurnos;
            UpdateCompletedSessionsCount();
        }

        private void GuardarTurno_Clicked(object sender, EventArgs e)
        {
            var cliente = ClientePicker.SelectedItem as ClienteModel;
            var fecha = FechaPicker.Date;
            var hora = HoraPicker.Time;

            if (cliente == null)
            {
                DisplayAlert("Error", "Por favor selecciona un cliente", "OK");
                return;
            }

            var fechaCompleta = fecha + hora;

            if (_turnoEditando != null)
            {
                _turnoEditando.IdCliente = cliente.IdCliente;
                _turnoEditando.Fecha = fechaCompleta;
                _databaseService.UpdateItem(_turnoEditando);
                _turnoEditando.Cliente = cliente;
                _turnoEditando = null;
            }
            else
            {
                var nuevoTurno = new SesionModel
                {
                    IdCliente = cliente.IdCliente,
                    Fecha = fechaCompleta,
                    Cliente = cliente,
                    Completado = false
                };
                _databaseService.InsertItem(nuevoTurno);
                Turnos.Add(nuevoTurno);
            }

            TurnosCollectionView.ItemsSource = Turnos.OrderBy(t => t.Fecha).ToList();
            ClientePicker.SelectedItem = null;
            FechaPicker.Date = DateTime.Now;
            HoraPicker.Time = TimeSpan.Zero;
        }

        private void CompletadoTurno_Clicked(object sender, EventArgs e)
        {
            var turno = (sender as Button).BindingContext as SesionModel;
            if (turno != null)
            {
                turno.Completado = true;
                _databaseService.UpdateItem(turno);

                Turnos.Remove(turno);
                CompletedTurnos.Add(turno);
                CompletedSessionsCollectionView.ItemsSource = CompletedTurnos.OrderBy(t => t.Fecha).ToList();
                TurnosCollectionView.ItemsSource = Turnos.OrderBy(t => t.Fecha).ToList();

                var cliente = Clientes.FirstOrDefault(c => c.IdCliente == turno.IdCliente);
                if (cliente != null)
                {
                    cliente.Sesiones--;
                    _databaseService.UpdateItem(cliente);

                    if (cliente.Sesiones <= 0)
                    {
                        DisplayAlert("Información", $"{cliente.NombreCompleto} tiene 0 sesiones restantes.", "OK");
                    }
                }

                UpdateCompletedSessionsCount();
            }
        }

        private void EditarTurno_Clicked(object sender, EventArgs e)
        {
            var turno = (sender as Button).BindingContext as SesionModel;
            if (turno != null)
            {
                _turnoEditando = turno;
                ClientePicker.SelectedItem = Clientes.FirstOrDefault(c => c.IdCliente == turno.IdCliente);
                FechaPicker.Date = turno.Fecha.Date;
                HoraPicker.Time = turno.Fecha.TimeOfDay;
            }
        }

        private async void Button_Clicked_Cliente(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is ClienteModel selectedCliente)
            {
                await Navigation.PushAsync(new ClienteIdInfoPage(selectedCliente.IdCliente));
            }
        }

        private void UpdateCompletedSessionsCount()
        {
            var currentMonthCompletedSessions = CompletedTurnos.Count;
            CompletedSessionsCountLabel.Text = $"Sesiones completadas este mes: {currentMonthCompletedSessions}";
        }
    }
}