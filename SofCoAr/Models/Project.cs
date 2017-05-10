namespace SofCoAr.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Project: BaseEntity
    {
        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Project()
        {
            BillingMilestones = new HashSet<BillingMilestone>();
        }*/

        public override int Id { get; set; }

        public string Name { get; set; }

        public int IdCustomer { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ProjectManager { get; set; }

        public int IdTypeService { get; set; }

        public int IdTypeSolution { get; set; }

        public int IdTypeTecnology { get; set; }

        public int? EstimatedEarnings { get; set; }

        public string Analytics { get; set; }

        public string PurchaseOrder { get; set; }

        public string Link { get; set; }

        public int? IdService { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillingMilestone> BillingMilestones { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual CustomerService CustomerService { get; set; }
    }
}
