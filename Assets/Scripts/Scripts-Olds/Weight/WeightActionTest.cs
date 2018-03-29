using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightActionTest : MonoBehaviour {

    public Key key1;
    public Key key2;
	
	void Start () {
	}

    private void Update()
    {
        if(key1.solved && key2.solved)
        {
            Debug.Log("logrado");
        }
    }

}
