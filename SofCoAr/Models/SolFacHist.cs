namespace SofCoAr.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class SolFacHist: BaseEntity
    {
        public override int Id { get; set; }

        public int IdBillingMilestone { get; set; }

        public int IdSolFacState { get; set; }

        public DateTime Date { get; set; }

        public int IdUser { get; set; }

        public bool? Canceled { get; set; }

        public virtual BillingMilestone BillingMilestone { get; set; }

        public virtual SolFacState SolFacState { get; set; }

        public virtual User User { get; set; }
    }
}
