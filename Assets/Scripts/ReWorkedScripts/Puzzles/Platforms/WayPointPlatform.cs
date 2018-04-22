using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointPlatform : Platform {

    public Waypoint wp;

    [Header("Lerp T")]
    public float t;

    bool reverse;

	void Start ()
    {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
	}
	
	void Execute ()
    {
        if (isActive)
        {
            transform.position = Vector3.Lerp(transform.position, wp.transform.position, t);
            if (Vector3.Distance(transform.position, wp.transform.position) < 0.2f)
            {
                if (!reverse)
                {
                    if(wp.next != null)
                        wp = wp.next;
                    else
                    {
                        reverse = true;
                    }
                    

                }
                else
                {
                    if (wp.last != null)
                    {
                        wp = wp.last;
                    }
                    else
                    {
                        reverse = false;
                    }
                }
            }
        }
	}

    void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}
