using SQLite;

namespace KineApp.Model
{
    [SQLite.Table("SintomaParte")]
    internal class SintomaParte
    {
        [PrimaryKey, AutoIncrement]
        public int IdSintomaParte {  get; set; }

        public int IdSintoma {  get; set; }
        public int IdParte { get; set; }
        public string Nivel { get; set; }

    }
}
