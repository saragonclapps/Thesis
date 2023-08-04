using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;

public class Weight : MonoBehaviour
{
    private ObjectToWeight _objectKey;
    private bool _isMovingObjectKey;
    private Vector3 _objectKeyInitialPosition;
    private Vector3 _objectKeyDestinationPosition;
    
    /// <summary>
    /// Executes when new object enters the weight
    /// </summary>
    public UnityEvent onWeightEnter;

    /// <summary>
    /// Executes when an object leaves the weight
    /// </summary>
    public UnityEvent onWeightExit;

    /// <summary>
    /// Get the current state of Weight
    /// </summary>
    public bool IsActiveWeight { get; private set; }

    #region MonoBehabior

    private void Awake()
    {
        onWeightEnter ??= new UnityEvent();
        onWeightExit ??= new UnityEvent();
    }
    
    private void Start()
    {
        _objectKeyDestinationPosition = transform.position + (Vector3.up * 1.2f);
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
        IsActiveWeight = false;
    }

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }

    #endregion MonoBehavior

    #region Custom
    
    /// <summary>
    /// Is a subscription to UpdatesManager and this execute MonoBehaviour (Update)
    /// </summary>
    private void Execute()
    {
        if (!_isMovingObjectKey || !_objectKey) return;

        _objectKey.transform.position = Vector3.Lerp(_objectKeyInitialPosition, _objectKeyDestinationPosition , 0.1f );
    }
    

    /// <summary>
    /// Set the Weight object to activate
    /// </summary>
    /// <param name="objectToWeight">object to add</param>
    public void AddToWeight(ObjectToWeight objectToWeight) {
        // Only exist one object to activate mechanism
        if (_objectKey) return;

        _isMovingObjectKey = true; 
        _objectKeyInitialPosition = objectToWeight.transform.position;
        _objectKey = objectToWeight;
        IsActiveWeight = true;
        var objectToWeightRenderer =objectToWeight.GetComponent<Renderer>();
        objectToWeightRenderer.material.SetFloat("_NormalPush", 4f);
        var rotation = objectToWeight.GetComponent<Rotation>();
        rotation.enabled = true;
        var objectToWeightRigidbody = objectToWeight.GetComponent<Rigidbody>();
        objectToWeightRigidbody.useGravity = false;
        objectToWeightRigidbody.isKinematic = true;
        onWeightEnter.Invoke();
    }

    /// <summary>
    /// Remove the Weight object to disable
    /// </summary>
    /// <param name="objectToWeight">object to remove</param>
    public void RemoveFromWeight(ObjectToWeight objectToWeight) {
        _objectKey = null;
        IsActiveWeight = false;
        var objectToWeightRenderer =objectToWeight.GetComponent<Renderer>();
        objectToWeightRenderer.material.SetFloat("_NormalPush", -0.5f);
        var rotation = objectToWeight.GetComponent<Rotation>();
        rotation.enabled = false;
        var objectToWeightRigidbody = objectToWeight.GetComponent<Rigidbody>();
        objectToWeightRigidbody.useGravity = true;
        objectToWeightRigidbody.isKinematic = false;
        onWeightExit.Invoke();
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = new Color(0, 255, 0, 0.7f); ;
        Gizmos.DrawWireSphere(_objectKeyDestinationPosition, 0.1f);
    }

    #endregion Custom
}
