using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BossHealth))]
[RequireComponent(typeof(BossReset))]
public class BossAI : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 3.5f;
    public float reachThreshold = 0.5f;

    [Header("리셋 후 감지 유예 시간")]
    public float sensorGracePeriod = 2f;

    private NavMeshAgent _agent;
    private List<IBossSensor> _sensors = new List<IBossSensor>();
    private float _sensorActiveTime = 0f;

    private enum BossState { Idle, MovingToSound, Dead }
    private BossState _state = BossState.Idle;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = moveSpeed;

        // 같은 GameObject의 모든 센서 수집
        _sensors.AddRange(GetComponents<IBossSensor>());
    }

    private void OnEnable()
    {
        foreach (var sensor in _sensors)
            sensor.OnTargetDetected += SetTarget;
    }

    private void OnDisable()
    {
        foreach (var sensor in _sensors)
            sensor.OnTargetDetected -= SetTarget;
    }

    private void Update()
    {
        if (_state == BossState.Dead) return;

        if (Time.time >= _sensorActiveTime)
        {
            foreach (var sensor in _sensors)
                sensor.Tick();
        }

        UpdateMovement();
    }

    private void SetTarget(Vector3 pos)
    {
        if (_state == BossState.Dead) return;
        if (Time.time < _sensorActiveTime) return;
        _state = BossState.MovingToSound;
        _agent.isStopped = false;  // 실제 목표가 생길 때만 해제
        _agent.SetDestination(pos);
    }

    private void UpdateMovement()
    {
        if (_state != BossState.MovingToSound) return;
        if (!_agent.hasPath) return;

        if (_agent.remainingDistance <= reachThreshold)
        {
            _state = BossState.Idle;
            _agent.ResetPath();
        }
    }

    // BossHealth가 호출
    public void OnDead()
    {
        _state = BossState.Dead;
        _agent.isStopped = true;
        Debug.Log("[BossAI] 사망");
        // TODO: 사망 연출
    }

    // BossReset이 호출
    public void OnReset()
    {
        _state = BossState.Idle;
        _agent.isStopped = true;   // SetTarget()이 호출될 때까지 정지 유지
        _agent.ResetPath();
        _sensorActiveTime = Time.time + sensorGracePeriod;

        // Re-subscribe so sound sensing is always live after reset,
        // even if OnEnable was skipped (boss was already active).
        foreach (var sensor in _sensors)
        {
            sensor.OnTargetDetected -= SetTarget;
            sensor.OnTargetDetected += SetTarget;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, 2f);
    }
}