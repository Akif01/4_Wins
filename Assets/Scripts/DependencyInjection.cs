using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class DependencyInjection
{
    private static readonly Dictionary<Type, object> _services =  new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    private static void OnAfterAssembliesLoaded()
    {
    }

    private static void Register<T>(T service) => _services[typeof(T)] = service;

    private static T GetService<T>() => (T)_services[typeof(T)];

    private static object GetService(Type serviceType) => _services[serviceType];

    private static void InjectDependencies(object target, FieldInfo[] fieldsWithInject, PropertyInfo[] propertiesWithInject)
    {
        foreach (var field in fieldsWithInject)
        {
            var requiredDependency = GetService(field.FieldType);
            field.SetValue(target, requiredDependency);
        }

        foreach (var property in propertiesWithInject)
        {
            var requiredDependency = GetService(property.PropertyType);
            property.SetValue(target, requiredDependency);
        }
    }

    public static void InjectDependencies(object target)
    {
        var type = target.GetType();

        // Find all fields with the [Inject] attribute
        FieldInfo[] fieldsWithInject = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(f => f.GetCustomAttributes(typeof(InjectAttribute), false).Length > 0)
            .ToArray();

        // Find all properties with the [Inject] attribute
        PropertyInfo[] propertiesWithInject = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(p => p.GetCustomAttributes(typeof(InjectAttribute), false).Length > 0)
            .ToArray();

        InjectDependencies(target, fieldsWithInject, propertiesWithInject);
    }
}
