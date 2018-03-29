using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVacuumAction {

    void Execute(params object[] pC);
    void Initialize();
    void Exit();
}
