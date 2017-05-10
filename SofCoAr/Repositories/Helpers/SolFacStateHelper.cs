using SofCoAr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace SofCoAr.Repositories.Repositories.Helpers
{
    public static class SolFacStateHelper
    {
        //si el resultado es null -> no se permite la accion
        public static SolFacState GetNextState(SofcoContext context, SolFacState currState)
        {
            SolFacState nextState;
            if (currState == null)
            {
                nextState = context
                                .SolFacStates
                                .Where(s => s.Code == "PEND")
                                .FirstOrDefault();
            }
            else
            {
                nextState = context.SolFacStates.Where(sf =>
                   sf.Code == currState.NextState
                ).FirstOrDefault();
            }

            return nextState;
        }

        //si la tabla de historicos no tiene ningula linea, el estado es pendiente
        public static SolFacState GetCurrentState(SofcoContext context, int idBillingMilestone)
        {
            SolFacHist solFacHist = context.SolFacHists
                                     .Include(x => x.SolFacState).AsNoTracking()
                                     .Where(h => h.IdBillingMilestone == idBillingMilestone)
                                     .OrderByDescending(o => o.Date)
                                     .Take(1).SingleOrDefault();

            SolFacState currState;

            if (solFacHist == null)
            {
                currState = null;
            }
            else
            {
                currState = solFacHist.SolFacState;
            }

            return currState;
        }

        //si el resultado es null -> no se permite la accion
        public static SolFacState GetPrevState(SofcoContext context, SolFacState currState)
        {
            SolFacState prevState;
            if (currState == null)
            {
                prevState = null;
            }
            else
            {
                prevState = context.SolFacStates.Where(sf =>
                   sf.Code == currState.PrevState
                ).FirstOrDefault();
            }

            return prevState;
        }
    }
}
