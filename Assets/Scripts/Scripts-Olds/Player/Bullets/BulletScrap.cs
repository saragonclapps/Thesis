using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScrap : MonoBehaviour {

    public float speed;
    private float _timmer = 2f;
    private float _tick;

    void Execute()
    {
        if (_tick <= _timmer)
        {
            _tick += Time.deltaTime;
            transform.position += transform.forward * speed * Time.deltaTime;
            speed *= 0.99f;
        }
        else
        {
            EventManager.DispatchEvent(GameEvent.BULLET_SCRAP_DESTROY, this);
        }
 
    }

    #region pool statics
    public static void InitializeScrapBullet(BulletScrap bulletObj)
    {
        bulletObj.gameObject.SetActive(true);
        bulletObj._tick = 0;
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, bulletObj.Execute);
    }

    public static void DestroyScrapBullet(BulletScrap bulletObj)
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, bulletObj.Execute);
        bulletObj.gameObject.SetActive(false);
    }
    #endregion

    private void OnCollisionEnter(Collision c)
    {
        EventManager.DispatchEvent(GameEvent.BULLET_SCRAP_DESTROY, this);
    }

    private void OnTriggerEnter(Collider c)
    {

        EventManager.DispatchEvent(GameEvent.BULLET_SCRAP_DESTROY, this);

    }
}
