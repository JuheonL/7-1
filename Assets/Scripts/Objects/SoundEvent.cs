using UnityEngine;
using System;

public static class SoundEvent
{
    public static event Action<Vector3, float> OnSoundEmitted;

    public static void Emit(Vector3 position, float intensity)
    {
        OnSoundEmitted?.Invoke(position, intensity);
    }
}