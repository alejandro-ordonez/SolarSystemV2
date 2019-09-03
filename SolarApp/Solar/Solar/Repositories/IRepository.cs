using Solar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Solar.Repositories
{
    public interface IRepository
    {
        //bool InsertReadingsToExisting(int id, DataPanel data);
       // Task InsertNewPanel(Panel p);
        //List<Panel> GetPanels();
        Task<bool> InsertReadingsToExisting(Panel p);
        Task<bool> InsertNewPanel(Panel p);
        Task<List<Panel>> GetPanels();
    }
}
