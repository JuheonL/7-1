using UnityEngine;
using System;

public interface IBossSensor
{
    event Action<Vector3> OnTargetDetected;
    //
    void Tick();
    //
}