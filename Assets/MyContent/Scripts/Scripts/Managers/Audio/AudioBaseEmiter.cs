using UnityEngine;

public abstract class AudioBaseEmiter : MonoBehaviour {
    protected bool _mute;

    public abstract void SetMute(bool mute);
    protected static AudioClip RandomSoundFromArray(AudioClip[] array) {
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, array.Length);
        var toPlay = array[n];
        // move picked sound to index 0 so it's not picked next time
        array[n] = array[0];
        array[0] = toPlay;
        return toPlay;
    }
}
