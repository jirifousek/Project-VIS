using Project.Data.TDGW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.TransactionScripts.UserTransactionScripts
{
    public class UserLoginTransactionScript : ITransactionScript<bool>
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public bool Output { get; private set; }

        public void Execute()
        {
            var gateway = new UserTableDataGateway();
            var user = gateway.GetUserById(Id);

            if (user is null)
            {
                throw new Exception("User not found");
            }

            Output = user.Password == Password;
        }
    }   
}
