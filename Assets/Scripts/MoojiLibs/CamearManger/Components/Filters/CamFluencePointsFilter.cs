using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mooji
{
    public class CamFluencePointsFilter : MonoBehaviour , ICamFluencePointsFilter
    {

        [SerializeField]
        private List<CamInfluenceTriggerPoint> debugTriggerPointLst;


        private CamFilterIntent                 _camFilterIntent;
        private CamFilterResultInfo             _resultInfo;
        private float _currAllWeightSum         = 0f;


        void Awake()
        {

            _resultInfo = new CamFilterResultInfo();
        }

        public bool filter( CamFilterIntent camFilterIntent , out CamFilterResultInfo info )
        {
            this._camFilterIntent = camFilterIntent;

            List<CamInfluenceTriggerPoint>  tempLst = createInfluenceQuery();

            //  过滤碰撞的 collider 存放入 _camInfiuencePointLst 中
            bool hasHit = tempLst.Count > 0;

            if ( hasHit )
            {
                Vector3[] positions = tempLst.Select<CamInfluenceTriggerPoint , Vector3>( tp => tp.getCamRigPosition() ).ToArray();
                Vector3[] facings   = tempLst.Select<CamInfluenceTriggerPoint , Vector3>( tp => tp.getCamRigfacing() ).ToArray();
                Vector3[] ups       = tempLst.Select<CamInfluenceTriggerPoint , Vector3>( tp => tp.getCamRigUp() ).ToArray();
                float[][] weights   = tempLst.Select<CamInfluenceTriggerPoint , float[]>( tp => tp.getPositionAndQuaternionInfluenceWeight( camFilterIntent.followTargetGhost ) ).ToArray();


                Vector3 mixedPositionVec3   = Vector3.zero;
                Vector3 mixedUpVec3         = Vector3.zero;
                Vector3 mixedForwardVec3    = Vector3.zero;
                float   posWeightSum        = 0f;
                float   qWeightSum          = 0f;

                for ( int i = 0 ; i < weights.Length ; i++ )
                {

                    float posW = weights[i][0];
                    float qW = weights[i][1];

                    posWeightSum += posW;
                    qWeightSum += qW;

                    mixedPositionVec3 += positions[i] * posW;
                    mixedForwardVec3 += facings[i] * qW;
                    mixedUpVec3 += ups[i] * qW;
                }

                _resultInfo.resultMixedPosition = mixedPositionVec3 / posWeightSum;
                _resultInfo.resultMixedForward = mixedForwardVec3 / qWeightSum;
                _resultInfo.resultMixedUp = mixedUpVec3 / qWeightSum;
                _resultInfo.hitTriggerPointLst = tempLst;

                debugTriggerPointLst = tempLst;
            }

            info = _resultInfo;

            return hasHit;
        }



        private List<CamInfluenceTriggerPoint> createInfluenceQuery()
        {
            //  在碰撞范围内所有的碰撞体
            Collider[] hitColliderArr = Physics.OverlapSphere( _camFilterIntent.followTargetGhost.position , _camFilterIntent.colliderRadius , _camFilterIntent.layerMask );


            // 不要使用实例变量，没有 onColliderExit 。。。
            List<CamInfluenceTriggerPoint> tempLst = new List<CamInfluenceTriggerPoint>();


            for ( int i = 0 ; i < hitColliderArr.Length ; i++ )
            {
                Collider tempCollider = hitColliderArr[i];

                bool continueFlag = filterRequireComponent( tempCollider );

                if ( continueFlag )
                {
                    CamInfluenceTriggerPoint tempCamFluenceTriggerPoint = tempCollider.GetComponent<CamInfluenceTriggerPoint>();

                    if ( !tempLst.Contains( tempCamFluenceTriggerPoint ) )
                    {
                        tempLst.Add( tempCamFluenceTriggerPoint );
                    }
                }

            }

            return tempLst;
        }


        private bool filterRequireComponent( Collider collider )
        {
            if ( _camFilterIntent.requireComponentArr == null || _camFilterIntent.requireComponentArr.Length == 0 )
                return true;


            foreach ( Type requireType in _camFilterIntent.requireComponentArr )
            {
                if ( collider.GetComponentInChildren( requireType ) == null )
                {
                    return false;
                }
            }

            return true;
        }

    }
}
