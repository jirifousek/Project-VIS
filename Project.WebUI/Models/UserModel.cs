using System.ComponentModel.DataAnnotations;

namespace Project.WebUI.Models
{
    public class UserModel
    {
        [Display(Name = "Personal no")]
        public int Id { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; } = string.Empty;
        [Display(Name = "Last name")]
        public string LastName { get; set; } = string.Empty;
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;
        [Display(Name = "Position")]
        public string Role { get; set; } = string.Empty;
        [Display(Name = "Authorization")]
        public string Authorization { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public UserModel() { }

        public UserModel(int id, string firstName, string lastName, string password, string role, string authorization, string status)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Role = role;
            Authorization = authorization;
            Status = status;
        }

        public override string ToString()
        {
            return $"{Id} {FirstName} {LastName} {Password} {Role} {Authorization} {Status}";
        }


    }
}
