using UnityEngine;
using System;

[Flags]
public enum Status { None, Airborne }

public class StatusHandler : MonoBehaviour
{
    public Status Status { get; private set; } = Status.None;
    
    public bool MatchStatus(Status s) => (Status & s) == s;
    public bool HasStatus(Status s) => (Status & s) != 0;
    public void AddStatus(Status s) => Status |= s;
    public void RemoveStatus(Status s) => Status &= ~s;

}
