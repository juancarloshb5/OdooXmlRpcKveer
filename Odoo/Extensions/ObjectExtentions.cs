using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;

namespace Odoo.Extensions
{
    public static class ObjectExtentions
    {
        public static object OdooDataFormat(this object value)
        {
            if (value == null)
                return value;
            if (value.GetType() == typeof(bool))
            {
                var condition = (bool)value;
                if (condition)
                    value = "true";
                else
                    value = "false";

            }
            return value;
        }

        public static List<XmlRpcStruct> ToXmlRpcStructList(this object[] values)
        {
            var list = new List<XmlRpcStruct>();
            foreach (var value in values)
            {
                var item = (XmlRpcStruct)value;
                list.Add(item);
            }
            return list;
        }
        




    }

    
}
