using UnityEngine;
using System.Collections.Generic;

public class SilentArea : MonoBehaviour
{
    public static readonly List<SilentArea> All = new List<SilentArea>();

    private Collider _col;

    private void Awake() => _col = GetComponent<Collider>();
    private void OnEnable() => All.Add(this);
    private void OnDisable() => All.Remove(this);

    public bool Contains(Vector3 point) => _col != null && _col.bounds.Contains(point);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 0.5f, 1f, 0.2f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}

