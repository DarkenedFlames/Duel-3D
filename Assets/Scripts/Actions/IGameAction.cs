using UnityEngine;

public interface IGameAction
{
    void Execute(ActionContext context);
}

public class ActionContext
{
    public object Source;
    public Character Target;

    public bool TryGetSourceTransform(out Transform sourceTransform)
    {
        sourceTransform = null;

        if (Source == null)
        {
            Debug.LogError("ActionContext.TryGetTransform() failed because ActionContext.Source == null!");
            return false;
        }

        sourceTransform = Source switch
        {
            Region => ((Region)Source).transform,                               // if Region  => region transform
            Ability => ((Ability)Source).GameObject.transform,                  // if Ability => owner transform
            CharacterEffect => ((CharacterEffect)Source).GameObject.transform,  // if Effect  => owner transform
            _ => null
        };

        return sourceTransform != null;
    }
}