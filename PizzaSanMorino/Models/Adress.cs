namespace PizzaSanMorino.Models
{
    public class Adress : BaseModel
    {
        public string City { get; set; }

        public string Street { get; set; }

        public string BuildingNumber { get; set; }

        public string AppartmentNumber { get; set; }

        public int ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}