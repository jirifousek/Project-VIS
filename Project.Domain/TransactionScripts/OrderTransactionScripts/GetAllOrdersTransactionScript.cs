using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.DTO;
using Project.Data.TDGW;

namespace Project.Domain.TransactionScripts.OrderTransactionScripts
{
    public class GetAllOrdersTransactionScript : ITransactionScript<List<OrderHeaderDTO>>
    {
        public List<OrderHeaderDTO> Output { get; private set; }
        public void Execute()
        {
            OrderTableDataGateway orderTableDataGateway = new OrderTableDataGateway();
            Output = orderTableDataGateway.GetAllOrders();
        }
    }
}
