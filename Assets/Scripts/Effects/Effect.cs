using System.Collections.Generic;
using System.Linq;

public class Effect
{
    public EffectDefinition Definition { get; private set; }
    public List<EffectBehavior> Behaviors { get; private set; }
    public EffectHandler Handler { get; private set; }

    private float _timer;
    public int CurrentStacks { get; private set; } = 1;

    public Effect(EffectDefinition definition, EffectHandler handler)
    {
        Definition = definition;
        Handler = handler;
        _timer = definition.duration;

        // instantiate runtime behaviors
        Behaviors = definition.behaviors
            .Select(b => b.CreateRuntimeBehavior(this))
            .ToList();

        foreach (var behavior in Behaviors)
            behavior.OnApply();
    }

    public bool Tick(float deltaTime)
    {
        foreach (var behavior in Behaviors)
            behavior.OnTick(deltaTime);

        _timer -= deltaTime;

        if (_timer <= 0f)
        {
            switch (Definition.expireType)
            {
                case ExpireType.FullRemove:
                    Expire();
                    return true;

                case ExpireType.LoseOneStackAndRefresh:
                    CurrentStacks--;
                    if (CurrentStacks <= 0)
                    {
                        Expire();
                        return true;
                    }
                    _timer = Definition.duration;
                    break;

                case ExpireType.LoseOneStackNoRefresh:
                    CurrentStacks--;
                    if (CurrentStacks <= 0)
                    {
                        Expire();
                        return true;
                    }
                    break;
            }
        }

        return false;
    }

    private void Expire()
    {
        foreach (var behavior in Behaviors)
            behavior.OnExpire();
    }

    public void ApplyStacking()
    {
        switch (Definition.stackingType)
        {
            case StackingType.None:
                // do nothing
                break;

            case StackingType.RefreshDuration:
                _timer = Definition.duration;
                foreach (var behavior in Behaviors)
                    behavior.OnApply();
                break;

            case StackingType.AddStacks:
                if (CurrentStacks < Definition.maxStacks)
                    CurrentStacks++;
                _timer = Definition.duration;
                foreach (var behavior in Behaviors)
                    behavior.OnApply();
                break;

            case StackingType.Ignore:
                // do absolutely nothing
                break;
        }
    }
}
