using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Project.Data.DTO;
using Project.Data.TDGW;

namespace Project.Domain.TransactionScripts.OrderTransactionScripts
{
    public class CreateOrderTransactionScript : ITransactionScript<int>
    {
        private readonly OrderHeaderDTO orderHeader;
        private readonly List<OrderItemDTO> orderItems;

        public int Output { get; private set; }

        public CreateOrderTransactionScript(OrderHeaderDTO orderHeader, List<OrderItemDTO> orderItems)
        {
            this.orderHeader = orderHeader;
            this.orderItems = orderItems;
            this.Output = -1;
        }

        public void Execute()
        {
            var orderTDGW = new OrderTableDataGateway();
            int orderId = orderTDGW.CreateOrder(orderHeader, orderItems);
            Output = orderId;
        }
    }
}
