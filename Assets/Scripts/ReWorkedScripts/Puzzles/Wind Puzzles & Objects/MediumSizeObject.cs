using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Material))]
public class MediumSizeObject : MonoBehaviour, IVacuumObject {

    [HideInInspector]
    public bool wasShooted;

    [HideInInspector]
    public Material material;//Edit for shoot vfx.
    private BoxCollider _bC;

    float _alphaCut;

    Vector3 _initialPosition;

    float _disolveTimmer = 1;
    float _disolveTick;
    bool _disolve;

    bool _isAbsorved;
    bool _isAbsorvable;
    bool _isBeeingAbsorved;
    Rigidbody _rb;

    public bool isAbsorved { get { return _isAbsorved; } set { _isAbsorved = value; } }
    public bool isAbsorvable { get { return _isAbsorvable; } }
    public bool isBeeingAbsorved { get { return _isBeeingAbsorved; } set { _isBeeingAbsorved = value; } }
    public Rigidbody rb { get { return _rb; } set { _rb = value; } }

    public bool respawnable;

    private void Start()
    {
        _initialPosition = transform.position;
        _isAbsorvable = false;
        _rb = GetComponent<Rigidbody>();
        material = GetComponent<Renderer>().material;
        _bC = GetComponent<BoxCollider>();
        SpawnVFXActivate(true);

    }

    void SpawnVFXActivate(bool dir)
    {
        if (dir)
        {
            _alphaCut = 1;
            UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, SpawnVFX);
        }
        else
        {
            _alphaCut = 0;
            UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, DespawnVFX);
        }
    }

    void SpawnVFX()
    {
        material.SetFloat("_DisolveAmount", _alphaCut);
        _alphaCut -= Time.deltaTime;
        if(_alphaCut <= 0)
        {
            UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, SpawnVFX);
        }
    }

    void DespawnVFX()
    {
        material.SetFloat("_DisolveAmount", _alphaCut);
        _alphaCut += Time.deltaTime;
        if (_alphaCut >= 1)
        {
            UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, DespawnVFX);
            transform.position = _initialPosition;
            transform.rotation = Quaternion.identity;
            rb.velocity = Vector3.zero;
            SpawnVFXActivate(true);
        }
    }
    
    public void SuckIn(Transform origin, float atractForce)
    {
        if (!wasShooted)
        {
            var direction = origin.position - transform.position;
            var distance = direction.magnitude;

            if (distance <= 0.5f)
            {
                _bC.enabled = false;
                rb.isKinematic = true;
                transform.position = origin.position;
                isAbsorved = true;
                transform.SetParent(origin);
                
            }
            else if (distance < 1f)
            {
                rb.isKinematic = true;
                
                var dir = (origin.position - transform.position).normalized;
                transform.position += dir * atractForce/10 * Time.deltaTime;
            }
            else
            {
                rb.isKinematic = false;
                var forceMagnitude = (10) * atractForce / Mathf.Pow(distance, 2);
                var force = direction.normalized * forceMagnitude;
                rb.AddForce(force);
                
            }
        }
    }

    public void BlowUp(Transform origin, float atractForce, Vector3 direction)
    {
        if(!wasShooted)
        {
            rb.isKinematic = false;
            isAbsorved = false;
            transform.SetParent(null);
            var distanceRaw = transform.position - origin.position;
            var distance = distanceRaw.magnitude;
            var forceMagnitude =  10 * atractForce * 10 / Mathf.Pow(distance, 2);
            forceMagnitude = Mathf.Clamp(forceMagnitude, 0, 2000);
            var force = direction.normalized * forceMagnitude;
            rb.AddForce(force);
        }
    }

    public void ReachedVacuum()
    {
        
    }

    public void Shoot(float shootForce, Vector3 direction)
    {
        _bC.enabled = true;
        wasShooted = true;
        isAbsorved = false;
        rb.isKinematic = false;
        transform.SetParent(null);
        rb.velocity = direction * shootForce/rb.mass;
        _disolveTick = 0;
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, DisolveTimmer);
    }

    void DisolveTimmer()
    {
        if (respawnable)
        {
            _disolveTick += Time.deltaTime;
            if(_disolveTick > _disolveTimmer)
            {
                _disolve = true;
                _disolveTick = 0;
                UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, DisolveTimmer);
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        wasShooted = false;
        if (_disolve)
        {
            SpawnVFXActivate(false);
            _disolve = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (_disolve)
        {
            SpawnVFXActivate(false);
            _disolve = false;
        }
    }

    public void ViewFX(bool activate)
    {
        if (activate)
        {
            //View VFX
            
            material.SetFloat("_Alpha", 0.3f);
        }
        else
        {
            //Reset view VFX
            material.SetFloat("_Alpha", 1f);
        }
    }

    public void Exit()
    {
        _bC.enabled = true;
        ViewFX(false);
        transform.SetParent(null);
        rb.isKinematic = false;
        isAbsorved = false;
    }
}
