using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilityExecution
{
    public Ability Ability { get; private set; }
    public bool IsActive { get; private set; } = false;

    public AbilityExecution(Ability ability)
    {
        Ability = ability;
    }

    void PerformInstruction(AbilityHook hook)
    {
        AbilityContext ctx = new(Ability.Handler, Ability.Handler.gameObject);

        foreach (AbilityInstructionBinding binding in Ability.Definition.bindings)
            if (binding.Hook.Equals(hook))
                binding.Execute(ctx);            
    }

    public void Activate()
    {
        if (IsActive) return;
        IsActive = true;
        PerformInstruction(AbilityHook.OnActivate);
    }

    public void Tick() => PerformInstruction(AbilityHook.OnTick);
    

    public void End()
    {
        PerformInstruction(AbilityHook.OnEnd);
        IsActive = false;
    }
}
