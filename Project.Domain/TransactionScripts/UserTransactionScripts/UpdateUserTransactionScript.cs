using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.TDGW;
using Project.Data.DTO;

namespace Project.Domain.TransactionScripts.UserTransactionScripts
{
    public class UpdateUserTransactionScript : ITransactionScript<int>
    { 
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Authorization { get; set; }
        public string Status { get; set; }
        public int Output { get; private set; }

        public void Execute()
        {
            var user = new UserDTO
            {
                Id = Convert.ToInt32(Id),
                FirstName = FirstName,
                LastName = LastName,
                Password = Password,
                Role = Role,
                Authorization = Authorization,
                Status = Status
            };

            var gateway = new UserTableDataGateway();
            gateway.UpdateUser(user);
        }
    }
}
