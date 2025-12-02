using UnityEditor;

[CustomEditor(typeof(EffectDefinition))]
public class EffectDefinitionEditor : Editor
{
    SerializedProperty effectName;
    SerializedProperty icon;

    SerializedProperty stackingType;
    SerializedProperty expiryType;
    SerializedProperty scalesWithStacks;
    SerializedProperty maxStacks;
    SerializedProperty duration;
    SerializedProperty period;

    // Actions
    SerializedProperty onApply;
    SerializedProperty onPulse;
    SerializedProperty onExpire;
    SerializedProperty onMaxStack;
    SerializedProperty onRefreshed;
    SerializedProperty onStackGained;
    SerializedProperty onStackLost;
    SerializedProperty onExtended;

    void OnEnable()
    {
        effectName = serializedObject.FindProperty("effectName");
        icon = serializedObject.FindProperty("icon");

        stackingType = serializedObject.FindProperty("EffectStackingType");
        expiryType = serializedObject.FindProperty("expiryType");
        scalesWithStacks = serializedObject.FindProperty("ScalesWithStacks");
        maxStacks = serializedObject.FindProperty("maxStacks");

        duration = serializedObject.FindProperty("duration");
        period = serializedObject.FindProperty("period");

        onApply = serializedObject.FindProperty("OnApplyActions");
        onPulse = serializedObject.FindProperty("OnPulseActions");
        onExpire = serializedObject.FindProperty("OnExpireActions");
        onMaxStack = serializedObject.FindProperty("OnMaxStackReachedActions");
        onRefreshed = serializedObject.FindProperty("OnRefreshedActions");
        onStackGained = serializedObject.FindProperty("OnStackGainedActions");
        onStackLost = serializedObject.FindProperty("OnStackLostActions");
        onExtended = serializedObject.FindProperty("OnExtendedActions");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(effectName);
        EditorGUILayout.PropertyField(icon);

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Effect Settings", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(stackingType);
        EditorGUILayout.PropertyField(expiryType);
        EditorGUILayout.PropertyField(scalesWithStacks);
        EditorGUILayout.PropertyField(maxStacks);
        EditorGUILayout.PropertyField(duration);
        EditorGUILayout.PropertyField(period);

        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(onApply);
        EditorGUILayout.PropertyField(onMaxStack);
        EditorGUILayout.PropertyField(onStackGained);
        EditorGUILayout.PropertyField(onStackLost);
        EditorGUILayout.PropertyField(onExpire);

        if (period.floatValue > 0f) EditorGUILayout.PropertyField(onPulse);

        bool stackingIsExtend = (EffectStackingType) stackingType.enumValueIndex == EffectStackingType.ExtendDuration;
        if (duration.floatValue > 0f && stackingIsExtend) EditorGUILayout.PropertyField(onExtended);

        bool stackingIsIgnore = (EffectStackingType) stackingType.enumValueIndex == EffectStackingType.Ignore;
        bool expireLoseAll = (ExpiryType) expiryType.enumValueIndex == ExpiryType.LoseAllStacks;
        bool refreshedDead = duration.floatValue == 0f || (stackingIsIgnore && expireLoseAll);
        if (!refreshedDead) EditorGUILayout.PropertyField(onRefreshed);

        serializedObject.ApplyModifiedProperties();
    }
}
