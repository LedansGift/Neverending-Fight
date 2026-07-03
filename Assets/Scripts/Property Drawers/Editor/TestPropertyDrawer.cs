using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TestAttackNode))]
public class TestPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect typePosition = new Rect(position.x, position.y, position.width, position.height);
        SerializedProperty typeProperty = property.FindPropertyRelative("attackPatternType");
        EditorGUI.PropertyField(typePosition, typeProperty);

        AttackPatternType patternType = (AttackPatternType)typeProperty.enumValueIndex;

        if (patternType == AttackPatternType.MeleeAttack)
        {
            Rect fieldSpawnPos = new Rect(
                position.x,
                position.y + EditorGUIUtility.singleLineHeight,
                position.width,
                position.height
            );
            //SerializedProperty fieldProperty = property.FindPropertyRelative();
        }

        EditorGUI.EndProperty();
    }
}
