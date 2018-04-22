using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class FadeAwayPlatform : Platform {

    Renderer _renderer;
    BoxCollider _collider;

    [Header("Smooth Fade Away Transition")]
    public AnimationCurve curve;

    [Header("Period")]
    public float period;
    float _tick;

	// Use this for initialization
	void Start ()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<BoxCollider>();
        _tick = 0;
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
	}
	
	// Update is called once per frame
	void Execute ()
    {
        if (isActive)
        {
            _tick += Time.deltaTime;
            if(_tick > period)
            {
                _tick = 0;
            }

            var value = curve.Evaluate(_tick / period);
            var alpha = value < 0 ? 0 : value; 
            var col = _renderer.material.color;
            _renderer.material.color = new Color(col.r, col.g, col.b, alpha);


            _collider.isTrigger = alpha == 0? true: false;
        }
        else
        {
            if(_tick > 0)
            {
                _tick -= Time.deltaTime;
                var value = curve.Evaluate(_tick / period);
                var alpha = value < 0 ? 0 : value;
                var col = _renderer.material.color;
                _renderer.material.color = new Color(col.r, col.g, col.b, alpha);
            }
        }

	}

    void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}
