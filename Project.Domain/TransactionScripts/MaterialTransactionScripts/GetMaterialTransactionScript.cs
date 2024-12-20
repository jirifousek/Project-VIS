using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.DTO;
using Project.Data.TDGW;

namespace Project.Domain.TransactionScripts.MaterialTransactionScripts
{
    public class GetlMaterialTransactionScript : ITransactionScript<MaterialDTO>
    {
        public MaterialDTO Output { get; private set; }
        public int Id { get; set; }
        public void Execute()
        {
            var gateway = new MaterialTableDataGateway();
            Output = gateway.GetMaterialById(Id);
        }
    }
}
