using AutoLotDAL_EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using AutoLotDAL_EF.Repos;

namespace AutoLotDalTest
{
    class Program
    {
        static void Main(string[] args)
        {
            BaseRepo<Car> cars = new BaseRepo<Car>();
            WriteLine("******Cars******");
            foreach(var car in cars.GetAll())
            {
                WriteLine(car);
            }

            cars.Add(new Car { Make = "make1", Color = "color1", PetName = "name1" });
            WriteLine("***Add car***");

            cars.AddRange(new List<Car>(){new Car { Make = "make2", Color = "color2", PetName = "name2" },
                new Car { Make = "make3", Color = "color3", PetName = "name3" } });
            WriteLine("***Add cars***");

            WriteLine("******Cars******");
            foreach (var car in cars.GetAll())
            {
                WriteLine(car);
            }

            cars.GetOne(1).Color = "blue";
            WriteLine("***Update car***");

            WriteLine("******Cars******");
            foreach (var car in cars.GetAll())
            {
                WriteLine(car);
            }

            cars.Delete(cars.GetOne(5));
            WriteLine("***Delete car***");

            WriteLine("******Cars******");
            foreach (var car in cars.GetAll())
            {
                WriteLine(car);
            }

            ReadKey();
            
        }
    }
}
