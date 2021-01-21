using AutoLotConsoleApp.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using static System.Console;

namespace AutoLotConsoleApp
{
    internal class Program
    {
        private static int AddNewRecord()
        {
            using(var context = new AutoLotEntities())
            {
                try
                {
                    var car = new Car()
                    {
                        CarId = 100,
                        Make = "Yugo",      
                        Color = "Brown", 
                        CarNickName="Brownie"
                    };
                    context.Cars.Add(car);
                    context.SaveChanges();
                    return car.CarId;
                }
                catch(Exception ex)
                {
                    WriteLine(ex.InnerException?.Message);
                    return 0;
                }
            }
        }

        private static void AddNewRecords(IEnumerable<Car> carsToAdd)
        {
            using (var context = new AutoLotEntities())
            {
                try
                {
                    context.Cars.AddRange(carsToAdd);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    WriteLine(ex.InnerException?.Message);
                }
            }
        }

        private static void PrintAllInventory()
        {
            using (var context = new AutoLotEntities())
            {
                foreach (var elem in context.Cars)
                {
                    WriteLine(elem);
                }
            }
        }

        private static void RemoveRecord(int carld)
        {
            using (var context = new AutoLotEntities())
            {

                Car carToDelete = context.Cars.Find(carld);
                if (carToDelete != null)
                {
                    context.Cars.Remove(carToDelete);
                    if (context.Entry(carToDelete).State != EntityState.Deleted)
                    {
                        throw new Exception("Unable to delete the record");
                    }
                    context.SaveChanges();
                }
            }
        }

        private static void RemoveMultipleRecords(IEnumerable<Car> carsToRemove)
        {
            using (var context = new AutoLotEntities())
            {
                try
                {
                    context.Cars.RemoveRange(carsToRemove);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    WriteLine(ex.InnerException?.Message);
                }
            }
        }

        private static void UpdateRecord(int carld)
        {
            // Найти запись об автомобиле, подлежащую обновлению, по первичному ключу,
            using (var context = new AutoLotEntities())
            {
                // Получить запись об автомобиле, обновить ее и сохранить!
                Car carToUpdate = context.Cars.Find(carld);
                if (carToUpdate != null)
            {
                    WriteLine(context.Entry(carToUpdate).State);
                    carToUpdate.Color = "Blue";
                    WriteLine(context.Entry(carToUpdate).State);
                    context.SaveChanges();
                } 
            } 
        }

        
        public static void Main(string[] args)
        {
            WriteLine("***Fun with ADO.NET EF***\n");
            int carld = AddNewRecord();
            WriteLine(carld);

            AddNewRecords(new List<Car>() {new Car(){CarId = 101, CarNickName = "petname", Color = "Black", Make = "Lada" },
                new Car(){CarId = 102, CarNickName = "petname", Color = "Black", Make = "Lada" } });

            PrintAllInventory();

            ReadLine();

        }
    }
}