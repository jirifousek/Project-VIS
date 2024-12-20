using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.DTO;
using Project.Data.TDGW;

namespace Project.Domain.TransactionScripts.MaterialTransactionScripts
{
    public class DeleteMaterialTransactionScript : ITransactionScript<bool>
    {
        public bool Output { get; private set; }
        public int Id { get; set; }
        public void Execute()
        {
            var gateway = new MaterialTableDataGateway();
            gateway.DeleteMaterial(Id);
        }
    }
}
