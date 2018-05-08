using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IElectricObject  {

    bool isElectrified { set; get; }
    void Electrify();
}
