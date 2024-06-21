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

            // Asignar los nombres de los clientes a los turnos
            foreach (var turno in turnosList)
            {
                turno.Cliente = Clientes.FirstOrDefault(c => c.IdCliente == turno.IdCliente);
            }

            Turnos = new ObservableCollection<SesionModel>(turnosList);
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
                    Cliente = cliente
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
                Turnos.Remove(turno);
                _databaseService.DeleteItem(turno);

                var cliente = Clientes.FirstOrDefault(c => c.IdCliente == turno.IdCliente);
                if (cliente != null)
                {
                    cliente.Sesiones--;
                    _databaseService.UpdateItem(cliente);
                }
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
    }
}