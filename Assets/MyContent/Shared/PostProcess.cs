using UnityEngine;

#if UNITY_EDITOR
[ExecuteAlways]
#endif
public class PostProcess : MonoBehaviour {
    public Material postprocessEffect;

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (postprocessEffect)
        {
            Graphics.Blit(src, dst, postprocessEffect);
        }
    }
}
