using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFire : Platform
{

    [SerializeField]
    PlatformFirePropulsor[] propulsors;

    public float platformSpeed;
    public float lerpValue;
    
    Vector3 directionedSpeed;

    bool isMoving;

    void Start ()
    {
        var p = GetComponentsInChildren<PlatformFirePropulsor>();
        propulsors = new PlatformFirePropulsor[p.Length];
        propulsors = p;

        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
	}
	
	void Execute()
    {
        for (int i = 0; i < propulsors.Length; i++)
        {
            if (propulsors[i].isOnFire)
            {
                switch (propulsors[i].dir)
                {
                    case PlatformFirePropulsor.direction.X:
                        directionedSpeed.x = Mathf.Lerp(directionedSpeed.x, platformSpeed, lerpValue);
                        Debug.Log("X");
                        break;
                    case PlatformFirePropulsor.direction.X_NEGATIVE:
                        directionedSpeed.x = Mathf.Lerp(directionedSpeed.x, -platformSpeed, lerpValue);
                        Debug.Log("-X");
                        break;
                    case PlatformFirePropulsor.direction.Z:
                        directionedSpeed.z = Mathf.Lerp(directionedSpeed.z, platformSpeed, lerpValue);
                        Debug.Log("Z");
                        break;
                    case PlatformFirePropulsor.direction.Z_NEGATIVE:
                        directionedSpeed.z = Mathf.Lerp(directionedSpeed.z, -platformSpeed, lerpValue);
                        Debug.Log("-Z");
                        break;
                    default:
                        break;
                }
                isMoving = true;
                propulsors[i].isOnFire = false;
            }
        }
        if (!isMoving)
        {
            directionedSpeed = Vector3.Lerp(directionedSpeed, Vector3.zero, lerpValue);
            
        }
        transform.position += directionedSpeed * Time.deltaTime;
        isMoving = false;
    }

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}
