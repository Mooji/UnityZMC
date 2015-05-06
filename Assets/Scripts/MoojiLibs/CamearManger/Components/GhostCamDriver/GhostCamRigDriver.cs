using System;
using UnityEngine;

namespace Mooji
{
    public class GhostCamRigDriver : MonoBehaviour , IGhostCamRigDriver
    {
        private CamInfluenceTriggerPoint _triggerPoint;
        private Transform _targetGhostPlayer;
        private Transform _targetGhostRig;
        private CamRig _cRig;
        private Vector3 _offsetVec3;

        void Awake()
        {
            _cRig = GetComponentInChildren<CamRig>();

            _triggerPoint = GetComponent<CamInfluenceTriggerPoint>();

            if ( _cRig == null )
                throw new Exception( "自己 或 子物体必须包含一个 CamRig 组件, @see CamRig" );

            if ( _triggerPoint == null )
                throw new Exception( "自己必须包含一个 CamInfluenceTriggerPoint 组件, @see CamInfluenceTriggerPoint" );

        }

        public void run( CamFilterResultInfo resultInfo )
        {
            if ( _targetGhostPlayer == null || _triggerPoint == null )
                return;

            Quaternion quaternionTo     =  Quaternion.LookRotation( resultInfo.resultMixedForward , resultInfo.resultMixedUp );
            Vector3 positionTo          = ( ( _targetGhostPlayer.position + _offsetVec3 ) + resultInfo.resultMixedPosition ) * 0.5f;
            _targetGhostRig.position = Vector3.Lerp( _targetGhostRig.position , positionTo , Time.deltaTime * _triggerPoint.moveSpeed );
            _targetGhostRig.rotation = Quaternion.Lerp( _targetGhostRig.rotation , quaternionTo , Time.deltaTime * _triggerPoint.rotationSpeed );

        }

        public void init( Transform targetGhostPlayer , Transform targetGhostRig )
        {
            if ( this._targetGhostPlayer != null && this._targetGhostRig != null )
                return;

            this._targetGhostPlayer = targetGhostPlayer;
            this._targetGhostRig = targetGhostRig;

            this._targetGhostRig.position = _cRig.transform.position;
            this._targetGhostRig.rotation = _cRig.transform.rotation;

            this._offsetVec3 = _cRig.transform.position - targetGhostPlayer.position;


        }
    }
}
