using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarnBehaviour : MonoBehaviour {


    public Transform player;
    public int dardoCount;

    public float timmer;
    private float _tick;
	void Start ()
    {
        _tick = 0;
        for (int i = 0; i < dardoCount; i++)
        {
            SpawnDardo();
        }

        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
    }

    void Execute()
    {
        if(transform.childCount < dardoCount)
        {
            if (_tick > timmer)
            {
                
                SpawnDardo();
                _tick = 0;
            }
            else _tick += Time.deltaTime;
        }
    }

    void SpawnDardo()
    {
        var pos = new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y, transform.position.z + Random.Range(-5, 5));
        EventManager.DispatchEvent(GameEvent.DARDO_SPAWN, pos, this.transform);
    }

    void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, 3);
    }
}
