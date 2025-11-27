using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class CharacterStatuses : MonoBehaviour
{
    public List<StatusDefinition> initialStatuses;

    public ReadOnlyCollection<CharacterStatus> Statuses => statuses.AsReadOnly();
    private readonly List<CharacterStatus> statuses = new();

    void Awake()
    {
        foreach (StatusDefinition definition in initialStatuses)
            AddStatus(definition);
    }

    public bool TryGetStatus(StatusDefinition definition, out CharacterStatus status)
    {
        status = statuses.Find(s => s.Definition == definition);
        return status != null;
    }

    public void AddStatus(StatusDefinition definition)
    {
        if (!TryGetStatus(definition, out CharacterStatus status))
            statuses.Add(status);
    }

    public void RemoveStatus(StatusDefinition definition)
    {
        if (TryGetStatus(definition, out CharacterStatus status))
            statuses.Remove(status);
    }
}