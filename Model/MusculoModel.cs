using SQLite;

using System.ComponentModel.DataAnnotations.Schema;

namespace KineApp.Model
{
    [SQLite.Table("Musculo")]
    public class MusculoModel
    {
        [PrimaryKey, AutoIncrement]
        public int IdMusculo { get; set; }

        public string Nombre { get; set; }

        public byte[] Punto { get; set; }

        public int IdParte { get; set; }
    }
}
