using UnityEngine;

public class RoomExitTrigger : MonoBehaviour
{
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        PlayerDeath playerDeath = other.GetComponent<PlayerDeath>();
        if (playerDeath != null)
            playerDeath.Respawn();
        else
            Debug.LogWarning("[RoomExitTrigger] Player에 PlayerDeath 컴포넌트가 없습니다.");
    }
}
