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
}
