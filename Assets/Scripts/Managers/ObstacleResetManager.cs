using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleResetManager : MonoBehaviour
{
    [Header("리셋 대상 (IResettable 구현 오브젝트)")]
    public List<MonoBehaviour> resettableObjects;

    [Header("리셋 딜레이")]
    public float resetDelay = 1.5f;

    private List<IResettable> _resettables = new List<IResettable>();

    private void Awake()
    {
        foreach (var obj in resettableObjects)
        {
            if (obj == null)
            {
                Debug.LogWarning("[ObstacleResetManager] resettableObjects 목록에 null 항목이 있습니다.");
                continue;
            }
            if (obj is IResettable r)
                _resettables.Add(r);
            else
                Debug.LogWarning($"{obj.name} 은 IResettable을 구현하지 않음");
        }
    }

    public void Register(IResettable resettable)
    {
        if (!_resettables.Contains(resettable))
            _resettables.Add(resettable);
    }

    public void TriggerReset()
    {
        StartCoroutine(ResetRoutine());
    }

    private IEnumerator ResetRoutine()
    {
        yield return new WaitForSeconds(resetDelay);

        foreach (var r in _resettables)
            r.Reset();

        Debug.Log("[ObstacleResetManager] 리셋 완료");
    }
}