// File: AbilityExecution.cs
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The transient execution container created for each cast/execution of an ability.
/// Holds runtime behaviors, tags, attributes, and a simple event bus.
/// Execution is responsible for driving behavior hooks.
/// </summary>
public class AbilityExecution
{
    public AbilityHandler Handler { get; private set; }       // the ability owner handler
    public Ability Ability { get; private set; }     // the instance being executed (tracks cooldown, etc.)
    public Vector3 CastDirection { get; private set; }
    public object Context { get; private set; }              // optional arbitrary context/payload

    public bool IsActive { get; private set; } = false;
    
    public AbilityTag Tags { get; private set; } = AbilityTag.None;
    readonly SimpleAttributeSet attributes = new(); // use flags/enum
    readonly EventBus eventBus = new();

    readonly List<AbilityBehavior> behaviors = new();

    public AbilityExecution(AbilityHandler handler, Ability ability, Vector3 castDirection, object ctx = null)
    {
        Handler = handler;
        Ability = ability;
        CastDirection = castDirection;
        Context = ctx;
    }

    public void AddTag(AbilityTag t) => Tags |= t;
    public void RemoveTag(AbilityTag t) => Tags &= ~t;
    public bool HasTag(AbilityTag t) => (Tags & t) != 0;
    public bool MatchTag(AbilityTag t) => (Tags & t) == t;

    public void SetAttribute(string key, float value) => attributes.Set(key, value);
    public float GetAttribute(string key) => attributes.Get(key); // use flags/enum

    // use enum for topic
    public void Subscribe(string topic, Action<object> cb) => eventBus.Subscribe(topic, cb);
    public void Unsubscribe(string topic, Action<object> cb) => eventBus.Unsubscribe(topic, cb);
    public void Publish(string topic, object payload = null) => eventBus.Publish(topic, payload);

    public void AddBehavior(AbilityBehavior b)
    {
        behaviors.Add(b);
        behaviors.Sort((a, b2) => a.Priority.CompareTo(b2.Priority));
    }

    /// <summary>Initialize runtime behaviors from definitions.</summary>
    public void InitializeBehaviors(IEnumerable<AbilityBehaviorDefinition> defs)
    {
        foreach (var def in defs)
        {
            var runtime = def.CreateRuntimeBehavior();
            runtime.Initialize(def, this);
            AddBehavior(runtime);
        }
    }

    /// <summary>
    /// Run the Activate phase in deterministic order.
    /// </summary>
    public void Activate()
    {
        if (IsActive) return;
        IsActive = true;
        // Activate behaviors in priority order
        foreach (var b in behaviors.ToArray()) // ToArray to be safe if behaviors mutate list
        {
            if (!b.IsEligible()) continue;
            b.OnActivate();
        }
        // notify listeners
        Publish("OnActivate", this);
    }

    /// <summary>Tick - call while this execution is active (e.g. during a cast/channel or while an area object exists).</summary>
    public void Tick(float dt)
    {
        if (!IsActive) return;
        foreach (var b in behaviors.ToArray())
        {
            if (!b.IsEligible()) continue;
            b.OnTick(dt);
        }
        Publish("OnTick", dt);
    }

    /// <summary>End - call when execution completes or is cancelled.</summary>
    public void End()
    {
        if (!IsActive) return;
        foreach (var b in behaviors.ToArray())
        {
            if (!b.IsEligible()) continue;
            b.OnEnd();
        }
        IsActive = false;
        Publish("OnEnd", this);
    }
}

/// <summary>Small attribute bag used by AbilityExecution for local values.</summary>
public class SimpleAttributeSet // use flags/enums
{
    readonly Dictionary<string, float> map = new Dictionary<string, float>();
    public void Set(string key, float value) => map[key] = value;
    public float Get(string key) => map.TryGetValue(key, out var v) ? v : 0f;
}

/// <summary>Very small EventBus. Topics are strings for flexibility.</summary>
/// Make a global event manager that gives the execution events to AnimationHandler, Input, etc
public class EventBus
{
    readonly Dictionary<string, List<Action<object>>> subs = new Dictionary<string, List<Action<object>>>();

    public void Subscribe(string topic, Action<object> cb)
    {
        if (!subs.TryGetValue(topic, out var list))
        {
            list = new List<Action<object>>();
            subs[topic] = list;
        }
        list.Add(cb);
    }

    public void Unsubscribe(string topic, Action<object> cb)
    {
        if (subs.TryGetValue(topic, out var list)) list.Remove(cb);
    }

    public void Publish(string topic, object payload = null)
    {
        if (!subs.TryGetValue(topic, out var list)) return;
        var snapshot = list.ToArray();
        foreach (var cb in snapshot) cb(payload);
    }
}
