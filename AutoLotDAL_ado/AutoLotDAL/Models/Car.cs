namespace AutoLotDAL.Models
{
    /// <summary>
    /// stand-in class for AutoLot.db dbo.Inventory
    /// </summary>
    public class Car
    {
        public int CarId { get; set; }
        public string Color { get; set; }
        public string Make { get; set; }
        public string PetName { get; set; }
    }
}