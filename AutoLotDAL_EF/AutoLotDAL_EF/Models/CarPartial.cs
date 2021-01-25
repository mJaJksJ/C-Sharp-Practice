using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLotDAL_EF.Models
{
    public partial class Car
    {
        public override string ToString()
        {
            return $"{this.Id}. {this.Make} {this.PetName}: {this.Color}";
        }
    }
}
