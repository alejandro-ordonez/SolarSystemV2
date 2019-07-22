using Solar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Solar.Repositories
{
    public interface IRepository
    {
        Task<bool> InsertReadingsToExisting(int id, DataPanel data);
        Task InsertNewPanel(Panel p);
        Task<List<Panel>> GetPanels();
    }
}
