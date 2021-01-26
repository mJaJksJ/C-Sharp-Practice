using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AutoLotDAL_EF.Models.Base
{
    public class EntityBase
    {
        [Key]
        public virtual int Id { get; set; }
        //[Timestamp]
        //public byte[] Timestamp { get; set; }
    }
}
