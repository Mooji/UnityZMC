using System;
using UnityEngine;

namespace Mooji
{

    public class GhostCamRigDriver : MonoBehaviour , IGhostCamRigDriver
    {
        /// <summary>
        /// 远近距离
        /// </summary>
        public float distanceOffset = 1f;
        private ICamFluenceTriggerPoint _triggerPoint;


        void Awake()
        {
            _triggerPoint = GetComponent<ICamFluenceTriggerPoint>();
        }


        public void run( CamFilterResultInfo resultInfo )
        {
            // position
            Vector3 posVec = Vector3.Lerp( _triggerPoint.getCamRig().transform.position , _triggerPoint.getCamRigPosition() , Time.deltaTime );
            _triggerPoint.setCamRigPosition( posVec );

            ////  rotation
            //Quaternion quaternionTo  = Quaternion.LookRotation( resultInfo.resultMixedForward , resultInfo.resultMixedUp );
            //Quaternion resultQ = Quaternion.Lerp( _triggerPoint.getCamRigRotation() , quaternionTo , Time.deltaTime );
            //_triggerPoint.setCamRigRotation( quaternionTo );

        }

        public void init( CamDriverIntent intent )
        {
        }
    }
}
