using SQLite;

namespace KineApp.Model
{
    [SQLite.Table("Parte")]
    internal class ParteModel
    {
        [PrimaryKey,AutoIncrement]
        public int IdParte { get; set; }

        public string Nombre { get; set; }

        public byte[] ImgParte { get; set; }
    }
}
