using UnityEngine;

public interface IGameAction
{
    void Execute(GameObject source, GameObject target);
}