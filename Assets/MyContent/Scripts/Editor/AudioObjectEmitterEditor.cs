using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioObjectEmitter))]
public class AudioObjectEmitterEditor : Editor 
{
    public override void OnInspectorGUI() 
    {
        // Draw default inspector
        DrawDefaultInspector();

        AudioObjectEmitter audioObjectEmitter = (AudioObjectEmitter)target;

        // If there's an AudioClipData assigned
        if (audioObjectEmitter.audioClipDataHits != null) 
        {
            bool clipsChanged = false;

            // Display all AudioClips in AudioClipData
            for (int i = 0; i < audioObjectEmitter.audioClipDataHits.clips.Length; i++) 
            {
                AudioClip originalClip = audioObjectEmitter.audioClipDataHits.clips[i];
                audioObjectEmitter.audioClipDataHits.clips[i] = (AudioClip)EditorGUILayout.ObjectField("Audio Clip " + (i + 1), audioObjectEmitter.audioClipDataHits.clips[i], typeof(AudioClip), false);

                if (originalClip != audioObjectEmitter.audioClipDataHits.clips[i])
                    clipsChanged = true;
            }

            if (GUILayout.Button("Add Audio Clip")) 
            {
                System.Array.Resize(ref audioObjectEmitter.audioClipDataHits.clips, audioObjectEmitter.audioClipDataHits.clips.Length + 1);
                clipsChanged = true;
            }

            if (audioObjectEmitter.audioClipDataHits.clips.Length > 0 && GUILayout.Button("Remove Last Audio Clip")) 
            {
                System.Array.Resize(ref audioObjectEmitter.audioClipDataHits.clips, audioObjectEmitter.audioClipDataHits.clips.Length - 1);
                clipsChanged = true;
            }

            if (clipsChanged)
            {
                EditorUtility.SetDirty(audioObjectEmitter.audioClipDataHits);
            }
        }
    }
}