using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class RandomMove : MonoBehaviour {                                   
    public bool isActive = true;
    public float scalePosition, scaleRotation = 1f;
    public float positionSpeed, rotationSpeed = 1f;
    
    private float _randomSpeedRotation, _randomSpeedPosition = 0.1f;
    private Vector3 _newPosition, _firstPosition;
    private Quaternion _newRotation, _firstRotation;
    
    
    [Range(0f, 1f)]
    public float shakePositionRange, shakeRotationRange = 0.1f;
    
    
    
    void Start () {
       _newPosition = _firstPosition = transform.localPosition;
       _newRotation = _firstRotation = transform.rotation;
    
       StartCoroutine(ChangeNew());
       UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
    }
    
    private void OnDestroy()
    {
       UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }

    private IEnumerator ChangeNew()
    {
       var randomTime = RndValue(3, 5);
       while (true)
       {
           yield return new WaitForSeconds(randomTime);
           randomTime = RndValue(3, 5);
           _randomSpeedPosition = RndValue(0.0005f, 0.005f) * positionSpeed;
           _randomSpeedRotation = RndValue(0.0005f, 0.005f) * rotationSpeed;
           _newPosition = ResetNew(_newPosition, _firstPosition, scalePosition);
           _newRotation = ResetNew(_newRotation, _firstRotation, scaleRotation);
       }
    }
    
    private Quaternion ResetNew(Quaternion value, Quaternion originValue, float scale = 1)
    {
       var randNrX = RndValue(shakePositionRange, -shakePositionRange);
       var randNrY = RndValue(shakePositionRange, -shakePositionRange);
       var randNrZ = RndValue(shakePositionRange, -shakePositionRange);
       var randNrW = RndValue(shakePositionRange, -shakePositionRange);
    
       return value = new Quaternion(
           originValue.x + randNrX * scale,
           originValue.y + randNrY * scale,
           originValue.z + randNrZ * scale,
           originValue.w + randNrW * scale);
    }
    
    private Vector3 ResetNew(Vector3 value, Vector3 originValue, float scale = 1)
    {
       var randNrX = RndValue(shakePositionRange, -shakePositionRange);
       var randNrY = RndValue(shakePositionRange, -shakePositionRange);
       var randNrZ = RndValue(shakePositionRange, -shakePositionRange);
    
       return value = new Vector3(
           originValue.x + randNrX * scale,
           originValue.y + randNrY * scale,
           originValue.z + randNrZ * scale);
    }

    private static float RndValue(float min, float max) {
       return min + Rand.uniform * (max - min);
    }

    private void Execute () {
    
       if (isActive) Shake();
    }

    private void Shake()
    {
       transform.localPosition = Vector3.Lerp(transform.localPosition, _newPosition, _randomSpeedPosition);
       transform.localRotation = Quaternion.Lerp(transform.localRotation, _newRotation, _randomSpeedRotation);
    }
    
    private void OnDrawGizmos()
    {
       //PENDING!!!!!
       /*var collider = GetComponentInChildren<BoxCollider>();
       Gizmos.color = new Color(255, 255, 255, 0.5f);
       Gizmos.matrix = transform.localToWorldMatrix;
       Gizmos.DrawCube(collider.center, collider.size);*/
    }
}
    
public static class Rand
{
   private static readonly System.Random _rnd = new System.Random();
    public static float uniform
    {
       get { return ((float)_rnd.Next(0, 8388606) / 8388607f); }
    }
}