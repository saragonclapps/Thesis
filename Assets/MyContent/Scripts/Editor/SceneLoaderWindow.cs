using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public class SceneLoaderWindow : EditorWindow
{
    private const string FOLDER_PATH = "Assets/MyContent/Scenes/Selection";

    private string[] _scenePaths = new string[] {
        "Assets/MyContent/Scenes/Execution/Managers.unity",
        "Assets/MyContent/Scenes/Execution/Player.unity",
        "Assets/MyContent/Scenes/Execution/UI.unity"
    };

    [MenuItem("Window/Scene Loader")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SceneLoaderWindow), false, "Scene Loader");
    }

    private void OnEnable()
    {
        RefreshSceneList();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Scenes in folder: " + FOLDER_PATH, EditorStyles.boldLabel);

        if (_scenePaths != null)
        {
            foreach (var scenePath in _scenePaths)
            {
                var sceneName = Path.GetFileNameWithoutExtension(scenePath);
                if (!GUILayout.Button(sceneName)) continue;
                EditorSceneManager.OpenScene(scenePath);

                foreach (var scenePathOpenFirst in _scenePaths) {
                    SceneManager.LoadScene(scenePathOpenFirst, LoadSceneMode.Additive);
                }
            }
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Refresh Scene List"))
        {
            RefreshSceneList();
        }
    }

    private void RefreshSceneList()
    {
        var guids = AssetDatabase.FindAssets("t:Scene", new[] { FOLDER_PATH });
        _scenePaths = guids.Select(AssetDatabase.GUIDToAssetPath).ToArray();
    }
}