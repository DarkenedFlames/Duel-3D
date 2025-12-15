using System;

/// <summary>
/// Base interface for action conditions. Conditions determine if an action should execute.
/// </summary>
public interface IActionCondition
{
    bool IsSatisfied(ActionContext context);
}