using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFallTrigger : MonoBehaviour {
    private const float TIMMER = 3;
    private float _currentTick;
    
    public string cutSceneTag;
    public Animator blackOut;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer != 9) return;
        _currentTick = 0;
        AudioPlayerEmitter.instance.SetMute(true);
        EventManager.DispatchEvent(GameEvent.CAMERA_STORY, cutSceneTag);
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
    }

    private void Execute() {
        _currentTick += Time.deltaTime;
        if (!(_currentTick > TIMMER)) return;
        blackOut.SetTrigger("FadeOutLose");
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }

    private void OnDestroy() {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}