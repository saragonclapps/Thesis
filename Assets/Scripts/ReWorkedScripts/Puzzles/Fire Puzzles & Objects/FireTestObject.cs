using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTestObject : MonoBehaviour, IFlamableObjects {

    bool _isOnFire;
    
    public float maxLife;
    public float fireSensitivity;
    float currentLife;

    public ParticleSystem fireParticle;

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

    void Start ()
    {
        isOnFire = false;
        currentLife = maxLife;
        fireParticle.Stop();
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
    }
	
	void Execute () {
        if (isOnFire)
        {
            var scale = currentLife / maxLife;
            if(scale > 0)
            {
                transform.localScale = new Vector3(scale, scale, scale);
                currentLife -= Time.deltaTime * fireSensitivity;
            }
            else
            {
                fireParticle.Stop();
            }
        }
	}
}
