using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.TDGW;
using Project.Data.DTO;

namespace Project.Domain.TransactionScripts.UserTransactionScripts
{
    public class CreateUserTransactionScript : ITransactionScript<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int Output { get; private set; }

        public void Execute()
        {
            var user = new UserDTO
            {
                FirstName = FirstName,
                LastName = LastName,
                Password = Password,
                Role = Role
            };

            var gateway = new UserTableDataGateway();
            gateway.InsertUser(user);
        }
    }
}
