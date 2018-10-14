﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchOnFireForObjects : MonoBehaviour, IFlamableObjects {

    bool _isOnFire;
    
    public float maxLife;
    public float fireSensitivity;
    float currentLife;
    ObjectToWeight otw;

    public ParticleSystem fireParticle;
    MediumSizeObject m;
    Renderer rend;

    public bool isOnFire
    {
        get{ return _isOnFire; }
        set{ _isOnFire = value; }
    }

    public void SetOnFire()
    {
        isOnFire = true;
        fireParticle.Play();
    }

    void Start()
    {
        isOnFire = false;
        currentLife = maxLife;
        fireParticle.Stop();
        rend = GetComponent<Renderer>();
        rend.material.SetColor("_BorderColor", Color.red);
        rend.material.SetFloat("_DisolveAmount", 0);
        otw = GetComponent<ObjectToWeight>();
        m = GetComponent<MediumSizeObject>();
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
    }
	
	void Execute ()
    {
        if (isOnFire)
        {
            if(currentLife > 0)
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
                    rend.material.SetColor("_AlbedoColor", Color.white);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
	}

    void FireEffect()
    {
        //Just for burn effect
        var scale = currentLife / maxLife;
        var c = Vector4.Lerp(Color.black, Color.white, scale);
        rend.material.SetColor("_AlbedoColor", c);
        rend.material.SetFloat("_DisolveAmount", 1 - scale);
        
    }

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}
