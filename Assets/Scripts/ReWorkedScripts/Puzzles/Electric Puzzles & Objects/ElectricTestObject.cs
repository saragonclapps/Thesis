using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTestObject : MonoBehaviour, IElectricObject {

    bool _isElectrified;

    public bool isElectrified {
        get { return _isElectrified; }
        set { _isElectrified = value; }
    }

    public void Electrify()
    {
        _isElectrified = true;
    }


}
