using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoLotDAL_EF.Models;

namespace AutoLotDAL_EF.Repos
{
    public class InventoryRepo: BaseRepo<Car>
    {
        public override List<Car> GetAll() => Context.Cars.OrderBy(x => x.PetName).ToList();
    }
}
