using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class InfluenceBox : MonoBehaviour,InfluenceZone {

    [SerializeField] private float weight;

    private BoxCollider col;

    public float GetInfluence(Vector3 pos)
    {
        if (!col) col = GetComponent<BoxCollider>();
        pos = transform.InverseTransformPoint(pos);
        Bounds bound = new Bounds(col.center, col.size);
        return bound.Contains(pos) ? weight : 0f;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
