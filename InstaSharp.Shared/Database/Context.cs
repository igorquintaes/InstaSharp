using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using static System.Environment;

namespace InstaSharp.Shared.Database
{
    public class Context
    {
        private readonly string databaseName;
        private readonly string databaseDir;
        private readonly string databaseCompleteDir;

        public Context()
            : this(nameof(InstaSharp),
                   GetFolderPath(SpecialFolder.MyDocuments))
        { }

        public Context(string databaseName, string databaseDir)
        {
            this.databaseName = databaseName + ".sqlite";
            this.databaseDir = databaseDir;
            databaseCompleteDir = Path.Combine(this.databaseDir, this.databaseName);

            if (!File.Exists(databaseCompleteDir))
            {
                var createDbQuery = @"
                    CREATE TABLE `UsersInst` (
                        `id` INTEGER PRIMARY KEY,
	                    `username`	TEXT NOT NULL
                    );".Replace("\r\n", "");

                SQLiteConnection.CreateFile(databaseCompleteDir);
                using (var m_dbConnection = new SQLiteConnection($"Data Source={databaseCompleteDir};Version=3;"))
                using (var createPokemonsTable = new SQLiteCommand(createDbQuery, m_dbConnection))
                {

                    m_dbConnection.Open();
                    createPokemonsTable.ExecuteNonQuery();
                }
            }
        }

        public void AddUsername(string username)
        {
            using (var m_dbConnection = new SQLiteConnection($"Data Source={databaseCompleteDir};Version=3;", true))
            {
                m_dbConnection.Open();
                var query = $"insert into UsersInst (username) values ('{username}')";

                using (var insertCommand = new SQLiteCommand(query, m_dbConnection))
                    insertCommand.ExecuteNonQuery();
            }
        }

        public List<string> GetAllDbUsers()
        {
            var list = new List<string>();
            using (var m_dbConnection = new SQLiteConnection($"Data Source={databaseCompleteDir};Version=3;", true))
            {
                m_dbConnection.Open();

                var query = $"select username from UsersInst";
                using (var command = new SQLiteCommand(query, m_dbConnection))
                using (var sqReader = command.ExecuteReader())
                {
                    while (sqReader.Read())
                    {
                        list.Add(sqReader.GetString(0));
                    }
                }
            }

            return list;
        }
    }
}
