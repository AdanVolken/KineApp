using SQLite;
using System.ComponentModel;

namespace KineApp.Model
{
    [SQLite.Table("Cliente")]
    class ClienteModel
    {
        [PrimaryKey, AutoIncrement]
        public int IdCliente   { get; set; }    

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni {  get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string ObraSocial { get; set; }
        public int Sesiones {  get; set; }
        public string Dolor { get; set; }
        public int IdMusculo { get; set; }
        public  int Estampillado { get; set; }

    }
}
