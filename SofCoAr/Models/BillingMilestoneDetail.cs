namespace SofCoAr.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class BillingMilestoneDetail: BaseEntity
    {
        public override int Id { get; set; }

        public int IdBillingMilestone { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public string Detail { get; set; }

        public int IdCurrencySign { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal SubTotal { get; set; }

        public string CurrencySignName { get; set; }

        public virtual BillingMilestone BillingMilestone { get; set; }

        public virtual CurrencySign CurrencySign { get; set; }
    }
}
