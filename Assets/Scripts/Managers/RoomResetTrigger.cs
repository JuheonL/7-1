using UnityEngine;

public class RoomResetTrigger : MonoBehaviour
{
    public ObstacleResetManager roomManager;
    public string playerTag = "Player";

    private bool _playerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        _playerInside = true;
    }

    private void Awake()
    {
        if (roomManager == null)
            roomManager = FindAnyObjectByType<ObstacleResetManager>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (!_playerInside) return;

        _playerInside = false;

        if (roomManager != null)
            roomManager.TriggerReset();
        else
            Debug.LogWarning("[RoomResetTrigger] roomManager가 없어 리셋할 수 없습니다.");
    }
}