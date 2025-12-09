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


        GetProperty("characterToMove", out SerializedProperty characterToMoveProp);
        GetProperty("referenceType", out SerializedProperty referenceTypeProp);
        GetProperty("reference", out SerializedProperty referenceProp);
        GetProperty("mode", out SerializedProperty modeProp);
        GetProperty("direction", out SerializedProperty directionProp);

        AddHeight(characterToMoveProp);
        AddHeight(referenceTypeProp);

        if ((MoveReferenceType)referenceTypeProp.enumValueIndex == MoveReferenceType.TowardsReference)
            AddHeight(referenceProp);

        AddHeight(modeProp);
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

        GetProperty("characterToMove", out SerializedProperty characterToMoveProp);
        GetProperty("referenceType", out SerializedProperty referenceTypeProp);
        GetProperty("reference", out SerializedProperty referenceProp);
        GetProperty("mode", out SerializedProperty modeProp);
        GetProperty("direction", out SerializedProperty directionProp);

        DrawField(characterToMoveProp);
        DrawField(referenceTypeProp);

        if ((MoveReferenceType)referenceTypeProp.enumValueIndex == MoveReferenceType.TowardsReference)
            DrawField(referenceProp);
        
        DrawField(modeProp);
        DrawField(directionProp);
    }
}
