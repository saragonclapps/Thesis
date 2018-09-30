using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightActionTest : MonoBehaviour {

    public Weight weight;
	
	void Start ()
    {
        weight.AddOnWeightEvent(OnWeight);
	}

    void OnWeight()
    {
        Debug.Log("Abrete Sesamo");
    }

}
