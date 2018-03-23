using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistController : MonoBehaviour {

    private bool isActive;
    private SphereCollider _sc;
    private LargeSizeObject _lso;
    private Rigidbody _rb;
    private ParticleSystem _ps;
    private ParticleSystem.EmissionModule _psEM;
    HiddenVFX vignette;


    //Damage in max range
    //public float borderDamagePerSecond;

    //Damage radius
    public float radius;
    public float life;

    //Player Head
    //public Transform target;

    private int destroyCounter;
    

    private void Start()
    {
        _ps = GetComponentInChildren<ParticleSystem>();
        _psEM = _ps.emission;
        _rb = GetComponent<Rigidbody>();
        _lso = GetComponent<LargeSizeObject>();
        _sc = GetComponent<SphereCollider>();
        _sc.radius = radius;
        //vignette = EnemyManager.instance.vignette;
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
    }

    void Execute()
    {
        /*if (isActive)
        {
            var distance = Mathf.Abs((transform.position - target.position).magnitude);
            var damage = distance > 0 ? borderDamagePerSecond * radius / distance : float.MaxValue;
            EventManager.DispatchEvent(GameEvent.PLAYER_TAKE_DAMAGE, damage * Time.deltaTime);
        }*/
        if (_lso.isBeeingAbsorved)
        {
            life -= _lso.movement * Time.deltaTime;
            _psEM.rateOverTime = life;
        }
        if(life < 0 && _psEM.enabled)
        {
            //Prueba
            _psEM.enabled = false;
            _sc.center += Vector3.up * 400;
            /*if (destroyCounter < 50)
            {
                destroyCounter++;
                _sc.center += Vector3.up * 4;
                _ps.transform.localScale *= 0.9f;
            }
            else
            {
                Die();
            }*/
        }
        if (!_ps.IsAlive())
        {
            Die();
        }
    }

    private void Die()
    {
        _lso.Exit();
        EventManager.DispatchEvent(GameEvent.MIST_DIE, transform.position);
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            vignette = EnemyManager.instance.vignette;
            vignette.isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            vignette.isActive = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = new Color(0, 0, 0, 0.5f);
        Gizmos.DrawSphere(Vector3.zero,  10);
    }
}
