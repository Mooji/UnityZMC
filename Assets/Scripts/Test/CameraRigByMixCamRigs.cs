using UnityEngine;
using System.Collections;
using System.Linq;

public class CameraRigByMixCamRigs : MonoBehaviour {

    private InfluenceZone[] m_Influencers;
    [SerializeField] private float[] m_NormalizedWeights;
    [SerializeField] private Transform m_TargetRig;
    [SerializeField] private LayerMask m_LayerMask;

    
    void Update()
    {
        if (!m_TargetRig) return;
        
        transform.InfluenceQuery(m_LayerMask, "Untagged", out m_NormalizedWeights, out m_Influencers, typeof(CamRig)).NormalizeInfulenceWeights(ref m_NormalizedWeights);
        CamRig[] rigs = new CamRig[m_Influencers.Length];
        for (int i = 0; i < rigs.Length; i++)
        {
            rigs[i] = m_Influencers[i].GetTransform().GetComponentInChildren<CamRig>();
        }

        Vector3[] positions = rigs.Select<CamRig, Vector3>(rig => rig.position).ToArray();
        Vector3[] facings = rigs.Select<CamRig, Vector3>(rig => rig.facing).ToArray();
        Vector3[] ups = rigs.Select<CamRig, Vector3>(rig => rig.up).ToArray();
        m_TargetRig.position = m_TargetRig.position.Mix(positions, m_NormalizedWeights);
        m_TargetRig.LookAt(m_TargetRig.forward.Mix(facings, m_NormalizedWeights), m_TargetRig.up.Mix(ups, m_NormalizedWeights) - m_TargetRig.position);
        
        if (m_TargetRig.GetComponent<CamRig>() == null) m_TargetRig.gameObject.AddComponent<CamRig>();
        CamRig targetRig = m_TargetRig.GetComponent<CamRig>();

        float[] fovs = rigs.Select<CamRig, float>(rig => rig.fov).ToArray();
        targetRig.fov = targetRig.fov.Mix(fovs, m_NormalizedWeights);
    }

    void OnDrawGizmos()
    {
        if (m_TargetRig == null || m_Influencers == null || m_Influencers.Length < 1 || m_NormalizedWeights.Length < 1) return;
        
        for(int i=0; i<m_Influencers.Length ;i++){
            UnityEditor.Handles.color = new Color(1f, 1f, 0f, (i < m_NormalizedWeights.Length) ? m_NormalizedWeights[i] : 0f);
            UnityEditor.Handles.DrawDottedLine(m_TargetRig.position, m_Influencers[i].GetTransform().position, 10f);
        }

        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawLine(transform.position, m_TargetRig.position);
    }
}
