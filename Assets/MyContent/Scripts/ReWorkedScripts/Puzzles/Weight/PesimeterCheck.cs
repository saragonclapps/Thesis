using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PesimeterCheck : MonoBehaviour {

    public bool isEmpty;
    public Weight weight;

    #region MonoBehavior

    private void Start () {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
    }

    private void Execute() {
    }

    private void OnDestroy() {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
    
    private void OnTriggerEnter(Collider other) {
        var objectToWeight = other.GetComponent<ObjectToWeight>();
        if (!objectToWeight) return;
        weight.AddToWeight(objectToWeight);
        isEmpty = false;
    }

    private void OnTriggerExit(Collider other) {
        var objectToWeight = other.GetComponent<ObjectToWeight>();
        if (!objectToWeight || !weight.IsActiveWeight) return;
        weight.RemoveFromWeight(objectToWeight);
    }
    #endregion
}
