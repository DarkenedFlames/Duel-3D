using UnityEditor;
using UnityEngine;

[ActionDrawer(typeof(AMoveCharacter))]
public class AMoveCharacterDrawer : IActionTypeDrawer
{
    const float VSpace = 2f;

    public float GetHeight(SerializedProperty property, GUIContent label)
    {
        float height = 0f;

        void GetProperty(string name, out SerializedProperty prop) =>
            prop = property.FindPropertyRelative(name);

        void AddHeight(SerializedProperty prop) =>
            height += EditorGUI.GetPropertyHeight(prop, true) + VSpace;

        // Add Conditions field
        GetProperty("Conditions", out SerializedProperty conditionsProp);
        if (conditionsProp != null)
            height += PolymorphicActionDrawer.GetConditionsListHeightStatic(conditionsProp);

        GetProperty("characterToMove", out SerializedProperty characterToMoveProp);
        GetProperty("referenceType", out SerializedProperty referenceTypeProp);
        GetProperty("reference", out SerializedProperty referenceProp);
        GetProperty("mode", out SerializedProperty modeProp);
        GetProperty("direction", out SerializedProperty directionProp);
        GetProperty("externalVelocityDamping", out SerializedProperty externalVelocityDampingProp);

        AddHeight(characterToMoveProp);
        AddHeight(referenceTypeProp);

        if ((MoveReferenceType)referenceTypeProp.enumValueIndex == MoveReferenceType.TowardsReference)
            AddHeight(referenceProp);

        AddHeight(modeProp);

        if ((MoveCharacterMode)modeProp.enumValueIndex == MoveCharacterMode.Force)
            AddHeight(externalVelocityDampingProp);

        AddHeight(directionProp);

        return height;
    }

    public void Draw(SerializedProperty property, Rect position, GUIContent label)
    {
        float y = position.y;

        void GetProperty(string name, out SerializedProperty prop) =>
            prop = property.FindPropertyRelative(name);

        void DrawField(SerializedProperty prop)
        {
            float h = EditorGUI.GetPropertyHeight(prop, true);
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), prop, true);
            y += h + VSpace;
        }

        // Draw Conditions field
        GetProperty("Conditions", out SerializedProperty conditionsProp);
        if (conditionsProp != null)
        {
            float conditionsHeight = PolymorphicActionDrawer.GetConditionsListHeightStatic(conditionsProp);
            PolymorphicActionDrawer.DrawConditionsListStatic(new Rect(position.x, y, position.width, conditionsHeight), conditionsProp);
            y += conditionsHeight;
        }

        GetProperty("characterToMove", out SerializedProperty characterToMoveProp);
        GetProperty("referenceType", out SerializedProperty referenceTypeProp);
        GetProperty("reference", out SerializedProperty referenceProp);
        GetProperty("mode", out SerializedProperty modeProp);
        GetProperty("direction", out SerializedProperty directionProp);
        GetProperty("externalVelocityDamping", out SerializedProperty externalVelocityDampingProp);

        DrawField(characterToMoveProp);
        DrawField(referenceTypeProp);

        if ((MoveReferenceType)referenceTypeProp.enumValueIndex == MoveReferenceType.TowardsReference)
            DrawField(referenceProp);
        
        DrawField(modeProp);

        if ((MoveCharacterMode)modeProp.enumValueIndex == MoveCharacterMode.Force)
            DrawField(externalVelocityDampingProp);
        
        DrawField(directionProp);
    }
}
