using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricParticleEmitter : MonoBehaviour, IHandEffect {

    public GameObject particlePrefab;
    public Transform start;
    public List<Transform> end;

    public int particleAmount;
    public float particleTimmer;
    public float timmerDispersion;

    LineRenderer line;
    Vector3[] linePositions;
    [Header("Line Attributes")]
    public float lineDispersion;

    [Header("Particle Attributes")]
    public float timmer;
    public float dispersion;
    public float rayWidth;
    public Color rayStartColor;
    public Color rayEndColor;

    float _randomParticleTimmer;
    float _tick;

    public bool _isPlaying;

    public void Initialize(List<Transform> ends)
    {
        end = ends;
    }

    void Execute()
    {
		if(_tick > _randomParticleTimmer)
        {
            foreach (var tr in end)
            {
                for (int i = 0; i < particleAmount; i++)
                {
                    _tick = 0;
                    _randomParticleTimmer = particleTimmer + Random.Range(-timmerDispersion, timmerDispersion);
                    var pgo = Instantiate(particlePrefab, start.position, start.rotation,transform);
                    pgo.GetComponent<ElectricParticle>().Initialize(start, tr, timmer, dispersion , rayWidth, rayStartColor, rayEndColor);
                }
                SetLine(tr);  
            }
        }
        _tick += Time.deltaTime;
	}

    public void SetLine(Transform tr)
    {
        var distance = Vector3.Distance(start.position, tr.position);
        var dir = (tr.position - start.position).normalized;
        var points = Random.Range(5, 10);
        var increment = distance / points;
        linePositions = new Vector3[points];

        start.forward = dir;

        for (int i = 0; i < points -1 ; i++)
        {
            linePositions[i] = start.position + (dir * increment * i);
            linePositions[i] += start.up * Random.Range(-lineDispersion, lineDispersion) + start.right * Random.Range(-lineDispersion, lineDispersion);
        }
        linePositions[points-1] = tr.position;
        line.positionCount = points;
        line.SetPositions(linePositions);
    }

    #region IHandEffect
    public void StopEffect()
    {
        _isPlaying = false;
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }

    public void StartEffect()
    {
        if (!_isPlaying)
        {
            _isPlaying = true;
            gameObject.SetActive(true);
            _randomParticleTimmer = particleTimmer + Random.Range(-timmerDispersion, timmerDispersion);
            line = GetComponent<LineRenderer>();
            UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
        }
    }

    public void TerminateEffect()
    {
        StopEffect();
        gameObject.SetActive(false);
    }

    public bool IsPlaying()
    {
        return _isPlaying;
    }
    #endregion

}
