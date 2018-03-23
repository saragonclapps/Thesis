using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RotationInCenterY : MonoBehaviour
{

    public float rotationSpeed = 99.0f;

    private void Start() { UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute); }
    private void OnDestroy() { UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute); }

    void Execute()
    {
        transform.Rotate(new Vector3(0f, 1f, 0f) * Time.deltaTime * this.rotationSpeed);
    }

    public void SetRotationSpeed(float speed)
    {
        this.rotationSpeed = speed;
    }
}
