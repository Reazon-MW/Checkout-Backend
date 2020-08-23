using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutProj.Models
{
    public class DiseaseRepository : DefaultRepository<Disease>
    {
        public DiseaseRepository(DatabaseContext ctx) : base(ctx)
        {
            List = ctx.Diseases;
        }

        public Disease CreateDisease(string name, string description)
        {
            Disease disease = new Disease
            {
                Name = name,
                Description = description
            };
            Add(disease);
            DbContext.SaveChanges();
            return disease;
        }
    }
}
