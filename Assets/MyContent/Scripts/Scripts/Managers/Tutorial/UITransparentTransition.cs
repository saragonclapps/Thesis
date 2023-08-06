using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = Logger.Debug;

public class UITransparentTransition : MonoBehaviour {
    private float _transitionValue = 0;
    private bool _isTransitioning = false;
    private bool _state;
    private string _fromEvent;
    private Image[] _images;
    private TextMeshProUGUI[] _texts;

    private void Start() {
        _images = GetComponentsInChildren<Image>();
        _texts = GetComponentsInChildren<TextMeshProUGUI>();
        SetAlpha(0);
    }

    public void StartTransition(bool state, string fromEvent) {
        _state = state;
#if UNITY_EDITOR
        Debug.Log(this, "state: " + state + " fromEvent: " + fromEvent + " _fromEvent: " + _fromEvent + " _isTransitioning: " + _isTransitioning);
        Debug.Log(this, _transitionValue);
#endif
        if (_isTransitioning && fromEvent == _fromEvent) {
            return;
        }
        if (_transitionValue == 0 && state == false) {
            return;
        }
        if (_isTransitioning && fromEvent != _fromEvent) {
            StopAllCoroutines();
            _isTransitioning = false;
        }
        _fromEvent = fromEvent;
        StartCoroutine(TransitionFade(state));
    }

    private IEnumerator TransitionFade(bool state) {
        _isTransitioning = true;
        while (_transitionValue <= 0.94f && state || _transitionValue >= 0.06f && !state) {
            _transitionValue = Mathf.Lerp(_transitionValue, System.Convert.ToInt32(state), Time.deltaTime * 2f);
            SetAlpha(_transitionValue);

            yield return new WaitForEndOfFrame();
        }

        _isTransitioning = false;
        if (state) {
            _transitionValue = 1;
        }
        else {
            _transitionValue = 0;
        }
        
        SetAlpha(_transitionValue);
    }

    private void SetColor(Color color) {
        foreach (var image in _images) {
            image.color = color;
        }

        foreach (var textMeshProUGUI in _texts) {
            textMeshProUGUI.color = color;
        }
    }

    private void SetAlpha(float alpha) {
        foreach (var image in _images) {
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        }

        foreach (var textMeshProUGUI in _texts) {
            textMeshProUGUI.color = new Color(
                textMeshProUGUI.color.r,
                textMeshProUGUI.color.g,
                textMeshProUGUI.color.b,
                alpha
            );
        }
    }
}