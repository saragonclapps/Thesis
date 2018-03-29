using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyMovement : MonoBehaviour {

    #region Global Variables
    public bool isActive;

    //Fairy Movemente Variables
    public float yShake;
    public float yShakeSpeed;
    public float xRange;
    public float yRange;
    public float zRange;
    public float randomRangeMoveXZ;

    //Position Variables
    [HideInInspector]
    public Vector3 targetPosition;
    [HideInInspector]
    public Vector3 originalPosition;
    #endregion

    void Start () {
        originalPosition = transform.position;
        targetPosition = originalPosition + new Vector3(0f, yShake/2, 0f);
        isActive = true;
        StartCoroutine(RandomMovement());

        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
    }
	public void Execute()
    {
        if (isActive)
            Move();
    }

    /// <summary>
    /// Move Behaviour
    /// </summary>
    void Move()
    {
        if(Mathf.Abs(targetPosition.y -  transform.position.y) < 0.1f)
        {
            if(targetPosition.y > transform.position.y)
            {
                targetPosition.y -= yShake; 
            }
            else
            {
                targetPosition.y += yShake;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, yShakeSpeed);
        }
        var dir = targetPosition - transform.position;
        var dirWithoutY = new Vector3(dir.x, 0, dir.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dirWithoutY), Time.deltaTime);
    }

    #region MovementeRandomize
    IEnumerator RandomMovement()
    {
        while(isActive)
        {
            targetPosition.x += Random.Range(-randomRangeMoveXZ, randomRangeMoveXZ);
            targetPosition.z += Random.Range(-randomRangeMoveXZ, randomRangeMoveXZ);

            targetPosition.x = Mathf.Clamp(targetPosition.x, originalPosition.x - xRange, originalPosition.x + xRange);
            targetPosition.z = Mathf.Clamp(targetPosition.z, originalPosition.z - zRange, originalPosition.z + zRange);

            var t = Random.Range(1f, 3f);
            var wait = new WaitForSeconds(t);
            yield return wait;

        }
    }
    IEnumerator FairyDirectionedMovement(Vector3 target, float distanceToTravel, float wait)
    {
        while (true)
        {
            var rawDir = target - transform.position;
            var randomizedDir = rawDir + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));

            targetPosition = transform.position + randomizedDir * distanceToTravel;

            yield return new WaitForSeconds(wait);
        }
    }
    IEnumerator FairyDirectionedMovement(Transform target, float distanceToTravel, float wait)
    {
        while (true)
        {
            var rawDir = target.position - transform.position;
            var distance = rawDir.magnitude;
            var randomizedDir = rawDir + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));

            targetPosition = transform.position + randomizedDir * distanceToTravel;

            yield return new WaitForSeconds(wait);
        }
    }
    #endregion

    #region Activation
    public void Activate()
    {
        isActive = true;
        StartCoroutine(RandomMovement());
    }
    
    /// <summary>
    /// Stop All Movement Behaviours
    /// </summary>
    public void DeActivate()
    {
        isActive = false;
        StopAllCoroutines();
        //StopCoroutine(RandomMovement());
    }

    /// <summary>
    /// Activate Directed Move with Fairy Behaviour
    /// </summary>
    /// <param name="target">Target position(for still targets)</param>
    /// <param name="distanceToTravel">Distance To Travel</param>
    /// <param name="wait">Wait Time</param>
    public void ActivateDirectionedMove(Vector3 target, float distanceToTravel, float wait)
    {
        
        StartCoroutine(FairyDirectionedMovement(target, distanceToTravel, wait));
        isActive = true;
    }
    
    /// <summary>
    /// Activate Directed Move with Fairy Behaviour
    /// </summary>
    /// <param name="target">Target Transform(for moving targets)</param>
    /// <param name="distanceToTravel">Distance To Travel</param>
    /// <param name="wait">Wait Time</param>
    public void ActivateDirectionedMove(Transform target, float distanceToTravel, float wait)
    {
        StartCoroutine(FairyDirectionedMovement(target, distanceToTravel, wait));
        isActive = true;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(originalPosition, new Vector3(xRange * 2, yShake, zRange * 2));
       
    }

}
