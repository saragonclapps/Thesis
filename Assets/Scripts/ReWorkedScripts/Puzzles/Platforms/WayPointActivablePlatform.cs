using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointActivablePlatform : Platform {

    public Waypoint activeWaypoint;
    public Waypoint pasiveWaypoint;

    Waypoint targetWaypoint;

    public AnimationCurve motionCurve;

    public float period;
    float _curveTick;

    public new bool isActive
    {
        get
        {
            return _isActive;
        }
        set
        {
            _isActive = value;
            if (_isActive)
            {
                targetWaypoint = activeWaypoint;
            }
            else
            {
                targetWaypoint = pasiveWaypoint;
            }
        }
    }

    void Start ()
    {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
        isActive = false;
	}

    void Execute()
    {
        var actualDistance = Mathf.Abs((targetWaypoint.transform.position - transform.position).magnitude);
        var dir = (targetWaypoint.transform.position - transform.position).normalized;
        var speed = motionCurve.Evaluate(_curveTick / period * 2) < 0.01f ? 0.01f: motionCurve.Evaluate(_curveTick / period * 2);

        if (actualDistance < 0.2f)
        {
            speed = 0;
            _curveTick = 0;
        }
        else
        {
            _curveTick += Time.deltaTime;
        }
        transform.position += dir * speed;
    }

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }

}
