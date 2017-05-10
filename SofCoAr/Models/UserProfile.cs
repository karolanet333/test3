namespace SofCoAr.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class UserProfile: BaseEntity
    {
        public override int Id { get; set; }

        public int Profile_Id { get; set; }

        public int User_Id { get; set; }

        public virtual Profile Profile { get; set; }

        public virtual User User { get; set; }

    }
}
