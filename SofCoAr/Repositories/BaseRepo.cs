
using SofCoAr.Helper;
using SofCoAr.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofCoAr.Repositories
{
    public class BaseRepo<T>: IDisposable, IBaseRepo<T> where T:BaseEntity
    {
        public readonly SofcoContext _context;

        public BaseRepo(SofcoContext context = null)
        {
            if (context == null)
            {
                _context = new SofcoContext();
            }
            else
            {
                _context = context;
            }
        }

        public virtual void Add(T item)
        {
            _context.Set<T>().Add(item);
            _context.Entry<T>(item).State = EntityState.Added;
        }

        public virtual T GetById(int id)
        {
            var rpta = _context.Set<T>().FirstOrDefault(i => i.Id == id);
            return rpta;
        }

        public virtual IEnumerable<T> GetAll()
        {
            bool hasName = ReflectionHelper.HasProperty<T>("Name");
            IEnumerable<T> rpta;
            if (hasName)
            {
                var propName = typeof(T).GetProperty("Name");
                rpta = _context.Set<T>().ToList().OrderBy(o => propName.GetValue(o, null));
            }
            else
            {
                rpta = _context.Set<T>().ToList();
            }
            return rpta;
        }

        public virtual void Delete(int id)
        {
            // ToDo - Integrate with EF Core
            var itemToRemove = GetById(id);
            if (itemToRemove != null)
            {
                _context.Set<T>().Remove(itemToRemove);
            }

        }

        public virtual void Edit(T item)
        {
            // ToDo - Integrate with EF Core
            var itemToUpdate = GetById(item.Id);
            if (itemToUpdate != null)
            {
                ReflectionHelper.SetPropertyValues<T>(item, ref itemToUpdate);
            }
            _context.Entry<T>(itemToUpdate).State = EntityState.Modified;
        }

        public virtual void AddOrEdit(T item)
        {
            // ToDo - Integrate with EF Core
            var itemToUpdate = GetById(item.Id);
            if (itemToUpdate != null)
            {
                ReflectionHelper.SetPropertyValues<T>(item, ref itemToUpdate);
                _context.Entry<T>(itemToUpdate).State = EntityState.Modified;
            }
            else
            {
                Add(item);
            }
        }

        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }

        protected void ClearAddedEntries()
        {
            var objectContext = ((IObjectContextAdapter)_context).ObjectContext;
            var objectStateEntries = objectContext
                             .ObjectStateManager
                             .GetObjectStateEntries(EntityState.Added);

            foreach (var objectStateEntry in objectStateEntries)
            {
                if (objectStateEntry.Entity != null)
                    objectContext.Detach(objectStateEntry.Entity);
            }
        }

        protected void ClearModifiedEntries()
        {
            var objectContext = ((IObjectContextAdapter)_context).ObjectContext;
            var objectStateEntries = objectContext
                             .ObjectStateManager
                             .GetObjectStateEntries(EntityState.Modified);

            foreach (var objectStateEntry in objectStateEntries)
            {
                if (objectStateEntry.Entity != null)
                    objectContext.Detach(objectStateEntry.Entity);
            }
        }

        public virtual void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}
