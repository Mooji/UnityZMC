using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

namespace Mooji
{
    public class AIPlayerMovementByTweener : AIMovement , ISubscriber
    {
        /// <summary>
        /// 
        /// </summary>                  
        public string aniKeyOnGround = "OnGround";
        /// <summary>
        /// animator中 控制转向的 变量
        /// </summary>
        public string aniKeyTrun        = "Turn";
        /// <summary>
        /// animator中 控制移动的 变量
        /// </summary>
        public string aniKeyForward     = "Forward";
        /// <summary>
        /// 移动的最大速度 5.66米/s
        /// </summary>
        public float moveMaxSpeed       = 5.66f;
        /// <summary>
        /// 旋转的完毕的速度
        /// </summary>
        public float rotationSpeed      = .15f;
        /// <summary>
        /// 快到达目的地后，从跑 -》 走 -》 停 Ani的 间隔
        /// </summary>
        public float runToWalkSpeed     = 0.2f;

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
        /// 动画组件
        /// </summary>
        private Animator                    _ani;
        /// <summary>
        /// 控制转向的Tweener
        /// </summary>
        private Tweener                     _lookAtTweener      = null;
        /// <summary>
        /// 控制移动的Tweener
        /// </summary>
        private Tweener                     _movementTweener    = null;
        /// <summary>
        /// 控制移动完毕后的 收尾ani 从跑到走再到停止的Tweenwe
        /// </summary>
        private Tweener                     _moveCompleteTweener = null;
        /// <summary>
        /// 点击地面后 生成一个 点击标志
        /// </summary>
        private GameObject                  _locationMaskGo = null;

        private TweenParams _tpLookAt;
        private TweenParams _tpMovement;

        private Transform _keepLookAtTF;

        private int _navMeshPathCornersLen = 0;

        //  =================================================================

        //  ========================= impl ISubscriber =========================
        public List<ObserverMsgTypeEnum> subscriberMessages()
        {
            List<ObserverMsgTypeEnum> lst = new List<ObserverMsgTypeEnum>();
            //lst.Add( ObserverMsgTypeEnum.INPUT_CLICKED_HIT_GAME_OBJECT );
            return lst;
        }
        public void receivedMessage( ObserverMsgTypeEnum messageType , object paramsObj , PublisheOptionVo options )
        {
            switch ( messageType )
            {
                case ObserverMsgTypeEnum.INPUT_CLICKED_HIT_3D_GAME_OBJECT:
                {
                    moveToByClickFloor( ( (RaycastHit) paramsObj ).point );
                    break;
                }
            }// end switch ...
        }


        //  ========================= Behaviour LifeCycle =========================
        public void OnDestroy()
        {
            hideLocaltionMask( true );

            cleanAllTweeners();

            if ( _currCanMoveNavMeshPath != null )
            {
                _currCanMoveNavMeshPath.ClearCorners();
                _currCanMoveNavMeshPath = null;
            }

        }

        protected override void absStart()
        {
            _currCanMoveNavMeshPath = new NavMeshPath();

            _tpLookAt = new TweenParams().SetEase( Ease.Linear );
            _tpMovement = new TweenParams().SetEase( Ease.Linear ).OnUpdate( this.tweenOnMovementUpdate ).OnComplete( this.tweenOnMovementComplete );

            _ani = GetComponent<Animator>();
            _ani.applyRootMotion = false;
            _ani.SetBool( aniKeyOnGround , true );

            BMC.getObserverBehaviour().registerMsg( this );
        }


        //  ====================================    [IPlayerMove]   =====================================
        /// <summary>
        /// 点击地板后移动
        /// </summary>
        /// <param name="navMeshPath"></param>
        public void moveToByClickFloor( Vector3 targetPosition )
        {
            NavMeshHit nmhit;
            Debug.Log( "TgtPos " + targetPosition );
            if ( NavMesh.SamplePosition( targetPosition , out nmhit , Mathf.Infinity , -1 ) )
            {
                Debug.Log( "NMTgtPos " + nmhit.position );

                if ( NavMesh.CalculatePath( this.transform.position , nmhit.position , base.gameLayerByMovement , _currCanMoveNavMeshPath ) )
                {

                    _navMeshPathCornersLen = _currCanMoveNavMeshPath.corners.Length;
                    _currPathIndex = 0;
                    _currPathVec3 = _currCanMoveNavMeshPath.corners[_currPathIndex];

                    doMoveToByClickFloor();

                    showLocaltionMask( targetPosition );
                }
            }
        }

