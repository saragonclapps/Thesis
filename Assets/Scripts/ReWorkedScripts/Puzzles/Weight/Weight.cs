using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weight : MonoBehaviour {

    List<ObjectToWeight> _total;
    /// <summary>
    /// Executes when weight is reached
    /// </summary>
    public delegate void OnWeight();

    /// <summary>
    /// Executes when new object enters the weight
    /// </summary>
    public delegate void OnWeightEnter();

    /// <summary>
    /// Executes when an object leaves the weight
    /// </summary>
    public delegate void OnWeightExit();

    OnWeight callbacks;
    OnWeightEnter enterCallbacks;
    OnWeightExit exitCallbacks;

    public float actionWeight;
    float totalWeight;

    private bool wasOnWeight;

    private void Start()
    {
        _total = new List<ObjectToWeight>();
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
        wasOnWeight = false;
    }

    void Execute()
    {
        totalWeight = 0;
        foreach (var otw in _total)
        {
            totalWeight += otw.mass;
        }
        if(totalWeight >= actionWeight && callbacks != null)
        {
            callbacks();
        }
	}

    public void AddToWeight(ObjectToWeight otw)
    {
        if (!_total.Contains(otw))
        {
            _total.Add(otw);
        }

        //EnterCallbacks
        float total = 0;
        foreach (var o in _total)
        {
            total += o.mass;
        }
        if(total >= actionWeight && enterCallbacks != null)
        {
            enterCallbacks();
            wasOnWeight = true;
        }

    }

    public void RemoveFromWeight(ObjectToWeight otw)
    {
        if (_total.Contains(otw))
        {
            _total.Remove(otw);
        }

        //ExitCallbacks
        float total = 0;
        foreach (var o in _total)
        {
            total += o.mass;
        }
        if (total <= actionWeight && exitCallbacks != null && wasOnWeight)
        {
            exitCallbacks();
            wasOnWeight = false;
        }
    }

    public void AddOnWeightEvent(OnWeight callback)
    {
        callbacks += callback;
    }

    public void RemoveOnWeightEvent(OnWeight callback)
    {
        callbacks -= callback;
    }

    public void AddOnWeightEnterEvent(OnWeightEnter callback)
    {
        enterCallbacks += callback;
    }

    public void RemoveOnWeightEnterEvent(OnWeightEnter callback)
    {
        enterCallbacks -= callback;
    }

    public void AddOnWeightExitEvent(OnWeightExit callback)
    {
        exitCallbacks += callback;
    }

    public void RemoveOnWeightExitEvent(OnWeightExit callback)
    {
        exitCallbacks -= callback;
    }

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}
