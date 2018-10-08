using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFirePropulsor : MonoBehaviour, IFlamableObjects {

    public direction dir;

    bool _isOnFire;
    public bool isOnFire { get { return _isOnFire; } set { _isOnFire = value; } }

    public void SetOnFire()
    {
        _isOnFire = true;
    }

    /*// Use this for initialization
    void Start ()
    {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
	}
	
	// Update is called once per frame
	void Execute ()
    {
        if (_isOnFire)
        {
            isOnFire = false;
        }
	}

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }*/

    public enum direction
    {
        X,
        X_NEGATIVE,
        Z,
        Z_NEGATIVE
    }
}
