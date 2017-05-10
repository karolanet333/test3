using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofCoAr.Models.DTO
{
    public class BillingMilestoneAproveRejectDTO
    {
        public BillingMilestone BillingMilestone { get; set; }
        //public string str { get; set; }
        public int IdUser { get; set; }
        public Boolean Rechazar { get; set; }
    }
}
