using System;
using System.Collections.Generic;

public class Gate<T>
{
    public event Action<T> OnLock;
    public event Action<T> OnUnlock;

    private readonly HashSet<T> locked = new();
    private readonly Dictionary<T, float> timers = new();

    public bool IsOpen => locked.Count == 0;

    public void Lock(T key)
    {
        if (locked.Add(key)) OnLock?.Invoke(key);
    }

    public void Lock(T key, float duration)
    {
        if (locked.Add(key)) OnLock?.Invoke(key);
        if (duration > 0f) timers[key] = duration;
    }

    public void Unlock(T key)
    {
        if (locked.Remove(key))
        {
            timers.Remove(key);
            OnUnlock?.Invoke(key);
        }
    }

    public void Unlock(T key, float duration)
    {
        bool wasLocked = locked.Contains(key);

        if (wasLocked)
        {
            Unlock(key);
            Lock(key, duration);
        }
        else
        {
            Lock(key, duration);
        }
    }

    public void Tick(float deltaTime)
    {
        if (timers.Count == 0) return;

        var expired = new List<T>();

        // Enumerate over a copy of the keys to avoid modifying the collection while iterating
        foreach (var key in new List<T>(timers.Keys))
        {
            float remaining = timers[key] - deltaTime;
            if (remaining <= 0f) expired.Add(key);
            else timers[key] = remaining;
        }

        foreach (var key in expired)
        {
            timers.Remove(key);
            if (locked.Remove(key)) OnUnlock?.Invoke(key);
        }
    }


    public IEnumerable<T> GetLocks() => locked;
    public string GetLockSummary() => string.Join(", ", locked);
    public void Reset()
    {
        locked.Clear();
        timers.Clear();
    }
}



public enum AbilityKey
{
    Mana,
    Cooldown
}

public enum EffectKey
{

}

public enum PickupKey
{

}

public enum ProjectileKey
{

}

public enum WeaponKey
{
    Cooldown,
    Stamina,
    InactiveHitbox
}