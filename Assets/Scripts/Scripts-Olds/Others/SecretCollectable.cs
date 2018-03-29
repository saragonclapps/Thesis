using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretCollectable : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            EventManager.DispatchEvent(GameEvent.SECRET_COLLECTED);
            Destroy(gameObject);
        }
    }
}
