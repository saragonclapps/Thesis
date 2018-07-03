using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Elevator : MonoBehaviour {

    public VacuumSwitch elevatorSwitch;
    public bool isActive;
    public Transform objective;
    public ParticleSystem activeParticles;
    public Transform[] cameraPositions;
    Action<Transform> cameraDemo;

    Transform player;
    bool startedFadedOut;
    float _timmer = 2;
    float _tick;
    int cameraCount = 0;

    public Animator blackOutAnimator;

	void Start ()
    {
        _tick = 0;
        cameraDemo = CameraDemo;
        elevatorSwitch.AddOnSwitchEvent(SwitchOn);
        isActive = false;
        activeParticles.Stop();
    }

    void SwitchOn()
    {
        isActive = true;
        elevatorSwitch.RemoveOnSwitchEvent(SwitchOn);
        activeParticles.Play();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isActive && other.gameObject.layer == 9)
        {
            player = other.transform.root;
            blackOutAnimator.SetTrigger("FadeOutAndIn");
            EventManager.DispatchEvent(GameEvent.CAMERA_STORY, cameraDemo);
            EventManager.AddEventListener(GameEvent.TRANSITION_FADEOUT_DEMO, FadeOutEnd);
        }
    }

    private void FadeOutEnd(object[] parameterContainer)
    {
        player.position = transform.position;
        player.rotation = transform.rotation;
        cameraCount++;
        if(cameraCount > 1)
        {
            EventManager.RemoveEventListener(GameEvent.TRANSITION_FADEOUT_DEMO, FadeOutEnd);
            EventManager.AddEventListener(GameEvent.TRANSITION_FADEIN_DEMO, FadeInEnd);
            player.position = objective.position;
            player.rotation = objective.rotation;
        }
    }

    private void FadeInEnd(object[] parameterContainer)
    {
        EventManager.RemoveEventListener(GameEvent.TRANSITION_FADEIN_DEMO, FadeInEnd);
        EventManager.DispatchEvent(GameEvent.CAMERA_NORMAL);
    }

    void CameraDemo(Transform transform)
    {
        transform.position = cameraPositions[cameraCount].position;
        transform.rotation = cameraPositions[cameraCount].rotation;
        if(_tick< _timmer)
        {
            _tick += Time.deltaTime;
        }
        else if(!startedFadedOut)
        {
            blackOutAnimator.SetTrigger("FadeOutAndIn");
            startedFadedOut = true;
        }
    }
}
