using System;
using UnityEngine;

namespace Mooji
{
    public enum PositionBlendMode
    {
        /// <summary>
        /// 没有融合，只是移动到 Mixed resultPosition
        /// </summary>
        NONE ,
        /// <summary>
        /// 融合 （GhostPlayerOffset +( Mixed resultPosition + offset)） * .5f
        /// </summary>
        BLEND ,
        /// <summary>
        /// 从 Mixed resultPosition 发射一条射线，保持对 GhostPlayer 的相对位置 
        /// </summary>
        RAY ,
        /// <summary>
        /// 自定义一个 保持对 GhostPlayer 的相对位置
        /// </summary>
        CUSTOM

    }

    public class GhostCamRigDriver : MonoBehaviour , IGhostCamRigDriver
    {
        /// <summary>
        /// 默认的GhostCamRigPosition融合方式
        /// </summary>
        public PositionBlendMode positionBlendMode;
        /// <summary>
        /// 远近距离
        /// </summary>
        public float distanceOffset = 1f;
        /// <summary>
        /// PositionBlendMode.CUSTOM 模式下偏移量
        /// </summary>
        public Vector3 customBlendModeOffsetVec;
        /// <summary>
        /// 移动速度
        /// </summary>
        public float moveSpeed = 1f;
        /// <summary>
        /// 
        /// </summary>
        public float rotationSpeed = 1f;


        private CamInfluenceTriggerPoint _triggerPoint;
        private Transform _targetGhostPlayer;
        private Transform _targetGhostRig;
        private CamRig _cRig;
        private Vector3 _offsetVec3;
        private Vector3 _offsetVec3Ray = Vector3.zero;

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

            //  rotation
            Quaternion quaternionTo  =  Quaternion.LookRotation( resultInfo.resultMixedForward , resultInfo.resultMixedUp );
            _targetGhostRig.rotation = Quaternion.Lerp( _targetGhostRig.rotation , quaternionTo , Time.deltaTime * rotationSpeed );

            //  position 
            Vector3 positionTo = Vector3.zero;

            if ( resultInfo.hitTriggerPointLst.Count == 1 )
            {
                positionTo = _targetGhostPlayer.position + _offsetVec3 * distanceOffset;
            }

            else
            {
                switch ( positionBlendMode )
                {
                    case PositionBlendMode.NONE:
                    {
                        positionTo = resultInfo.resultMixedPosition;
                        break;
                    }
                    case PositionBlendMode.BLEND:
                    {
                        positionTo = ( resultInfo.resultMixedPosition + ( _targetGhostPlayer.position + _offsetVec3 * distanceOffset ) ) * .5f;
                        break;
                    }
                    case PositionBlendMode.RAY:
                    {
                        RaycastHit raycastInfo;
                        if ( Physics.Raycast( resultInfo.resultMixedPosition , _targetGhostRig.forward , out raycastInfo , 200 , LayerMask.GetMask( "gameLayer_clickInput" ) ) )
                        {
                            Debug.DrawLine( resultInfo.resultMixedPosition , raycastInfo.point , Color.blue , 1f );
                            _offsetVec3Ray = resultInfo.resultMixedPosition - raycastInfo.point;
                            positionTo = _targetGhostPlayer.position + _offsetVec3Ray * distanceOffset;
                        }

                        break;
                    }
                    case PositionBlendMode.CUSTOM:
                    {
                        _offsetVec3Ray = resultInfo.resultMixedPosition - customBlendModeOffsetVec ;
                        positionTo = _targetGhostPlayer.position + _offsetVec3Ray * distanceOffset;
                        break;
                    }
                }
            }

            _targetGhostRig.position = Vector3.Lerp( _targetGhostRig.position , positionTo , Time.deltaTime * moveSpeed );





        }

        public void init( CamDriverIntent intent )
        {
            if ( this._targetGhostPlayer != null && this._targetGhostRig != null )
                return;

            this._targetGhostPlayer = intent.ghostTarget;
            this._targetGhostRig = intent.ghostCamRig;


            if ( this._targetGhostRig.GetComponentInChildren<CamRig>() == null )
                throw new Exception( "指定的GhostRig Go 必须包含一个 CamRig 组件" );


            this._targetGhostRig.position = _cRig.transform.position;
            this._targetGhostRig.rotation = _cRig.transform.rotation;

            this._offsetVec3 = _cRig.transform.position - _targetGhostPlayer.position;


        }
    }
}
