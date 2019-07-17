using Solar.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Solar.Repositories
{
    public class SQLiteRepository
    {
        public string PathDB { get; set; }
        readonly SQLiteAsyncConnection database;

        public SQLiteRepository()
        {
            PathDB = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TodoSQLite.db3");
            database = new SQLiteAsyncConnection(PathDB);
            database.CreateTableAsync<Panel>().Wait();
            database.CreateTableAsync<Reading>().Wait();
        }

    }
}
