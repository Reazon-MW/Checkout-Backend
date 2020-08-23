using CheckoutProj.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutProj.Models
{
    public class DefaultRepository<TDefEntity> : IRepository<TDefEntity> where TDefEntity : class
    {

        public DbSet<TDefEntity> List;
        public DatabaseContext DbContext { get; set; }

        public DefaultRepository(DatabaseContext ctx)
        {
            DbContext = ctx;
            if (DbContext == null)
                throw new ArgumentNullException(nameof(ctx));
        }


        public void Add(TDefEntity e)
        {
            List.Add(e);
            DbContext.SaveChanges();
        }

        public void Delete(TDefEntity e)
        {
            List.Remove(e);
            DbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            Delete(Find(id));
            DbContext.SaveChanges();
        }

        public TDefEntity Find(int id)
        {
            return List.Find(id);
        }

        public sealed override string ToString()
        {
            PropertyInfo[] _PropertyInfos = null;
            if (_PropertyInfos == null)
                _PropertyInfos = this.GetType().GetProperties();

            var sb = new StringBuilder();

            foreach (var info in _PropertyInfos)
            {
                var value = info.GetValue(this, null) ?? "(null)";
                sb.AppendLine(info.Name + ": " + value.ToString());
            }

            return sb.ToString();
        }
    }
}
