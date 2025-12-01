using UnityEngine;

public static class LogFormatter
{
    public static void LogNullField(string fieldName, string className, Object source) =>
        Debug.LogError($"{className} was configured with a null field: {fieldName}", source);
    
    public static void LogNullArgument(string argumentName, string methodName, string className, Object source) =>
        Debug.LogError($"{className}.{methodName} was passed a null argument: {argumentName}", source);

    public static void LogNullCollectionField(string argumentName, string methodName, string className, Object source) =>
        Debug.LogError($"{className}.{methodName} was passed a null or empty collection argument: {argumentName}", source);
}