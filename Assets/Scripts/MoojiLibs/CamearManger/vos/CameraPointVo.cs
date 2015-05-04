using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Mooji
{
    public class CameraPointVo : AI
    {

        public bool isDefaultMainCameraPoint = false ;
        /// <summary>
        /// 初始权重
        /// </summary>
        public float weight;
        /// <summary>
        /// 工具生成的 世界坐标
        /// </summary>
        public Vector3    worldPosition;
        /// <summary>
        /// 攻击生成的 世界rotation
        /// </summary>
        public Quaternion worldQuaternion;

        public bool isOnTriggerExitAddDefaultPoint = false;


        public Vector3 cp_forward;
        public Vector3 cp_up;

        /// <summary>
        /// 要跟随的对象
        /// </summary>
        private GameObject _followTarget;
        private Vector3 _followTargetPositionOffset = Vector3.zero;
        private CameraSwitchManager _csm;
        private SphereCollider _sc;
        private GameObject _mainCamera;
        private float _tempWeight = 0f;





        public void Awake()
        {
            _csm = GameObject.Find( "--CameraPoints Manager--" ).GetComponentInParent<CameraSwitchManager>();
            _sc = GetComponent<SphereCollider>();
            _mainCamera = GameObject.FindGameObjectWithTag( "MainCamera" );
            _tempWeight = weight;
        }

        public void OnTriggerEnter( Collider other )
        {
            if ( checkIsCurrLeadGo( other ) )
                _csm.someGoTriggerStateChanged( other , _sc , 1 );
        }
        public void OnTriggerStay( Collider other )
        {
            if ( checkIsCurrLeadGo( other ) )
                _csm.someGoTriggerStateChanged( other , _sc , 2 );
        }
        public void OnTriggerExit( Collider other )
        {
            if ( checkIsCurrLeadGo( other ) )
                _csm.someGoTriggerStateChanged( other , _sc , 3 );
        }

        private bool checkIsCurrLeadGo( Collider other )
        {
            PlayerUnitManager pum = other.GetComponent<PlayerUnitManager>();
            if ( pum == null )
                return false;

            return ( pum.gameObject == SceneBlackBoard.getInstance().getBlackBoard<PlayerInfoBlackBoard>().getCurrleadGo() );
        }



        //  =============================================================================================================

        public int getWeightedRatio( float weightSum )
        {
            return (int) ( this.weight / weightSum * 100 );
        }


        //  =============================================================================================================

        public void initDefaultCameraPoint()
        {
            _followTargetPositionOffset = _mainCamera.transform.position - _followTarget.transform.position;
        }

        public void setFollowerTarget(GameObject go)
        {
            _followTarget = go;
        }

        public void cameraFollowTarget()
        {
            if ( isDefaultMainCameraPoint )
            {
                if ( _followTarget == null )
                    return;

                joinLateUpdateCallBack( doFollowTarget );
            }
        }

        public void stopFollowTarget()
        {
            removeLateUpdateCallBack( doFollowTarget );
        }

        private void doFollowTarget()
        {
            if ( _mainCamera == null )
                return;

            _mainCamera.transform.position = Vector3.Lerp(

                _mainCamera.transform.position ,
                _followTarget.transform.position + _followTargetPositionOffset ,
                Time.deltaTime

                );


            _mainCamera.transform.rotation = Quaternion.Lerp( _mainCamera.transform.rotation , worldQuaternion , Time.deltaTime );
            // transform.position = Vector3.SmoothDamp( this.transform.position , targetTransfrom.transform.position + offSet , ref cameraVelocity , smoothTime , Mathf.Infinity , Time.deltaTime );
        }



        public void reSet()
        {
            this.weight = _tempWeight;

            stopFollowTarget();
        }
    }
}
