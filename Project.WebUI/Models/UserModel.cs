namespace Project.WebUI.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public UserModel() { }

        public UserModel(int id, string firstName, string lastName, string password, string role)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Role = role;
        }

        public override string ToString()
        {
            return $"{Id} {FirstName} {LastName} {Password} {Role}";
        }


    }
}
