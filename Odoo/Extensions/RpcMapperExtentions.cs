using CookComputing.XmlRpc;
using Odoo.Concrete;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Odoo.Extensions
{
    public static class RpcMapperExtentions
    {
        public static RpcRecord ToRpcRecord<TEntity>(this TEntity entity, RpcConnection rpcConnection, string model, int? id) where TEntity : class
        {
            var fields = new List<RpcField>();

            foreach (var property in entity.GetType().GetProperties())
            {
                var value = property.GetValue(entity).OdooDataFormat();


                if (value != null)
                {
                    var field = new RpcField()
                    {
                        FieldName = property.Name.ToLowerAndSplitWithUnderscore(),
                        Value = value
                    };
                    fields.Add(field);
                }

            }

            var record = new RpcRecord(rpcConnection, model, id, fields);

            return record;
        }
        public static TEntity ToEntity<TEntity>(this RpcRecord record) where TEntity : class
        {
            var entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
            foreach (PropertyInfo prop in entity.GetType().GetProperties())
            {
                var propName = prop.Name.ToLowerAndSplitWithUnderscore();
                var value = record.GetField(propName).Value;

                if (value == null)
                {
                    prop.SetValue(entity, value);
                }
                else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                {
                    prop.SetValue(entity, Convert.ToDateTime(value));
                }
                else if (value.GetType() == typeof(object[]))
                {
                    var values = value as object[];
                    var valueId = values[0];
                    prop.SetValue(entity, valueId);
                }

                else
                {
                    prop.SetValue(entity, value);
                }
            }

            return entity;

        }


        public static XmlRpcStruct ToXmlRpcStruct(this IEnumerable<RpcField> fields)
        {
            var values = new XmlRpcStruct();

            foreach (var field in fields)
            {
                values[field.FieldName] = field.Value;
            }

            return values;
        }
    }
}
