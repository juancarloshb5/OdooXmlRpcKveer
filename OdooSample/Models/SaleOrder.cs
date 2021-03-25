using Odoo.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace OdooSample.Models
{
    public class SaleOrder
    {
        public int CompanyId { get; set; }
        public int CurrencyId { get; set; }

        public DateTime DateOrder { get; set; }
        public string Name { get; set; }
        public int PartnerId { get; set; }
        public string State { get; set; }
    }

    public class Map
    {

        
        public static Partner Partner(RpcRecord saleOrderRecord)
        {
            var street = (string)saleOrderRecord.GetField("street").Value;
            var name = (string)saleOrderRecord.GetField("name").Value;

            var parter = new Partner()
            {
                Street = street,
                Name = name
            };

            return parter;
        }
        public static SaleOrder SaleOrder(RpcRecord saleOrderRecord)
        {
            var company = (int) saleOrderRecord.GetField("company_id").Value;
            var name = (string) saleOrderRecord.GetField("name").Value;

            var saleOrder = new SaleOrder()
            {
                CompanyId = company,
                Name = name
            };

            return saleOrder;
        }
    }
}
