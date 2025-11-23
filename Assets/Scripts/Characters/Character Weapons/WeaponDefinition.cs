using UnityEngine;

[CreateAssetMenu]
public class WeaponDefinition : ScriptableObject
{
    public string WeaponName;
    public StatDefinition ExpendedStat;
    public float StatCost;
    public float CooldownTime;
    public float UseTime;
    public string AnimationTrigger = "AttackTrigger";
}