using SQLite;

namespace KineApp.Model
{
    [SQLite.Table("Sesion")]
    public class SesionModel
    {
        [PrimaryKey,AutoIncrement]
        public int IdSesion { get; set; }

        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }

        [Ignore]
        public ClienteModel Cliente { get; set; }
    }
}
