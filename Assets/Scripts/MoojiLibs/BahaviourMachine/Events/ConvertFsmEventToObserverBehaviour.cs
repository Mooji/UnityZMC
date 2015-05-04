using System;
using System.Collections.Generic;
using BehaviourMachine;
using Mooji;
using UnityEngine;


namespace Mooji
{
    public class ConvertFsmEventToObserverBehaviour : MonoBehaviour , ISubscriber
    {
        private MoojiConCurrentMonoState _justOneConcurrent;
        private StateMachine _sm;

        public void Start()
        {
            BMC.getObserverBehaviour().registerMsg( this );
            _sm = this.gameObject.GetComponent<StateMachine>();
            sendFinishedFsmEvent();
        }

        public List<ObserverMsgTypeEnum> subscriberMessages()
        {
            List<ObserverMsgTypeEnum> lst = new List<ObserverMsgTypeEnum>();

            //  系统自带的
            lst.Add( ObserverMsgTypeEnum.BEHAVIOUR_MACHINE_GLOBALBLACKBOARD_FINISHED );
            //  自定义
            lst.Add( ObserverMsgTypeEnum.BEHAVIOUR_MACHINE_FSM_EVENT );
            //  设置Concurrent State behaviour (实时监听)
            lst.Add( ObserverMsgTypeEnum.BEHAVIOUR_MACHINE_SET_AS_CONCURRENT );

            return lst;
        }

        public void receivedMessage( ObserverMsgTypeEnum messageType , object paramsObj , PublisheOptionVo options )
        {
            switch ( messageType )
            {
                case ObserverMsgTypeEnum.BEHAVIOUR_MACHINE_GLOBALBLACKBOARD_FINISHED:
                {
                    if ( paramsObj is IMoojiBehaviourManager )
                    {
                        sendFinishedFsmEvent();
                    }
                    else
                    {
                        throw new Exception( "BEHAVIOUR_MACHINE_GLOBALBLACKBOARD_FINISHED 该消息 必须是实现了 IMoojiBehaviourManager 的实例！" );
                    }

                    break;
                }

                case ObserverMsgTypeEnum.BEHAVIOUR_MACHINE_FSM_EVENT:
                {
                    int fsmEventID = (int) paramsObj;
                    sendFinishedFsmEvent( fsmEventID );
                    break;
                }

                case ObserverMsgTypeEnum.BEHAVIOUR_MACHINE_SET_AS_CONCURRENT:
                {
                    _justOneConcurrent = paramsObj as MoojiConCurrentMonoState;
                    break;
                }
            }
        }

        public void OnDestroy()
        {
            if ( BMC.getObserverBehaviour() != null )
                BMC.getObserverBehaviour().cleanTargetSubscriberMsg( this );
        }

        private void sendFinishedFsmEvent()
        {
            if ( _sm != null )
                _sm.SendEvent( GlobalBlackboard.FINISHED );


            if ( _justOneConcurrent != null )
                _sm.concurrentState = this._justOneConcurrent;

        }

        private void sendFinishedFsmEvent( int fsmEventId )
        {
            if ( _sm != null )
                _sm.SendEvent( fsmEventId );

            if ( _justOneConcurrent != null )
                _sm.concurrentState = this._justOneConcurrent;
        }
    }
}