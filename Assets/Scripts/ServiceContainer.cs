using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceContainer
{
    private static Dictionary<Type, object> _services =  new Dictionary<Type, object>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    static void OnAfterAssembliesLoaded()
    {
        
    }

    public static void Register<T>(T service)
    {
        _services[typeof(T)] = service;
    }

    public static T GetService<T>()
    {
        return (T)_services[typeof(T)];
    }
}
