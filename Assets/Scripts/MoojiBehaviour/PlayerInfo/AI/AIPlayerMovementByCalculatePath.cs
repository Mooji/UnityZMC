using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Mooji
{
    [RequireComponent( typeof( Animator ) )]
    [RequireComponent( typeof( Rigidbody ) )]
    public class AIPlayerMovementByCalculatePath : AIMovement , ISubscriber , IPlayerMove
    {
        //  =================================================================
        /// <summary>
        /// animator中 控制转向的 变量
        /// </summary>
        public string aniKeyTrun        = "Turn";
        /// <summary>
        /// animator中 控制移动的 变量
        /// </summary>
        public string aniKeyForward     = "Forward";
        /// <summary>
        /// 急转弯角度
        /// </summary>
        public float sharpTurnAngle = 145f;
        /// <summary>
        /// 急转弯速度
        /// </summary>
        public float sharpTrunSpeed = .15f;
        /// <summary>
        /// 旋转系数
        /// </summary>
        public float trunDampTime = 0.2f;

        //  =================================================================
        /// <summary>
        /// 帮助转身最大角度
        /// </summary>
        public float trunFastMaxAngle = 360f;
        /// <summary>
        /// 帮助转身最小角度
        /// </summary>
        public float trunFastMinAngel = 180f;
        /// <summary>
        /// 移动系数
        /// </summary>
        public float moveDampTime = 0.2f;
        /// <summary>
        /// 移动减慢系数最小值
        /// </summary>
        public float moveStopOffsetMin = 1.5f;
        /// <summary>
        /// 移动减慢系数最大值
        /// </summary>
        public float moveStopOffsetMax = 15f;
        /// <summary>
        /// 最大移动速度
        /// </summary>
        public float movementMaxSpeed = 5.66f;

        //  =================================================================
        /// <summary>
        /// 当前可以移动NavmeshPath
        /// </summary>
        private NavMeshPath                 _currCanMoveNavMeshPath;
        /// <summary>
        /// 当前要移动的vec3
        /// </summary>
        private Vector3                     _currPathVec3;
        /// <summary>
        /// 当前路径index
        /// </summary>
        private int                         _currPathIndex = 0;
        /// <summary>
        /// 
        /// </summary>
        private float                       _forwardAmount;
        /// <summary>
        /// 动画组件
        /// </summary>
        private Animator                    _ani;
        /// <summary>
        /// 急转的Tweener
        /// </summary>
        private Tweener                     _lookAtTweener = null;

        private GameObject                  _lookAtPositionDeviceGo;
        private GameObject                  _acceptAttackMask;
        private Vector3                     _currMovevelocity;
        private GameConfigVo                _gameConfigVo;


        //  ========================= AIMovement =========================
        //protected override PlayerMovementDescribeVo absCreatePlayerDescribeVo()
        //{
        //    PlayerMovementDescribeVo pmd = new PlayerMovementDescribeVo();
        //    pmd.isAddClickGroundMask = true;
        //    pmd.playerControlerType = PlayerConstant.PlayerControlerType.INCONTROLLABLE;
        //    return pmd;
        //}

        //protected override void absClickGroundMaskAddComplete( GameObject _maskGo )
        //{
        //    _maskGo.SetActive( false );
        //}

        protected override void absStart()
        {

            _gameConfigVo = BMC.getGameConfigBehaviour().gameConfigVo;

            _ani = GetComponent<Animator>();
            _ani.applyRootMotion = true;
            _ani.updateMode = AnimatorUpdateMode.Normal;
            _ani.cullingMode = AnimatorCullingMode.AlwaysAnimate;


            playerRigidbody.useGravity = true;
            playerRigidbody.isKinematic = false;
            playerRigidbody.detectCollisions = !playerRigidbody.isKinematic;
            playerRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            playerRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            playerRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;



            //  构建一个lookAt朝向定位球 最后一个路径 朝向有点小偏差
            _lookAtPositionDeviceGo = new GameObject();
            _lookAtPositionDeviceGo.name = "[朝向]定位球";


            BMC.getObserverBehaviour().registerMsg( this );

        }

        //  ========================= impl ISubscriber =========================
        public List<ObserverMsgTypeEnum> subscriberMessages()
        {
            List<ObserverMsgTypeEnum> lst = new List<ObserverMsgTypeEnum>();
            lst.Add( ObserverMsgTypeEnum.PLAYER_CONTROLER_CHANGED );
            lst.Add( ObserverMsgTypeEnum.PLAYER_FOLLOW_TARGET_CHANGED );
            lst.Add( ObserverMsgTypeEnum.INPUT_CLICKED_HIT_3D_GAME_OBJECT );
            //lst.Add( ObserverMsgTypeEnum.PLAYER_TRIGGER_MOVEMENT_POSITION_DEVICE );
            return lst;
        }

        public void OnTriggerEnter( Collider other )
        {
            Debug.Log( " hit " + other.name );
        }



        //  ====================================    [IPlayerMove]   =====================================
        /// <summary>
        /// 
        /// </summary>
        /// <param name="navMeshPath"></param>
        public void doMove( Vector3 targetPosition )
        {
            clearNavMeshPath();

            if ( _currCanMoveNavMeshPath == null )
                _currCanMoveNavMeshPath = new NavMeshPath();



            if ( NavMesh.CalculatePath( playerRigidbody.position , targetPosition , NavMesh.GetAreaFromName( _gameConfigVo.gameLayer_unit_movement ) , _currCanMoveNavMeshPath ) )
            {

                _ani.SetFloat( "OnAttack" , 0 );
                _ani.SetFloat( "OnUnderAttack" , 0 );

                //  有可移动路径
                if ( _currCanMoveNavMeshPath != null )
                {

                    //BMC.getObserverBehaviour().publisheMsgByCoroutine( ObserverMsgTypeEnum.PLAYER_MOVEMENT_STATE_CHANGED , new object[] { this.gameObject , PlayerConstant.PlayerMovementState.READY_TO_MOVE } );

                    ////  startMoving
                    //joinUpdateCallBack( this.moving );

                    //BMC.getObserverBehaviour().publisheMsgByCoroutine( ObserverMsgTypeEnum.PLAYER_MOVEMENT_STATE_CHANGED , new object[] { this.gameObject , PlayerConstant.PlayerMovementState.MOVING } );


                    //if ( _acceptAttackMask == null )
                    //    _acceptAttackMask = BMC.getPrefabsBehaviour().instantiatePrefab( PrefbasAssets.PrefabAssetsConstant.CLICKED_GROUND_MASK , false );

                    //_acceptAttackMask.transform.position = targetPosition;


                    //int pathLen = _currCanMoveNavMeshPath.corners.Length;
                    //Vector3 lastOnePathVec = _currCanMoveNavMeshPath.corners[pathLen - 1];
                    //Vector3 lastTwoPathVec = _currCanMoveNavMeshPath.corners[pathLen - 2];
                    //Vector3 offsetVec = lastOnePathVec - lastTwoPathVec;
                    //Vector3 direction = offsetVec / offsetVec.magnitude;
                    //_lookAtPositionDeviceGo.transform.position = lastOnePathVec;
                    //_lookAtPositionDeviceGo.transform.position += direction * .5f;

                }
            }

        }
        //  ====================================    [IPlayerMove end ]   =====================================
        public void receivedMessage( ObserverMsgTypeEnum messageType , object paramsObj , PublisheOptionVo options )
        {
            switch ( messageType )
            {

                //case ObserverMsgTypeEnum.PLAYER_TRIGGER_MOVEMENT_POSITION_DEVICE:
                //{

                //    //  是否还有下一条路径
                //    if ( hasNextPath() )
                //    {
                //        _currPathIndex += 1;
                //        _currPathVec3 = _currCanMoveNavMeshPath.corners[_currPathIndex];
                //    }
                //    else
                //    {
                //        _ani.SetFloat( aniKeyTrun , 0 );
                //        _ani.SetFloat( aniKeyForward , 0 );
                //        removeUpdateCallBack( this.moving );
                //    }

                //    break;
                //}


                case ObserverMsgTypeEnum.PLAYER_CONTROLER_CHANGED:
                {

                    //object[] objs                                       = paramsObj as object[];
                    //List<GameObject> goLst                              = objs[0] as List<GameObject>;
                    //List < PlayerConstant.PlayerControlerType> pctLst   = objs[1] as List<PlayerConstant.PlayerControlerType>;

                    //int index =  goLst.IndexOf( this.gameObject );

                    //if ( index >= 0 )
                    //{
                    //    PlayerConstant.PlayerControlerType pct = pctLst[index];

                    //    switch ( pct )
                    //    {
                    //        case PlayerConstant.PlayerControlerType.PROTAGONIST:
                    //        {
                    //            break;

                    //        }
                    //        case PlayerConstant.PlayerControlerType.FOLLOWER:
                    //        {
                    //            break;
                    //        }
                    //        case PlayerConstant.PlayerControlerType.INCONTROLLABLE:
                    //        {
                    //            break;
                    //        }

                    //    }

                    //    //base.maskGo.SetActive( true );

                    //    base.playerDescribeVo.playerControlerType = pct;
                    //}

                    break;
                }


                case ObserverMsgTypeEnum.PLAYER_FOLLOW_TARGET_CHANGED:
                {
                    //GameObject followTargetGo = paramsObj as GameObject;

                    //if ( followTargetGo == this.gameObject )
                    //{
                    //    base.playerDescribeVo.floowOffsetVec3 = Vector3.zero;
                    //    return;
                    //}

                    ////  =============== 应该从 数据库配置文件取 这个对象 应该在那个位置   ===============
                    //if ( this.gameObject.name == "player1" )
                    //{
                    //    base.playerDescribeVo.floowOffsetVec3 = followTargetGo.transform.position - new Vector3( 1.5f , 0 , 0 );
                    //}
                    //else if ( this.gameObject.name == "player2" )
                    //{
                    //    base.playerDescribeVo.floowOffsetVec3 = followTargetGo.transform.position - new Vector3( -1.5f , 0 , 0 );
                    //}
                    //else if ( this.gameObject.name == "player3" )
                    //{
                    //    base.playerDescribeVo.floowOffsetVec3 = followTargetGo.transform.position - new Vector3( 0f , 0 , 1.5f );
                    //}
                    //else if ( this.gameObject.name == "player4" )
                    //{
                    //    base.playerDescribeVo.floowOffsetVec3 = followTargetGo.transform.position - new Vector3( 0f , 0 , -1.5f );
                    //}
                    ////  =============== 应该从 数据库配置文件取 这个对象 应该在那个位置   ===============

                    break;
                }

            }// end switch ...

        }


        protected void moving()
        {

            doCheckCurrPath();

            doCreateCurrPathMovevelocity();

            doLookAtCurrPath();

            doMoveToCurrPath();


            float tempCurrPathAngle  = Vector3.Angle( _currPathVec3 - this.playerRigidbody.position , this.transform.forward );

            if ( tempCurrPathAngle >= sharpTurnAngle )
            {
                _ani.SetFloat( aniKeyForward , 0f );
                _ani.SetFloat( aniKeyTrun , 0f );

                removeUpdateCallBack( this.moving );

                joinUpdateCallBack( this.doLookAtCurrPath );

                if ( _lookAtTweener != null )
                    _lookAtTweener.Kill( false );

                _lookAtTweener = this.transform.FindChild( "HelpRotationGO" ).DOLookAt( _currPathVec3 , sharpTrunSpeed , AxisConstraint.Y ).OnComplete( () =>
                {
                    removeUpdateCallBack( this.doLookAtCurrPath );
                    joinUpdateCallBack( this.moving );

                } );

            }
        }

        //private void invokeReStartMoving()
        //{
        //    removeUpdateCallBack( this.doCheckCurrPath );
        //    removeUpdateCallBack( this.doCreateCurrPathMovevelocity );
        //    removeUpdateCallBack( this.doLookAtCurrPath );
        //    joinUpdateCallBack( this.moving );

        //}

        private void doCheckCurrPath()
        {
            if ( _currPathIndex == 0 )
            {
                _currPathIndex = 1;
                _currPathVec3 = _currCanMoveNavMeshPath.corners[_currPathIndex];
            }
            else if ( _currPathIndex >= 1 && hasNextPath() )
            {
                float tempCurrPathSurplusDistance   = Vector3.Distance( _currPathVec3 , this.transform.position );

                if ( tempCurrPathSurplusDistance <= .5f )
                {
                    _currPathIndex += 1;
                    _currPathVec3 = _currCanMoveNavMeshPath.corners[_currPathIndex];
                }
            }
        }

        private void doCreateCurrPathMovevelocity()
        {
            _currMovevelocity = _currPathVec3 - this.transform.position;
            _currMovevelocity = this.transform.InverseTransformDirection( _currMovevelocity );
            _currMovevelocity = Vector3.ProjectOnPlane( _currMovevelocity , Vector3.up );

        }


        private void doLookAtCurrPath()
        {
            float turnAmount = 0f;
            float turnSpeed  = 0f;

            if ( _currPathIndex == _currCanMoveNavMeshPath.corners.Length - 1 )
            {
                Vector3 tempVelocity    = _lookAtPositionDeviceGo.transform.position - this.transform.position;
                tempVelocity = this.transform.InverseTransformDirection( tempVelocity );
                tempVelocity = Vector3.ProjectOnPlane( tempVelocity , Vector3.up );
                turnAmount = Mathf.Atan2( tempVelocity.x , tempVelocity.z );
            }
            else
            {
                turnAmount = Mathf.Atan2( _currMovevelocity.x , _currMovevelocity.z );
            }


            _ani.SetFloat( aniKeyTrun , turnAmount , trunDampTime , getDeltaTime() );
            turnSpeed = Mathf.Lerp( trunFastMaxAngle , trunFastMinAngel , _forwardAmount );
            transform.Rotate( 0 , turnAmount * turnSpeed * getDeltaTime() , 0 );
        }

        private void doMoveToCurrPath()
        {

            //float speed = Vector3.Dot( this.playerRigidbody.velocity , transform.forward );

            _forwardAmount = _currMovevelocity.z;

            if ( _currPathIndex >= 1 && _currPathIndex < _currCanMoveNavMeshPath.corners.Length - 1 )
            {
                _forwardAmount = movementMaxSpeed;
            }
            else if ( _currPathIndex == _currCanMoveNavMeshPath.corners.Length - 1 )
            {
                _forwardAmount *= Mathf.Lerp( moveStopOffsetMin , moveStopOffsetMax , getDeltaTime() * _forwardAmount );
            }

            _ani.SetFloat( aniKeyForward , _forwardAmount , moveDampTime , getDeltaTime() );

        }

        //  ===================================================     [private]       ===================================================

        private Boolean hasNextPath()
        {
            if ( _currCanMoveNavMeshPath == null )
                return false;

            int tempIndex = _currPathIndex + 1;

            if ( tempIndex < _currCanMoveNavMeshPath.corners.Length )
            {
                return true;
            }
            return false;
        }


        private void clearNavMeshPath()
        {
            _currPathIndex = 0;

            if ( _currCanMoveNavMeshPath != null )
            {
                _currCanMoveNavMeshPath.ClearCorners();
            }
        }


        private float getDeltaTime()
        {
            return Time.fixedDeltaTime;
        }

        //private Tweener _moveTweener;
        //public void OnAnimatorMove()
        //{
        //    if ( _moveTweener != null )
        //        _moveTweener.Kill();

        //    _moveTweener = this.transform.DOMove( _currPathVec3 , Vector3.Distance( this.transform.position , _currPathVec3 ) / .5f ).SetEase(Ease.Linear);
        //}




    }
}
