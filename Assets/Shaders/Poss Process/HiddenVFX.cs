using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Camera))]
public class HiddenVFX : MonoBehaviour {

    public Material mat;
    [HideInInspector]public bool isActive;
    [HideInInspector]public bool activatePostProcess;
    private Material[] characterMaterial;


    float _timmer = 0.5f;
    float _tick;
    // Use this for initialization
    void Start()
    {
        _tick = 0;

        //new
        var temp = FindObjectOfType<PlayerController>().GetComponentsInChildren<Renderer>();
        characterMaterial = temp
                            .SelectMany(x => x.GetComponent<Renderer>().materials)
                            .Where(y => y.shader.name == "Created/CustomLightingCustomRim")
                            .ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (_tick < _timmer)
            {
                activatePostProcess = true;
                _tick += Time.deltaTime;
                mat.SetFloat("_Power", _tick / _timmer);
                //New
                foreach (var item in characterMaterial)
                {
                    item.SetFloat("_Force", _tick / _timmer);
                }
            }
        }
        else
        {
            if (_tick > 0)
            {
                activatePostProcess = true;
                _tick -= Time.deltaTime;
                mat.SetFloat("_Power", _tick / _timmer);
                //new
                foreach (var item in characterMaterial)
                {
                    item.SetFloat("_Force", _tick / _timmer);
                }
            }
            else activatePostProcess = false;
        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (activatePostProcess) Graphics.Blit(src, dst, mat);
        else Graphics.Blit(src, dst);
    }
}
