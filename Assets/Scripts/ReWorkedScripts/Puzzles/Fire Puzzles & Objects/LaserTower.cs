using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : MonoBehaviour
{

    List<IHeat> _targets;
    public float targetTemperature;
    public float damage;
    IHeat _laserTarget;

    public float delay = 2;
    [SerializeField]
    float _delayTick;

    LineRenderer line;

	void Start ()
    {
        _targets = new List<IHeat>();
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
	}
	
	
	void Execute ()
    {
       
		if(_targets != null && _targets.Count > 0)
        {
            foreach (var t in _targets)
            {
                if(t.temperature > targetTemperature)
                {
                    if(_laserTarget == null || _laserTarget.temperature < t.temperature)
                    {
                        _laserTarget = t;
                        _delayTick = 0;
                    }
                    
                }
            }
        }
        else
        {
            _laserTarget = null;
        }
        if (_laserTarget != null)
            DrawLaser();
        else
        {
            _delayTick = 0;
            line.enabled = false;
        }
	}

    private void DrawLaser()
    {
        _delayTick += Time.deltaTime;
        if(_delayTick > delay)
        {
            line.enabled = true;
            line.positionCount = 2;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, _laserTarget.Transform.position);

            _laserTarget.Hit(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var h = other.GetComponent<IHeat>();
        if (h != null)
        {
            
            if (!_targets.Contains(h))
            {
                _targets.Add(h);
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        var h = other.GetComponent<IHeat>();
        if(h != null)
        {
            if (_targets.Contains(h))
            {
                if(_laserTarget == h)
                {
                    _laserTarget = null;
                    line.enabled = false;
                }
                _targets.Remove(h);
                
            }
        }
    }
}
