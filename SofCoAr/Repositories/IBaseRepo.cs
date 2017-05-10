using SofCoAr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofCoAr.Repositories
{
    public interface IBaseRepo<T> where T: BaseEntity
    {
        void Add(T item);
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Delete(int id);
        void Edit(T item);
        void AddOrEdit(T item);
        void SaveChanges();
        void Dispose();
    }
}
