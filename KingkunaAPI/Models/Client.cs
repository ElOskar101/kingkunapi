namespace KingkunaAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Client")]
    public partial class Client
    {
        public int ClientID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreateAt { get; set; }

        [Column(TypeName = "date")]
        public DateTime HireDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime CancelDate { get; set; }

        public int DemoDays { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        public int Status { get; set; }

        public double Amount { get; set; }
    }
}
