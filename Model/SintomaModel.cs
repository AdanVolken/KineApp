using SQLite;

using System.ComponentModel.DataAnnotations.Schema;

namespace KineApp.Model
{
    [SQLite.Table("Sintoma")]
    internal class SintomaModel
    {
        [PrimaryKey, AutoIncrement]
        public int IdSintoma { get; set; } 

        public string Nombre { get; set; }
        public string Descri {  get; set; }
    }
}
