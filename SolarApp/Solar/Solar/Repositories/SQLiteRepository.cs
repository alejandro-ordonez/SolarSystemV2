using Solar.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
using Xamarin.Essentials;

namespace Solar.Repositories
{
    public class SQLiteRepository:IRepository
    {
        //TODO Change SQLiteExtensions https://bitbucket.org/twincoders/sqlite-net-extensions/raw/8febda909abc74b62330b92e5480830925116ed7/readme.md
        //Try to use it before like a normal class, or try to register as a dependency service as it is usually done.
        private string PathDB { get; set; }
        readonly SQLiteConnection database;

        public SQLiteRepository()
        {
            PathDB = Path.Combine(FileSystem.AppDataDirectory, "Test.db3");
            //PathDB = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Solar.db3");
            //database = new SQLiteAsyncConnection(PathDB);
            database = new SQLiteConnection(PathDB);
            database.CreateTable<Panel>();
            database.CreateTable<DataPanel>();
            database.CreateTable<Reading>();
            /*database.CreateTableAsync<Panel>().Wait();
            database.CreateTableAsync<DataPanel>().Wait();
            database.CreateTableAsync<Reading>().Wait();*/
        }

        public bool InsertReadingsToExisting(int id, DataPanel data)
        {
            var x = database.Find<Panel>(p => p.Id == id);
            if (x != null)
            {
                database.InsertWithChildren(x);
                return true;
            }
            else
            {
                return false;
            }
            /*Async
             * var x = await database.FindAsync<Panel>(p => p.Id == id);
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
            }*/
        }

        public async Task InsertNewPanel(Panel p)
        {
            database.Insert(p);
            //await database.InsertAsync(p);
        }

        public List<Panel> GetPanels()
        {
            return database.GetAllWithChildren<Panel>();
            //return await database.Table<Panel>().ToListAsync();
        }
    }
}
