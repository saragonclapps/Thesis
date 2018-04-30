using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmAngle : MonoBehaviour {

    const float MAX_Y_ANGLE = -40f;
    const float MIN_Y_ANGLE = -120f;

    public float _currentY;
    Animator _anim;

    void Start () {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
        _anim = GetComponent<Animator>();
	}
	
	void Execute () {
        _currentY += GameInput.instance.cameraAngle;
        _currentY = Mathf.Clamp(_currentY, MIN_Y_ANGLE, MAX_Y_ANGLE);

        _anim.SetFloat("armAngle", _currentY);
        _anim.SetBool("isAbsorbing", GameInput.instance.absorbButton || GameInput.instance.blowUpButton);
    }

    void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}
