using UnityEngine;

public class BossHealth : MonoBehaviour, IHealth, IResettable
{
    [Header("체력 설정")]
    public float maxHealth = 100f;

    public float CurrentHealth { get; private set; }

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth -= amount;
        Debug.Log($"[BossHealth] 체력: {CurrentHealth}/{maxHealth}");

        if (CurrentHealth <= 0)
            GetComponent<BossAI>().OnDead();
    }

    public void Reset()
    {
        CurrentHealth = maxHealth;
        Debug.Log("[BossHealth] 체력 리셋");
    }
}