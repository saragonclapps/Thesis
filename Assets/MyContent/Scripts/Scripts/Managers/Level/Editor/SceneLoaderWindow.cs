using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public class SceneLoaderWindow : EditorWindow {
    private const float COLUMNS_NUMBER = 2;
    private const float WIDTH_COLUMNS = 1 / COLUMNS_NUMBER;
    private const string FOLDER_PATH = "Assets/MyContent/Scenes/Selection";

    private string[] _sceneExecution = new string[] {
        "Assets/MyContent/Scenes/Execution/Managers.unity",
        "Assets/MyContent/Scenes/Execution/Player.unity",
        "Assets/MyContent/Scenes/Execution/UI.unity"
    };

    private string[] _scenePaths;

    [MenuItem("Window/Scene Loader")]
    public static void ShowWindow() {
        GetWindow(typeof(SceneLoaderWindow), false, "Scene Loader");
    }

    private void OnEnable() {
        RefreshSceneList();
    }

    private void OnGUI() {
        EditorGUILayout.LabelField("Scenes in folder: " + FOLDER_PATH, EditorStyles.boldLabel);
        var originalColor = GUI.backgroundColor;
        if (_scenePaths != null) {
            EditorGUILayout.BeginHorizontal();
            for (var index = 0; index < _scenePaths.Length; index++) {
                if (index % COLUMNS_NUMBER == 0 && index != 0) {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }

                var scenePath = _scenePaths[index];
                var sceneName = Path.GetFileNameWithoutExtension(scenePath);
                var paintingGreen =
                    scenePath.IndexOf(EditorSceneManager.GetActiveScene().name, StringComparison.Ordinal) != -1;
                GUI.backgroundColor = paintingGreen ? Color.green : Color.white;
                if (!GUILayout.Button(sceneName, GUILayout.Width(EditorGUIUtility.currentViewWidth * WIDTH_COLUMNS))) continue;
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                // EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                // foreach (var scenePathOpenFirst in _sceneExecution) {
                //     EditorSceneManager.OpenScene(scenePathOpenFirst, OpenSceneMode.Additive);
                // }
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Refresh Scene List")) {
            RefreshSceneList();
        }
    }

    private void RefreshSceneList() {
        var guids = AssetDatabase.FindAssets("t:Scene", new[] { FOLDER_PATH });
        _scenePaths = guids.Select(AssetDatabase.GUIDToAssetPath).ToArray();
    }
}