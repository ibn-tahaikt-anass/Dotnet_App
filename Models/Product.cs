namespace MES.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string ProductName { get; set; }
        public required string Categorie { get; set; }
        public  decimal Price { get; set; }
    }
}
