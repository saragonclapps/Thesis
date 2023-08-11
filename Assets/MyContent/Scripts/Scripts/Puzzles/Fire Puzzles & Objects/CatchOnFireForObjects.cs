using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MediumSizeObject))]
[RequireComponent(typeof(ObjectToWeight))]
public class CatchOnFireForObjects : MonoBehaviour, IFlamableObjects {
    private bool _isOnFire;
    public float _currentLife;
    private ObjectToWeight _objectToWeight;
    public float maxLife;
    public float fireSensitivity;
    private event Action OnStartFireEvents = delegate { };

    public ParticleSystem fireParticle;
    private MediumSizeObject _mediumSizeObject;
    private Renderer _renderer;

    public bool consumable;

    public bool isOnFire {
        get => _isOnFire;
        set => _isOnFire = value;
    }

    public void SetOnFire() {
        isOnFire = true;
        fireParticle.Play();
    }

    public void SubscribeStartFire(Action observer) {
        OnStartFireEvents += observer;
    }

    public void UnSubscribeStartFire(Action observer) {
        OnStartFireEvents -= observer;
    }

    private void Start() {
        isOnFire = false;
        _currentLife = maxLife;
        fireParticle.Stop();
        _renderer = GetComponent<Renderer>();
        // rend.material.SetColor("_BorderColor", Color.red);
        // rend.material.SetFloat("_DisolveAmount", 0);
        _objectToWeight = GetComponent<ObjectToWeight>();
        _mediumSizeObject = GetComponent<MediumSizeObject>();
        if (consumable) {
            UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
        }
    }

    private void Execute() {
        if (!isOnFire) return;
        if (!(_currentLife > 0)) {
            fireParticle.Stop();
            if (_mediumSizeObject.respawnable) {
                _mediumSizeObject.RepositionOnSpawn();
                _currentLife = maxLife;
                isOnFire = false;
                // rend.material.SetColor("_AlbedoColor", Color.white);
            }
            else {
                Die();
            }

            return;
        }

        _currentLife -= Time.deltaTime * fireSensitivity;
        FireEffect();
    }

    private void FireEffect() {
        OnStartFireEvents();
        //Just for burn effect
        // var scale = currentLife / maxLife;
        // var c = Vector4.Lerp(Color.black, Color.white, scale);
        // rend.material.SetColor("_AlbedoColor", c);
        // rend.material.SetFloat("_DisolveAmount", 1 - scale);
    }

    private void Die() {
        if (fireParticle.transform.parent != null) {
            OnStartFireEvents();
            fireParticle.transform.SetParent(null);
            transform.position += Vector3.up * 500000;
        }
        else {
            OnStartFireEvents = delegate { };
            Destroy(gameObject);
            Destroy(fireParticle);
        }
    }

    private void OnDestroy() {
        if (consumable) UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}