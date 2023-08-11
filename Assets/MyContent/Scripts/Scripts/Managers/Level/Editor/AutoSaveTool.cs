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
            Save();
            nextSaveTime = Time.realtimeSinceStartup + 60;
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