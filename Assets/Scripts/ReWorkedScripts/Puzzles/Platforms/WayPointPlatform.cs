using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointPlatform : Platform {

    public Waypoint wp;

    [Header("Time to reach next Waypoint")]
    public float period;
    [Header("Time")]
    public float stillTime;
    [Header("Motion Curve")]
    public AnimationCurve motionCurve;

    float _curveTick;
    float _stillTimmer;

    Vector3 dir;

    bool reverse;
    float initialDistance;

	void Start ()
    {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
        Restart();
        
	}

    void Execute()
    {
        if (isActive)
        {
            var actualDistance = Mathf.Abs((wp.transform.position - transform.position).magnitude);
            var speed = motionCurve.Evaluate(_curveTick / period * 2) < 0.01f ? 0.01f: motionCurve.Evaluate(_curveTick / period * 2);

            if (actualDistance < 0.2f)
            {
                speed = 0;

                if(_stillTimmer < stillTime)
                {
                    _stillTimmer += Time.deltaTime;
                }
                else
                {
                    SetNextWaypoint();
                }
            }else if( actualDistance < initialDistance / 2)
            {

                _curveTick -= Time.deltaTime;
            }
            else
            {
                _curveTick += Time.deltaTime;
            }
            transform.position += dir * speed;
        }
    }

    private void SetNextWaypoint()
    {
        if (!reverse)
        {
            if (wp.next != null)
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
        Restart();
    }

    private void Restart()
    {
        dir = (wp.transform.position - transform.position).normalized;
        initialDistance = Mathf.Abs((wp.transform.position - transform.position).magnitude);
        _stillTimmer = 0;
        _curveTick = 0;
    }

    void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}
