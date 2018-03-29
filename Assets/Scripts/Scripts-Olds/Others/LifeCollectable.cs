using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCollectable : MonoBehaviour {

    public float healAmount;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            EventManager.DispatchEvent(GameEvent.PLAYER_TAKE_DAMAGE, -healAmount);
            Destroy(gameObject);
        }
    }
}
