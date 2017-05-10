using SofCoAr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofCoAr.Repositories.Repositories.Helpers
{
    public static class SolFacActionsHelper
    {
        public static SolFacActionHist GetLastMilestoneAmount(SofcoContext context, int? idBillingMilestone)
        {
            SolFacActionHist lastHist = null;

            if (idBillingMilestone != null)
            {
                var query = context.SolFacActionHists
                .Where(h => h.IdMilestone == idBillingMilestone)
                .OrderByDescending(o => o.Fecha);

                lastHist = query
                    .FirstOrDefault();
            }

            return lastHist;
        }

    }
}
