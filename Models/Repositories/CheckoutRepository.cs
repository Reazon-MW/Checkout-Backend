using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutProj.Models
{
    public class CheckoutRepository : DefaultRepository<Checkout>
    {
        public CheckoutRepository(DatabaseContext ctx) : base(ctx)
        {
            List = ctx.Checkouts;
        }

        public bool ReviewCheckout(int checkoutID)
        {
            Find(checkoutID).IsRevieved = true;
            DbContext.SaveChanges();
            return true;
        }
    }
}
