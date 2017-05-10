namespace SofCoAr.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class SolFacState: BaseEntity
    {
        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SolFacState()
        {
            BillingMilestones = new HashSet<BillingMilestone>();
            SolFacHists = new HashSet<SolFacHist>();
        }*/

        public override int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(50)]
        public string ProfileAllowed { get; set; }

        [StringLength(50)]
        public string NextState { get; set; }

        [StringLength(50)]
        public string PrevState { get; set; }

        [StringLength(50)]
        public string ShortName { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillingMilestone> BillingMilestones { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SolFacHist> SolFacHists { get; set; }
    }
}
