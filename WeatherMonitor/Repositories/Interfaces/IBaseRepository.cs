using System;
using System.Collections.Generic;
using WeatherMonitor.Models.Base;

namespace WeatherMonitor.Repositories.Interfaces
{
    public interface IBaseRepository<TDbModel> where TDbModel : BaseModel
    {
        List<TDbModel> GetAll();
        TDbModel Get(Guid id);
        TDbModel Create(TDbModel model);
        TDbModel Update(TDbModel model);
        void Delete(Guid id);
    }
}