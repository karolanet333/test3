
using SofCoAr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofCoAr.Repositories
{
    public class CurrencySignRepo : BaseRepo<CurrencySign>, ICurrencySignRepo
    {
        public CurrencySignRepo(SofcoContext context = null) : base(context)
        {
        }
    }
}