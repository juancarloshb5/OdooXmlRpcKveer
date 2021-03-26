using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Odoo.Extensions
{
    public static class XmlRpcStructExtentions
    {
        public static XmlRpcStruct ToXmlRpcStruct<TEntity>(this TEntity entity)
        {
            var vals = new XmlRpcStruct();

            foreach (var field in typeof(TEntity).GetProperties())
            {
                vals.Add(field.Name.ToLowerAndSplitWithUnderscore(), field.GetValue(entity));
            }
            return vals;
        }

        public static XmlRpcStruct NotNull(this XmlRpcStruct xmlRpcStruct, string[] fieldNames)
        {
            var notNullStruct = new XmlRpcStruct();
            foreach (var field in fieldNames)
            {
                if (xmlRpcStruct[field] != null)
                    notNullStruct.Add(field, xmlRpcStruct[field]);
            }

            return notNullStruct;
        }

        public static List<TEntity> ToEntityList<TEntity>(this IEnumerable<XmlRpcStruct> fields) where TEntity : class
        {
            var entities = new List<TEntity>();

            foreach (var field in fields)
            {
                var entity = field.ToEntity<TEntity>();
                entities.Add(entity);
            }

            return entities;
        }
        public static TEntity ToEntity<TEntity>(this XmlRpcStruct entityStruct) where TEntity : class
        {
            var entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
            foreach (PropertyInfo prop in entity.GetType().GetProperties())
            {

                var propName = prop.Name.ToLowerAndSplitWithUnderscore();
                var value =  entityStruct[propName];

                var falseNull = prop.PropertyType != typeof(bool) && value.GetType() == typeof(bool); 

                if (value == null || falseNull )
                {
                    prop.SetValue(entity, null);
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


       

    }
}
