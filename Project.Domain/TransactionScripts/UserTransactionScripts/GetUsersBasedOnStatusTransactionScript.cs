using Project.Data.DTO;
using Project.Data.TDGW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.TransactionScripts.UserTransactionScripts
{
    public class GetUsersBasedOnStatusTransactionScript : ITransactionScript<List<UserDTO>>
    {
        public string Status { get; set; }
        public List<UserDTO> Output { get; private set; }

        public void Execute()
        {
            var gateway = new UserTableDataGateway();
            
            var users = gateway.GetAllUsers();
            Output = users.Where(u => u.Status == Status).ToList();


        }
    }   
}
