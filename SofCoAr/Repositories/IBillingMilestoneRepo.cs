using SofCoAr.Models;
using SofCoAr.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofCoAr.Repositories
{
    public interface IBillingMilestoneRepo : IBaseRepo<BillingMilestone>
    {
        IEnumerable<BillingMilestone> GetAllFiltered(int idCustomer, int idService, int idProject);
        IEnumerable<BillingMilestone> GetAllFilteredWithDetails(int idCustomer, int idService, int idProject);
        void EditHito(BillingMilestone o, int idUser, bool dividido = false);
        void AddHito(BillingMilestoneEditDTO o);
        void DivideHito(BillingMilestoneDivideDTO o);
        void AproveRejectHito(BillingMilestone o, int idUser, Boolean rechazar);
    }
}
