using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour {

    public Key key1;
    public Key key2;
    public GameObject[] activeObjects;

	// Use this for initialization
	void Start () {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
	}
	
	// Update is called once per frame
	void Execute () {
		if(key1.solved && key2.solved)
        {
            foreach (var item in activeObjects)
            {
                item.SetActive(true);
            }
            gameObject.SetActive(false);
            UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
        }
	}
}
