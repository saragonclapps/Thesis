using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameManager : MonoBehaviour
{
    private DiskAbsorver _objective;
    private Transform _currentTransform;
    public Material postprocessEffect;
    
    public bool ActiveFx { get; private set; }
    
    private void Start()
    {
        _objective = FindObjectOfType<DiskAbsorver>();
        _currentTransform = transform;
        ActiveFx = GetComponent<PostProcess>();
        UpdatesManager.instance.AddUpdate(UpdateType.LATE, Execute);
    }
    
    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (postprocessEffect && ActiveFx)
        {
            Graphics.Blit(src, dst, postprocessEffect);
        }
        else
        {
            Graphics.Blit(src, dst);
        }
    }

    private void Execute()
    {
        if (ActiveFx && _objective)
        {
            var distance = Vector3.Distance(_currentTransform.position, _objective.transform.position);
            var powerFrame = (Mathf.Clamp(distance, 0, 50) / 50) * -0.5f;
            var valueX = (powerFrame + 1.5f);
            var valueY = (powerFrame + 1.5f);
            postprocessEffect.SetFloat("_TilingX", valueX);
            postprocessEffect.SetFloat("_TilingY", valueY);
        }
        
        if (!Input.GetKeyDown(KeyCode.T)) return;
        
        ActiveFx = !ActiveFx;
    }
    
    private void OnDestroy()
    {
        postprocessEffect.SetFloat("_TilingX", 1);
        postprocessEffect.SetFloat("_TilingY", 1);
        UpdatesManager.instance.RemoveUpdate(UpdateType.LATE, Execute);
    }
}
