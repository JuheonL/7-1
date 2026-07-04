using UnityEngine;
using System;

public class SoundSensor : MonoBehaviour, IBossSensor
{
    [Header("감지 설정")]
    public float hearingRange = 20f;

    public event Action<Vector3> OnTargetDetected;

    private void OnEnable()
    {
        SoundEvent.OnSoundEmitted += OnSoundHeard;
    }

    private void OnDisable()
    {
        SoundEvent.OnSoundEmitted -= OnSoundHeard;
    }

    // IBossSensor — BossAI가 매 프레임 호출
    // SoundSensor는 이벤트 기반이라 Tick에서 할 일 없음
    public void Tick() { }

    private void OnSoundHeard(Vector3 soundPos, float intensity)
    {
        float dist = Vector3.Distance(transform.position, soundPos);
        if (dist > hearingRange * intensity) return;

        OnTargetDetected?.Invoke(soundPos);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.15f);
        Gizmos.DrawSphere(transform.position, hearingRange);
    }
}