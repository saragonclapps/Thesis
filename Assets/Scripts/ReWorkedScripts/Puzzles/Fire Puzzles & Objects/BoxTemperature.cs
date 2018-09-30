using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTemperature : MonoBehaviour, IHeat, IFlamableObjects
{
    [SerializeField]
    float _temperature;
    bool _isOnFire;

    public float temperature{ get{ return _temperature; }}
    public Transform Transform { get { return transform; } }
    public bool isOnFire
    {
        get { return _isOnFire; }
        set { _isOnFire = value; }
    }

    public float life;
    public float maxTemperature;
    public float heatTransferMultiplier;

    bool _setToDestroy;

    public void SetOnFire()
    {
        _temperature += Time.deltaTime * heatTransferMultiplier;
    }

    void Start ()
    {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
	}
	
	void Execute ()
    {
        _temperature -= Time.deltaTime * heatTransferMultiplier / 10;
        _temperature = Mathf.Clamp(_temperature, 0, maxTemperature);

        if (_setToDestroy)
        {
            DestroyBox();
        }
	}

    private void DestroyBox()
    {
        if(transform.position.y > 10000)
        {
            Destroy(gameObject);
        }
        transform.position += transform.up * 100000;
    }

    public void Hit(float damage)
    {
        life -= damage * Time.deltaTime;
        if(life < 0)
        {
            _setToDestroy = true;
        }
    }

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}
