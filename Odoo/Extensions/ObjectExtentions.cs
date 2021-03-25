using System;

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

        
    }
}
