using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.DTO;
using Project.Data.TDGW;

namespace Project.Domain.TransactionScripts.MaterialTransactionScripts
{
    public class GetAllMaterialsTransactionScript : ITransactionScript<List<MaterialDTO>>
    {
        public List<MaterialDTO> Output { get; private set; }
        public void Execute()
        {
            var gateway = new MaterialTableDataGateway();
            Output = gateway.GetAllMaterials();
        }
    }
}
