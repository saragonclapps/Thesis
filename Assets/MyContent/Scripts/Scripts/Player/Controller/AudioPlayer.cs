using UnityEngine;


public class AudioPlayer: MonoBehaviour {
                
    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] 
    private AudioClip[] _footsFepSounds;
        
    [SerializeField]
    private LandChecker _lC;
    
    AudioSource _stepsSources;
        
    private void PlayFootStepAudio()
    {
        if (!_lC.land)
        {
            return;
        }
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, _footsFepSounds.Length);
        var toPlay = _footsFepSounds[n];
        AudioManager.instance.PlayAudio(toPlay);
        // move picked sound to index 0 so it's not picked next time
        _footsFepSounds[n] = _footsFepSounds[0];
        _footsFepSounds[0] = toPlay;
    }
}
