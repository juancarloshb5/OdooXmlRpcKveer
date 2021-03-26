using Odoo.Concrete;
using OdooSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdooSample.Services
{
    public class PartnerService : RpcService<Partner>
    {

        public PartnerService(RpcConnection connection) :base(connection, OdooDbSets.Contacts)
        {

        }
    }
}
