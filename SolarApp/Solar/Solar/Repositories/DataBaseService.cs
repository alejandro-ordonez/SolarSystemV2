using Microsoft.EntityFrameworkCore;
using Solar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Solar.Repositories
{
    public class DataBaseService: IRepository
    {
        private readonly SolarDbContext repository;

        public DataBaseService(SolarDbContext repository)
        {
            this.repository = repository;
        }
        public async Task<bool> InsertReadingsToExisting(Panel p)
        {
            repository.Panels.Update(p);
            await repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InsertNewPanel(Panel p)
        {
            await repository.Panels.AddAsync(p);
            await repository.SaveChangesAsync();
            return true;
        }

        public async Task<Panel> GetPanelAsync(int id)
        {
            return await repository.Panels.FirstOrDefaultAsync<Panel>(p => p.Id == id);
        }

        public async Task<List<Panel>> GetPanels()
        {
            var panels = await repository.Panels.ToListAsync();
            foreach (var item in panels)
            {
                item.Location = new Xamarin.Forms.Maps.Position(item.Latitude, item.Longitude);
            }
            return panels;
        }
    }
}
