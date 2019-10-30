using Microsoft.EntityFrameworkCore;
using Solar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<bool> InsertReadingsToExisting(int id, DataPanel Measurement)
        {
            var panel = await repository.Panels.Include(P => P.Data).ThenInclude(data=> data.IV).SingleAsync(P=> P.Id==id);
            panel.Data.Add(Measurement);
            repository.Panels.Update(panel);
            await repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InsertNewPanel(Panel p)
        {
            var _user = App.UserLogged;
            var user = await repository.Users.Include(u => u.Panels).SingleAsync(u=> u.Id==_user.Id);
            user.Panels.Add(p);
            repository.Users.Update(user);
            await repository.SaveChangesAsync();
            return true;
        }

        public async Task<Panel> GetPanelAsync(int id)
        {
            //return await repository.Panels.SingleAsync(panel => panel.Id == id);
            var p = await repository.Panels
                .Include(panel=>panel.Data)
                    .ThenInclude(data => data.IV)
                .SingleAsync(panel => panel.Id == id);
            return p;

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
