using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.DTO;
using Project.Data.TDGW;

namespace Project.Domain.TransactionScripts.OrderTransactionScripts
{
    public class GetOrderItemsTransactionScript : ITransactionScript<List<OrderItemDTO>>
    {
        public List<OrderItemDTO> Output { get; private set; }
        public int Id { get; set; }
        public void Execute()
        {
            OrderTableDataGateway orderItemTableDataGateway = new OrderTableDataGateway();
            Output = orderItemTableDataGateway.GetOrderItems(Id);
        }
    }
}
