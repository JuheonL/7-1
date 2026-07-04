using UnityEngine;

public class DamageObstacle : MonoBehaviour, IResettable
{
    [Header("설정")]
    public float damageAmount = 25f;

    [Header("연출 (선택)")]
    public GameObject destroyEffectPrefab;
    public AudioClip destroySound;

    private Vector3 _initPosition;
    private Quaternion _initRotation;
    private bool _isDestroyed = false;

    private void Awake()
    {
        _initPosition = transform.position;
        _initRotation = transform.rotation;
    }

    private void Start()
    {
        var manager = FindAnyObjectByType<ObstacleResetManager>();
        manager?.Register(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isDestroyed) return;

        // IDamageable 구현 여부만 확인 — 구체 클래스 몰라도 됨
        IDamageable target = collision.gameObject.GetComponent<IDamageable>();
        if (target == null) return;

        target.TakeDamage(damageAmount);
        DestroyObstacle();
    }

    private void DestroyObstacle()
    {
        _isDestroyed = true;

        if (destroyEffectPrefab != null)
            Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
        if (destroySound != null)
            AudioSource.PlayClipAtPoint(destroySound, transform.position);

        gameObject.SetActive(false);
    }

    public void Reset()
    {
        _isDestroyed = false;
        transform.position = _initPosition;
        transform.rotation = _initRotation;
        gameObject.SetActive(true);
    }
}