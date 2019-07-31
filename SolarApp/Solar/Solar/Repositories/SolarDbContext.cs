using Microsoft.EntityFrameworkCore;
using Solar.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
namespace Solar.Repositories
{
    public class SolarDbContext:DbContext, IRepository
    {
        public string DbPath { get; set; }

        #region Tables
        public DbSet<Panel> Panels { get; set; }
        public DbSet<DataPanel> Datas { get; set; }
        public DbSet<Reading> Readings { get; set; }
        #endregion

        public SolarDbContext():base()
        {
            DbPath = Path.Combine(FileSystem.AppDataDirectory, "Test2.sqlite");
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Panel>().HasKey(p => p.Id);
            modelBuilder.Entity<DataPanel>().HasKey(p => p.Id);
            modelBuilder.Entity<Reading>().HasKey(p => p.Id);
            //modelBuilder.Entity<Panel>().HasMany<DataPanel>();
            //modelBuilder.Entity<DataPanel>().HasMany<Reading>();
        }

        public async Task<bool> InsertReadingsToExisting(int id, DataPanel data)
        {
            var panel = await Panels.FirstOrDefaultAsync(p => p.Id == id);
            panel.Data.Add(data);
            Panels.Update(panel);
            await SaveChangesAsync();
            return true;
        }

        public async Task<bool> InsertNewPanel(Panel p)
        {
            await Panels.AddAsync(p);
            await SaveChangesAsync();
            return true;
        }

        public async Task<List<Panel>> GetPanels()
        {
            var panels =await Panels.ToListAsync();
            foreach(var item in panels)
            {
                item.Location = new Xamarin.Forms.Maps.Position(item.Latitude, item.Longitude);
            }
            return panels;
        }

    }
}
