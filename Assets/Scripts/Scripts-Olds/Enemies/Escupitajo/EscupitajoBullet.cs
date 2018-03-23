using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscupitajoBullet : MonoBehaviour {

    public int playerLayer;

    void Execute () {
	    
	}

    public static void InitializeEscupitajoBullet(EscupitajoBullet bulletObj)
    {
        bulletObj.gameObject.SetActive(true);
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, bulletObj.Execute);
    }

    public static void DestroyEscupitajoBullet(EscupitajoBullet bulletObj)
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, bulletObj.Execute);
        bulletObj.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == playerLayer)
        {
            EventManager.DispatchEvent(GameEvent.PLAYER_TAKE_DAMAGE, 100f);
        }
        EventManager.DispatchEvent(GameEvent.ESCUPITAJO_BULLET_DESTROY, gameObject);

    }
}
