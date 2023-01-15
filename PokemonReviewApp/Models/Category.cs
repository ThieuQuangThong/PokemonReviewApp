namespace PokemonReviewApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public ICollection<PokemonCategory> PokemonCategories { get; set; } 
    }
}
