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


        public TEntity Get(int id)
        {
            var filter = new RpcFilter().Equal("id", id).ToArray();
            var entity = connection.SearchAndRead(model, filter, propertiesName, limit: 1)
                .ToXmlRpcStructList()
                .ToEntityList<TEntity>()
                .FirstOrDefault();
            return entity;


        }
        public IEnumerable<TEntity> SearchAndRead(RpcFilter filter) 
        {
            var entitesObject = connection.SearchAndRead(model, filter.ToArray(), propertiesName).ToXmlRpcStructList();
            var entities = entitesObject.ToEntityList<TEntity>();
            return entities;

        }

        public int Create(TEntity entity)
        {
            var propertiesName = typeof(TEntity).GetPropertiesName().ToLowerAndSplitWithUnderscore();
            var entityStruct = entity.ToXmlRpcStruct().NotNull(propertiesName);
            return connection.Create(model, entityStruct);
        }

        public bool Write(int id, TEntity entity)
        {
            var propertiesName = typeof(TEntity).GetPropertiesName().ToLowerAndSplitWithUnderscore();
            var entityStruct = entity.ToXmlRpcStruct().NotNull(propertiesName);
            return connection.Write(model, new int[1] { id } , entityStruct);
        }

        public bool Remove(int id)
        {
            return connection.Remove(model, new int[1] { id });
        }


    }
}
