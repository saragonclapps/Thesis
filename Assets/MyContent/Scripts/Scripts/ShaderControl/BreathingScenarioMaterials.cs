using System.Collections;
using UnityEngine;

public class BreathingScenarioMaterials: MonoBehaviour {
    private float _curedLerpValue;
    private bool _isExecuting;
    public static BreathingScenarioMaterials instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        ResetFromBeginning();
    }


    private void Execute() {
        Shader.SetGlobalFloat("timeBreathing", Mathf.Sin(Time.time));
    }
    
    public void OnStartWinEvent() {
        StartCoroutine(WinEvent());
    }

    private IEnumerator WinEvent() {
        if (_isExecuting) yield break;
        _isExecuting = true;
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
        while (_curedLerpValue <= 0.99f && _isExecuting) {
            _curedLerpValue = Mathf.Lerp(_curedLerpValue, 1, Time.deltaTime * 0.5f);
            Debug.Log(_curedLerpValue);
            var timeBreathing = Mathf.Lerp(Shader.GetGlobalFloat("timeBreathing"), 1,_curedLerpValue);
            Shader.SetGlobalFloat("CuredLerp", timeBreathing);
            Shader.SetGlobalFloat("timeBreathing", timeBreathing);
            yield return new WaitForEndOfFrame();
        }
    }

    public void ResetFromBeginning() {
        _isExecuting = false;
        _curedLerpValue = 0;
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
        Shader.SetGlobalFloat("timeBreathing", 0);
        Shader.SetGlobalFloat("CuredLerp",  0);
    }

    private void OnDestroy() {
        if (UpdatesManager.instance != null) {
            UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
        }
    }
}