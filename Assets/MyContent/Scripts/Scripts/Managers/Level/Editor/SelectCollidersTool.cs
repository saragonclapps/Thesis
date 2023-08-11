using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SelectCollidersTool
{
    [MenuItem("Tools/Select Colliders")]
    public static void SelectColliders()
    {
        // Recopila todos los objetos seleccionados actualmente
        GameObject[] selectedObjects = Selection.gameObjects;

        // Lista para almacenar los objetos que coincidan con el criterio
        List<GameObject> matchingObjects = new List<GameObject>();

        // Examina cada objeto seleccionado y sus hijos recursivamente
        foreach (GameObject obj in selectedObjects)
        {
            FindMatchingObjects(obj, matchingObjects);
        }

        // Cambia la selección para que solo incluya los objetos que coinciden
        Selection.objects = matchingObjects.ToArray();
    }

    // Función recursiva para buscar objetos que coincidan en un objeto y sus hijos
    private static void FindMatchingObjects(GameObject obj, List<GameObject> matchingObjects)
    {
        // Compara el nombre del objeto ignorando la sensibilidad de mayúsculas/minúsculas
        if (obj.name.Equals("collider", System.StringComparison.OrdinalIgnoreCase))
        {
            matchingObjects.Add(obj);
        }

        // Recorre y busca en todos los hijos del objeto actual
        foreach (Transform child in obj.transform)
        {
            FindMatchingObjects(child.gameObject, matchingObjects);
        }
    }
}