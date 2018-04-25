using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailTipController : MonoBehaviour {

    public Transform target;
    [Range(0,1)]
    public float t;

	void Start () {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
	}
	
	void Execute () {
        transform.position = Vector3.Lerp(transform.position, target.position, t);
	}

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}
