using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioLibrary))]
public class AudioLibraryEditor : Editor
{
    private string _newKey = "";
    private AudioClip _newClip;

    public override void OnInspectorGUI()
    {
        var audioLibrary = (AudioLibrary)target;

        // Draw default inspector
        DrawDefaultInspector();

        EditorGUILayout.Space();

        // Add new audio clip
        EditorGUILayout.LabelField("Add new audio clip", EditorStyles.boldLabel);
        _newKey = EditorGUILayout.TextField("Key", _newKey);
        _newClip = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", _newClip, typeof(AudioClip), false);

        if (GUILayout.Button("Add Audio Clip"))
        {
            if (!string.IsNullOrEmpty(_newKey) && _newClip != null)
            {
                audioLibrary.AddClip(_newKey, _newClip);
                EditorUtility.SetDirty(audioLibrary);
                _newKey = "";
                _newClip = null;
            }
        }

        // Display existing audio clips
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Audio Clips", EditorStyles.boldLabel);
        foreach (var entry in audioLibrary.audioClips)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(entry.Key, GUILayout.MaxWidth(100));
            EditorGUILayout.ObjectField(entry.Value, typeof(AudioClip), false);
            EditorGUILayout.EndHorizontal();
        }
    }
}