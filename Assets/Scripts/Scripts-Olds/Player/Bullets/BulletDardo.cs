using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDardo : MonoBehaviour {

    public float speed;
    private float _timmer = 2f;
    private float _tick;

    private bool active;

    void Execute () {
        if (_tick <= _timmer)
        {
            _tick += Time.deltaTime;
            transform.position += transform.forward * speed * Time.deltaTime;
            speed *= 0.99f;
        }
        else if (active)
        {
            var rb = GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.velocity = transform.forward * speed;
            active = false;
            //var col = GetComponent<CapsuleCollider>();
            //col.radius = 0.12f;
            //col.center = new Vector3(0, 0.05f, 0.1f);
        }
    }

    #region pool statics
    public static void InitializeDardoBullet(BulletDardo bulletObj)
    {
        bulletObj.gameObject.SetActive(true);
        bulletObj.active = true;
        bulletObj._tick = 0;
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, bulletObj.Execute);
    }

    public static void DestroyDardoBullet(BulletDardo bulletObj)
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, bulletObj.Execute);
        var rb = bulletObj.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;

        var col = bulletObj.GetComponent<CapsuleCollider>();
        col.radius = 0.34f;
        col.center = new Vector3(0, 0, 0.1f);

        bulletObj.gameObject.SetActive(false);
    }
    #endregion

    private void OnCollisionEnter(Collision c)
    {
        if(c.collider.gameObject.layer == 11)
        {
            EventManager.DispatchEvent(GameEvent.BULLET_DARDO_DESTROY, this);
            EventManager.DispatchEvent(GameEvent.ESCUPITAJO_RECIEVE_DAMAGE, c.gameObject, transform);
        }
        else
        {
            var rb = GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;
            active = false;
            _tick = _timmer;
            var col = GetComponent<CapsuleCollider>();
            col.radius = 0.12f;
            col.center = new Vector3(0, 0.05f, 0.1f);

        }
    }
    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 11)
        {
            EventManager.DispatchEvent(GameEvent.BULLET_DARDO_DESTROY, this);
            EventManager.DispatchEvent(GameEvent.ESCUPITAJO_RECIEVE_DAMAGE, c.gameObject, transform);
        }
        else
        {
            var rb = GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;
            active = false;
            _tick = _timmer;
            var col = GetComponent<CapsuleCollider>();
            col.radius = 0.12f;
            col.center = new Vector3(0, 0.05f, 0.1f);

        }
    }

}
