using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTutorialInputPoint : MonoBehaviour {

    public int fireEvent = 1;
    public float time = 5;
    public GameInput.Features Unlock;

    private void OnTriggerStay(Collider c)
    {
        if (c.gameObject.layer == 9 && !PanelManager.instance.isActivePanelInput)
        {
            GameInput.instance.ChangeLockFeature(Unlock, true);
            PanelManager.instance.FireSequenceInput(fireEvent, time);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
        Gizmos.DrawSphere(Vector3.zero, 5);
        Gizmos.DrawIcon(transform.position, "Input.png", true);
    }
}
