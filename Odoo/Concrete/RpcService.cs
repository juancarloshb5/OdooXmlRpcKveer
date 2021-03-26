using Odoo.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odoo.Concrete
{
    public class RpcService<TEntity> where TEntity : class
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
            var entitesObject = connection.SearchAndRead(model, new RpcFilter().ToArray(), propertiesName).ToXmlRpcStructList();
            var entities = entitesObject.ToEntityList<TEntity>();
            return entities;

        }

        public int Create(TEntity entity)
        {
            var propertiesName = typeof(TEntity).GetPropertiesName().ToLowerAndSplitWithUnderscore();
            var entityStruct = entity.ToXmlRpcStruct().NotNull(propertiesName);
            return connection.Create(model, entityStruct);
        }


    }
}