        protected void showLocaltionMask( Vector3 position )
        {
            if ( _locationMaskGo == null )
                _locationMaskGo = BMC.getPrefabsBehaviour().instantiatePrefab( PrefbasAssets.PrefabAssetsConstant.CLICKED_GROUND_MASK , true );

            if ( !_locationMaskGo.activeSelf )
                _locationMaskGo.SetActive( true );

            _locationMaskGo.transform.position = position;
        }

        protected void hideLocaltionMask( bool isDestory )
        {
            if ( _locationMaskGo != null )
            {
                _locationMaskGo.SetActive( false );

                if ( isDestory )
                {
                    GameObject.DestroyObject( _locationMaskGo );
                }
            }
        }

        private void doMoveToByClickFloor()
        {
            //  lookAt
            if ( _currPathIndex >= 1 )
            {
                if ( _lookAtTweener != null )
                    _lookAtTweener.Kill( false );

                _lookAtTweener = this.transform.DOLookAt( _currPathVec3 , rotationSpeed , AxisConstraint.Y , Vector3.up ).SetAs( _tpLookAt );
            }

            //  movement
            if ( _movementTweener != null )
                _movementTweener.Kill();

            float movementDuration = Vector3.Distance( this.transform.position , _currPathVec3 ) / moveMaxSpeed;
            _movementTweener = this.transform.DOMove( _currPathVec3 , movementDuration ).SetAs( _tpMovement );

        }

        private void tweenOnMovementComplete()
        {
            if ( _currPathIndex + 1 < _navMeshPathCornersLen )
            {
                _currPathIndex += 1;
                _currPathVec3 = _currCanMoveNavMeshPath.corners[_currPathIndex];

                doMoveToByClickFloor();
            }
            else
            {

                hideLocaltionMask( false );

                if ( _moveCompleteTweener != null )
                    _moveCompleteTweener.Kill();

                _moveCompleteTweener = DOVirtual.Float( moveMaxSpeed , 0 , runToWalkSpeed , ( float val ) =>
                {
                    _ani.SetFloat( aniKeyForward , val );
                } );

                _ani.SetFloat( aniKeyTrun , 0 );

            }
        }
        private void tweenOnMovementUpdate()
        {
            _ani.SetFloat( aniKeyForward , moveMaxSpeed );
        }

        private void cleanAllTweeners()
        {
            if ( _lookAtTweener != null && _lookAtTweener.IsPlaying() )
            {
                _lookAtTweener.Kill();
                _lookAtTweener = null;
            }

            if ( _movementTweener != null && _movementTweener.IsPlaying() )
            {
                _movementTweener.Kill();
                _movementTweener = null;
            }

            if ( _moveCompleteTweener != null && _moveCompleteTweener.IsPlaying() )
            {
                _moveCompleteTweener.Kill();
                _moveCompleteTweener = null;
            }
        }

        //  ============================================= [public] =============================================

        public void stopMove()
        {
            _ani.SetFloat( aniKeyTrun , 0 );
            _ani.SetFloat( aniKeyForward , 0 );
            cleanAllTweeners();
        }


        //  ================= 保持朝向 =====================
        public void keepLookAt( Transform keepLookAtTF )
        {
            _keepLookAtTF = keepLookAtTF;
            joinUpdateCallBack( this.doKeepLookAt );
        }

        public void unKeepLookAt()
        {
            _keepLookAtTF = null;
            removeUpdateCallBack( this.doKeepLookAt );
        }
        private void doKeepLookAt()
        {
            if ( _keepLookAtTF != null )
                this.transform.LookAt( _keepLookAtTF.position );
        }
        //  ================= 保持朝向 =====================
    }
}
