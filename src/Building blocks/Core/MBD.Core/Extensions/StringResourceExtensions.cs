using System.Reflection;
using System.Resources;

namespace MBD.Core.Extensions
{
    public static class StringResourceExtensions
    {
        public static string GetResource(this string key) => 
            GetMessageByResourceKey(key, Assembly.GetCallingAssembly());

        public static string GetResource(this string key, Assembly assembly) =>
            GetMessageByResourceKey(key, assembly);

        public static string GetResource(this string key, string assemblyName) =>
            GetMessageByResourceKey(key, Assembly.Load(assemblyName));

        private static string GetMessageByResourceKey(string key, Assembly assembly)
        {
            var assemblyName = assembly.GetName().Name;
            var resourceManager = new ResourceManager($"{assemblyName}.Resources.Messages", assembly);
            return resourceManager.GetString(key);
        }
    }
}