using Solar.Models;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Solar.Repositories
{
    public class SQLiteRepository:IRepository
    {
        private string PathDB { get; set; }
        readonly SQLiteAsyncConnection database;

        public SQLiteRepository()
        {
            PathDB = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Solar.db3");
            database = new SQLiteAsyncConnection(PathDB);
            database.CreateTableAsync<Panel>().Wait();
            database.CreateTableAsync<Reading>().Wait();
            database.CreateTableAsync<DataPanel>().Wait();
            
        }

        public async Task<bool> InsertReadingsToExisting(int id, DataPanel data)
        {
            
            var x = await database.FindAsync<Panel>(p => p.Id == id);
            if (x != null)
            {
                await database.InsertAllAsync(data.IV);
                await database.InsertAsync(data);
                x.Data.Add(data);
                await database.UpdateAsync(x);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task InsertNewPanel(Panel p)
        {
            await database.InsertAsync(p);
        }

        public Task<List<Panel>> GetPanels()
        {
            return database.Table<Panel>().ToListAsync();
        }
    }
}
