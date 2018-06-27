using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDisk : MonoBehaviour {

    public Vector3 rotation;

	void Start () {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
	}
	
	void Execute () {
        transform.Rotate(rotation* Time.deltaTime);
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            Destroy(gameObject);
            HUDManager.instance.saveDisk.enabled = true;
        }
    }

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}
