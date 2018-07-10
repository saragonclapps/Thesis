using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndPanel : MonoBehaviour {

    float curedLerpValue = 0;
    List<Material> breathingMaterials;
    public float activeDistance;

    public Animator cpuCap;
    public Animator cpuDrive;
    public Vector3 offset;

    public string cutSceneTag;
    bool _cutSceneStart;

    void Start()
    {
        EventManager.AddEventListener(GameEvent.SAVEDISK_ENTER, SaveDiskEnter);
    }

    void OnTriggerStay(Collider other)
    {
        var dist = Vector3.Distance(transform.position + offset, other.transform.position) < activeDistance;
        var saveDisk = HUDManager.instance.saveDisk.enabled;
        var layer = other.gameObject.layer;

        if (layer == 9)
        {
            if (saveDisk && dist && !_cutSceneStart)
            {
                EventManager.DispatchEvent(GameEvent.CAMERA_STORY, cutSceneTag);
                _cutSceneStart = true;
            }
            cpuCap.SetBool("isNear", true);
            cpuDrive.SetBool("isNear", true);
        }
    }

    private void SaveDiskEnter(object[] parameterContainer)
    {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
        EventManager.RemoveEventListener(GameEvent.SAVEDISK_ENTER, SaveDiskEnter);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            cpuCap.SetBool("isNear", false);
            cpuDrive.SetBool("isNear", false);
        }
    }

    void Execute()
    {
        curedLerpValue = Mathf.Lerp(curedLerpValue, 1, Time.deltaTime * 0.5f);
        foreach (var mat in LevelManager.instance.breathingScenarioMaterials)
        {
            mat.SetFloat("CuredLerp", curedLerpValue);
        }
        
        if(curedLerpValue >= 0.9f)
        {
            LevelManager.instance.whiteOutAnimator.SetTrigger("WhiteOut");
            UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + offset, activeDistance);
    }

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }

}
