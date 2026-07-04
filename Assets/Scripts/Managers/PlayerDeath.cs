using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour, IDamageable
{
    [Header("연결")]
    public ObstacleResetManager roomManager;
    public Transform respawnPoint;

    [Header("체력")]
    public float maxHealth = 100f;
    private float _currentHealth;

    [Header("사망 딜레이")]
    public float deathDelay = 1.0f;

    private bool _isDead = false;
    private CharacterController _cc;

    private void Awake()
    {
        _currentHealth = maxHealth;
        _cc = GetComponent<CharacterController>();
        if (roomManager == null)
            roomManager = FindAnyObjectByType<ObstacleResetManager>();
    }

    public void TakeDamage(float amount)
    {
        if (_isDead) return;

        _currentHealth -= amount;
        Debug.Log($"[Player] 체력: {_currentHealth}/{maxHealth}");

        if (_currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        if (_isDead) return;
        StartCoroutine(DieRoutine());
    }

    private IEnumerator DieRoutine()
    {
        _isDead = true;
        Debug.Log("[Player] 사망");

        yield return new WaitForSeconds(deathDelay);

        Warp(respawnPoint != null ? respawnPoint.position : transform.position);

        _currentHealth = maxHealth;
        _isDead = false;

        if (roomManager != null)
            roomManager.TriggerReset();
    }

    public void Respawn()
    {
        Warp(respawnPoint != null ? respawnPoint.position : transform.position);
    }

    private void Warp(Vector3 position)
    {
        if (_cc != null)
        {
            _cc.enabled = false;
            transform.position = position;
            _cc.enabled = true;
        }
        else
        {
            transform.position = position;
        }
    }
}