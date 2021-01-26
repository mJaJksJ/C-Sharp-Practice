namespace AutoLotDAL_EF.Models
{
    using AutoLotDAL_EF.Models.Base;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order: EntityBase
    {

        [Column("OrderId")]
        public override int Id { get; set; }

        public int CustomerId { get; set; }

        public int CarId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }

        [ForeignKey(nameof(CarId))]
        public virtual Car Inventory { get; set; }
    }
}
