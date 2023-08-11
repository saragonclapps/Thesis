using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SelectObjectsByMaterial : EditorWindow
{
    Material targetMaterial;

    [MenuItem("Tools/Select Objects By Material")]
    public static void ShowWindow()
    {
        GetWindow<SelectObjectsByMaterial>("Select Objects By Material");
    }

    void OnGUI()
    {
        GUILayout.Label("Select objects by their material", EditorStyles.boldLabel);

        targetMaterial = (Material)EditorGUILayout.ObjectField("Target Material", targetMaterial, typeof(Material), false);

        if (GUILayout.Button("Select Objects"))
        {
            SelectObjectsWithMaterial();
        }
    }

    void SelectObjectsWithMaterial()
    {
        if (targetMaterial == null)
        {
            Debug.LogWarning("No material selected!");
            return;
        }

        List<GameObject> objectsWithMaterial = new List<GameObject>();

        Renderer[] renderers = FindObjectsOfType<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (ArrayContains(renderer.sharedMaterials, targetMaterial))
            {
                objectsWithMaterial.Add(renderer.gameObject);
            }
        }

        Selection.objects = objectsWithMaterial.ToArray();
    }

    bool ArrayContains(Material[] materials, Material target)
    {
        foreach (Material mat in materials)
        {
            if (mat == target)
            {
                return true;
            }
        }
        return false;
    }
}