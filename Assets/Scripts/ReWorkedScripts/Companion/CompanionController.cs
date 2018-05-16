using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionController : MonoBehaviour {

    public AnimationCurve curve;
    public float amplitute;
    public float frecMultiplier;

	// Use this for initialization
	void Start () {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
	}
	
	// Update is called once per frame
	void Execute ()
    {
        transform.position += transform.up * curve.Evaluate(Time.time/ frecMultiplier) * amplitute;
	}

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}
