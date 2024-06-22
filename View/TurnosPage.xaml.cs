using Microsoft.Maui.Controls;
using KineApp.Model;
using KineApp.DataAcces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Storage;

namespace KineApp.Views
{
    public partial class TurnosPage : ContentPage
    {
        private KineDBconexion _databaseService;
        private SesionModel _turnoEditando;
        public ObservableCollection<SesionModel> Turnos { get; set; }
        public ObservableCollection<ClienteModel> Clientes { get; set; }
        private int CompletedSessions { get; set; }
        public ObservableCollection<SesionModel> CompletedSessionsList { get; set; }

        public TurnosPage()
        {
            InitializeComponent();
            _databaseService = new KineDBconexion();
            LoadData();
            LoadCompletedSessions();
            LoadCompletedSessionsList();
            BindingContext = this;
        }

        private void LoadData()
        {
            Clientes = new ObservableCollection<ClienteModel>(_databaseService.GetItems<ClienteModel>());
            var turnosList = _databaseService.GetItems<SesionModel>()
                .Where(t => !t.Completado) // Excluir sesiones completadas
                .OrderBy(t => t.Fecha)
                .ToList();

            // Asignar los nombres de los clientes a los turnos
            foreach (var turno in turnosList)
            {
                turno.Cliente = Clientes.FirstOrDefault(c => c.IdCliente == turno.IdCliente);
            }

            Turnos = new ObservableCollection<SesionModel>(turnosList);
        }

        private void LoadCompletedSessions()
        {
            var currentMonthKey = DateTime.Now.ToString("yyyy-MM");
            CompletedSessions = Preferences.Get(currentMonthKey, 0);
        }

        private void LoadCompletedSessionsList()
        {
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            CompletedSessionsList = new ObservableCollection<SesionModel>(
                _databaseService.GetItems<SesionModel>()
                    .Where(t => t.Completado && t.Fecha >= firstDayOfMonth && t.Fecha <= lastDayOfMonth)
                    .OrderBy(t => t.Fecha)
                    .ToList()
            );
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

                // Remover el turno de la lista visible
                Turnos.Remove(turno);

                // Incrementar contador de sesiones completadas del mes
                IncrementarSesionesCompletadas(turno.Fecha);

                // Agregar el turno a la lista de completados
                CompletedSessionsList.Add(turno);
                CompletedSessionsCollectionView.ItemsSource = CompletedSessionsList.OrderBy(t => t.Fecha).ToList();
            }
        }

        private void IncrementarSesionesCompletadas(DateTime fecha)
        {
            var currentMonthKey = DateTime.Now.ToString("yyyy-MM");
            var completedSessions = Preferences.Get(currentMonthKey, 0);
            Preferences.Set(currentMonthKey, completedSessions + 1);
            CompletedSessions = completedSessions + 1;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadData(); // Asegúrate de recargar los datos para actualizar la lista de turnos
            LoadCompletedSessions();
            LoadCompletedSessionsList();
            BindingContext = this;
        }
    }
}