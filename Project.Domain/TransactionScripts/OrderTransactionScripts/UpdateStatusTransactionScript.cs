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
    public class UpdateStatusTransactionScript : ITransactionScript<bool>
    {

        public bool Output { get; private set; }
        public int Id { get; set; }
        public string Status { get; set; } = "";

        public void Execute()
        {
            var orderTDGW = new OrderTableDataGateway();
            orderTDGW.UpdateOrderStatus(Id, Status);
        }
    }
}
