
using UnityEngine;
using BehaviourMachine;
using Mooji;
using System.Collections.Generic;
using System;


namespace Mooji
{
    public class GameBootstrapper : MonoBehaviour , IMoojiBehaviourManager , ISubscriber
    {

        private GameConfigVo            _gameConfigVo;
        private PlayerInfoBlackBoard    _playerInfoBlackBoard;
        public void Awake()
        {
            _playerInfoBlackBoard = SceneBlackBoard.getInstance().addBlackBoard<PlayerInfoBlackBoard>();
        }

        public void Start()
        {
            _gameConfigVo = BMC.getGameConfigBehaviour().gameConfigVo;

            sendFsmEvent( FSMEventsBlackBoard.FSM_DEFAULT_SETTING );

            BMC.getObserverBehaviour().registerMsg( this );
        }


        public List<ObserverMsgTypeEnum> subscriberMessages()
        {
            List<ObserverMsgTypeEnum> _msgLst = new List<ObserverMsgTypeEnum>();
            _msgLst.Add( ObserverMsgTypeEnum.INPUT_CLICKED_HIT_3D_GAME_OBJECT );
            _msgLst.Add( ObserverMsgTypeEnum.INPUT_CLICKED_HIT_2D_GAME_OBJECT );
            return _msgLst;
        }

        public void receivedMessage( ObserverMsgTypeEnum messageType , object paramsObj , PublisheOptionVo options )
        {
            this.enabled = true;

            switch ( messageType )
            {

                case ObserverMsgTypeEnum.INPUT_CLICKED_HIT_3D_GAME_OBJECT:
                {
                    sendFsmEvent( FSMEventsBlackBoard.FSM_CLICK_3D_GO );
                    break; 
                }
                case ObserverMsgTypeEnum.INPUT_CLICKED_HIT_2D_GAME_OBJECT:
                {
                    sendFsmEvent( FSMEventsBlackBoard.FSM_CLICK_2D_GO );
                    break;
                }
            }
        }


        protected void sendFsmEvent( int id )
        {
            BMC.getObserverBehaviour().publisheMsg( ObserverMsgTypeEnum.BEHAVIOUR_MACHINE_FSM_EVENT , id );
        }

       


    }
}

