using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IObservable{
    void Subscribe(Action observer);
    void UnSubscribe(Action observer);
}
