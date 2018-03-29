using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDestruct : MonoBehaviour {


    private ParticleSystem _ps;

	void Start () {
        _ps = GetComponent<ParticleSystem>(); 
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
	}
	
	void Execute () {
        if (!_ps.IsAlive())
        {
            Destroy(gameObject);
        }
	}

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}
