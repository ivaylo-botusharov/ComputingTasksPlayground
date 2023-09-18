namespace LoanApproval.RulesEngine;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;
using System.Linq.Dynamic.Core.CustomTypeProviders;

public class CustomTypeProvider : IDynamicLinkCustomTypeProvider
{
    private readonly HashSet<Type> customTypes = new HashSet<Type>();
    private readonly HashSet<string> customTypeNamespaces = new HashSet<string>();
    private readonly List<Assembly> customTypeAssemblies = new List<Assembly>();

    // Public property to store custom type namespaces
    public HashSet<string> CustomTypeNamespaces { get; } = new HashSet<string>();

    public void RegisterCustomTypesFromAssembly(Assembly assembly)
    {
        customTypeAssemblies.Add(assembly);
        ScanAndRegisterCustomTypes(assembly);
    }

    private void ScanAndRegisterCustomTypes(Assembly assembly)
    {
        // Scan and register custom types from the assembly
        var typesToAdd = assembly.GetExportedTypes()
            .Where(type => !type.IsPrimitive && !customTypes.Contains(type))
            .ToList();

        foreach (var type in typesToAdd)
        {
            customTypes.Add(type);
        }
    }
    
    public Type? ResolveType(string typeName)
    {
        // Resolve custom types based on typeName
        return customTypes.SingleOrDefault(t => t.Name == typeName);
    }

    public IEnumerable<DynamicProperty> GetDynamicProperties(Type type)
    {
        // Provide dynamic properties for the given type
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var propertyInfo in properties)
        {
            yield return new DynamicProperty(propertyInfo.Name, propertyInfo.PropertyType);
        }
    }

    public HashSet<Type> GetCustomTypes()
    {
        return customTypes;
    }

    public Dictionary<Type, List<MethodInfo>> GetExtensionMethods()
    {
        // Implement if you have extension methods related to custom types
        var extensionMethods = new Dictionary<Type, List<MethodInfo>>();

        foreach (var customType in customTypes)
        {
            var methods = customType
                .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false))
                .ToList();

            if (methods.Count > 0)
            {
                extensionMethods[customType] = methods;
            }
        }

        return extensionMethods;
    }

    public Type? ResolveTypeBySimpleName(string typeName)
    {
        // Implement if you need to resolve types by their simple names
        return customTypes.SingleOrDefault(t => t.Name == typeName);
    }
}
