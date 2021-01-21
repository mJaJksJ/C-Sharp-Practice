using System;
using System.Collections.Generic;
using System.Linq;
using AutoLotDAL.DataOperations;
using AutoLotDAL.Models;
using AutoLotDAL.BulkImport;

namespace AutoLotClient
{
    static class Program
    {
        private static readonly InventoryDal Dal = new InventoryDal();
        private static List<Car> _cars = new List<Car>();

        public static void DoBulkCopy()
        {
            Console.WriteLine("***Do Bulk Copy***");
            var cars = new List<Car>()
            {
                new Car() {CarId = 40, Color = "Blue", Make = "Honda", PetName = "MyCarl"},
                new Car() {CarId = 41, Color = "Red", Make = "Volvo", PetName = "MyCar2"},
                new Car() {CarId = 42, Color = "White", Make = "VW", PetName = "МуСагЗ"},
                new Car() {CarId = 43, Color = "Yellow", Make = "Toyota", PetName = "MyCar4"}
            };
            ProcessBulkImport.ExecuteBulkImport(cars, "Inventory");
            var lst = Dal.GetAllInventory();
            InitCars();
            ShowAllCars();
        }

        private static void InitCars()
        {
            _cars = Dal.GetAllInventory();
        }

        private static void MoveCustomer()
        {
            Console.WriteLine("***Simple Transaction***\n");
            Dal.ProcessCreditRisk(true, 1);
            Dal.ProcessCreditRisk(false, 4);
            Console.WriteLine("Check CreditRisk table for results");
            Console.WriteLine("*********************");
        }

        private static void ShowAllCars()
        {
            Console.WriteLine("***All cars***");
            Console.WriteLine("CarId\tMake\tColor\tPet Name");
            foreach (var itm in _cars)
            {
                Console.WriteLine($"{itm.CarId}\t{itm.Make}\t{itm.Color}\t{itm.PetName}");
            }

            Console.WriteLine("*********************");
        }

        private static void FirstCarByColor()
        {
            var car = Dal.GetCar(_cars.OrderBy(x => x.Color).Select(x => x.CarId).First());
            Console.WriteLine("***First Car By Color***");
            Console.WriteLine("CarId\tMake\tColor\tPet Name");
            Console.WriteLine($"{car.CarId}\t{car.Make}\t{car.Color}\t{car.PetName}");
            Console.WriteLine("*********************");
        }

        private static void DeleteCar(int carId)
        {
            try
            {
                Dal.DeleteCar(carId);
                InitCars();
                Console.WriteLine("***Car deleting***");
                Console.WriteLine("*********************");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occurred: {ex.Message}");
            }
        }

        private static void InsertCar(Car car)
        {
            Dal.InsertAuto(car);
            InitCars();
            Console.WriteLine("***Car inserting***");
            Console.WriteLine("*********************");
        }

        private static void UpdateCarPetName(int carId)
        {
            Console.WriteLine("***Car updating***");
            Console.WriteLine($"Last pet name: {Dal.LookUpPetName(carId)}");
            Dal.UpdateCarPetName(carId, $"{Dal.GetCar(carId)?.PetName}_new");
            InitCars();
            Console.WriteLine($"New pet name: {Dal.LookUpPetName(carId)}");
            Console.WriteLine("*********************");
        }

        public static void Main(string[] args)
        {
            InitCars();
            ShowAllCars();
            FirstCarByColor();

            DeleteCar(5);
            ShowAllCars();

            InsertCar(new Car {Color = "Blue", Make = "Pilot", PetName = "TowMonster"});
            ShowAllCars();

            UpdateCarPetName(4);
            ShowAllCars();

            MoveCustomer();
            
            DoBulkCopy();

            Console.ReadLine();
        }
    }
}