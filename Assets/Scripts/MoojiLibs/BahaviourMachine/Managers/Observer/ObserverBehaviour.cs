using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Mooji
{
    /// <summary>
    /// 消息调度中心
    /// 相当于单例类
    /// </summary>
    public class ObserverBehaviour : MonoBehaviour , IMoojiBehaviourManager
    {

        private Dictionary<ObserverMsgTypeEnum , List<ISubscriber>> _msgMapping;
        private List<object[]> _coroutinePublisheMsgLst;
        public void Awake()
        {
            _msgMapping = new Dictionary<ObserverMsgTypeEnum , List<ISubscriber>>();
            _coroutinePublisheMsgLst = new List<object[]>();
        }

        public void Start()
        {
            BMC.registerInitCompleteBehaviourManager( BehaviourManagerType.OB_SERVER , this );
            StartCoroutine( "coroutinePublisheMsg" );
        }

        /// <summary>
        /// 为订阅者，订阅指定的消息
        /// </summary>
        /// <param name="keyLst"></param>
        /// <param name="target"></param>
        public void registerMsg( List<ObserverMsgTypeEnum> keyLst , ISubscriber target )
        {
            if ( keyLst == null || _msgMapping == null )
                return;

            foreach ( ObserverMsgTypeEnum key in keyLst )
            {
                List<ISubscriber> targetLst = null;

                if ( !_msgMapping.ContainsKey( key ) )
                {
                    targetLst = new List<ISubscriber>();
                    _msgMapping[key] = targetLst;
                }
                else
                {
                    targetLst = _msgMapping[key];
                }

                if ( !targetLst.Contains( target ) )
                {
                    targetLst.Add( target );
                }
            }
        }// end fun registerMsg ...

        public void registerMsg( ISubscriber subscriber )
        {
            List<ObserverMsgTypeEnum> keyLst = subscriber.subscriberMessages();

            if ( _msgMapping == null )
                return;

            if ( null == keyLst )
                return;

            foreach ( ObserverMsgTypeEnum key in keyLst )
            {
                List<ISubscriber> targetLst = null;

                if ( !_msgMapping.ContainsKey( key ) )
                {
                    targetLst = new List<ISubscriber>();
                    _msgMapping[key] = targetLst;
                }
                else
                {
                    targetLst = _msgMapping[key];
                }

                if ( !targetLst.Contains( subscriber ) )
                {
                    targetLst.Add( subscriber );
                }
            }
        }// end fun registerMsg ...

        public void publisheMsg( ObserverMsgTypeEnum key , System.Object msgParams = null , PublisheOptionVo options = null )
        {
            if ( _msgMapping.ContainsKey( key ) )
            {
                List<ISubscriber> targetLst = _msgMapping[key];

                foreach ( ISubscriber target in targetLst )
                {
                    if ( target is MonoBehaviour )
                    {
                        MonoBehaviour mb    =  target as MonoBehaviour;
                        GameObject targetGo = mb.gameObject;

                        if ( targetGo.activeSelf && targetGo.activeInHierarchy && mb.enabled )
                        {
                            target.receivedMessage( key , msgParams , options );
                        }
                    }
                    else
                    {
                        target.receivedMessage( key , msgParams , options );
                    }
                }
            }
        }

        private IEnumerator coroutinePublisheMsg()
        {
            while ( true )
            {
                if ( _coroutinePublisheMsgLst.Count != 0 )
                {

                    ObserverMsgTypeEnum key   = (ObserverMsgTypeEnum) _coroutinePublisheMsgLst[0][0];
                    System.Object msgParams   = _coroutinePublisheMsgLst[0][1];
                    PublisheOptionVo options  = _coroutinePublisheMsgLst[0][2] as PublisheOptionVo;
                    this.publisheMsg( key , msgParams , options );
                    _coroutinePublisheMsgLst.RemoveAt( 0 );
                }
                yield return null;
            }
        }

        public void publisheMsgByCoroutine( ObserverMsgTypeEnum key , System.Object msgParams = null , PublisheOptionVo options = null )
        {
            _coroutinePublisheMsgLst.Add( new object[] { key , msgParams , options } );
        }


        /// <summary>
        /// 移除订阅者 目前已经订阅的所有消息
        /// </summary>
        /// <param name="target"></param>
        public void cleanTargetSubscriberMsg( ISubscriber target )
        {
            if ( _msgMapping == null )
                return;

            //  当前订阅的消息
            List<ObserverMsgTypeEnum> currScriberMsgLst =  target.subscriberMessages();

            if ( currScriberMsgLst == null )
                return;


            List<ObserverMsgTypeEnum> delKeyLst = null;

            foreach ( ObserverMsgTypeEnum key in currScriberMsgLst )
            {
                List<ISubscriber> targetLst = _msgMapping[key];
                if ( targetLst.Contains( target ) )
                {
                    targetLst.Remove( target );
                    if ( targetLst.Count == 0 )
                    {
                        delKeyLst = delKeyLst == null ? new List<ObserverMsgTypeEnum>() : delKeyLst;
                        delKeyLst.Add( key );
                    }
                    targetLst = null;
                }
            }

            if ( delKeyLst != null )
            {
                foreach ( ObserverMsgTypeEnum key in delKeyLst )
                {
                    _msgMapping.Remove( key );
                }
            }
        } // end fun cleanTargetSubscriberMsg ...








    }



}