using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioLibrary audioLibrary;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(string key)
    {
        var clip = audioLibrary.GetClip(key);
        if (clip == null) return;
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
