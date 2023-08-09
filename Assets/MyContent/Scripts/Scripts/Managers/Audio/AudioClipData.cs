using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipData", menuName = "AudioClipData", order = 1)]
public class AudioClipData : ScriptableObject {
    public AudioClip[] clips;
}