﻿using System;
using UnityEngine;
using UnityEngine.Events;

public class CatchOnFireForObjects : MonoBehaviour, IFlamableObjects
{
    bool _isOnFire;
    public float maxLife;
    public float fireSensitivity;
    float currentLife;
    ObjectToWeight otw;
    private event Action OnStartFireEvents = delegate { }; 

    public ParticleSystem fireParticle;
    MediumSizeObject m;
    Renderer rend;

    public bool consumable;

    public bool isOnFire
    {
        get { return _isOnFire; }
        set { _isOnFire = value; }
    }

    public void SetOnFire()
    {
        isOnFire = true;
        fireParticle.Play();
    }

    public void SubscribeStartFire(Action observer)
    {
        OnStartFireEvents += observer;
    }

    public void UnSubscribeStartFire(Action observer)
    {
        OnStartFireEvents -= observer;
    }

    void Start()
    {
        isOnFire = false;
        currentLife = maxLife;
        fireParticle.Stop();
        rend = GetComponent<Renderer>();
        // rend.material.SetColor("_BorderColor", Color.red);
        // rend.material.SetFloat("_DisolveAmount", 0);
        otw = GetComponent<ObjectToWeight>();
        m = GetComponent<MediumSizeObject>();
        if (consumable)
            UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
    }

    void Execute()
    {
        if (isOnFire)
        {
            if (currentLife > 0)
            {
                currentLife -= Time.deltaTime * fireSensitivity;
                FireEffect();
            }
            else
            {
                fireParticle.Stop();
                if (m.respawnable)
                {
                    m.RepositionOnSpawn();
                    currentLife = maxLife;
                    isOnFire = false;
                    // rend.material.SetColor("_AlbedoColor", Color.white);
                }
                else
                {
                    Die();
                }
            }
        }
    }

    void FireEffect()
    {
        OnStartFireEvents();
        //Just for burn effect
        // var scale = currentLife / maxLife;
        // var c = Vector4.Lerp(Color.black, Color.white, scale);
        // rend.material.SetColor("_AlbedoColor", c);
        // rend.material.SetFloat("_DisolveAmount", 1 - scale);
    }

    void Die()
    {
        if (fireParticle.transform.parent != null)
        {
            OnStartFireEvents();
            fireParticle.transform.SetParent(null);
            transform.position += Vector3.up * 500000;
        }
        else
        {
            OnStartFireEvents = delegate {};
            Destroy(gameObject);
            Destroy(fireParticle);
        }
    }

    private void OnDestroy()
    {
        if (consumable) UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}