using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

[CustomEditor(typeof(TutorialSetup))]
public class TutorialSetupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var tutorialSetup = (TutorialSetup)target;
        var tutorialsProperty = serializedObject.FindProperty("tutorials");
        serializedObject.Update();

        EditorGUILayout.LabelField("Tutorials", EditorStyles.boldLabel);

        for (var i = 0; i < tutorialsProperty.arraySize; i++)
        {
            var tutorialEntry = tutorialsProperty.GetArrayElementAtIndex(i);
            var key = tutorialEntry.FindPropertyRelative("key");
            var value = tutorialEntry.FindPropertyRelative("value");

            EditorGUILayout.BeginHorizontal();
            key.stringValue = EditorGUILayout.TextField(key.stringValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical();
            value.FindPropertyRelative("type").enumValueIndex = (int)(TutorialSetupEntryDataType)EditorGUILayout.EnumPopup("Tutorial Type", (TutorialSetupEntryDataType)value.FindPropertyRelative("type").enumValueIndex);
            if (value.FindPropertyRelative("type").enumValueIndex == (int)TutorialSetupEntryDataType.BUTTON) {
                value.FindPropertyRelative("text").stringValue = EditorGUILayout.TextField("Text", value.FindPropertyRelative("text").stringValue);
                value.FindPropertyRelative("soundKey").stringValue = EditorGUILayout.TextField("Sound Key", value.FindPropertyRelative("soundKey").stringValue);
                value.FindPropertyRelative("button").objectReferenceValue = EditorGUILayout.ObjectField("Button", value.FindPropertyRelative("button").objectReferenceValue, typeof(Sprite), false);
            }
            else if (value.FindPropertyRelative("type").enumValueIndex == (int)TutorialSetupEntryDataType.CAMERA_ANIMATION) {
                value.FindPropertyRelative("animator").objectReferenceValue = EditorGUILayout.ObjectField("animator", value.FindPropertyRelative("animator").objectReferenceValue, typeof(Animator), true);
            }
           
            EditorGUILayout.EndVertical();
            EditorGUILayout.Separator();
            EditorGUILayout.Space();
        }
        if (HasDuplicateKeys(tutorialSetup.tutorials))
        {
            EditorGUILayout.HelpBox("Duplicate keys found. The last duplicate item will be removed. Duplicate keys found. The last duplicate item will be removed.", MessageType.Warning);
        }
        
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add element"))
        {
            tutorialSetup.tutorials.Add(new TutorialSetupEntry());
        }

        if (GUILayout.Button("Delete element") && tutorialSetup.tutorials.Count > 0)
        {
            tutorialSetup.tutorials.RemoveAt(tutorialSetup.tutorials.Count - 1);
        }

        if (GUILayout.Button("Clear list"))
        {
            tutorialSetup.tutorials.Clear();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        serializedObject.ApplyModifiedProperties();
    }
    
    
    
    private static bool HasDuplicateKeys(IEnumerable<TutorialSetupEntry> tutorials)
    {
        return tutorials
            .GroupBy(x => x.key)
            .Any(g => g.Count() > 1);
    }
}