
using SofCoAr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofCoAr.Repositories
{
    public class PaymentMethodRepo : BaseRepo<PaymentMethod>, IPaymentMethodRepo
    {
        public PaymentMethodRepo(SofcoContext context = null) : base(context)
        {
        }
    }
}
