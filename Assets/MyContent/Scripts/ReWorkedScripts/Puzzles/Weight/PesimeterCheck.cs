using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PesimeterCheck : MonoBehaviour {

    public bool isEmpty;
    public Weight weight;
    public Action ObservableRemoveWeight;

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
        var flammable = other.GetComponent<IFlamableObjects>();
        if (flammable != null) {
            ObservableRemoveWeight = () => weight.RemoveFromWeight(objectToWeight);
            flammable.SubscribeStartFire(ObservableRemoveWeight);
        }
        isEmpty = false;
    }

    private void OnTriggerExit(Collider other) {
        var objectToWeight = other.GetComponent<ObjectToWeight>();
        if (!objectToWeight || !weight.IsActiveWeight) return;
        var flammable = other.GetComponent<IFlamableObjects>();
        flammable?.UnSubscribeStartFire(ObservableRemoveWeight);
        weight.RemoveFromWeight(objectToWeight);
    }
    #endregion
}
