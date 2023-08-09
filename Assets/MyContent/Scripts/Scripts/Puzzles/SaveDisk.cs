using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SaveDisk : MonoBehaviour {
    public Vector3 rotation;
    public ParticleSystem aura;
    private Material _material;
    public bool isDissolving;
    private float _dissolveLerp;


    private void Start() {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
        _material = GetComponent<MeshRenderer>().material;
    }

    private void Execute() {
        transform.Rotate(rotation * Time.deltaTime);
        if (!isDissolving) return;
        _dissolveLerp += Time.deltaTime;
        _material.SetFloat("_Cutoff", _dissolveLerp);
        if (!(_dissolveLerp >= 1)) return;
        EventManager.DispatchEvent(GameEvent.SAVE_DISK_COLLECTED);
        AudioManager.instance.PlayAudio("GetKey", AudioMode.OneShot, AudioGroup.SFX_COLLECTABLES);
        Destroy(gameObject);
        aura.Stop();
        HUDManager.instance.saveDisk.enabled = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 9) {
            isDissolving = true;
        }
    }

    private void OnDestroy() {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}