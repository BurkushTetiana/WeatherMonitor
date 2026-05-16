using System;
using System.Collections.Generic;
using System.Linq;
using WeatherMonitor.Database;
using WeatherMonitor.Models.Base;
using WeatherMonitor.Repositories.Interfaces;

namespace WeatherMonitor.Repositories.Implementations
{
    public class BaseRepository<TDbModel> : IBaseRepository<TDbModel>
        where TDbModel : BaseModel
    {
        private ApplicationContext Context { get; set; }

        public BaseRepository(ApplicationContext context)
        {
            Context = context;
        }

        public List<TDbModel> GetAll() =>
            Context.Set<TDbModel>().ToList();

        public TDbModel Get(Guid id) =>
            Context.Set<TDbModel>().FirstOrDefault(m => m.Id == id);

        public TDbModel Create(TDbModel model)
        {
            model.Id = Guid.NewGuid();
            Context.Set<TDbModel>().Add(model);
            Context.SaveChanges();
            return model;
        }

        public TDbModel Update(TDbModel model)
        {
            var toUpdate = Context.Set<TDbModel>().FirstOrDefault(m => m.Id == model.Id);
            if (toUpdate != null)
            {
                Context.Entry(toUpdate).CurrentValues.SetValues(model);
                Context.SaveChanges();
            }
            return toUpdate;
        }

        public void Delete(Guid id)
        {
            var toDelete = Context.Set<TDbModel>().FirstOrDefault(m => m.Id == id);
            if (toDelete != null)
            {
                Context.Set<TDbModel>().Remove(toDelete);
                Context.SaveChanges();
            }
        }
    }
}