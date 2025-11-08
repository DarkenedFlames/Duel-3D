using UnityEngine;

public enum AbilityType { Primary, Secondary, Utility, Special }

[CreateAssetMenu(fileName = "New Ability Definition", menuName = "Duel/Abilities/Definition")]
public class AbilityDefinition : ScriptableObject
{
    [Header("General")]
    public string abilityName; // make enum?
    public Sprite icon;
    public AbilityType abilityType;
    public AbilityAnimationTrigger castAnimationTrigger = AbilityAnimationTrigger.Cast;
    [Tooltip("How much mana to spend on cast")]
    public float manaCost = 0f;
    [Tooltip("Cooldown in seconds after cast")]
    public float cooldown = 0f;
    [Tooltip("Cast time in seconds (0 = instant)")]
    public float castTime = 0f;
    // add description, or to ability if need runtime data

    [Header("Activation Conditions")]
    [SerializeReference] public ActivationCondition[] activationConditions;

    [Header("Behaviors")]
    [Tooltip("Ordered list of behavior definitions that will run during execution (priority order)")]
    public AbilityBehaviorDefinition[] behaviorDefinitions;
}
