using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexMove : MonoBehaviour {

    public Material[] materialReference;
    public AnimationCurve moveX;
    public AnimationCurve moveY;
    public AnimationCurve moveZ;

    private void Start()
    {
        UpdatesManager.instance.AddUpdate(UpdateType.LATE, Execute);
    }

    private void Execute()
    {
        if (materialReference.Length > 0)
        {
            foreach (var item in materialReference)
            {
                item.SetVector("DisplacementVertex", new Vector3(moveX.Evaluate(Time.time),
                                                                    moveY.Evaluate(Time.time),
                                                                    moveZ.Evaluate(Time.time)));
            }
        }
    }

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.LATE, Execute);
    }
}
