using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public interface InfluenceZone {
    float GetInfluence(Vector3 pos);
    Transform GetTransform();
}

public static class InfluenceEntensions
{
    public static Transform InfluenceQuery(this Transform trs, LayerMask mask, string tag, out float[] weights, out InfluenceZone[] zones)
    {
        List<InfluenceZone> Influencers = CheckInfluencers(trs.position, mask);
        Influencers.RemoveAll(z => !z.GetTransform().CompareTag(tag));

        zones = Influencers.ToArray();
        weights = new float[zones.Length];

        for (int i = 0; i < zones.Length; i++)
        {
            weights[i] = zones[i].GetInfluence(trs.position);
        }

        return trs;
    }

    public static Transform InfluenceQuery(this Transform trs, LayerMask mask, string tag, out float[] weights, out InfluenceZone[] zones, params Type[] RequireComponents)
    {
        List<InfluenceZone> Influencers = CheckInfluencers(trs.position, mask);
        Influencers.RemoveAll(z => !z.GetTransform().CompareTag(tag));
        foreach (Type c in RequireComponents)
        {
            Influencers.RemoveAll(z => (z.GetTransform().GetComponentInChildren(c) == null));
        }

        zones = Influencers.ToArray();
        weights = new float[zones.Length];

        for (int i = 0; i < zones.Length; i++)
        {
            weights[i] = zones[i].GetInfluence(trs.position);
        }

        return trs;
    }

    public static List<InfluenceZone> CheckInfluencers(Vector3 position, LayerMask mask)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 1f, mask);
        List<InfluenceZone> Influencers = new List<InfluenceZone>();
        foreach (Collider c in colliders)
        {
            InfluenceZone Influencer = c.GetComponent<InfluenceZone>();
            if (Influencer == null) continue;
            Influencers.Add(Influencer);
        }
        return Influencers;
    }

    public static Transform NormalizeInfulenceWeights(this Transform trs, ref float[] weights)
    {
        if (weights == null || weights.Length < 1) { return null; }
        float weightSum = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            weightSum += weights[i];
        }
        if (weightSum == 0) weightSum = 1f;
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] /= weightSum;
        }
        return trs;
    }

    public static Vector3 Mix(this Vector3 vector, Vector3[] vectors, float[] weights)
    {
        Vector3 mixed = Vector3.zero;
        for (int i = 0; i < vectors.Length; i++)
        {
            mixed += vectors[i] * ((i < weights.Length) ? weights[i] : 1f);
        }
        return mixed;
    }

    public static float Mix(this float f, float[] fs, float[] weights)
    {
        float mixed = 0;
        for (int i = 0; i < fs.Length; i++)
        {
            mixed += fs[i] * ((i < weights.Length) ? weights[i] : 1f);
        }
        return mixed;
    }
}