using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class InfluenceSphere : MonoBehaviour, InfluenceZone {

    [SerializeField] private float weight;

    private SphereCollider col;

    public float GetInfluence(Vector3 pos)
    {
        if (!col) col = GetComponent<SphereCollider>();
        return (col.radius > 0) && ((pos - transform.position).sqrMagnitude < (col.radius * col.radius)) ? weight : 0f;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
