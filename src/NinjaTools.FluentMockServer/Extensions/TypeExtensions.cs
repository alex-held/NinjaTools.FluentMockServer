using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NinjaTools.FluentMockServer.Extensions
{
    internal static class TypeExtensions
    {
        public static List<TField> GetFieldsOfType<TField>(this Type type)
        {
            return type.GetFields(
                    BindingFlags.Public | BindingFlags.Static
                                        | BindingFlags.FlattenHierarchy)
                .Where(p => type.IsAssignableFrom(p.FieldType))
                .Select(pi => (TField) pi.GetValue(null))
                .ToList();
        }
    }
}
