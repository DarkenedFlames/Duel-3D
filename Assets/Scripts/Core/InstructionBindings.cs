using UnityEngine;
using System;
using Unity.VisualScripting;

[Serializable]
public class ProjectileInstructionBinding
{
    public ProjectileHook Hook;
    [SerializeReference] public IInstruction Instruction;

    public void Execute(IInstructionContext context)
    {
        Instruction?.Execute(context);
    }
}

[Serializable]
public class AreaInstructionBinding
{
    public AreaHook Hook;
    [SerializeReference] public IInstruction Instruction;

    public void Execute(IInstructionContext context)
    {
        Instruction?.Execute(context);
    }
}

[Serializable]
public class AbilityInstructionBinding
{
    public AbilityHook Hook;
    [SerializeReference] public IInstruction Instruction;

    public void Execute(IInstructionContext context)
    {
        Instruction?.Execute(context);
    }
}

[Serializable]
public class PickupInstructionBinding
{
    public PickupHook Hook;
    [SerializeReference] public IInstruction Instruction;

    public void Execute(IInstructionContext context)
    {
        Instruction?.Execute(context);
    }
}

[Serializable]
public class EffectInstructionBinding
{
    public EffectHook Hook;
    [SerializeReference] public IInstruction Instruction;

    public void Execute(IInstructionContext context)
    {
        Instruction?.Execute(context);
    }
}

[Serializable]
public class WeaponInstructionBinding
{
    public WeaponHook Hook;
    [SerializeReference] public IInstruction Instruction;

    public void Execute(IInstructionContext context)
    {
        Instruction?.Execute(context);
    }
}