using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DebugInfluence : MonoBehaviour {

    [SerializeField] private LayerMask m_LayerMask;
    
    private InfluenceZone[] m_Influencers;
    [SerializeField] private float[] m_NormalizedWeights;

	// Update is called once per frame
	void Update () {
        transform.InfluenceQuery(m_LayerMask, "Untagged", out m_NormalizedWeights, out m_Influencers, typeof(CamRig)).NormalizeInfulenceWeights(ref m_NormalizedWeights);
	}
}
