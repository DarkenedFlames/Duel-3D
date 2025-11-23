using UnityEngine;
using System.Collections.Generic;

public enum AbilityAnimationTrigger { None, Cast, Channel }
public enum AbilityType { Primary, Secondary, Utility, Special }

[CreateAssetMenu(fileName = "New Ability Definition", menuName = "Duel/AbilityDefinition")]
public class AbilityDefinition : ScriptableObject
{
    [Header("General")]
    public string abilityName;
    public Sprite icon;
    public AbilityType abilityType;
    public AbilityAnimationTrigger castAnimationTrigger = AbilityAnimationTrigger.Cast;

    [Tooltip("How much mana to spend on cast")]
    public float manaCost = 0f;

    [Tooltip("Cooldown in seconds after cast")]
    public float cooldown = 0f;

    [SerializeReference] public ActivationCondition[] activationConditions;

    [Tooltip("Ordered list of behaviors that will run during execution."), SerializeReference]
    public List<IGameAction> actions = new();

}
