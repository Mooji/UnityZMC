using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mooji
{
    public class PlayerUnitManager : AI , ISubscriber
    {
        private PlayerInfoBlackBoard _playerInfoBlackBoard;
        private AIBattleUnit _palyerBattleUnitComponent;

        private Animator                    _animatorComponent;
        private AIPlayerMoveAndRotation     _moveAndRotationComponent;

        public float canAttackDistance = 10f;

        public void Awake()
        {
            _playerInfoBlackBoard       = SceneBlackBoard.getInstance().getBlackBoard<PlayerInfoBlackBoard>();
            _animatorComponent          = GetComponent<Animator>();
            _moveAndRotationComponent   = GetComponent<AIPlayerMoveAndRotation>();
            _palyerBattleUnitComponent  = GetComponent<AIBattleUnit>();

        }

        public void Start()
        {
            BMC.getObserverBehaviour().registerMsg( this );
        }


        /// <summary>
        /// 开始移动到指定的地点
        /// </summary>
        /// <param name="targetPosition"></param>
        public void doJustMoveToPosition( Vector3 targetPosition)
        {
            _moveAndRotationComponent.move( targetPosition );
        }

        /// <summary>
        /// 停止移动
        /// </summary>
        public void doStopMoveToPosition()
        {
            _moveAndRotationComponent.stopMove();
        }



        public List<ObserverMsgTypeEnum> subscriberMessages()
        {
            List<ObserverMsgTypeEnum> lst = new List<ObserverMsgTypeEnum>();
            lst.Add( ObserverMsgTypeEnum.PLAYER_START_TO_MOVE_BY_CLICK_GROUND );
            lst.Add( ObserverMsgTypeEnum.PLAYER_START_ATTACK );
            return lst;
        }

        public void receivedMessage( ObserverMsgTypeEnum messageType , object paramsObj , PublisheOptionVo options )
        {
            switch ( messageType )
            {


                case ObserverMsgTypeEnum.PLAYER_START_TO_MOVE_BY_CLICK_GROUND:
                {

                    _animatorComponent.SetFloat( "OnAttack" , 0f );

                    //  如果之前有选中可攻击的作战单位，把选中的 maskGo Active设置 false
                    _playerInfoBlackBoard.getSelectBattleUnitLocaltionMaskGo().SetActive( false );


                    //  告诉移动组建，开始移动
                    RaycastHit raycastHit = (RaycastHit) paramsObj;
                    this.doMoveToByPosition( raycastHit.point );

                    break;
                }

                case ObserverMsgTypeEnum.PLAYER_START_ATTACK:
                {

                    AIBattleUnit selectBattleUnit = this.getCurrSelectBattleUnit( paramsObj );

                    if ( _palyerBattleUnitComponent.gameObject != _playerInfoBlackBoard.getCurrleadGo() )
                    {
                        throw new Exception( "PlayerUnitManager 只能添加到主角身上" );
                    }

                    //  如果攻击的不是自己
                    if ( _palyerBattleUnitComponent != selectBattleUnit )
                    {
                        _palyerBattleUnitComponent.attack( selectBattleUnit );
                    }

                    break;
                }
            }
        }

        public void doMoveToByPosition( Vector3 position )
        {
            //_moveAndRotationComponent.moveToByClickFloor( position );
        }

       
        public void doAttack( AIBattleUnit target )
        {

            _animatorComponent.SetFloat( "OnAttack" , 1f );

            //_moveAndRotationComponent.keepLookAt( target.transform );

            _palyerBattleUnitComponent.attack( target );
        }

        private AIBattleUnit getCurrSelectBattleUnit( object paramsObj )
        {

            AIBattleUnit selectBattleUnit = paramsObj as AIBattleUnit;

            if ( selectBattleUnit == null )
                selectBattleUnit = _playerInfoBlackBoard.currSelectBattleUnit;

            return selectBattleUnit;

        }
    }
}
