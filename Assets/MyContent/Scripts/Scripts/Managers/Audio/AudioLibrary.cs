using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "ScriptableObjects/Audio Library", order = 0)]
public class AudioLibrary : ScriptableObject
{
    public Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    public void AddClip(string key, AudioClip clip)
    {
        if (!audioClips.ContainsKey(key))
        {
            audioClips.Add(key, clip);
        }
    }

    public AudioClip GetClip(string key) {
        return audioClips.TryGetValue(key, out var clip) ? clip : null;
    }
}