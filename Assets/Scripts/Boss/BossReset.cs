using UnityEngine;
using UnityEngine.AI;

public class BossReset : MonoBehaviour, IResettable
{
    [SerializeField] private ObstacleResetManager _roomManager;

    private Vector3 _initPosition;
    private BossHealth _health;
    private BossAI _ai;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _initPosition = transform.position;
        _health = GetComponent<BossHealth>();
        _ai = GetComponent<BossAI>();
        _agent = GetComponent<NavMeshAgent>();

        if (_roomManager == null)
            _roomManager = FindAnyObjectByType<ObstacleResetManager>();
            // FindFirstObjectByType는 씬의 첫 번째 감지된<> 컴포넌트의 주소를 반환한다. (여러개가 있으면 첫 번째만 반환)
    }

    private void Start()
    {
        if (_roomManager != null)
            _roomManager.Register(this);
        else
            Debug.LogWarning("[BossReset] ObstacleResetManager를 찾을 수 없음 — 보스가 리셋되지 않습니다!");
    }

   public void Reset()
   {
    gameObject.SetActive(true);
    _ai.OnReset();               // 1. 상태·센서 초기화 (isStopped=true)
    _agent.Warp(_initPosition);  // 2. 워프 — isStopped=true이므로 재탐색 없음
    _health.Reset();

        Debug.Log("[BossReset] 보스 리셋");
    }
}
// parentheses closing class