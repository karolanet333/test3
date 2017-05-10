namespace SofCoAr.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class SolFacActionHist: BaseEntity
    {
        public override int Id { get; set; }

        public int IdMilestone { get; set; }

        public int IdSolFacActionState { get; set; }

        public int IdUser { get; set; }

        public decimal? MontoAnte { get; set; }

        public decimal MontoNuevo { get; set; }

        public int? IdMilestoneSource { get; set; }

        public DateTime Fecha { get; set; }

        public virtual BillingMilestone BillingMilestone { get; set; }

        public virtual BillingMilestone BillingMilestoneSource { get; set; }

        public virtual SolFacActionState SolFacActionState { get; set; }

        public virtual User User { get; set; }
    }
}
