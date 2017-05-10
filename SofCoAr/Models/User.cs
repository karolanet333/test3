namespace SofCoAr.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class User: BaseEntity
    {
        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            SolFacActionHists = new HashSet<SolFacActionHist>();
            SolFacHists = new HashSet<SolFacHist>();
            UserProfiles = new HashSet<UserProfile>();
        }*/

        public override int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int? IdProfile { get; set; }

        public virtual Profile Profile { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SolFacActionHist> SolFacActionHists { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SolFacHist> SolFacHists { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}
