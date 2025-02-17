using Assets.Scripts.Eventing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class DependencyInjection
{
    private static readonly Dictionary<Type, object> _singeltonServices =  new();
    private static readonly Dictionary<Type, Func<object>> _transientServices =  new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnBeforeSceneLoad()
    {
        _singeltonServices.Clear();
        _transientServices.Clear();

        RegisterSingelton(new EventBroker());
    }

    private static void RegisterSingelton<T>(T service) => _singeltonServices.Add(typeof(T), service);
    private static T GetSingelton<T>() => (T)_singeltonServices[typeof(T)];
    private static object GetSingelton(Type serviceType) 
    {
        object service = null;
        _singeltonServices.TryGetValue(serviceType, out service);

        return service;
    }

    private static void RegisterTransient(Type serviceType, Func<object> instantiate) => _transientServices.Add(serviceType, instantiate);
    private static object GetTransient(Type serviceType) => _transientServices[serviceType]?.Invoke() ?? Activator.CreateInstance(serviceType);

    private static void InjectDependencies(object target, FieldInfo[] fieldsWithInject, PropertyInfo[] propertiesWithInject)
    {
        foreach (var field in fieldsWithInject)
        {
            var requiredDependency = GetSingelton(field.FieldType);
            requiredDependency ??= GetTransient(field.FieldType);

            field.SetValue(target, requiredDependency);
        }

        foreach (var property in propertiesWithInject)
        {
            var requiredDependency = GetSingelton(property.PropertyType);
            requiredDependency ??= GetTransient(property.PropertyType);

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
