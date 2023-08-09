using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToWeight : MonoBehaviour {
    private Rigidbody _rb;
    public float mass;
    public Weight weight;

    private readonly float _timmer = 2;
    [SerializeField] 
    private float _tick;

    private void Start() {
        _rb = GetComponent<Rigidbody>();
        mass = _rb.mass;
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
    }

    private void Execute() {
        if (weight == null) return;
    }
    
    private void OnDestroy() {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}