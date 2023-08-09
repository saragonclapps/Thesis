using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDiskEnd : MonoBehaviour {
    private bool _isActive = true;
    private float disolveLerp = 1;
    private Material _mat;
    private Animator _anim;
    public float disolveSpeed;


    private void Start() {
        _mat = GetComponent<MeshRenderer>().material;
        _mat.SetFloat("_DisolveAmount", disolveLerp);

        _anim = GetComponent<Animator>();
        EventManager.AddEventListener(GameEvent.SAVE_DISK_END, OnFinalSceneStart);
    }

    private void OnFinalSceneStart(object[] parameterContainer) {
        EventManager.RemoveEventListener(GameEvent.SAVE_DISK_END, OnFinalSceneStart);
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
        HUDManager.instance.saveDisk.enabled = false;
    }


    private void Execute() {
        if (disolveLerp > 0) {
            disolveLerp -= Time.deltaTime * disolveSpeed;
            _mat.SetFloat("_DisolveAmount", disolveLerp);
        }
        else {
            if (_anim == null) _anim = GetComponent<Animator>();
            _anim.SetTrigger("EnterDrive");
            UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
        }
    }

    public void OnEnterDrive() {
        EventManager.DispatchEvent(GameEvent.SAVE_DISK_ENTER);
        AudioManager.instance.PlayAudio("OnFinished", AudioMode.OneShot, AudioGroup.SFX_PUZZLES);
        _isActive = false;
    }

    private void OnDestroy() {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
        EventManager.RemoveEventListener(GameEvent.CAMERA_STORY, OnFinalSceneStart);
        EventManager.RemoveEventListener(GameEvent.SAVE_DISK_END, OnFinalSceneStart);
    }
}