using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.DTO;
using Project.Data.TDGW;

namespace Project.Domain.TransactionScripts.UserTransactionScripts
{
    public class GetAllUsersTransactionScript : ITransactionScript<List<UserDTO>>
    {
        public List<UserDTO> Output { get; private set; }

        public void Execute()
        {
            var gateway = new UserTableDataGateway();
            Output = gateway.GetAllUsers();
        }
    }
}
