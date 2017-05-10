namespace SofCoAr.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class SolFacActionState: BaseEntity
    {
        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SolFacActionState()
        {
            SolFacActionHists = new HashSet<SolFacActionHist>();
        }*/

        public override int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SolFacActionHist> SolFacActionHists { get; set; }
    }
}
