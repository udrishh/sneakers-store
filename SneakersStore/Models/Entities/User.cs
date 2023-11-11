namespace SneakersStore.Models.Entities
{
    public class User
    {
        public User()
        {
            UserName = string.Empty;
            Password= string.Empty;
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public static List<User> GetAll()
        {
            var users = new List<User>();

            users.Add(new User() { Id = 1, UserName = "Bogdan", Password = "password"});
            users.Add(new User() { Id = 2, UserName = "Roxana", Password = "password" });

            return users;
        }
    }
}
