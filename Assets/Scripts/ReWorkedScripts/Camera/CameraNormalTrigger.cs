using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNormalTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 9)
        {
            EventManager.DispatchEvent(GameEvent.CAMERA_NORMAL);
        }
    }
}
