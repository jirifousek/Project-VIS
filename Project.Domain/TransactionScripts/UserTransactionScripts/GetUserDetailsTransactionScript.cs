using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.DTO;
using Project.Data.TDGW;

namespace Project.Domain.TransactionScripts.UserTransactionScripts
{
    public class GetUserDetailsTransactionScript : ITransactionScript<UserDTO>
    {
        public int Id { get; set; }
        public UserDTO Output { get; private set; }

        public void Execute()
        {
            var gateway = new UserTableDataGateway();
            var user = gateway.GetUserById(Id);

            if (user is null)
            {
                throw new Exception("User not found");
            }

            Output = user;
        }
    }
}
