using ShoeCatalog.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.Repositories.Interfaces
{
    public interface IShoeRepository: IGenericRepository<Shoe>
    {
        public Task AddShoeAsync(Shoe shoe);
    }
}
