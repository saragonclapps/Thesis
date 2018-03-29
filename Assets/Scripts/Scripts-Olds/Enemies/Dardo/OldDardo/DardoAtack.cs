using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DardoAtack : MonoBehaviour {

    #region Global Variables
    //Atack Move Variables
    private Transform _target;
    private Vector3 _chargingEndPosition;
    private float _chargeTimmer;
    private float _chargeTick;

    //Public Not Config Variables
    //[HideInInspector]
    public bool isCharging;
    [HideInInspector]
    public float chargingTolerance;

    //Public Config Variables
    public float chargeDistance;
    public float atackingSpeed;

    public float dardoDamage;
    #endregion

    /// <summary>
    /// State Preparation
    /// </summary>
    /// <param name="target"></param>
    public void Enter(Transform target)
    {
        _target = target;
        GetNewChargeEndPosition();
        isCharging = false;
    }

    /// <summary>
    /// Execute State Behaviour
    /// </summary>
	public void Execute()
    {
        if (isCharging)
        {
            if (_chargeTick >= _chargeTimmer)
            {
                isCharging = false;
                chargingTolerance = 1f;
                _chargeTick = 0;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, _chargingEndPosition, 0.03f);
                _chargeTick += Time.deltaTime;
            }
        }
        else
        {
            var dir = (_target.position - transform.position).normalized;
            transform.position += dir * atackingSpeed * Time.deltaTime;
        }
        transform.LookAt(_target.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == 9)
        {
            GetNewChargeEndPosition();
            chargingTolerance = 0f;
            _chargeTimmer = Random.Range(1, 1.5f);
            isCharging = true;
            EventManager.DispatchEvent(GameEvent.DARDO_HIT, _target.position, transform.position);
            EventManager.DispatchEvent(GameEvent.PLAYER_TAKE_DAMAGE, dardoDamage);
        }
    }

    /// <summary>
    /// Get a New end Position for the charge State
    /// </summary>
    private void GetNewChargeEndPosition()
    {
        var xRandom = Random.Range(1f, 2f);
        var yRandom = Random.Range(1f, 2f);
        var zRandom = Random.Range(1f, 2f);
        _chargingEndPosition = transform.position + new Vector3(xRandom, yRandom, zRandom);
        
    }
}
