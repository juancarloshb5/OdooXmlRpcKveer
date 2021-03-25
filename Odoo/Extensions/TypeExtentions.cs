using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Odoo.Extensions;

namespace Odoo.Extensions
{
    public static class TypeExtentions
    {
        public static string[] GetPropertiesName(this Type type)
        {
            return type.GetProperties().Select(p => p.Name).ToLowerAndSplitWithUnderscore();

        }


    }
}
