using System;
using System.Linq;
using System.Text;
using CrashKonijn.Goap.Configs.Interfaces;

namespace CrashKonijn.Goap.Classes.Validators
{
    public static class Extensions
    {
        public static IWorldKey[] GetWorldKeys(this IGoapSetConfig goapSetConfig)
        {
            return goapSetConfig.Actions
                .SelectMany((action) =>
                {
                    return action.Conditions
                        .Where(x => x.WorldKey != null)
                        .Select(y => y.WorldKey);
                })
                .Distinct()
                .ToArray();
        }
        
        public static ITargetKey[] GetTargetKeys(this IGoapSetConfig goapSetConfig)
        {
            return goapSetConfig.Actions
                .Where(x => x.Target != null)
                .Select(x => x.Target)
                .Distinct()
                .ToArray();
        }
        
        public static string GetGenericTypeName(this Type type)
        {
            var typeName = new StringBuilder();
            if (type.IsNested)
            {
                typeName.Append(type.DeclaringType.GetGenericTypeName());
                typeName.Append(".");
            }

            var genericLocation = type.Name.IndexOf('`');

            if (genericLocation != -1)
            {
                var genericArguments = type.GetGenericArguments();
                var genericTypeName = type.Name.Substring(0, type.Name.IndexOf('`'));
                var typeArgumentNames = string.Join(",", genericArguments.Select(a => a.Name));
                typeName.Append($"{genericTypeName}<{typeArgumentNames}>");
            }
            else
            {
                typeName.Append(type.Name);
            }

            return typeName.ToString();
        }
    }
}