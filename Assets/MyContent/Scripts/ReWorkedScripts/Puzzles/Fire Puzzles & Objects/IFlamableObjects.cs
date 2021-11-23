﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlamableObjects {

    bool isOnFire { get; set; }
    void SetOnFire();

    void SubscribeStartFire(Action observer);
    void UnSubscribeStartFire(Action observer);
}
