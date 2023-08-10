using UnityEngine;
using UnityEditor;

public class GridGeneratorWindow : EditorWindow {
    private GameObject modulePrefab;
    private int countX = 1;
    private int countZ = 1;

    [MenuItem("Window/Custom/Grid Generator")]
    public static void ShowWindow() {
        GetWindow<GridGeneratorWindow>("Grid Generator");
    }

    private void OnGUI() {
        GUILayout.Label("Settings", EditorStyles.boldLabel);

        modulePrefab =
            (GameObject)EditorGUILayout.ObjectField("Module Prefab", modulePrefab, typeof(GameObject), false);
        countX = EditorGUILayout.IntField("Count X", countX);
        countZ = EditorGUILayout.IntField("Count Z", countZ);

        if (GUILayout.Button("Generate Grid")) {
            GenerateGrid();
        }
    }

    private void GenerateGrid()
    {
        if (modulePrefab == null)
        {
            Debug.LogError("Module prefab is not set.");
            return;
        }

        Collider moduleCollider = modulePrefab.GetComponentInChildren<Collider>();
        if (moduleCollider == null)
        {
            Debug.LogError("The module prefab and its children do not have a collider.");
            return;
        }

        GameObject parentObject = new GameObject("GridParent");

        Vector3 moduleSize = moduleCollider.bounds.size;

        // El punto de inicio se basa en la cantidad total y el tamaño de los módulos
        Vector3 startPosition = new Vector3(-moduleSize.x * countX / 2.0f + moduleSize.x / 2.0f, 0, -moduleSize.z * countZ / 2.0f + moduleSize.z / 2.0f);

        for (int x = 0; x < countX; x++)
        {
            for (int z = 0; z < countZ; z++)
            {
                // Se calcula la posición usando el índice y el tamaño del módulo
                Vector3 position = startPosition + new Vector3(x * moduleSize.x, 0, z * moduleSize.z);
                GameObject newModule = Instantiate(modulePrefab, position, Quaternion.identity, parentObject.transform);
                newModule.name = modulePrefab.name + "_" + x + "_" + z;
            }
        }

        parentObject.transform.position = new Vector3(startPosition.x + moduleSize.x * (countX - 1) / 2.0f, 0, startPosition.z + moduleSize.z * (countZ - 1) / 2.0f);
    }
}