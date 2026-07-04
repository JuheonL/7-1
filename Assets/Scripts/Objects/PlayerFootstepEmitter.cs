using UnityEngine;
using System;
using StarterAssets;

public class PlayerFootstepEmitter : MonoBehaviour
{
    public static event Action<Vector3, float> OnFootstepEmitted;

    [Header("걸음 간격")]
    public float walkInterval = 0.5f;
    public float sprintInterval = 0.3f;

    [Header("소리 강도")]
    public float walkIntensity = 0.5f;
    public float sprintIntensity = 1.0f;

    [Header("판정")]
    public float sprintSpeedThreshold = 4f;
    public float moveThreshold = 0.1f;

    private ThirdPersonController _tpc;
    private CharacterController _cc;
    private float _nextStepTime;

    private void Awake()
    {
        _tpc = GetComponent<ThirdPersonController>();
        _cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!_tpc.Grounded) return;

        float horizontalSpeed = new Vector3(_cc.velocity.x, 0f, _cc.velocity.z).magnitude;
        if (horizontalSpeed < moveThreshold) return;

        bool isSprinting = horizontalSpeed >= sprintSpeedThreshold;
        float interval = isSprinting ? sprintInterval : walkInterval;

        if (Time.time < _nextStepTime) return;
        _nextStepTime = Time.time + interval;

        foreach (var area in SilentArea.All)
            if (area.Contains(transform.position)) return;

        float intensity = isSprinting ? sprintIntensity : walkIntensity;
        SoundEvent.Emit(transform.position, intensity);
        OnFootstepEmitted?.Invoke(transform.position, intensity);
    }
}
