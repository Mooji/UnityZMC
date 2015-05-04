
using UnityEngine;
using BehaviourMachine;
using Mooji;
using System.Collections.Generic;
using System;

namespace Mooji
{
    [BehaviourMachine.NodeInfo( category = "MoojiAction/Managers/" , icon = "Axis" )]
    public class PlayerTouchManagerAction : MoojiBehaviourMachineAction
    {
        private Camera                  _mainCamera;
        private GameConfigVo            _gameConfigVo;
        private int                     _gameClickInputLayerMask;

        protected override void actionAwake()
        {

            _gameConfigVo = BMC.getGameConfigBehaviour().gameConfigVo;
            _gameClickInputLayerMask = LayerMask.GetMask( _gameConfigVo.gameLayer_clickInput );
            _mainCamera = getMaincamera();
        }

        protected override void actionStart()
        {
            Ray camRay = _mainCamera.ScreenPointToRay( UnityEngine.Input.mousePosition );
            RaycastHit rayCastHitInfo;
            if ( Physics.Raycast( camRay , out rayCastHitInfo , 100f , _gameClickInputLayerMask ) )
            {
                this.doProcessClicked( rayCastHitInfo );
            }

        }


        private void doProcessClicked( RaycastHit raycastHit )
        {

            playerInfoBB.currClickedRaycastHit = raycastHit;

            //  点击的是地板
            if ( raycastHit.collider.name == _gameConfigVo.playerMovementFloor )
            {
                sendFSMEvent( FSMEventsBlackBoard.FSM_JUST_MOVE );
            }
            else
            {

                Debug.Log( playerInfoBB.getCurrleadGo().name );
                Debug.Log( raycastHit.collider.gameObject.name );

                //  选择的是自己
                if ( playerInfoBB.getCurrleadGo() == raycastHit.collider.gameObject )
                {
                    sendFSMEvent( FSMEventsBlackBoard.FSM_SELECT_SELF );
                }
                else
                {
                    //  选择了另一个可以控制的对象
                    PlayerUnitManager pum =  raycastHit.collider.GetComponent<PlayerUnitManager>();
                    if ( pum != null )
                    {
                        playerInfoBB.setCurrLeadGo( raycastHit.collider.gameObject );

                        sendFSMEvent( FSMEventsBlackBoard.FSM_LEAD_CHANGED );
                    }
                }



                //  点击的那个对象是否是 作战单位
                AIBattleUnit battleUnit = raycastHit.collider.gameObject.GetComponent<AIBattleUnit>();

                if ( battleUnit != null )
                {
                    playerInfoBB.currSelectBattleUnit = battleUnit;
                }
            }
        }

        private Camera getMaincamera()
        {
            if ( _mainCamera == null )
            {
                GameObject mainCameraGo = GameObject.FindGameObjectWithTag( "MainCamera" );

                if ( mainCameraGo != null )
                {

                    _mainCamera = mainCameraGo.GetComponent<Camera>();
                    if ( _mainCamera == null )
                        throw new Exception( "MainCamera is null" );
                }
            }

            return _mainCamera;
        }



    }
}
