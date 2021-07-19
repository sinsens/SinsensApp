using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SinsensApp.Helper
{
    public static class EnumHelper
    {
        private static readonly ConcurrentDictionary<Enum, string> EnumDescCache;

        private static readonly ConcurrentDictionary<string, IEnumerable<KeyValuePair<int, string>>> CacheDict;
        private static readonly Lazy<IEnumerable<Type>> TypeCache;

        static EnumHelper()
        {
            EnumDescCache = new ConcurrentDictionary<Enum, string>();
            CacheDict = new ConcurrentDictionary<string, IEnumerable<KeyValuePair<int, string>>>();
            TypeCache = new Lazy<IEnumerable<Type>>(() =>
            {
                return AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x => x.FullName.Contains("SinsensApp.") || x.FullName.Contains("Abp."))
                    .SelectMany(x => x.GetTypes());
            });
        }

        public static string GetEnumDescription(this Enum enumValue)
        {
            if (enumValue == null)
            {
                return string.Empty;
            }

            return EnumDescCache.GetOrAdd(enumValue, e =>
            {
                var str = e.ToString();
                var field = e.GetType().GetField(str);
                var objs = field?.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs == null || objs.Length <= 0)
                {
                    return str;
                }

                var da = (DescriptionAttribute)objs[0];
                return da.Description;
            });
        }
    }
}