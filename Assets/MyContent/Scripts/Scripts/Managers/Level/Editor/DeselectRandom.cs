using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DeselectRandom : EditorWindow
{
    int divisor = 1;

    [MenuItem("Tools/Deselect Random Objects")]
    public static void ShowWindow()
    {
        GetWindow<DeselectRandom>("Deselect Random Objects");
    }

    void OnGUI()
    {
        GUILayout.Label("Deselect random objects based on divisor", EditorStyles.boldLabel);

        divisor = EditorGUILayout.IntField("Divisor", divisor);

        if (GUILayout.Button("Deselect Random"))
        {
            DeselectRandomObjects();
        }
    }

    void DeselectRandomObjects()
    {
        if (divisor <= 0)
        {
            Debug.LogWarning("Divisor is zero or negative, no objects will be deselected.");
            return;
        }

        List<GameObject> selectedObjects = new List<GameObject>(Selection.gameObjects);
        List<GameObject> newSelection = new List<GameObject>(selectedObjects);

        int objectsToDeselect = selectedObjects.Count / divisor;

        for (int i = 0; i < objectsToDeselect; i++)
        {
            GameObject randomObject = selectedObjects[Random.Range(0, selectedObjects.Count)];
            newSelection.Remove(randomObject);
            selectedObjects.Remove(randomObject);
        }

        Selection.objects = newSelection.ToArray();
    }
}