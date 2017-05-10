
using SofCoAr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace SofCoAr.Repositories
{
    public class SolFacHistRepo : BaseRepo<SolFacHist>, ISolFacHistRepo
    {
        public SolFacHistRepo(SofcoContext context = null) : base(context)
        {
        }

        public override IEnumerable<SolFacHist> GetAll()
        {
            //var appContext = (AppDbContext)context;

            var list = _context.Set<SolFacHist>()
                .Include(x => x.SolFacState)
                //.Where(s => s.IdBillingMilestone == idBillingMilestone)
                .OrderBy(s => s.Date).ToList();

            foreach(var hist in list)
            {
                hist.SolFacState.SolFacHists = null;
            }

            return list;

        }
    }
}
