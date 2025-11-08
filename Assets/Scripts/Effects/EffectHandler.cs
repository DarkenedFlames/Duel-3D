using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    public List<Effect> Effects { get; private set; } = new();

    public bool HasEffect(string effectName) =>
        Effects.Any(e => e.Definition.effectName == effectName);

    public void ApplyEffect(EffectDefinition def)
    {
        var existing = Effects.Find(e => e.Definition.effectName == def.effectName);
        if (existing != null)
        {
            existing.ApplyStacking();
        }
        else
        {
            var newEffect = new Effect(def, this);
            Effects.Add(newEffect);
        }
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        for (int i = Effects.Count - 1; i >= 0; i--)
        {
            if (Effects[i].Tick(dt))
                Effects.RemoveAt(i);
        }
    }
}
