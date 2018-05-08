using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityVFX : IHandEffect {

    Transform _start;
    Vector3[] _end;

    ParticleSystem[] _particles;
    GameObject _particlePrefab;

    List<IElectricObject> _objectToInteract;

    bool isPlaying;

    int numberOfObjects;

    public ElectricityVFX(GameObject particlePrefab, List<IElectricObject> objectToInteract, Transform start)
    {
        _particlePrefab = particlePrefab;
        _objectToInteract = objectToInteract;
        _start = start;
        isPlaying = false;
        numberOfObjects = 0;
        _particles = new ParticleSystem[objectToInteract.Count];
    }
	
	void Execute ()
    {
        if(numberOfObjects != _objectToInteract.Count)
        {
            ObjectsNumberChanged();
            numberOfObjects = _objectToInteract.Count;
        }
        for (int i = 0; i < numberOfObjects; i++)
        {
            _particles[i].transform.forward = (_end[i] - _start.position).normalized;
        }
	}


    public void StopEffect()
    {
        for (int i = 0; i < _particles.Length; i++)
        {
            _particles[i].Stop();
        }
    }

    public void StartEffect()
    {

        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
        numberOfObjects = _objectToInteract.Count;
        ObjectsNumberChanged();
    }

    public void TerminateEffect()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
        for (int i = 0; i < _particles.Length; i++)
        {
            _particles[i].gameObject.SetActive(false);
        }
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }

    void ObjectsNumberChanged()
    {
        _end = new Vector3[_objectToInteract.Count];
        _particles = new ParticleSystem[_end.Length];
        for (int i = 0; i < _objectToInteract.Count; i++)
        {
            _particles[i] = GameObject.Instantiate(_particlePrefab, _start.position, _start.rotation).GetComponent<ParticleSystem>();
            _particles[i].Play();
        }
    }
}
