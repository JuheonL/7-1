using UnityEngine;
using System;

public class FootstepSensor : MonoBehaviour, IBossSensor
{
    [Header("감지 설정")]
    public float detectionRange = 5f;

    public event Action<Vector3> OnTargetDetected;

    private void OnEnable()
    {
        PlayerFootstepEmitter.OnFootstepEmitted += OnSoundHeard;
    }

    private void OnDisable()
    {
        PlayerFootstepEmitter.OnFootstepEmitted -= OnSoundHeard;
    }

    public void Tick() { }

    private void OnSoundHeard(Vector3 soundPos, float intensity)
    {
        float dist = Vector3.Distance(transform.position, soundPos);
        if (dist > detectionRange * intensity) return;

        OnTargetDetected?.Invoke(soundPos);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, detectionRange);
    }
}