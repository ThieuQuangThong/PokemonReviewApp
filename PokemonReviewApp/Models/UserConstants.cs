namespace PokemonReviewApp.Models
{
    public class UserConstants
    {
        public static List<User> Users = new List<User>()
        {
            new User(){UserName = "JvAd", EmailAddress = "jv@gmail.com", Password = "123", GivenName = "JV",Surname="Thieu", Role = "Admins"},
            new User(){UserName = "JvCtm", EmailAddress = "jv@gmail.com", Password = "123", GivenName = "JV",Surname="Thieu", Role = "Owner"},

        };
    }
}
