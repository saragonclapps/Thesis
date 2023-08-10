using UnityEngine;
using UnityEditor;
using Debug = Logger.Debug;

public class SnapshotsPlayerWindow : EditorWindow {
    private const float COLUMNS_NUMBER = 2;
    private const float WIDTH_COLUMNS = 1 / COLUMNS_NUMBER;
    private GameObject snapshots;

    [MenuItem("Window/Custom/Snapshots Player")]
    public static void ShowWindow() {
        GetWindow<SnapshotsPlayerWindow>("Snapshots Player");
    }

    private void OnGUI() {
        // Ensure the game is not running
        if (EditorApplication.isPlaying) {
            EditorGUILayout.HelpBox("Please stop the game to use this functionality.", MessageType.Warning);
            return; // Exit the OnGUI
        }

        snapshots = GameObject.Find("Snapshots");
        var player = GameObject.Find("Player");

        if (snapshots == null) {
            EditorGUILayout.HelpBox("There's no object named 'Snapshots' in the scene.", MessageType.Error);
            return;
        }

        if (player == null) {
            EditorGUILayout.HelpBox("There's no object named 'Player' in the scene.", MessageType.Error);
            return;
        }

        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < snapshots.transform.childCount; i++) {
            if (i % COLUMNS_NUMBER == 0 && i != 0) {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
            }

            var child = snapshots.transform.GetChild(i);
            var paintingGreen = child.transform.position == player.transform.position;
            GUI.backgroundColor = paintingGreen ? Color.green : Color.white;
            if (GUILayout.Button(child.name, GUILayout.Width(EditorGUIUtility.currentViewWidth * WIDTH_COLUMNS))) {
                MovePlayerToSnapshot(player.transform, child);
            }
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        GUI.backgroundColor = Color.white;

        if (GUILayout.Button("Refresh Scene List")) {
            RefreshSceneList();
        }
    }

    private void RefreshSceneList() {
        snapshots = GameObject.Find("Snapshots");
    }

    private static void MovePlayerToSnapshot(Transform player, Transform snapshot) {
        player.position = snapshot.position;
        player.rotation = snapshot.rotation;

        // Calculate desired position for the scene camera
        if (SceneView.lastActiveSceneView != null) {
            Vector3 desiredPosition = snapshot.position + SceneView.lastActiveSceneView.rotation * Vector3.back * 3.0f;
            Vector3 directionToDesired = (desiredPosition - snapshot.position).normalized;
            desiredPosition = snapshot.position + directionToDesired;

            // Set the scene camera's position
            SceneView.lastActiveSceneView.camera.transform.position = desiredPosition;
            SceneView.lastActiveSceneView.camera.transform.LookAt(snapshot.position);
        }

        // Select the player in the hierarchy
        Selection.activeGameObject = snapshot.gameObject;

        // Focus on the selected object in the scene view
        if (SceneView.lastActiveSceneView != null) {
            SceneView.lastActiveSceneView.FrameSelected();
        }
    }
}