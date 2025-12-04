using UnityEditor;
using UnityEngine;

[ActionDrawer(typeof(AMoveCharacter))]
public class AMoveCharacterDrawer : IActionTypeDrawer
{
    const float VSpace = 2f;

    public float GetHeight(SerializedProperty property, GUIContent label)
    {
        float height = 0f;

        SerializedProperty targetProp        = property.FindPropertyRelative("target");
        SerializedProperty referenceTypeProp = property.FindPropertyRelative("referenceType");
        SerializedProperty referenceProp     = property.FindPropertyRelative("reference");
        SerializedProperty modeProp          = property.FindPropertyRelative("mode");
        SerializedProperty directionProp     = property.FindPropertyRelative("direction");

        height += EditorGUI.GetPropertyHeight(targetProp, true) + VSpace;
        height += EditorGUI.GetPropertyHeight(referenceTypeProp, true) + VSpace;

        if ((MoveReferenceType)referenceTypeProp.enumValueIndex == MoveReferenceType.TowardsReference)
            height += EditorGUI.GetPropertyHeight(referenceProp, true) + VSpace;

        height += EditorGUI.GetPropertyHeight(modeProp, true) + VSpace;
        height += EditorGUI.GetPropertyHeight(directionProp, true) + VSpace;

        return height;
    }

    public void Draw(SerializedProperty property, Rect position, GUIContent label)
    {
        float y = position.y;

        SerializedProperty targetProp        = property.FindPropertyRelative("target");
        SerializedProperty referenceTypeProp = property.FindPropertyRelative("referenceType");
        SerializedProperty referenceProp     = property.FindPropertyRelative("reference");
        SerializedProperty modeProp          = property.FindPropertyRelative("mode");
        SerializedProperty directionProp     = property.FindPropertyRelative("direction");

        float h;

        h = EditorGUI.GetPropertyHeight(targetProp, true);
        EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), targetProp, true);
        y += h + VSpace;

        h = EditorGUI.GetPropertyHeight(referenceTypeProp, true);
        EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), referenceTypeProp, true);
        y += h + VSpace;

        if ((MoveReferenceType)referenceTypeProp.enumValueIndex == MoveReferenceType.TowardsReference)
        {
            h = EditorGUI.GetPropertyHeight(referenceProp, true);
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), referenceProp, true);
            y += h + VSpace;
        }

        h = EditorGUI.GetPropertyHeight(modeProp, true);
        EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), modeProp, true);
        y += h + VSpace;

        h = EditorGUI.GetPropertyHeight(directionProp, true);
        EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), directionProp, true);
        y += h + VSpace;
    }
}
