using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WhalesAndGames.Pools
{
    /// <summary>
    /// Creates a custom property drawer for pool items, in which the item will show it's name first.
    /// </summary>
    [CustomPropertyDrawer(typeof(PoolVariable))]
    public class PoolVariableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            if (property.FindPropertyRelative("variable").objectReferenceValue != null)
            {
                label.text = property.FindPropertyRelative("variable").objectReferenceValue.name;
            }
            else
            {
                label.text = "(Unassigned)";
            }

            // Shows the variables that compose the Item.
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }
    }

}