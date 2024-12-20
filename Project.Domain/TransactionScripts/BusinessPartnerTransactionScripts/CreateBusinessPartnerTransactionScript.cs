using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.DTO;
using Project.Data.TDGW;

namespace Project.Domain.TransactionScripts.BusinessPartrTransactionScripts
{
    public class CreateBusinessPartnerTransactionScript : ITransactionScript<int>
    {
        public int Output { get; private set; }
        public BusinessPartnerDTO BusinessPartner { get; set; }

        public void Execute()
        {
            var gateway = new BusinessPartnerTableDataGateway();
            Output = gateway.InsertBusinessPartner(BusinessPartner);
        }
    }
}
