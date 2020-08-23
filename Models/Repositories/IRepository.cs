using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutProj.Models
{
    public interface IRepository<TEntity> where TEntity : class
    {

        public void Add(TEntity e);
        public void Delete(TEntity e);
        public void Delete(int id);
        public TEntity Find(int id);
        public string ToString();
    }
}
