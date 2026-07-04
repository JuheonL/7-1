using UnityEngine;

public class BossContact : MonoBehaviour
{
    [Header("접촉 데미지")]
    public float damageAmount = 100f;
    public float contactRadius = 1f;
    public Vector3 contactCenter = new Vector3(0f, -1f, 0f);
    public string playerTag = "Player";

    private bool _inContact = false;

    private void Update()
    {
        Vector3 spherePos = transform.position + transform.TransformDirection(contactCenter);
        Collider[] hits = Physics.OverlapSphere(spherePos, contactRadius);

        foreach (var hit in hits)
        {
            if (!hit.CompareTag(playerTag)) continue;

            if (!_inContact)
            {
                _inContact = true;
                IDamageable target = hit.GetComponentInParent<IDamageable>();
                if (target == null)
                    target = hit.transform.root.GetComponentInChildren<IDamageable>();

                Debug.Log($"[BossContact] Player 감지 — IDamageable: {(target == null ? "NULL" : target.GetType().Name)}");
                target?.TakeDamage(damageAmount);
            }
            return;
        }

        _inContact = false;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 spherePos = transform.position + transform.TransformDirection(contactCenter);
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(spherePos, contactRadius);
    }
}
