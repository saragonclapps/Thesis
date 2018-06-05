using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFixTrigger : MonoBehaviour {

    public float distance;
    public float targetX;
    public float targetY;

    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.layer == 9)
        {
            EventManager.DispatchEvent(GameEvent.CAMERA_FIXPOS, targetX, targetY ,distance);
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.layer == 9)
        {
            EventManager.DispatchEvent(GameEvent.CAMERA_NORMAL);
        }
    }
}
