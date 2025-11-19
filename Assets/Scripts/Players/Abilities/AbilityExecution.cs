public class AbilityExecution
{
    public Ability Ability { get; private set; }
    public AbilityExecution(Ability ability) => Ability = ability;
    public void Activate() => Ability.Definition.actions.ForEach(a => a.Execute(Ability.Handler.gameObject));
}
