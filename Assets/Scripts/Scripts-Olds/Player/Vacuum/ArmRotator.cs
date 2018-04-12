using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotator : MonoBehaviour {

    public const float MAX_Y_ANGLE = -50f;
    public const float MIN_Y_ANGLE = -110f;

    private bool isAiming;
    private float _currentY;
    public bool aimToggle;

    [Range(0.1f,0.9f)]
    public float changeAngleSpeed;

    public LeftHandIKControl lhikc;

    public bool activateIK;

    void Awake()
    {
        isAiming = false;
    }

    private void Start()
    {
        //Was fixed
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
    }

    void Execute ()
    {
        
        _currentY += GameInput.instance.cameraAngle;
        _currentY = Mathf.Clamp(_currentY, MIN_Y_ANGLE, MAX_Y_ANGLE);
        if (aimToggle)
        {
            aimToggle = false;
            isAiming = !isAiming;
            lhikc.ikActive = isAiming;
        }
        if (!isAiming)
        {
            //lhikc.ikActive = GameInput.instance.absorbButton || GameInput.instance.blowUpButton;
            lhikc.ikActive = activateIK;
            if (lhikc.ikActive)
            {
                _currentY += GameInput.instance.cameraAngle;
                _currentY = Mathf.Clamp(_currentY, MIN_Y_ANGLE, MAX_Y_ANGLE);
                var targetRotation = Quaternion.Euler(_currentY, 0f, 0f);
                //transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, changeAngleSpeed);
                transform.localRotation = targetRotation;
            }
        }
        else
        {
            var targetRotation = Quaternion.Euler(0f, 0f, _currentY);
            //transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, changeAngleSpeed);
            transform.localRotation = targetRotation;
        }
        
    }

    void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}
