using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBasket : MonoBehaviour {

    public LayerMask layermask;
    private int _countBox;
    public GameObject[] pauseObjects;

    private void Start()
    {
        StateObjects(false);
    }

    private void StateObjects(bool state)
    {
        foreach (var item in pauseObjects)
        {
            item.SetActive(state);
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (layermask == 1 << c.gameObject.layer)
        {
            _countBox++;

            if (_countBox >= 2)
            {
                StateObjects(true);
                GetComponent<Animator>().SetBool("Ready", true);
            }
        }

    }

    private void OnTriggerExit(Collider c)
    {

        _countBox--;

    }
}
