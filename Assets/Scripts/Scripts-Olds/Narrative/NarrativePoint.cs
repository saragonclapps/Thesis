using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativePoint : MonoBehaviour, IObserver {

    public float fireScene = 1;
    private bool isActive = false;
    public GameObject[] activeEventObjects;
    public GameObject[] disableEventObjects;

    private void Start()
    {
        //GameInput.instance.ChangeLockFeature(GameInput.Features.STEALTH, true);
        //GameInput.instance.ChangeLockFeature(GameInput.Features.JUMP, true);
        //GameInput.instance.ChangeLockFeature(GameInput.Features.SKILL_CHANGE, true);


        //GameInput.instance.ChangeLockFeature(Unlock, true);
        PanelManager.instance.Subscribe(OnNotify);
    }

    private void OnTriggerStay(Collider c)
    {
        if (!isActive && c.gameObject.layer == 9 && !PanelManager.instance.isActivePanelNarration)
        {
            isActive = true;
            PanelManager.instance.FireSequenceScene(fireScene);
        }
    }

    public void OnNotify()
    {
        if (isActive)
        {
            foreach (var item in activeEventObjects)
                item.SetActive(true);
            foreach (var item in disableEventObjects)
                item.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawSphere(Vector3.zero, 5);
        Gizmos.DrawIcon(transform.position, "NarrativePoint.png", true);
    }
}
