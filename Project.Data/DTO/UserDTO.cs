using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Password { get; set; }
        public String Role { get; set; }
        public String Authorization { get; set; }
        public String Status { get; set; }
    }
}
