using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Scans the assembly of the specified type for classes ending with the given suffix
        /// and registers them as Scoped services with their matching interface (e.g. IService -> Service).
        /// </summary>
        public static IServiceCollection RegisterAssemblyTypesAsScoped(
            this IServiceCollection services, 
            Type assemblyMarkerType, 
            string typeSuffix)
        {
            var assembly = assemblyMarkerType.Assembly;
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith(typeSuffix));

            foreach (var type in types)
            {
                var interfaceType = type.GetInterface($"I{type.Name}");
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, type);
                }
            }

            return services;
        }
    }
}
