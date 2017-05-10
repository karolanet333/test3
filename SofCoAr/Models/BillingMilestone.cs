namespace SofCoAr.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class BillingMilestone : BaseEntity
    {
        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BillingMilestone()
        {
            BillingMilestoneDetails = new HashSet<BillingMilestoneDetail>();
            SolFacActionHists = new HashSet<SolFacActionHist>();
            SolFacActionHists1 = new HashSet<SolFacActionHist>();
            SolFacHists = new HashSet<SolFacHist>();
        }*/

        public override int Id { get; set; }

        public int IdProject { get; set; }

        public int IdCustomer { get; set; }

        public DateTime ScheduledDate { get; set; }

        public string Status { get; set; }

        public string CustomerDescription { get; set; }

        public string CustomerContact { get; set; }

        public string CustomerPhone { get; set; }

        public string ProjectName { get; set; }

        public int Plazo { get; set; }

        public string PaymentMethodDescription { get; set; }

        public decimal ImporteBruto { get; set; }

        public decimal Iva21 { get; set; }

        public decimal ImporteBrutoMasIva21 { get; set; }

        public decimal Total { get; set; }

        public string comments { get; set; }

        public decimal ImpCapital { get; set; }

        public decimal ImpProvBsAs { get; set; }

        public decimal ImpOtrasProv_1 { get; set; }

        public decimal ImpOtrasProv_2 { get; set; }

        public decimal ImpOtrasProv_3 { get; set; }

        public int? IdImpProv_1 { get; set; }

        public int? IdImpProv_2 { get; set; }

        public int? IdImpProv_3 { get; set; }

        public int IdPaymentMethod { get; set; }

        public string Name { get; set; }

        public int ContractNumber { get; set; }

        public int IdDocumentType { get; set; }

        public string ApplicantName { get; set; }

        public decimal Monto { get; set; }

        public int? IdService { get; set; }

        [StringLength(50)]
        public string CurrState { get; set; }

        public int? IdSolFacState { get; set; }

        public bool? Cobrado { get; set; }

        public decimal? MontoInic { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillingMilestoneDetail> BillingMilestoneDetails { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual CustomerService CustomerService { get; set; }

        public virtual DocumentType DocumentType { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual Project Project { get; set; }

        public virtual Province Province { get; set; }

        public virtual Province Province1 { get; set; }

        public virtual Province Province2 { get; set; }

        public virtual SolFacState SolFacState { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SolFacActionHist> SolFacActionHists { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SolFacActionHist> SolFacActionHists1 { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SolFacHist> SolFacHists { get; set; }
    }
}
