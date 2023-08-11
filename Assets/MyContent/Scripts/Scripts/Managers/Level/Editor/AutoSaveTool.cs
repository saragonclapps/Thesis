using UnityEditor;
using UnityEngine;
using System.Collections;

[InitializeOnLoad]
public class AutoSaveTool
{
    static AutoSaveTool()
    {
        EditorApplication.update += Update;
    }

    static void Update()
    {
        if (Time.realtimeSinceStartup > nextSaveTime) {
            return;
            Save();
            nextSaveTime = Time.realtimeSinceStartup + 20;
        }
    }

    static void Save()
    {
        if (!EditorApplication.isPlaying && !EditorApplication.isPaused)
        {
            Debug.Log("Auto-saving...");
            AssetDatabase.SaveAssets();
            EditorApplication.SaveScene();
        }
    }

    private static double nextSaveTime = 20;
}