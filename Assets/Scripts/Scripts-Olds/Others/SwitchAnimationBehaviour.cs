using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SwitchAnimationBehaviour : MonoBehaviour{

    public GameObject groupWall;//Set childs all walls.
    
    private Weight _weight;
    private bool _isActive;
    private List<Material> _materials;

    private void Start()
    {
        _weight = GetComponentInChildren<Weight>();
        _weight.AddOnWeightEnterEvent(OnWeightEnter);
        _weight.AddOnWeightExitEvent(OnWeightExit);
        //SetMaterials
        var materials1 = GetComponentsInChildren<Renderer>().Select(m => m.material).ToList();
        var materials2 = groupWall.GetComponentsInChildren<Renderer>().Select(m => m.material).ToList();
        _materials = materials1.Concat(materials2).ToList();
    }

    void OnWeightEnter()
    {
        foreach (var item in GetComponentsInChildren<Animation>())
        {
            if(!item.isPlaying)
                item.Play();
            item["Open"].speed = 1;
        }

        //Emission color
        float brightnessFactor = 3f;
        Color hdrColor = _materials[0].GetColor("_EmissionColor") * brightnessFactor;

        foreach (var item in _materials)
        {
            item.SetColor("_EmissionColor", hdrColor);
        }
    }

    void OnWeightExit()
    {

        foreach (var item in GetComponentsInChildren<Animation>())
        {
            if (!item.isPlaying)
                item.Play();

            item["Open"].speed = -1;
            //Loading......!!
        }

        //Emission color reset.
        float brightnessFactor = 0.3f;
        Color hdrColor = _materials[0].GetColor("_EmissionColor") * brightnessFactor;

        foreach (var item in _materials)
        {
            item.SetColor("_EmissionColor", hdrColor);
        }

    }
}
