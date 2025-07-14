using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{

    #region ComponentsSystem

    [SerializeField] private List<EntityComponent> components;

    private Dictionary<Type, EntityComponent> _componentDictionary;

    private void InitializeComponents()
    {
        _componentDictionary = new Dictionary<Type, EntityComponent>();

        foreach (var component in components)
        {
            if (component != null)
            {
                _componentDictionary[component.GetType()] = component;
            }
        }

        foreach (var component in components)
        {
            if (component != null)
            {
                component.Initialize(this);
            }
        }
    }

    public T GetEntityComponent<T>() where T : EntityComponent
    {
        _componentDictionary.TryGetValue(typeof(T), out var component);
        return component as T;
    }

    public bool HasComponent<T>() where T : EntityComponent
    {
        return _componentDictionary.ContainsKey(typeof(T));
    }

    public void AddComponent<T>(T component) where T : EntityComponent
    {
        if (component == null) return;

        var type = typeof(T);

        if (_componentDictionary.ContainsKey(type))
        {
            Debug.LogWarning($"Component {type.Name} already exists on {name}");
            return;
        }

        components.Add(component);
        _componentDictionary[type] = component;
        component.Initialize(this);
    }

    public void RemoveComponent<T>() where T : EntityComponent
    {
        var type = typeof(T);

        if (_componentDictionary.TryGetValue(type, out var component))
        {
            components.Remove(component);
            _componentDictionary.Remove(type);
        }
    }

    #endregion

    protected virtual void Awake()
    {
        InitializeComponents();
    }

    public abstract void Initialize();
}