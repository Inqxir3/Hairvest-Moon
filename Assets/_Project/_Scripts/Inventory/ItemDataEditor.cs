#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
namespace HairvestMoon.Inventory
{
    [CustomEditor(typeof(ItemData))]
    public class ItemDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty itemName = serializedObject.FindProperty("itemName");
            SerializedProperty itemIcon = serializedObject.FindProperty("itemIcon");
            SerializedProperty itemType = serializedObject.FindProperty("itemType");
            SerializedProperty sellValue = serializedObject.FindProperty("sellValue");
            SerializedProperty associatedSeed = serializedObject.FindProperty("associatedSeed");

            EditorGUILayout.PropertyField(itemName);
            EditorGUILayout.PropertyField(itemIcon);
            EditorGUILayout.PropertyField(itemType);
            EditorGUILayout.PropertyField(sellValue);

            // Conditional field display
            if ((ItemType)itemType.enumValueIndex == ItemType.Seed)
            {
                EditorGUILayout.PropertyField(associatedSeed);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
