using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System;
namespace Mooji
{
    public class AIPlayerMovement : AIMovement , ISubscriber
    {

        private RaycastHit _raycastHit;
        private Animator _ani;


        private Tweener _monementTweener;
        private float _currFrowardNum;
        private int _currPathIndex = 0;
        private int _maxPathLen = 0;

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
            _ani = GetComponent<Animator>();

            playerRigidbody.isKinematic = true;
            playerRigidbody.detectCollisions = false;

            BMC.getObserverBehaviour().registerMsg( this );
        }


        //  ========================= impl ISubscriber =========================
        public List<ObserverMsgTypeEnum> subscriberMessages()
        {
            List<ObserverMsgTypeEnum> lst = new List<ObserverMsgTypeEnum>();
            lst.Add( ObserverMsgTypeEnum.PLAYER_CONTROLER_CHANGED );
            lst.Add( ObserverMsgTypeEnum.PLAYER_FOLLOW_TARGET_CHANGED );
            lst.Add( ObserverMsgTypeEnum.INPUT_CLICKED_HIT_3D_GAME_OBJECT );
            return lst;
        }

        public void receivedMessage( ObserverMsgTypeEnum messageType , object paramsObj , PublisheOptionVo options )
        {
            switch ( messageType )
            {

                case ObserverMsgTypeEnum.INPUT_CLICKED_HIT_3D_GAME_OBJECT:
                {

                    //  点击命中info
                    //_raycastHit = (RaycastHit) paramsObj;

                    //if ( canMove() )
                    //{

                    //    //  正在走或跑的时候，要求改变路径
                    //    if ( _currFrowardNum != 0 && _monementTweener != null )
                    //    {
                    //        _monementTweener.Kill();

                    //        //  从跑到走
                    //        _monementTweener = DOVirtual.Float( _currFrowardNum , 0f , 1f , ( float val ) =>
                    //        {
                    //            _currFrowardNum = val;
                    //            _ani.SetFloat( "Forward" , _currFrowardNum );
                                
                    //        } );
                    //    }

                    //    //  移动中函数添加到 fixedUpdate  角度<60度开跑，其他事件等待转向
                    //    joinFixedUpdateCallBack( this.fixedMoving );
                    //}

                    //joinFixedUpdateCallBack( this.fixedRotation );

                    break;
                }


                case ObserverMsgTypeEnum.PLAYER_CONTROLER_CHANGED:
                {

                    object[] objs                                       = paramsObj as object[];
                    List<GameObject> goLst                              = objs[0] as List<GameObject>;
                    List < PlayerConstant.PlayerControlerType> pctLst   = objs[1] as List<PlayerConstant.PlayerControlerType>;

                    int index =  goLst.IndexOf( this.gameObject );

                    if ( index >= 0 )
                    {
                        PlayerConstant.PlayerControlerType pct = pctLst[index];

                        switch ( pct )
                        {
                            case PlayerConstant.PlayerControlerType.PROTAGONIST:
                            {
                                break;

                            }
                            case PlayerConstant.PlayerControlerType.FOLLOWER:
                            {
                                break;
                            }
                            case PlayerConstant.PlayerControlerType.INCONTROLLABLE:
                            {
                                break;
                            }

                        }

                        //base.maskGo.SetActive( true );

                        //base.playerDescribeVo.playerControlerType = pct;
                    }

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

        }// end fun ... 

        private void fixedMoving()
        {
            if ( Vector3.Angle( this.transform.forward , _raycastHit.point.normalized ) < 60 )
            {
                if ( _monementTweener == null )
                {
                    _monementTweener = DOVirtual.Float( _currFrowardNum , 1f , 1f , ( float val ) =>
                    {
                        _currFrowardNum = val;
                        _ani.SetFloat( "Forward" , _currFrowardNum );
                    } );
                }

            }
        }

        private void fixedRotation()
        {
            Vector3 move =  _raycastHit.point - this.transform.position;
            move = this.transform.InverseTransformDirection( move );
            move = Vector3.ProjectOnPlane( move , _raycastHit.normal );
            float m_TurnAmount = Mathf.Atan2( move.x , move.z );
            _ani.SetFloat( "Turn" , m_TurnAmount , .25f , Time.fixedDeltaTime );
        }

        private bool canMove()
        {
            //if ( base.playerDescribeVo.playerControlerType == PlayerConstant.PlayerControlerType.INCONTROLLABLE )
            //    return false;

            //  是否点击了地板
            if ( isHitOnPlayerMovementFloor( _raycastHit ) )
            {
                ////  控制方式
                //Vector3 resultPosition = playerDescribeVo.playerControlerType == PlayerConstant.PlayerControlerType.PROTAGONIST
                //                        ? this._raycastHit.point : this._raycastHit.point - playerDescribeVo.floowOffsetVec3;


                //if ( getPath( resultPosition ) != null )
                //{
                //    return true;
                //}

            }

            return false;
        }

    }
}
