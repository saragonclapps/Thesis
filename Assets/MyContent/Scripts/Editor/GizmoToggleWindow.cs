using UnityEngine;
using UnityEditor;

public class GizmoToggleWindow : EditorWindow {
    // Define an array of string that represent your class names
    public string[] classNames = { "TutorialTrigger", "CheckPoint", "MediumSizeObject" };

    // This will track the active state of each class' gizmos
    private bool[] classActiveStates;

    [MenuItem("Window/Custom/Gizmo Toggle Window")]
    public static void ShowWindow() {
        GetWindow<GizmoToggleWindow>("Gizmo Toggle Window");
    }

    private void OnEnable() {
        // Initialize the active states array
        classActiveStates = new bool[classNames.Length];
        for (int i = 0; i < classNames.Length; i++) {
            classActiveStates[i] = true; // By default, assume gizmos are on
        }
    }

    private void OnGUI() {
        for (var i = 0; i < classNames.Length; i++) {
            var newState = EditorGUILayout.Toggle(classNames[i], classActiveStates[i]);
            if (newState == classActiveStates[i]) continue;
            ToggleGizmosForClass(classNames[i], newState);
            classActiveStates[i] = newState;
        }
    }

    private static void ToggleGizmosForClass(string className, bool newState) {
        switch (className) {
            case "TutorialTrigger":
                TutorialTrigger.drawGizmos = newState;
                break;
            case "CheckPoint":
                CheckPoint.drawGizmos = newState;
                break;
            case "MediumSizeObject":
                MediumSizeObject.drawGizmos = newState;
                break;
        }
    }
}