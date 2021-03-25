using Odoo.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odoo.Concrete
{
    public class RpcService<TEntity>
    {
        private readonly RpcConnection connection;
        private readonly string model;
        private string[] propertiesName;
       


        public RpcService(RpcConnection connection, string model)
        {
            this.connection = connection;
            this.model = model;
            propertiesName = typeof(TEntity).GetPropertiesName();
        }

        public IEnumerable<TEntity> SearchAndRead()
        {
            var entitesObject = connection.SearchAndRead(model, new RpcFilter().ToArray(), propertiesName);

            return new List<TEntity>();

        }
        public IEnumerable<TEntity> SearchAndRead(RpcFilter filter)
        {
           var entitesObject =  connection.SearchAndRead(model, filter.ToArray(), propertiesName);

            return new List<TEntity>();

        }
    }
}
