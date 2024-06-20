using SQLite;

namespace KineApp.Model
{
    [SQLite.Table("MusculoSintoma")]
    class MusculoSintomaModel
    {
        [PrimaryKey,AutoIncrement]
        public int IdMusculoSintoma { get; set; }

        public int IdMusculo {  get; set; }
        public int IdSintoma { get; set; } 
        public string Sintomas { get; set; }
    }
}
