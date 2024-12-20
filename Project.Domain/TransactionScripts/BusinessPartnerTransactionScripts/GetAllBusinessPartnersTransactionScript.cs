using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.DTO;
using Project.Data.TDGW;

namespace Project.Domain.TransactionScripts.BusinessPartrTransactionScripts
{
    public class GetAllBusinessPartnersTransactionScript : ITransactionScript<List<BusinessPartnerDTO>>
    {
        public List<BusinessPartnerDTO> Output { get; private set; }

        public void Execute()
        {
            var gateway = new BusinessPartnerTableDataGateway();
            Output = gateway.GetAllBusinessPartners();
        }
    }
}
