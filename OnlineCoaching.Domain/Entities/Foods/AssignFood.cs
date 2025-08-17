namespace OnlineCoaching.Domain.Entities
{
    public class AssignFood : BaseEntity
    {
        
        public DateTime AssignedOn { get; set; } = DateTime.Now;
        public string? Notes { get; set; }
        public int Quantity { get; set; } // in grams/servings

        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public int FoodId { get; set; }
        public Food? Food { get; set; }
    }

}
