using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mooji
{
    public class CamFluencePointsFilter : MonoBehaviour , ICamFluencePointsFilter
    {

        [SerializeField]
        private List<ICamFluenceTriggerPoint>  _debugTriggerPointLst;
        private CamFilterIntent                 _camFilterIntent;
        private CamFilterResultInfo             _resultInfo;


        void Awake()
        {
            _resultInfo = new CamFilterResultInfo();
        }

        public CamFilterIntent createFilterIntent()
        {
            CameraInFluenceController controller =  GetComponent<CameraInFluenceController>();
            CamFilterIntent tempCamFilterIntent = new CamFilterIntent();
            tempCamFilterIntent.followTargetGhost = controller.ghostTarget;
            tempCamFilterIntent.layerMask = controller.layerMask;
            tempCamFilterIntent.requireComponentArr = null;
            tempCamFilterIntent.colliderRadius = 1f;
            return tempCamFilterIntent;
        }

        public bool filter( CamFilterIntent camFilterIntent , out CamFilterResultInfo info )
        {
            this._camFilterIntent = camFilterIntent;

            List<ICamFluenceTriggerPoint>  tempLst = createInfluenceQuery();

            //  过滤碰撞的 collider 存放入 _camInfiuencePointLst 中
            bool hasHit = tempLst.Count > 0;


            if ( tempLst.Count == 2 )
            {
                int a = 1;
            }

            if ( hasHit )
            {

                Vector3[] positions = tempLst.Select<ICamFluenceTriggerPoint , Vector3>( tp => tp.getCamRigPosition() ).ToArray();
                Vector3[] facings   = tempLst.Select<ICamFluenceTriggerPoint , Vector3>( tp => tp.getCamRigfacing() ).ToArray();
                Vector3[] ups       = tempLst.Select<ICamFluenceTriggerPoint , Vector3>( tp => tp.getCamRigUp() ).ToArray();
                float[][] weights   = tempLst.Select<ICamFluenceTriggerPoint , float[]>( tp => tp.getPositionAndQuaternionInfluenceWeight( camFilterIntent.followTargetGhost ) ).ToArray();


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

                _debugTriggerPointLst = tempLst;
            }

            info = _resultInfo;

            return hasHit;
        }



        private List<ICamFluenceTriggerPoint> createInfluenceQuery()
        {
            //  在碰撞范围内所有的碰撞体
            Collider[] hitColliderArr = Physics.OverlapSphere( _camFilterIntent.followTargetGhost.position , _camFilterIntent.colliderRadius , _camFilterIntent.layerMask );


            // 不要使用实例变量，没有 onColliderExit 。。。
            List<ICamFluenceTriggerPoint> tempLst = new List<ICamFluenceTriggerPoint>();


            for ( int i = 0 ; i < hitColliderArr.Length ; i++ )
            {
                Collider tempCollider = hitColliderArr[i];

                bool continueFlag = filterRequireComponent( tempCollider );

                if ( continueFlag )
                {
                    ICamFluenceTriggerPoint tempCamFluenceTriggerPoint = tempCollider.GetComponent<ICamFluenceTriggerPoint>();

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
