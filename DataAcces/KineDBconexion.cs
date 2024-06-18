using SQLite;
using System.Collections.Generic;
using System.IO;
using Microsoft.Maui.Storage;

namespace KineApp.DataAcces
{
    public class KineDBconexion
    {
        private static SQLiteConnection _database;

        public KineDBconexion()
        {
            if (_database == null)
            {
                InitializeDatabase();
            }
        }

        private void InitializeDatabase()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "Kine.db");

            if (!File.Exists(dbPath))
            {
                using (var stream = FileSystem.OpenAppPackageFileAsync("Kine.db").Result)
                {
                    using (var destStream = File.Create(dbPath))
                    {
                        stream.CopyTo(destStream);
                    }
                }
            }

            _database = new SQLiteConnection(dbPath);
        }

        public List<T> GetItems<T>() where T : new()
        {
            return _database.Table<T>().ToList();
        }
    }
}