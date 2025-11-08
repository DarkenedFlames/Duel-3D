using System;

[Flags]
public enum HookType
{
    None = 0,
    OnApply = 1 << 0,
    OnTick = 1 << 1,
    OnExpire = 1 << 2,
    OnTargetEnter = 1 << 3,
    OnTargetExit = 1 << 4
}