using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour {

    public Transform target;
    public float angle;
    public float distance;

    const int terrainLayer = 10;
    const int fogLayer = 17;
    int layerMask;


    public bool targetInSight;

    public void Configure(float viewAngle, float viewDistance)
    {
        angle = viewAngle;
        distance = viewDistance;
    }

	void Start ()
    {
        layerMask = (1 << terrainLayer) | (1 << fogLayer);
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
	}
	
	void Execute ()
    {
        var targetDir = target.position - transform.position;
        var targetAngle = Vector3.Angle(transform.forward, targetDir);
        var targetDistance = Mathf.Abs(Vector3.Distance(target.position, transform.position));

        targetInSight = targetAngle < angle &&
                        targetDistance < distance &&
                        !Physics.Raycast(transform.position, targetDir, targetDistance, layerMask);
        
        
	}

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = targetInSight ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, target.transform.position);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * distance));

        Vector3 rightLimit = Quaternion.AngleAxis(angle, transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + (rightLimit * distance));

        Vector3 leftLimit = Quaternion.AngleAxis(-angle, transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + (leftLimit * distance));

        Gizmos.DrawLine(transform.position + (rightLimit * distance), transform.position + (transform.forward * distance));
        Gizmos.DrawLine(transform.position + (leftLimit * distance), transform.position + (transform.forward * distance));


    }
}
